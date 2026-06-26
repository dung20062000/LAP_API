namespace LAP_API.DTOs.Driver;

/// <summary>
/// DTO dùng cho dropdown loại bằng lái.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/25/2026   created
/// </Modified>
public class LicenseTypeLookupDto
{
    /// Mã định danh loại bằng
    public int Value { get; set; }

    /// Tên loại bằng
    public string Name { get; set; } = string.Empty;

    /// Mã code loại bằng
    public string Code { get; set; } = string.Empty;
}
