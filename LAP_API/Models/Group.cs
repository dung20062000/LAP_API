using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAP_API.Models;

/// <summary>
/// Entity đại diện cho bảng nhóm xe (Vehicle.Groups) trong database.
/// Cho phép tổ chức cây phân cấp các nhóm xe.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
[Table("Vehicle.Groups")]
public class Group
{
    /// Khóa chính của nhóm xe.
    [Key]
    [Column("PK_VehicleGroupID")]
    public int Id { get; set; }

    /// ID của nhóm cha (dùng để xây dựng cấu trúc cây).
    [Column("ParentVehicleGroupID")]
    public int? ParentVehicleGroupID { get; set; }

    /// ID công ty sở hữu nhóm này.
    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    /// Tên nhóm xe.
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

    /// ID tỉnh thành liên kết với nhóm (nếu có).
    [Column("FK_BGTProvinceID")]
    public int? FK_BGTProvinceID { get; set; }

    /// Đánh dấu nhóm đã bị xóa (Soft delete).
    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }

    [Column("Flag")]
    public int Flag { get; set; }

    /// Trạng thái hoạt động của nhóm.
    [Column("Status")]
    public bool Status { get; set; }
}
