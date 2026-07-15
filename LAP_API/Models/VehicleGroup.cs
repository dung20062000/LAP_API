using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAP_API.Models;

/// <summary>
/// bảng liên kết giữa xe (Vehicle.Vehicles) và nhóm xe (Vehicle.Groups) trong database.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
[Table("Vehicle.VehicleGroups")]
public class VehicleGroup
{
    /// <summary>
    /// Id của công ty
    /// </summary>
    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    /// <summary>
    /// Id của nhóm xe
    /// </summary>
    [Column("FK_VehicleGroupID")]
    public int GroupId { get; set; }

    /// <summary>
    /// Id của xe
    /// </summary>
    [Column("FK_VehicleID")]
    public int VehicleId { get; set; }

    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }
}
