using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAP_API.Models;

[Table("Vehicle.Groups")]
public class Group
{
    [Key]
    [Column("PK_VehicleGroupID")]
    public int Id { get; set; }

    [Column("ParentVehicleGroupID")]
    public int? ParentVehicleGroupID { get; set; }

    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    [MaxLength(250)]
    [Column("Name")]
    public string GroupName { get; set; } = string.Empty;

    [Column("CreatedByUser")]
    public Guid? CreatedByUser { get; set; }

    [Column("CreatedDate")]
    public DateTime? CreatedDate { get; set; }

    [Column("UpdatedByUser")]
    public Guid? UpdatedByUser { get; set; }

    [Column("UpdatedDate")]
    public DateTime? UpdatedDate { get; set; }

    [Column("DistanceA")]
    public double? DistanceA { get; set; }

    [Column("DistanceB")]
    public double? DistanceB { get; set; }

    [Column("MinuteA")]
    public int? MinuteA { get; set; }

    [Column("MinuteB")]
    public int? MinuteB { get; set; }

    [Column("FK_BGTProvinceID")]
    public int? FK_BGTProvinceID { get; set; }

    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }

    [Column("Flag")]
    public int Flag { get; set; }

    [Column("Status")]
    public bool Status { get; set; }
}
