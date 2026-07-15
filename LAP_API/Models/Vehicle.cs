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
    /// <summary>
    /// Khóa chính của xe
    /// </summary>
    [Key]
    [Column("PK_VehicleID")]
    public long Id { get; set; }

    /// <summary>
    /// ID công ty sở hữu xe
    /// </summary>
    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    /// <summary>
    /// Biển số xe.
    /// </summary>
    [MaxLength(16)]
    [Column("VehiclePlate")]
    public string VehiclePlate { get; set; } = string.Empty;

    /// <summary>
    /// Mã số nội bộ của xe.
    /// </summary>
    [MaxLength(50)]
    [Column("PrivateCode")]
    public string PrivateCode { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái xe bị khóa (ví dụ do nợ cước).
    /// </summary>
    [Column("IsLocked")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Đánh dấu xe đã bị xóa (Soft delete).
    /// </summary>
    [Column("IsDeleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Mã xí nghiệp quản lý.
    /// </summary>
    [Column("XNCode")]
    public int XNCode { get; set; }
}
