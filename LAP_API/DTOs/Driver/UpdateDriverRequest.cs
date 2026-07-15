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
    /// <summary>
    /// ID nhân viên cần cập nhật (bắt buộc)
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Tên hiển thị lái xe (bắt buộc, tối đa 100 ký tự) (Nvachar)
    /// </summary>
    [Required(ErrorMessage = "Trường DisplayName không được để trống")]
    [MaxLength(100, ErrorMessage = "DisplayName không được vượt quá 100 ký tự.")]
    public string DisplayName { get; set; }

    /// <summary>
    /// Số giấy phép lái xe (vachar)
    /// </summary>
    [Required(ErrorMessage = "Trường DriverLicense không được để trống")]
    [MaxLength(32, ErrorMessage = "Số giấy phép lái xe không được vượt quá 32 ký tự.")]
    public string? DriverLicense { get; set; }

    /// <summary>
    /// Ngày cấp bằng lái
    /// </summary>
    [Required(ErrorMessage = "Trường IssueLicenseDate không được để trống")]
    public DateTime? IssueLicenseDate { get; set; }

    /// <summary>
    /// Ngày hết hạn bằng lái
    /// </summary>
    [Required(ErrorMessage = "Trường ExpireLicenseDate không được để trống")]
    public DateTime? ExpireLicenseDate { get; set; }

    /// <summary>
    /// Nơi cấp bằng lái (Nvachar)
    /// </summary>
    [Required(ErrorMessage = "Trường IssueLicensePlace không được để trống")]
    [MaxLength(150, ErrorMessage = "Nơi cấp bằng lái không được vượt quá 150 ký tự.")]
    public string? IssueLicensePlace { get; set; }

    /// <summary>
    /// Loại bằng (FK sang BCA.LicenseTypes)
    /// </summary>
    [Required(ErrorMessage = "Trường LicenseType không được để trống")]
    public int? LicenseType { get; set; }

    /// <summary>
    /// Số điện thoại di động
    /// </summary>
    public string? Mobile { get; set; }
}
