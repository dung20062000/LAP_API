using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAP_API.Models;

/// <summary>
/// Entity đại diện cho bảng gán nhóm xe cho người dùng (Admin.UserVehicleGroup).
/// Khóa chính là composite (FK_UserID, FK_VehicleGroupID).
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
[Table("Admin.UserVehicleGroup")]
public class UserVehicleGroup
{
    /// <summary>
    /// ID người dùng (FK).
    /// </summary>
    [Column("FK_UserID")]
    public Guid UserId { get; set; }

    /// <summary>
    /// ID nhóm xe (FK).
    /// </summary>
    [Column("FK_VehicleGroupID")]
    public int VehicleGroupId { get; set; }

    /// <summary>
    /// ID nhóm xe cha (dùng cho cấu trúc cây).
    /// </summary>
    [Column("ParentVehicleGroupID")]
    public int? ParentVehicleGroupId { get; set; }

    /// <summary>
    /// Ngày tạo bản ghi.
    /// </summary>
    [Column("CreatedDate")]
    public DateTime? CreatedDate { get; set; }

    /// <summary>
    /// Ngày cập nhật bản ghi.
    /// </summary>
    [Column("UpdatedDate")]
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Trạng thái xóa mềm.
    /// </summary>
    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }
}
