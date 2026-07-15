namespace LAP_API.DTOs.Driver;

/// <summary>
/// Response phân trang cho lưới danh sách lái xe.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/26/2026   created
/// </Modified>
public class DriverListResponse
{
    /// <summary>
    /// Tổng số bản ghi thỏa điều kiện lọc
    /// </summary>
    public int TotalRecord { get; set; }

    /// <summary>
    /// Danh sách dữ liệu trang hiện tại
    /// </summary>
    public IEnumerable<DriverDto> Items { get; set; } = Enumerable.Empty<DriverDto>();
}
