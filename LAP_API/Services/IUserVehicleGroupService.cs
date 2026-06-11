using LAP_API.DTOs.UserVehicleGroup;

namespace LAP_API.Services;

/// <summary>
/// Interface định nghĩa các nghiệp vụ liên quan đến quản lý gán nhóm xe cho người dùng.
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
public interface IUserVehicleGroupService
{
    /// <summary>
    /// Lấy danh sách người dùng đang hoạt động của công ty.
    /// </summary>
    Task<List<UserDto>> GetUsersAsync();

    /// <summary>
    /// Lấy danh sách nhóm xe chưa gán cho người dùng dưới dạng cây.
    /// </summary>
    /// <param name="userId">ID người dùng cần kiểm tra.</param>
    Task<List<VehicleGroupNodeDto>> GetUnassignedGroupsAsync(Guid userId);

    /// <summary>
    /// Lấy danh sách nhóm xe đã gán cho người dùng dưới dạng cây.
    /// </summary>
    /// <param name="userId">ID người dùng cần kiểm tra.</param>
    Task<List<VehicleGroupNodeDto>> GetAssignedGroupsAsync(Guid userId);

    /// <summary>
    /// Lưu danh sách gán nhóm xe mới cho người dùng (replace-all).
    /// </summary>
    /// <param name="userId">ID người dùng.</param>
    /// <param name="request">Request chứa danh sách GroupId mới.</param>
    Task AssignGroupsAsync(Guid userId, AssignGroupsRequest request);
}
