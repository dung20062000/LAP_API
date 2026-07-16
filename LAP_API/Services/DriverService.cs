using LAP_API.DTOs.Driver;
using LAP_API.Repositories;
using LAP_API.Repositories.DriverRepo;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace LAP_API.Services;

/// <summary>
/// Triển khai nghiệp vụ quản lý lái xe.
/// Gọi DriverRepository cho truy cập dữ liệu.
/// Sử dụng EPPlus (NonCommercial) để tạo file Excel.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/25/2026   created
/// </Modified>
/// <seealso cref="LAP_API.Services.BaseService" />
/// <seealso cref="LAP_API.Services.IDriverService" />
public class DriverService : BaseService, IDriverService
{
    private readonly IDriverRepository _driverRepo;

    /// <summary>
    /// Dùng để khởi tạo DriverService với các dependency cần thiết.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="driverRepo">The driver repo.</param>
    /// <param name="logger">The logger.</param>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    public DriverService(
        IUnitOfWork unitOfWork,
        IDriverRepository driverRepo,
        ILogger<DriverService> logger)
        : base(unitOfWork, logger)
    {
        _driverRepo = driverRepo;
    }


    /// <summary>
    /// Lấy danh sách dropdown lái xe.
    /// </summary>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    public async Task<IEnumerable<DriverLookupDto>> GetLookupAsync()
    {
        return await _driverRepo.GetLookupAsync();
    }


    /// <summary>
    /// Lấy danh sách dropdown loại bằng lái.
    /// </summary>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    public async Task<IEnumerable<LicenseTypeLookupDto>> GetLicenseTypeLookupAsync()
    {
        return await _driverRepo.GetLicenseTypeLookupAsync();
    }


    /// <summary>
    /// Lấy danh sách lái xe có phân trang.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Trả về dữ liệu danh sách kiểu DriverListResponse </returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    public async Task<DriverListResponse> GetListAsync(DriverListRequest request)
    {
        /// Đảm bảo giá trị phân trang hợp lệ
        if (request.Page < 1) request.Page = 1;
        if (request.PageSize < 1) request.PageSize = 20;

        var (totalRecord, items) = await _driverRepo.GetListAsync(request);

        return new DriverListResponse
        {
            TotalRecord = totalRecord,
            Items = items
        };
    }

    /// <summary>
    /// Cập nhật hàng loạt thông tin lái xe.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    public async Task<bool> BatchUpdateAsync(List<UpdateDriverRequest> items)
    {
        try
        {
            if (items.Count == 0)
                return true;

            return await _driverRepo.BatchUpdateAsync(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi khi cập nhật thông tin lái xe.");
            return false;
        }
    }


    /// <summary>
    /// Xóa mềm lái xe theo ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    public async Task<(bool Success, string? ErrorMessage)> SoftDeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
                return (false, "ID lái xe không hợp lệ");

            var success = await _driverRepo.SoftDeleteAsync(id);
            if (!success)
                return (false, "Không tìm thấy lái xe hoặc không có quyền xóa");

            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi khi xóa lái xe có ID: {Id}", id);
            return (false, "Có lỗi khi xóa dữ liệu. Vui lòng thử lại sau.");
        }
    }


    /// <summary>
    /// Xuất dữ liệu ra file Excel với EPPlus (NonCommercial license).
    /// Cấu trúc file:
    ///   Dòng 1: Tiêu đề báo cáo
    ///   Dòng 2: Thông tin bộ lọc đang áp dụng
    ///   Dòng 3-5: Trống
    ///   Dòng 6: Header cột (in đậm, có viền)
    ///   Dòng 7+: Dữ liệu (có viền)
    /// </summary>
    /// <param name="request"></param>
    /// <returns>file excel</returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    public async Task<FileStreamResult> ExportExcelAsync(DriverExportRequest request)
    {
        /// EPPlus NonCommercial
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var data = (await _driverRepo.GetForExportAsync(request)).ToList();
        
        /// Lấy danh sách loại bằng để có tên
        var licenseTypes = (await _driverRepo.GetLicenseTypeLookupAsync())
            .ToDictionary(x => x.Value, x => x.Name);

        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Data");

        /// Dòng Tiêu đề
        ws.Cells[1, 1].Value = "THÔNG TIN LÁI XE";
        ws.Cells[1, 1, 1, 9].Merge = true;
        ws.Cells[1, 1].Style.Font.Bold = true;
        ws.Cells[1, 1].Style.Font.Size = 14;
        ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        /// Thông tin bộ lọc (mỗi bộ lọc một dòng)
        int currentRow = 2;
        
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            ws.Cells[currentRow, 1].Value = $"Từ khóa: {request.Keyword}";
            ws.Cells[currentRow, 1, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 1].Style.Font.Italic = true;
            currentRow++;
        }

        if (request.DriverIds.Count > 0)
        {
            ws.Cells[currentRow, 1].Value = $"Số lái xe đã chọn: {request.DriverIds.Count}";
            ws.Cells[currentRow, 1, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 1].Style.Font.Italic = true;
            currentRow++;
        }

        if (request.LicenseTypeIds.Count > 0)
        {
            var licenseTypeNames = request.LicenseTypeIds
                .Where(id => licenseTypes.ContainsKey(id))
                .Select(id => licenseTypes[id])
                .ToList();
            
            var licenseTypesText = string.Join(", ", licenseTypeNames);
            ws.Cells[currentRow, 1].Value = $"Loại bằng đã chọn: {licenseTypesText}";
            ws.Cells[currentRow, 1, currentRow, 9].Merge = true;
            ws.Cells[currentRow, 1].Style.Font.Italic = true;
            currentRow++;
        }

       
        currentRow++; /// Bỏ qua một dòng trống

        /// Header cột 
        int headerRow = currentRow;
        string[] headers =
        {
            "STT", "Họ và tên", "Số điện thoại", "Số giấy phép lái xe",
            "Ngày cấp", "Ngày hết hạn", "Nơi cấp", "Loại bằng", "Ngày cập nhật"
        };

        for (int col = 0; col < headers.Length; col++)
        {
            var cell = ws.Cells[headerRow, col + 1];
            cell.Value = headers[col];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(68, 114, 196));
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ApplyBorder(cell);
        }

        /// Dòng 7+: Dữ liệu
        for (int i = 0; i < data.Count; i++)
        {
            var row = headerRow + 1 + i;
            var driver = data[i];

            ws.Cells[row, 1].Value = i + 1;
            ws.Cells[row, 2].Value = driver.DisplayName;
            ws.Cells[row, 3].Value = driver.Mobile;
            ws.Cells[row, 4].Value = driver.DriverLicense;
            ws.Cells[row, 5].Value = driver.IssueLicenseDate?.ToString("dd/MM/yyyy");
            ws.Cells[row, 6].Value = driver.ExpireLicenseDate?.ToString("dd/MM/yyyy");
            ws.Cells[row, 7].Value = driver.IssueLicensePlace;
            ws.Cells[row, 8].Value = driver.LicenseTypeName;
            ws.Cells[row, 9].Value = driver.UpdatedDate?.ToString("HH:mm\ndd/MM/yyyy");

            for (int col = 1; col <= 9; col++)
            {
                var cell = ws.Cells[row, col];
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);

                /// Căn giữa theo chiều dọc cho toàn bộ data
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                /// Căn lề
                if (col == 2)
                {
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                else /// Các cột còn lại
                {
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                /// Bật WrapText (tự động xuống dòng) cho cột 9
                if (col == 9)
                {
                    cell.Style.WrapText = true;
                }

                ApplyBorder(cell);
            }
        }

        /// Auto-fit cột
        ws.Cells[ws.Dimension.Address].AutoFitColumns(12);
        /// Đảm bảo cột STT không quá hẹp
        ws.Column(1).Width = Math.Max(ws.Column(1).Width, 6);

        var stream = new MemoryStream(package.GetAsByteArray());
        var fileName = $"DanhSachLaiXe_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = fileName
        };
    }


    /// <summary>
    /// Áp dụng viền mỏng cho ô Excel
    /// </summary>
    /// <param name="cell">The cell.</param>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/25/2026 created
    /// </Modified>
    private static void ApplyBorder(ExcelRange cell)
    {
        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
    }
}
