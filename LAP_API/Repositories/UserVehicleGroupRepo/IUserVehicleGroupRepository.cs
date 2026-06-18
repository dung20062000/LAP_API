using LAP_API.Models;

namespace LAP_API.Repositories.UserVehicleGroupRepo;

/// <summary>
/// Interface định nghĩa các thao tác truy xuất và cập nhật dữ liệu
/// liên quan đến người dùng và gán nhóm xe.
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
public interface IUserVehicleGroupRepository : IGenericRepository<UserVehicleGroup>
{
    /// <summary>
    /// Lấy danh sách người dùng đang hoạt động của công ty (không bị khóa, không bị xóa).
    /// </summary>
    Task<IEnumerable<User>> GetActiveUsersAsync();

    /// <summary>
    /// Lấy danh sách ID nhóm xe đã gán cho người dùng (IsDeleted != true).
    /// </summary>
    /// <param name="userId">ID người dùng.</param>
    Task<IEnumerable<int>> GetAssignedGroupIdsAsync(Guid userId);

    /// <summary>
    /// Cập nhật danh sách gán nhóm xe cho người dùng theo chiến lược replace-all:
    /// Soft-delete các bản ghi cũ không còn trong danh sách mới,
    /// thêm mới hoặc khôi phục các bản ghi còn thiếu.
    /// </summary>
    /// <param name="userId">ID người dùng.</param>
    /// <param name="groupIds">Danh sách ID nhóm xe mới cần gán.</param>
    /// <param name="groups">Danh sách đối tượng Group để lấy ParentId.</param>
    Task UpdateAssignedGroupsAsync(Guid userId, List<int> groupIds, IEnumerable<Models.Group> groups);
}
