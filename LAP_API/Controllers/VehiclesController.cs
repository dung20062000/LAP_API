using LAP_API.DTOs.Vehicle;
using LAP_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LAP_API.Controllers;

/// <summary>
/// Controller API cho các thao tác liên quan đến xe và ảnh.
/// Cung cấp các endpoint để lấy cây nhóm xe, danh sách xe
/// theo nhóm, và tìm kiếm ảnh lịch sử.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VehiclesController : BaseApiController
{
    /// <summary>Instance vehicle service.</summary>
    private readonly IVehicleService _vehicleService;

    /// <summary>
    /// Khởi tạo một instance mới của VehiclesController.
    /// </summary>
    /// <param name="vehicleService">Service được inject.</param>
    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    /// <summary>
    /// Lấy danh sách tất cả nhóm xe dưới dạng cây PrimeNG TreeNode.
    /// Mỗi nút bao gồm tên nhóm và số lượng xe đang hoạt động.
    /// </summary>
    /// <returns>
    /// Danh sách VehicleGroupTreeDto với key, label, data và children.
    /// </returns>
    [HttpGet("groups")]
    public async Task<IActionResult> GetGroupsTree()
    {
        var tree = await _vehicleService.GetGroupsTreeAsync();
        return OkResponse(tree);
    }

    /// <summary>
    /// Lấy danh sách phẳng các xe đang hoạt động,
    /// có thể lọc theo danh sách ID nhóm.
    /// </summary>
    /// <param name="groupIds">
    /// Danh sách ID nhóm để lọc xe.
    /// Để trống để lấy tất cả xe đang hoạt động.
    /// </param>
    /// <returns>
    /// Danh sách VehicleDto với Id, VehiclePlate, PrivateCode và DisplayName.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetVehicles([FromQuery] List<int> groupIds)
    {
        var vehicles = await _vehicleService.GetVehiclesByGroupIdsAsync(groupIds);
        return OkResponse(vehicles);
    }

    /// <summary>
    /// Tìm kiếm ảnh lịch sử từ cơ sở dữ liệu ảnh trung tâm
    /// trong khoảng thời gian được chỉ định kèm bộ lọc tùy chọn.
    /// </summary>
    /// <param name="request">
    /// ImageSearchRequest chứa vehiclePlate hoặc customerId,
    /// channels, khoảng thời gian, thứ tự sắp xếp và phân trang.
    /// </param>
    /// <returns>
    /// ImageSearchResponse có phân trang với totalCount và items,
    /// hoặc thông điệp lỗi nếu validation thất bại hoặc API lỗi.
    /// </returns>
    [HttpPost("images/search")]
    public async Task<IActionResult> SearchImages([FromBody] ImageSearchRequest request)
    {
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

        var (response, error) = await _vehicleService.SearchImagesAsync(request);
        if (error is not null)
        {
            return FailResponse<object>(error, 400);
        }

        return OkResponse(response!);
    }
}
