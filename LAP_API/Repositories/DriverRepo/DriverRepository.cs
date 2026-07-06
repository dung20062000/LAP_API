using Dapper;
using LAP_API.Common.Enums;
using LAP_API.DTOs.Driver;
using Microsoft.AspNetCore.Hosting.Server;
using System.Data;

namespace LAP_API.Repositories.DriverRepo;

/// <summary>
/// Triển khai các thao tác dữ liệu lái xe sử dụng Dapper (raw SQL).
/// Tất cả query được giới hạn chặt với FK_CompanyID = 15076.
/// Tránh SQL Injection bằng DynamicParameters.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/25/2026   created
/// </Modified>
public class DriverRepository : IDriverRepository
{
    private readonly IDbConnection _db;

    // Tạm thời fix cứng CompanyId cho phiên bản hiện tại
    private const int CompanyId = 15076;

    public DriverRepository(IDbConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Lấy danh sách dropdown lái xe chưa bị khóa và chưa bị xóa.
    /// </summary>
    /// <returns>Nhãn hiển thị theo dạng: DisplayName - DriverLicense.</returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    public async Task<IEnumerable<DriverLookupDto>> GetLookupAsync()
    {
        const string sql = @"
            SELECT
                PK_EmployeeID AS Value,
                CONCAT(DisplayName, N' - ', ISNULL(DriverLicense, N'')) AS Label
            FROM [HRM.Employees]
            WHERE FK_CompanyID = @CompanyId
              AND IsLocked = 0
              AND IsDeleted = 0
            ORDER BY DisplayName ASC;";

        var parameters = new DynamicParameters();
        parameters.Add("CompanyId", CompanyId, DbType.Int32);

        return await _db.QueryAsync<DriverLookupDto>(sql, parameters);
    }


    /// <summary>
    /// Lấy danh sách loại bằng đang hoạt động (IsActived = 1, IsDeteted = 0)
    /// </summary>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    public async Task<IEnumerable<LicenseTypeLookupDto>> GetLicenseTypeLookupAsync()
    {
        const string sql = @"
            SELECT
                PK_LicenseTypeID AS Value,
                Name,
                Code
            FROM [BCA.LicenseTypes]
            WHERE IsActived = 1
              AND IsDeteted = 0
            ORDER BY Name ASC;";

        return await _db.QueryAsync<LicenseTypeLookupDto>(sql);
    }


    /// <summary>
    /// Xây dựng câu SQL động với các bộ lọc tuỳ chọn và phân trang OFFSET/FETCH.
    /// Dùng QueryMultiple để lấy đồng thời TotalRecord và danh sách dữ liệu.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>
    /// Tuple gồm tổng số bản ghi và danh sách trang hiện tại.
    /// </returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/26/2026 created
    /// </Modified>
    public async Task<(int TotalRecord, IEnumerable<DriverDto> Items)> GetListAsync(DriverListRequest request)
    {
        var parameters = new DynamicParameters();
        parameters.Add("CompanyId", CompanyId, DbType.Int32);
        parameters.Add("Skip", request.Skip, DbType.Int32);
        parameters.Add("Take", request.PageSize, DbType.Int32);

        /// Xây dựng mệnh đề WHERE động
        var whereClauses = new List<string>
        {
            "e.FK_CompanyID = @CompanyId",
            "e.IsDeleted = 0"
        };

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keywordColumn = request.Type == DriverSearchType.DriverLicense
                ? "e.DriverLicense"
                : "e.DisplayName";

            /// ép kiểu dữ liệu trong SQL Server chuyển sang dạng đối chiếu: CI và AI (Case Insensitive, Accent Insensitive) để tìm kiếm không phân biệt chữ hoa chữ thường và dấu.
            whereClauses.Add($"{keywordColumn} COLLATE SQL_Latin1_General_CP1_CI_AI LIKE @Keyword");
            parameters.Add("Keyword", $"%{request.Keyword.Trim()}%", DbType.String);
        }

        if (request.DriverIds.Count > 0)
        {
            whereClauses.Add("e.PK_EmployeeID IN @DriverIds");
            parameters.Add("DriverIds", request.DriverIds);
        }

        if (request.LicenseTypeIds.Count > 0)
        {
            whereClauses.Add("e.LicenseType IN @LicenseTypeIds");
            parameters.Add("LicenseTypeIds", request.LicenseTypeIds);
        }

        var whereStr = string.Join(" AND ", whereClauses);

        var sql = $@"
            SELECT COUNT(1)
            FROM [HRM.Employees] e
            WHERE {whereStr};

            SELECT
                e.PK_EmployeeID  AS Id,
                e.EmployeeCode,
                e.Name,
                e.DisplayName,
                e.Mobile,
                e.DriverLicense,
                e.IssueLicenseDate,
                e.ExpireLicenseDate,
                e.IssueLicensePlace,
                e.LicenseType,
                lt.Name          AS LicenseTypeName,
                ISNULL(e.UpdatedDate, e.CreatedDate) AS UpdatedDate
            FROM [HRM.Employees] e
            LEFT JOIN [BCA.LicenseTypes] lt
                ON lt.PK_LicenseTypeID = e.LicenseType
               AND lt.IsActived = 1
               AND lt.IsDeteted = 0
            WHERE {whereStr}
            ORDER BY e.CreatedDate ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

        /// <summary>
        /// Bắn 1 request duy nhất mang theo cả 2 câu SQL xuống Database.
        /// Tối ưu hóa hiệu năng, tránh 2 lần round-trip.
        /// QueryMultipleAsync trả về một GridReader để đọc nhiều result set.
        /// </summary>
        using var multi = await _db.QueryMultipleAsync(sql, parameters);
        var totalRecord = await multi.ReadFirstAsync<int>();
        var items = await multi.ReadAsync<DriverDto>();

        return (totalRecord, items);
    }


