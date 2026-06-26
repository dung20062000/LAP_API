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
    /// Mã định danh của nhân viên (lái xe)
    public int Value { get; set; }

    /// Nhãn hiển thị trên dropdown: DisplayName - DriverLicense
    public string Label { get; set; } = string.Empty;
}
