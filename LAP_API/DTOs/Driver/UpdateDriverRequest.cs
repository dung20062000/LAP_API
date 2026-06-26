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
    [Required(ErrorMessage = "Trường DisplayName không được để trống")]
    [MaxLength(100, ErrorMessage = "DisplayName không được vượt quá 100 ký tự.")]
    public string DisplayName { get; set; }

    /// Số giấy phép lái xe (vachar)
    [Required(ErrorMessage = "Trường DriverLicense không được để trống")]
    [MaxLength(32, ErrorMessage = "Số giấy phép lái xe không được vượt quá 32 ký tự.")]
    public string? DriverLicense { get; set; }

    /// Ngày cấp bằng lái
    [Required(ErrorMessage = "Trường IssueLicenseDate không được để trống")]
    public DateTime? IssueLicenseDate { get; set; }

    /// Ngày hết hạn bằng lái
    [Required(ErrorMessage = "Trường ExpireLicenseDate không được để trống")]
    public DateTime? ExpireLicenseDate { get; set; }

    /// Nơi cấp bằng lái (Nvachar)
    [Required(ErrorMessage = "Trường IssueLicensePlace không được để trống")]
    [MaxLength(150, ErrorMessage = "Nơi cấp bằng lái không được vượt quá 150 ký tự.")]
    public string? IssueLicensePlace { get; set; }

    /// Loại bằng (FK sang BCA.LicenseTypes)
    [Required(ErrorMessage = "Trường LicenseType không được để trống")]
    public int? LicenseType { get; set; }

    /// Số điện thoại di động
    public string? Mobile { get; set; }
}