    /// <summary>
    /// Mở IDbTransaction, duyệt qua danh sách và execute UPDATE từng dòng.
    /// Bắt buộc cập nhật UpdatedDate = GETDATE(). Rollback nếu có lỗi.
    /// </summary>
    /// <param name="items">Danh sách lái xe cần được update</param>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/26/2026 created
    /// </Modified>
    public async Task<bool> BatchUpdateAsync(List<UpdateDriverRequest> items)
    {
        if (items.Count == 0)
            return true;

        const string sql = @"
            UPDATE [HRM.Employees]
            SET
                DisplayName       = @DisplayName,
                DriverLicense     = @DriverLicense,
                IssueLicenseDate  = @IssueLicenseDate,
                ExpireLicenseDate = @ExpireLicenseDate,
                IssueLicensePlace = @IssueLicensePlace,
                LicenseType       = @LicenseType,
                Mobile            = @Mobile,
                UpdatedDate       = GETDATE()
            WHERE PK_EmployeeID = @Id
              AND FK_CompanyID  = @CompanyId
              AND IsDeleted     = 0;";

        /// Đảm bảo connection được mở trước khi tạo transaction
        if (_db.State != ConnectionState.Open)
            _db.Open();


        /// <summary>
        /// sử dung transaction để đảm bảo tất cả các bản ghi được cập nhật thành công hoặc rollback nếu có lỗi
        /// </summary>
        using var transaction = _db.BeginTransaction();
        try
        {
            foreach (var item in items)
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", item.Id, DbType.Int32);
                parameters.Add("CompanyId", CompanyId, DbType.Int32);
                parameters.Add("DisplayName", item.DisplayName, DbType.String);
                parameters.Add("DriverLicense", item.DriverLicense, DbType.String);
                parameters.Add("IssueLicenseDate", item.IssueLicenseDate, DbType.DateTime);
                parameters.Add("ExpireLicenseDate", item.ExpireLicenseDate, DbType.DateTime);
                parameters.Add("IssueLicensePlace", item.IssueLicensePlace, DbType.String);
                parameters.Add("LicenseType", item.LicenseType, DbType.Int32);
                parameters.Add("Mobile", item.Mobile, DbType.String);

                await _db.ExecuteAsync(sql, parameters, transaction);
            }

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }


