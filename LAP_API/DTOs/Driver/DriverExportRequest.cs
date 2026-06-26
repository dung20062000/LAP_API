using LAP_API.Common.Enums;

namespace LAP_API.DTOs.Driver;

/// <summary>
/// Request filter cho chức năng xuất Excel (không có phân trang)
/// Kế thừa cùng bộ lọc với DriverListRequest nhưng bỏ phân trang
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/26/2026   created
/// </Modified>
public class DriverExportRequest
{
    /// Từ khóa tìm kiếm theo tên hoặc số giấy phép lái xe
    public string? Keyword { get; set; }

    /// Loại tìm kiếm: mặc định theo tên lái xe.
    public DriverSearchType Type { get; set; } = DriverSearchType.Name;

    /// Danh sách ID lái xe để lọc (rỗng = tất cả)
    public List<int> DriverIds { get; set; } = new();

    /// Danh sách ID loại bằng để lọc (rỗng = tất cả)
    public List<int> LicenseTypeIds { get; set; } = new();
}

