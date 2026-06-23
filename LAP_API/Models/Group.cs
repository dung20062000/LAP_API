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

    /// ID tỉnh thành liên kết với nhóm (nếu có).
    [Column("FK_BGTProvinceID")]
    public int? FK_BGTProvinceID { get; set; }

    /// Đánh dấu nhóm đã bị xóa (Soft delete).
    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }

    /// Trạng thái hoạt động của nhóm.
    [Column("Status")]
    public bool Status { get; set; }
}
