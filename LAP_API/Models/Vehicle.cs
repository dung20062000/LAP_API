using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAP_API.Models;

/// <summary>
/// Entity đại diện cho bảng thông tin xe (Vehicle.Vehicles) trong database.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>[Table("Vehicle.Vehicles")]
/// 
[Table("Vehicle.Vehicles")]
public class Vehicle
{
    /// Khóa chính của xe.
    [Key]
    [Column("PK_VehicleID")]
    public long Id { get; set; }

    /// ID công ty sở hữu xe.>
    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    /// Biển số xe.
    [MaxLength(16)]
    [Column("VehiclePlate")]
    public string VehiclePlate { get; set; } = string.Empty;

    /// Mã số nội bộ của xe.
    [MaxLength(50)]
    [Column("PrivateCode")]
    public string PrivateCode { get; set; } = string.Empty;

    /// Mã định danh thiết bị định vị (IMEI).
    [MaxLength(32)]
    [Column("IMEI")]
    public string? IMEI { get; set; }

    /// Trạng thái xe bị khóa (ví dụ do nợ cước).
    [Column("IsLocked")]
    public bool IsLocked { get; set; }

    /// Đánh dấu xe đã bị xóa (Soft delete).
    [Column("IsDeleted")]
    public bool IsDeleted { get; set; }

    /// Mã xí nghiệp quản lý.
    [Column("XNCode")]
    public int XNCode { get; set; }

    /// Xe có gắn camera hay không.
    [Column("IsCam")]
    public bool IsCam { get; set; }

    /// Xe có hỗ trợ xem video trực tuyến hay không.
    [Column("IsVideoCam")]
    public bool? IsVideoCam { get; set; }
}
