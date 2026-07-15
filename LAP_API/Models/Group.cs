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
    /// <summary>
    /// Khóa chính của nhóm xe.
    /// </summary>
    [Key]
    [Column("PK_VehicleGroupID")]
    public int Id { get; set; }

    /// <summary>
    /// ID của nhóm cha (dùng để xây dựng cấu trúc cây).
    /// </summary>
    [Column("ParentVehicleGroupID")]
    public int? ParentVehicleGroupID { get; set; }

    /// <summary>
    /// ID công ty sở hữu nhóm này.
    /// </summary>
    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    /// <summary>
    /// Tên nhóm xe.
    /// </summary>
    [MaxLength(250)]
    [Column("Name")]
    public string GroupName { get; set; } = string.Empty;

    /// <summary>
    /// ID tỉnh thành liên kết với nhóm (nếu có).
    /// </summary>
    [Column("FK_BGTProvinceID")]
    public int? FK_BGTProvinceID { get; set; }

    /// <summary>
    /// Đánh dấu nhóm đã bị xóa (Soft delete).
    /// </summary>
    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }

    /// <summary>
    /// Trạng thái hoạt động của nhóm.
    /// </summary>
    [Column("Status")]
    public bool Status { get; set; }
}
