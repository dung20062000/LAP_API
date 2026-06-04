using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAP_API.Models;

[Table("Vehicle.Vehicles")]
public class Vehicle
{
    [Key]
    [Column("PK_VehicleID")]
    public long Id { get; set; }

    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    [MaxLength(16)]
    [Column("VehiclePlate")]
    public string VehiclePlate { get; set; } = string.Empty;

    [MaxLength(50)]
    [Column("PrivateCode")]
    public string PrivateCode { get; set; } = string.Empty;

    [MaxLength(32)]
    [Column("IMEI")]
    public string? IMEI { get; set; }

    [Column("IsLocked")]
    public bool IsLocked { get; set; }

    [Column("IsDeleted")]
    public bool IsDeleted { get; set; }

    [Column("XNCode")]
    public int XNCode { get; set; }

    [Column("IsCam")]
    public bool IsCam { get; set; }

    [Column("IsVideoCam")]
    public bool? IsVideoCam { get; set; }
}
