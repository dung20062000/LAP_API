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
    /// Mã định danh nhân viên (PK_EmployeeID)
    public int Id { get; set; }

    /// Mã nhân viên
    public string EmployeeCode { get; set; } = string.Empty;

    /// Họ và tên (không dấu)
    public string Name { get; set; } = string.Empty;

    /// Tên hiển thị (có dấu)
    public string DisplayName { get; set; } = string.Empty;

    /// Số điện thoại di động
    public string? Mobile { get; set; }

    /// Số giấy phép lái xe
    public string? DriverLicense { get; set; }

    /// Ngày cấp bằng lái
    public DateTime? IssueLicenseDate { get; set; }

    /// Ngày hết hạn bằng lái
    public DateTime? ExpireLicenseDate { get; set; }

    /// Nơi cấp bằng lái
    public string? IssueLicensePlace { get; set; }

    /// Mã loại bằng (FK sang BCA.LicenseTypes)
    public int? LicenseType { get; set; }

    /// Tên loại bằng (join từ BCA.LicenseTypes)
    public string? LicenseTypeName { get; set; }

    /// Ngày cập nhật gần nhất
    public DateTime? UpdatedDate { get; set; }
}
