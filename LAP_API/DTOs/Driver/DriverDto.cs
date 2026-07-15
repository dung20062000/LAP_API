namespace LAP_API.DTOs.Driver;

/// <summary>
/// DTO hiển thị thông tin lái xe trên lưới dữ liệu.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/26/2026   created
/// </Modified>
public class DriverDto
{
    /// <summary>
    /// Mã định danh nhân viên (PK_EmployeeID)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Mã nhân viên
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// Họ và tên (không dấu)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Tên hiển thị (có dấu)
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Số điện thoại di động
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    ///  Số giấy phép lái xe
    /// </summary>
    public string? DriverLicense { get; set; }

    /// <summary>
    /// Ngày cấp bằng lái
    /// </summary>
    public DateTime? IssueLicenseDate { get; set; }

    /// <summary>
    /// Ngày hết hạn bằng lái
    /// </summary>
    public DateTime? ExpireLicenseDate { get; set; }

    /// <summary>
    /// Nơi cấp bằng lái
    /// </summary>
    public string? IssueLicensePlace { get; set; }

    /// <summary>
    /// Mã loại bằng (FK sang BCA.LicenseTypes)
    /// </summary>
    public int? LicenseType { get; set; }

    /// <summary>
    /// Tên loại bằng (join từ BCA.LicenseTypes)
    /// </summary>
    public string? LicenseTypeName { get; set; }

    /// <summary>
    /// Ngày cập nhật gần nhất
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
}
