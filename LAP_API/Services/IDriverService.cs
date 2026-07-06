using LAP_API.DTOs.Driver;
using Microsoft.AspNetCore.Mvc;

namespace LAP_API.Services;

/// <summary>
/// Interface nghiệp vụ cho quản lý lái xe.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/25/2026   created
/// </Modified>
public interface IDriverService
{
    /// <summary>
    /// Lấy danh sách dropdown lái xe.
    /// </summary>
    Task<IEnumerable<DriverLookupDto>> GetLookupAsync();

    /// <summary>
    /// Lấy danh sách dropdown loại bằng lái.
    /// </summary>
    Task<IEnumerable<LicenseTypeLookupDto>> GetLicenseTypeLookupAsync();

    /// <summary>
    /// Lấy danh sách lái xe có phân trang.
    /// </summary>
    Task<DriverListResponse> GetListAsync(DriverListRequest request);

    /// <summary>
    /// Cập nhật hàng loạt thông tin lái xe.
    /// </summary>
    Task<bool> BatchUpdateAsync(List<UpdateDriverRequest> items);

    /// <summary>
    /// Xóa mềm lái xe theo ID.
    /// </summary>
    Task<(bool Success, string? ErrorMessage)> SoftDeleteAsync(int id);

    /// <summary>
    /// Xuất danh sách lái xe ra file Excel.
    /// </summary>
    Task<FileStreamResult> ExportExcelAsync(DriverExportRequest request);

    /// <summary>
    /// Lấy thông tin chi tiết lái xe theo ID.
    /// </summary>
    Task<DriverDto?> GetByIdAsync(int id);
}
