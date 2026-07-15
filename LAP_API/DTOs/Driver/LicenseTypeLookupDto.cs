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
    /// <summary>
    /// Mã định danh loại bằng
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Tên loại bằng
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Mã code loại bằng
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
