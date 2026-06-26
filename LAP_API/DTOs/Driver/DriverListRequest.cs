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
    /// Từ khóa tìm kiếm theo tên hoặc số giấy phép lái xe.
    public string? Keyword { get; set; }

    /// Loại tìm kiếm: mặc định theo tên lái xe.
    public DriverSearchType Type { get; set; } = DriverSearchType.Name;

    /// Danh sách ID lái xe để lọc (rỗng = tất cả).
    public List<int> DriverIds { get; set; } = new();

    /// Danh sách ID loại bằng để lọc (rỗng = tất cả).
    public List<int> LicenseTypeIds { get; set; } = new();

    /// Số trang hiện tại (bắt đầu từ 1).
    public int Page { get; set; } = 1;

    /// Số dòng mỗi trang.
    public int PageSize { get; set; } = 20;

    /// Tính toán số dòng cần bỏ qua cho OFFSET.
    public int Skip => (Page > 0 ? Page - 1 : 0) * PageSize;
}

