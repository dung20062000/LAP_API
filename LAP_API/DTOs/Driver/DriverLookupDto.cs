namespace LAP_API.DTOs.Driver;

/// <summary>
/// DTO dùng cho dropdown lái xe.
/// Hiển thị theo dạng DisplayName - DriverLicense.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/25/2026   created
/// </Modified>
public class DriverLookupDto
{
    /// <summary>
    /// Mã định danh của nhân viên (lái xe)
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Nhãn hiển thị trên dropdown: DisplayName - DriverLicense
    /// </summary>
    public string Label { get; set; } = string.Empty;
}
