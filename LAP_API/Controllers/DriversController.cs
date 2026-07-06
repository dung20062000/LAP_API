using System.ComponentModel.DataAnnotations;
using LAP_API.DTOs.Driver;
using LAP_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LAP_API.Controllers;

/// <summary>
/// Controller API cho tính năng quản lý danh sách lái xe.
/// Cung cấp các endpoint: lookup dropdown, lưới phân trang,
/// cập nhật hàng loạt, xóa mềm và xuất Excel.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/25/2026   created
/// </Modified>
/// <seealso cref="LAP_API.Controllers.BaseApiController" />
[Route("api/[controller]")]
[ApiController]
public class DriversController : BaseApiController
{
    /// Instance driver service
    private readonly IDriverService _driverService;

    /// <summary>
    /// Khởi tạo một instance mới của DriversController.
    /// </summary>
    /// <param name="driverService">Service được inject.</param>
    public DriversController(IDriverService driverService)
    {
        _driverService = driverService;
    }


    /// <summary>
    /// Lấy danh sách dropdown lái xe đang hoạt động.
    /// Điều kiện: IsLocked = 0, IsDeleted = 0, CompanyId = 15076.
    /// Nhãn hiển thị: DisplayName - DriverLicense.
    /// </summary>
    /// <returns>Danh sách DriverLookupDto sắp xếp tăng dần theo tên.</returns>
    /// <Modified>
    /// Name       Date        Comments
    /// dungbt     6/25/2026   created
    /// </Modified>
    [HttpGet("driver-lookup")]
    public async Task<IActionResult> GetLookup()
    {
        var result = await _driverService.GetLookupAsync();
        return OkResponse(result);
    }

    /// <summary>
    /// Lấy danh sách dropdown loại bằng lái đang hoạt động.
    /// Điều kiện: IsActived = 1, IsDeteted = 0.
    /// </summary>
    /// <returns>Danh sách LicenseTypeLookupDto.</returns>
    /// <Modified>
    /// Name       Date        Comments
    /// dungbt     6/25/2026   created
    /// </Modified>
    [HttpGet("license-types-lookup")]
    public async Task<IActionResult> GetLicenseTypeLookup()
    {
        var result = await _driverService.GetLicenseTypeLookupAsync();
        return OkResponse(result);
    }


    /// <summary>
    /// Lấy danh sách lái xe có phân trang và bộ lọc động.
    /// Hỗ trợ lọc theo Keyword, DriverIds, LicenseTypeIds.
    /// </summary>
    /// <param name="request">Tham số lọc và phân trang.</param>
    /// <returns>DriverListResponse gồm TotalRecord và Items.</returns>
    /// <Modified>
    /// Name       Date        Comments
    /// dungbt     6/26/2026   created
    /// </Modified>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] DriverListRequest request)
    {
        var result = await _driverService.GetListAsync(request);
        return OkResponse(result);
    }


    /// <summary>
    /// Cập nhật hàng loạt thông tin lái xe (inline edit).
    /// Nhận danh sách các dòng có thay đổi và thực hiện trong một transaction.
    /// </summary>
    /// <param name="items">Danh sách payload cập nhật.</param>
    /// <returns>200 OK nếu thành công, 400 nếu dữ liệu rỗng.</returns>
    /// <Modified>
    /// Name       Date        Comments
    /// dungbt     6/26/2026   created
    /// </Modified>
    [HttpPut]
    public async Task<IActionResult> BatchUpdate([FromBody] List<UpdateDriverRequest> items)
    {
        if (items == null || items.Count == 0)
            return FailResponse("Danh sách cập nhật không được rỗng");

        var validationErrors = ValidateBatchUpdateItems(items);
        if (validationErrors.Count > 0)
            return FailResponse<DriverDto>("Dữ liệu cập nhật không hợp lệ", validationErrors);

        await _driverService.BatchUpdateAsync(items);
        return OkResponse("Cập nhật thành công");
    }

    /// <summary>
    /// Validates các dòng cập nhật lái xe trong danh sách items.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <returns>trả về Dictionary với key là tên thuộc tính và value là mảng lỗi tương ứng.</returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/26/2026 created
    /// </Modified>
    private static Dictionary<string, string[]> ValidateBatchUpdateItems(IReadOnlyList<UpdateDriverRequest> items)
    {
        var errors = new Dictionary<string, List<string>>();

        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item == null)
            {
                AddError(errors, $"items[{i}]", "Dữ liệu dòng cập nhật không được rỗng.");
                continue;
            }

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(item);
            Validator.TryValidateObject(item, context, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                var memberNames = validationResult.MemberNames?.Any() == true
                    ? validationResult.MemberNames
                    : new[] { string.Empty };

                foreach (var memberName in memberNames)
                {
                    var key = string.IsNullOrWhiteSpace(memberName)
                        ? $"items[{i}]"
                        : $"items[{i}].{memberName}";

                    AddError(errors, key, validationResult.ErrorMessage ?? "Dữ liệu không hợp lệ.");
                }
            }
        }

        return errors.ToDictionary(pair => pair.Key, pair => pair.Value.Distinct().ToArray());
    }

    /// <summary>
    /// Thêm một lỗi vào dictionary errors với key và message tương ứng.
    /// </summary>
    /// <param name="errors">The errors.</param>
    /// <param name="key">The key.</param>
    /// <param name="message">The message.</param>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/26/2026 created
    /// </Modified>
    private static void AddError(Dictionary<string, List<string>> errors, string key, string message)
    {
        if (!errors.TryGetValue(key, out var messages))
        {
            messages = new List<string>();
            errors[key] = messages;
        }

        messages.Add(message);
    }


    /// <summary>
    /// Xóa mềm lái xe theo ID (set IsDeleted = 1).
    /// Chỉ xóa được lái xe thuộc CompanyId = 15076.
    /// </summary>
    /// <param name="id">ID của lái xe cần xóa.</param>
    /// <returns>200 OK nếu thành công, 404 nếu không tìm thấy.</returns>
    /// <Modified>
    /// Name       Date        Comments
    /// dungbt     6/26/2026   created
    /// </Modified>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var (success, error) = await _driverService.SoftDeleteAsync(id);
        if (!success)
            return FailResponse(error ?? "Xóa thất bại", 404);

        return OkResponse("Xóa lái xe thành công");
    }


    /// <summary>
    /// Xuất danh sách lái xe ra file Excel (.xlsx) với EPPlus.
    /// File bao gồm tiêu đề, thông tin bộ lọc và dữ liệu đầy đủ (không phân trang).
    /// </summary>
    /// <param name="request">Tham số lọc (không có phân trang).</param>
    /// <returns>FileStreamResult với content type application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.</returns>
    /// <Modified>
    /// Name       Date        Comments
    /// dungbt     6/26/2026   created
    /// </Modified>
    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] DriverExportRequest request)
    {
        var fileResult = await _driverService.ExportExcelAsync(request);
        return fileResult;
    }

    /// <summary>
    /// Lấy thông tin chi tiết lái xe theo ID.
    /// </summary>
    /// <param name="id">ID của lái xe cần lấy.</param>
    /// <returns>Thông tin lái xe (DriverDto).</returns>
    /// <Modified>
    /// Name       Date        Comments
    /// dungbt     6/26/2026   created
    /// </Modified>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _driverService.GetByIdAsync(id);
        if (result == null)
            return FailResponse("Không tìm thấy lái xe", 404);

        return OkResponse(result);
    }
}
