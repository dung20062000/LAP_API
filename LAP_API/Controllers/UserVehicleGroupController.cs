using LAP_API.DTOs.UserVehicleGroup;
using LAP_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LAP_API.Controllers;

/// <summary>
/// Controller API quản lý việc gán nhóm xe cho người dùng.
/// Cung cấp các endpoint để lấy danh sách user, nhóm đã/chưa gán và lưu gán nhóm.
/// Tất cả dữ liệu được giới hạn bởi CompanyId = 15076.
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
/// <seealso cref="LAP_API.Controllers.BaseApiController" />
[Route("api")]
[ApiController]
public class UserVehicleGroupController : BaseApiController
{
    /// <summary>
    /// Instance service gán nhóm xe
    /// </summary>
    private readonly IUserVehicleGroupService _userVehicleGroupService;

    /// <summary>
    /// Khởi tạo một instance mới của UserVehicleGroupController.
    /// </summary>
    /// <param name="userVehicleGroupService">Service được inject.</param>
    public UserVehicleGroupController(IUserVehicleGroupService userVehicleGroupService)
    {
        _userVehicleGroupService = userVehicleGroupService;
    }


    /// <summary>
    /// Lấy danh sách người dùng đang hoạt động của công ty (CompanyId=15076).
    /// Lọc: không bị khóa, không bị xóa. Sắp xếp theo tên (Fullname).
    /// </summary>
    /// <returns>
    /// Danh sách UserDto gồm UserId, Username, Fullname, Email, UserType.
    /// </returns>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userVehicleGroupService.GetUsersAsync();
        return OkResponse(users);
    }


    /// <summary>
    /// Lấy danh sách nhóm xe chưa được gán cho người dùng cụ thể, dưới dạng cây.
    /// Truy vấn tất cả nhóm xe của CompanyId=15076, lọc ra những nhóm chưa gán.
    /// Trả về dạng Tree phân cấp dựa trên ParentVehicleGroupID.
    /// </summary>
    /// <param name="userId">ID người dùng cần kiểm tra (bắt buộc).</param>
    /// <returns>
    /// Danh sách VehicleGroupNodeDto dạng cây, với key, label, data, children.
    /// </returns>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    [HttpGet("groups/unassigned")]
    public async Task<IActionResult> GetUnassignedGroups([FromQuery] Guid userId)
    {
        if (userId == Guid.Empty)
            return FailResponse<object>("Người dùng không hợp lệ");

        var groups = await _userVehicleGroupService.GetUnassignedGroupsAsync(userId);
        return OkResponse(groups);
    }


    /// <summary>
    /// Lấy danh sách nhóm xe đã gán cho người dùng cụ thể, dưới dạng cây.
    /// Truy vấn các nhóm xe đã tồn tại trong Admin.UserVehicleGroup ứng với user đang chọn.
    /// Trả về dạng Tree phân cấp dựa trên ParentVehicleGroupID.
    /// </summary>
    /// <param name="userId">ID người dùng cần kiểm tra (bắt buộc).</param>
    /// <returns>
    /// Danh sách VehicleGroupNodeDto dạng cây, với key, label, data, children.
    /// </returns>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    [HttpGet("groups/assigned")]
    public async Task<IActionResult> GetAssignedGroups([FromQuery] Guid userId)
    {
        if (userId == Guid.Empty)
            return FailResponse<object>("Người dùng không hợp lệ");

        var groups = await _userVehicleGroupService.GetAssignedGroupsAsync(userId);
        return OkResponse(groups);
    }


    /// <summary>
    /// Lưu danh sách gán nhóm xe cho người dùng (replace-all strategy).
    /// Nhận mảng GroupId, soft-delete các gán cũ không còn trong danh sách,
    /// thêm mới hoặc khôi phục các gán mới.
    /// </summary>
    /// <param name="id">ID người dùng (từ route).</param>
    /// <param name="request">Body chứa mảng GroupId mới cần gán.</param>
    /// <returns>
    /// 200 OK khi lưu thành công, 400 nếu dữ liệu không hợp lệ.
    /// </returns>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    [HttpPost("users/{id}/groups")]
    public async Task<IActionResult> AssignGroups([FromRoute] Guid id, [FromBody] AssignGroupsRequest request)
    {
        if (id == Guid.Empty)
            return FailResponse<object>("Người dùng không hợp lệ");

        // Validate model state để đảm bảo dữ liệu đầu vào hợp lệ (Data Annotations)
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return FailResponse<object>("Dữ liệu không hợp lệ", errors);
        }

        await _userVehicleGroupService.AssignGroupsAsync(id, request);
        return OkResponse("Gán nhóm xe thành công");
    }
}
