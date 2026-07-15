using LAP_API.Common.Enums;

namespace LAP_API.DTOs.Driver;

/// <summary>
/// Request filter và phân trang cho lưới danh sách lái xe.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/26/2026   created
/// </Modified>
public class DriverListRequest
{
    /// <summary>
    /// Từ khóa tìm kiếm theo tên hoặc số giấy phép lái xe.
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// Loại tìm kiếm: mặc định theo tên lái xe.
    /// </summary>
    public DriverSearchType Type { get; set; } = DriverSearchType.Name;

    /// <summary>
    /// Danh sách ID lái xe để lọc (rỗng = tất cả).
    /// </summary>
    public List<int> DriverIds { get; set; } = new();

    /// <summary>
    /// Danh sách ID loại bằng để lọc (rỗng = tất cả).
    /// </summary>
    public List<int> LicenseTypeIds { get; set; } = new();

    /// <summary>
    /// Số trang hiện tại (bắt đầu từ 1).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Số dòng mỗi trang.
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Tính toán số dòng cần bỏ qua cho OFFSET.
    /// </summary>
    public int Skip => (Page > 0 ? Page - 1 : 0) * PageSize;
}

