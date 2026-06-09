using LAP_API.DTOs.Vehicle;
using LAP_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LAP_API.Controllers;


/// <summary>
/// Controller API cho các thao tác liên quan đến xe và ảnh.
/// Cung cấp các endpoint để lấy cây nhóm xe, danh sách xe
/// theo nhóm, và tìm kiếm ảnh lịch sử.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
/// <seealso cref="LAP_API.Controllers.BaseApiController" />
[Route("api/[controller]")]
[ApiController]
public class VehiclesController : BaseApiController
{
    /// Instance vehicle service
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
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
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
    /// <param name="groupIds">danh sách id của nhóm xe</param>
    /// <returns>
    /// Danh sách VehicleDto với Id, VehiclePlate, PrivateCode và DisplayName.
    /// </returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
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
    /// <param name="request">biến truyền vào</param>
    /// <returns>
    /// ImageSearchResponse có phân trang với totalCount và items,
    /// hoặc thông điệp lỗi nếu validation thất bại hoặc API lỗi.
    /// </returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
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
