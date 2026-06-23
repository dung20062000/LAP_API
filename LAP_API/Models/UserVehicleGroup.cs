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
    /// ID người dùng (FK).
    [Column("FK_UserID")]
    public Guid UserId { get; set; }

    /// ID nhóm xe (FK).
    [Column("FK_VehicleGroupID")]
    public int VehicleGroupId { get; set; }

    /// ID nhóm xe cha (dùng cho cấu trúc cây).
    [Column("ParentVehicleGroupID")]
    public int? ParentVehicleGroupId { get; set; }

    /// Ngày tạo bản ghi.
    [Column("CreatedDate")]
    public DateTime? CreatedDate { get; set; }

    /// Ngày cập nhật bản ghi.
    [Column("UpdatedDate")]
    public DateTime? UpdatedDate { get; set; }

    /// Trạng thái xóa mềm.
    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }
}