    /// <summary>
    /// Thực hiện soft delete: SET IsDeleted = 1 với điều kiện CompanyId.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/26/2026 created
    /// </Modified>
    public async Task<bool> SoftDeleteAsync(int id)
    {
        const string sql = @"
            UPDATE [HRM.Employees]
            SET IsDeleted   = 1,
                UpdatedDate = GETDATE()
            WHERE PK_EmployeeID = @Id
              AND FK_CompanyID  = @CompanyId;";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);
        parameters.Add("CompanyId", CompanyId, DbType.Int32);

        var rowsAffected = await _db.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }


    /// <summary>
    /// Lấy toàn bộ dữ liệu theo bộ lọc (không phân trang) dùng cho export - Giống GetListAsync.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Trả về danh sách</returns> 
    /// <Modified>
    /// Name Date Comments
    /// dungbt     6/26/2026   created
    /// </Modified>
    public async Task<IEnumerable<DriverDto>> GetForExportAsync(DriverExportRequest request)
    {
        var parameters = new DynamicParameters();
        parameters.Add("CompanyId", CompanyId, DbType.Int32);

        var whereClauses = new List<string>
        {
            "e.FK_CompanyID = @CompanyId",
            "e.IsDeleted = 0"
        };

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keywordColumn = request.Type == DriverSearchType.DriverLicense
                ? "e.DriverLicense"
                : "e.DisplayName";

            whereClauses.Add($"{keywordColumn} COLLATE SQL_Latin1_General_CP1_CI_AI LIKE @Keyword");
            parameters.Add("Keyword", $"%{request.Keyword.Trim()}%", DbType.String);
        }

        if (request.DriverIds.Count > 0)
        {
            whereClauses.Add("e.PK_EmployeeID IN @DriverIds");
            parameters.Add("DriverIds", request.DriverIds);
        }

        if (request.LicenseTypeIds.Count > 0)
        {
            whereClauses.Add("e.LicenseType IN @LicenseTypeIds");
            parameters.Add("LicenseTypeIds", request.LicenseTypeIds);
        }

        var whereStr = string.Join(" AND ", whereClauses);

        var sql = $@"
            SELECT
                e.PK_EmployeeID  AS Id,
                e.EmployeeCode,
                e.Name,
                e.DisplayName,
                e.Mobile,
                e.DriverLicense,
                e.IssueLicenseDate,
                e.ExpireLicenseDate,
                e.IssueLicensePlace,
                e.LicenseType,
                lt.Name          AS LicenseTypeName,
                ISNULL(e.UpdatedDate, e.CreatedDate) AS UpdatedDate
            FROM [HRM.Employees] e
            LEFT JOIN [BCA.LicenseTypes] lt
                ON lt.PK_LicenseTypeID = e.LicenseType
               AND lt.IsActived = 1
               AND lt.IsDeteted = 0
            WHERE {whereStr}
            ORDER BY e.CreatedDate ASC;";

        return await _db.QueryAsync<DriverDto>(sql, parameters);
    }

    /// <summary>
    /// Lấy thông tin chi tiết lái xe theo ID.
    /// </summary>
    /// <param name="id">ID của lái xe.</param>
    /// <returns>DriverDto hoặc null nếu không tìm thấy.</returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/26/2026 created
    /// </Modified>
    public async Task<DriverDto?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);
        parameters.Add("CompanyId", CompanyId, DbType.Int32);

        var sql = @"
            SELECT
                e.PK_EmployeeID  AS Id,
                e.EmployeeCode,
                e.Name,
                e.DisplayName,
                e.Mobile,
                e.DriverLicense,
                e.IssueLicenseDate,
                e.ExpireLicenseDate,
                e.IssueLicensePlace,
                e.LicenseType,
                lt.Name          AS LicenseTypeName,
                ISNULL(e.UpdatedDate, e.CreatedDate) AS UpdatedDate
            FROM [HRM.Employees] e
            LEFT JOIN [BCA.LicenseTypes] lt
                ON lt.PK_LicenseTypeID = e.LicenseType
               AND lt.IsActived = 1
               AND lt.IsDeteted = 0
            WHERE e.PK_EmployeeID = @Id
              AND e.FK_CompanyID = @CompanyId
              AND e.IsDeleted = 0;";

        return await _db.QueryFirstOrDefaultAsync<DriverDto>(sql, parameters);
    }
}