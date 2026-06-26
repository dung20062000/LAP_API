using LAP_API.DTOs.Driver;

namespace LAP_API.Repositories.DriverRepo;

/// <summary>
/// Interface định nghĩa các thao tác dữ liệu cho lái xe.
/// Tất cả query được giới hạn với CompanyId = 15076.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/25/2026   created
/// </Modified>
public interface IDriverRepository
{
    /// <summary>
    /// Lấy danh sách dropdown lái xe đang hoạt động.
    /// </summary>
    Task<IEnumerable<DriverLookupDto>> GetLookupAsync();

    /// <summary>
    /// Lấy danh sách dropdown loại bằng lái đang hoạt động.
    /// </summary>
    Task<IEnumerable<LicenseTypeLookupDto>> GetLicenseTypeLookupAsync();

    /// <summary>
    /// Lấy danh sách lái xe có phân trang và bộ lọc động.
    /// </summary>
    /// <returns>Tuple gồm tổng số bản ghi và danh sách trang hiện tại.</returns>
    Task<(int TotalRecord, IEnumerable<DriverDto> Items)> GetListAsync(DriverListRequest request);

    /// <summary>
    /// Cập nhật hàng loạt thông tin lái xe trong một transaction.
    /// </summary>
    Task<bool> BatchUpdateAsync(List<UpdateDriverRequest> items);

    /// <summary>
    /// Xóa mềm lái xe theo ID (set IsDeleted = 1).
    /// </summary>
    Task<bool> SoftDeleteAsync(int id);

    /// <summary>
    /// Lấy toàn bộ dữ liệu theo bộ lọc (không phân trang) dùng cho export.
    /// </summary>
    Task<IEnumerable<DriverDto>> GetForExportAsync(DriverExportRequest request);
}
