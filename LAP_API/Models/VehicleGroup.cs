using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAP_API.Models;

[Table("Vehicle.VehicleGroups")]
public class VehicleGroup
{
    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    [Column("FK_VehicleGroupID")]
    public int GroupId { get; set; }

    [Column("FK_VehicleID")]
    public int VehicleId { get; set; }

    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }
}
