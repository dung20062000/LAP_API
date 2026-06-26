using System.ComponentModel.DataAnnotations;

namespace LAP_API.DTOs.Driver;

/// <summary>
/// Payload cập nhật inline cho một dòng lái xe trong lưới.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/26/2026   created
/// </Modified>
public class UpdateDriverRequest
{
    /// ID nhân viên cần cập nhật (bắt buộc)
    [Required]
    public int Id { get; set; }

    /// Tên hiển thị lái xe (bắt buộc, tối đa 100 ký tự) (Nvachar)
    [Required]
    [MaxLength(100, ErrorMessage = "DisplayName không được vượt quá 100 ký tự.")]
    public string DisplayName { get; set; }

    /// Số giấy phép lái xe (vachar)
    [Required]
    [MaxLength(32, ErrorMessage = "Số giấy phép lái xe không được vượt quá 32 ký tự.")]
    public string? DriverLicense { get; set; }

    /// Ngày cấp bằng lái
    [Required]
    public DateTime? IssueLicenseDate { get; set; }

    /// Ngày hết hạn bằng lái
    [Required]
    public DateTime? ExpireLicenseDate { get; set; }

    /// Nơi cấp bằng lái (Nvachar)
    [Required]
    [MaxLength(150, ErrorMessage = "Nơi cấp bằng lái không được vượt quá 100 ký tự.")]
    public string? IssueLicensePlace { get; set; }

    /// Loại bằng (FK sang BCA.LicenseTypes)
    [Required]
    public int? LicenseType { get; set; }

    /// Số điện thoại di động
    public string? Mobile { get; set; }
}
