using LAP_API.DTOs.Vehicle;
using LAP_API.Repositories;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace LAP_API.Services;

/// <summary>
///     lấy thông tin xe và nhóm xe, đồng thời gọi API lấy ảnh từ hệ thống bên thứ 3
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/4/2026 create
/// </Modified>
/// <seealso cref="LAP_API.Services.BaseService" />
/// <seealso cref="LAP_API.Services.IVehicleService" />
public class VehicleService : BaseService, IVehicleService
{
    private readonly IHttpClientFactory _httpClientFactory;
    // Tạm thời fix cứng CompanyId cho phiên bản hiện tại
    private const int CompanyId = 15076;
    // Giới hạn tối đa số ngày có thể tìm kiếm ảnh để tránh tải quá nhiều dữ liệu từ hệ thống bên thứ 3
    private const int MaxImageSearchDays = 30;

    public VehicleService(
        IUnitOfWork unitOfWork,
        IHttpClientFactory httpClientFactory,
        ILogger<VehicleService> logger)
        : base(unitOfWork, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// lấy danh sách tất cả nhóm xe dưới dạng cây PrimeNG TreeNode, mỗi nút bao gồm tên nhóm và số lượng xe đang hoạt động
    /// </summary>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
    public async Task<List<VehicleGroupTreeDto>> GetGroupsTreeAsync()
    {
        //Lấy tất cả nhóm đang hoạt động
        var groups = (await _unitOfWork.Groups.GetAllActiveAsync()).ToList();
        if (!groups.Any())
            return new List<VehicleGroupTreeDto>();

        //Lấy số lượng xe trực tiếp của từng nhóm
        var vehicleCounts = await _unitOfWork.Groups
            .GetVehicleCountByGroupIdsAsync(groups.Select(g => g.Id));

        //Xây dựng bản đồ cha-con để duyệt nhanh
        var childrenMap = groups
            .Where(g => g.ParentVehicleGroupID.HasValue)
            .GroupBy(g => g.ParentVehicleGroupID!.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        var resolvedCounts = new Dictionary<int, int>();
        var visited = new HashSet<int>();

        // Hàm đệ quy để tính tổng số xe của một nhóm (bao gồm tất cả con cháu)
        int GetRecursiveCount(int groupId)
        {
            if (resolvedCounts.TryGetValue(groupId, out var cachedCount))
                return cachedCount;

            // Chống lặp vô hạn nếu dữ liệu có vòng (circular dependency)
            if (!visited.Add(groupId))
                return 0;

            int count = 0;

            if (childrenMap.TryGetValue(groupId, out var children))
            {
                // Nếu có con, tổng = tổng xe của các con
                foreach (var child in children)
                {
                    count += GetRecursiveCount(child.Id);
                }
            }
            else
            {
                // Nếu là nhóm lá, lấy số xe trực tiếp
                count = vehicleCounts.GetValueOrDefault(groupId, 0);
            }

            visited.Remove(groupId);
            resolvedCounts[groupId] = count;
            return count;
        }

        //Chuyển đổi list Groups sang Dictionary của DTOs
        var groupDict = groups.ToDictionary(
            g => g.Id,
            g => new VehicleGroupTreeDto
            {
                Key = g.Id.ToString(),
                Label = $"{g.GroupName} ({GetRecursiveCount(g.Id)})",
                Data = g.Id.ToString(),
                Children = new List<VehicleGroupTreeDto>()
            });

        var rootDtos = new List<VehicleGroupTreeDto>();

        //Tổ chức cấu trúc cây bằng cách gắn nút con vào nút cha tương ứng
        foreach (var g in groups)
        {
            var dto = groupDict[g.Id];
            if (g.ParentVehicleGroupID.HasValue && groupDict.TryGetValue(g.ParentVehicleGroupID.Value, out var parentDto))
            {
                parentDto.Children.Add(dto);
            }
            else
            {
                // Nếu không có cha (hoặc cha không nằm trong list hoạt động), coi là nút gốc
                rootDtos.Add(dto);
            }
        }

        return rootDtos;
    }

    /// <summary>
    /// Lấy danh sách xe đang hoạt động theo nhóm xe, nếu không truyền groupId nào thì trả về tất cả xe đang hoạt động
    /// </summary>
    /// <param name="groupIds">danh sách nhóm xe</param>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
    public async Task<List<VehicleDto>> GetVehiclesByGroupIdsAsync(List<int> groupIds)
    {
        var vehicles = groupIds.Count == 0
            ? await _unitOfWork.Vehicles.GetActiveVehiclesAsync()
            : await _unitOfWork.Vehicles.GetByGroupIdsAsync(groupIds);

        return vehicles
            .DistinctBy(v => v.Id)
            .Select(v => new VehicleDto
            {
                Id = v.Id,
                VehiclePlate = v.VehiclePlate,
                XNCode = v.XNCode,
                DisplayName = v.PrivateCode != v.VehiclePlate
                    ? $"{v.PrivateCode} ({v.VehiclePlate})"
                    : v.VehiclePlate,
            }).ToList();
    }

    /// <summary>
    /// Lấy thông tin ảnh của xe theo biển số, kênh, khoảng thời gian và phân trang, sắp xếp theo thời gian ảnh
    /// </summary>
    /// <param name="request">Thông tin biến param</param>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
    public async Task<(ImageSearchResponse? response, string? error)> SearchImagesAsync(
        ImageSearchRequest request)
    {
        // Validation
        if (request.StartTime > request.EndTime)
            return (null, "Giờ bắt đầu không được lớn hơn giờ kết thúc");

        if (request.StartTime > DateTime.Now)
            return (null, "Giờ bắt đầu không được lớn hơn thời gian hiện tại");

        if (request.StartTime < DateTime.Now.Date.AddDays(-MaxImageSearchDays))
            return (null, $"Thời gian chọn không được cách ngày hiện tại quá {MaxImageSearchDays} ngày");

        if ((request.EndTime - request.StartTime).TotalDays > MaxImageSearchDays)
            return (null, $"Khoảng thời gian không được vượt quá {MaxImageSearchDays} ngày");

        var pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
        var pageSize = request.PageSize > 0 ? request.PageSize : 20;

        var searchBody = new
        {
            CustomerId = request.CustomerId,
            VehicleName = request.VehiclePlate,
            Channels = request.Channels.Select(c => new
            {
                Channel = c,
                StorageTime = 90,
                Frequency = 5
            }).ToList(),
            StartTime = request.StartTime.ToString("yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
            EndTime = request.EndTime.ToString("yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
        };

        try
        {
            var client = _httpClientFactory.CreateClient();
            var content = JsonContent.Create(searchBody);

            var response = await client.PostAsync(
                "http://centraldb.bagroup.io/api/v3/images/ImagesFrequency",
                content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("External image API returned {StatusCode}: {Error}", response.StatusCode, errorContent);
                return (null, "Không thể lấy dữ liệu ảnh từ hệ thống");
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var items = new List<ImageItemDto>();

            System.Text.Json.JsonElement arr = default;
            if (doc.RootElement.TryGetProperty("data", out var dataEl))
                arr = dataEl;
            else if (doc.RootElement.TryGetProperty("Data", out dataEl))
                arr = dataEl;
            else if (doc.RootElement.TryGetProperty("items", out dataEl))
                arr = dataEl;
            else if (doc.RootElement.TryGetProperty("Items", out dataEl))
                arr = dataEl;
            else if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                arr = doc.RootElement;

            if (arr.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var item in arr.EnumerateArray())
                {
                    items.Add(new ImageItemDto
                    {
                        // Mapping short keys from API response
                        VehiclePlate = item.TryGetProperty("v", out var v) ? v.GetString() ?? "" : "",
                        ImageTime = item.TryGetProperty("c", out var it) ? it.GetDateTime() : DateTime.MinValue,
                        Url = item.TryGetProperty("u", out var u) ? u.GetString() ?? "" : "",
                        Speed = item.TryGetProperty("s", out var s) ? s.GetInt32() : 0,
                        Channel = item.TryGetProperty("k", out var ch) ? ch.GetInt32() : 0,
                        DriverName = item.TryGetProperty("n", out var n) ? n.GetString() ?? "" : "",
                    });
                }
            }

            var sorted = request.SortOrder == "asc"
                ? items.OrderBy(x => x.ImageTime)
                : items.OrderByDescending(x => x.ImageTime);

            return (new ImageSearchResponse
            {
                TotalCount = sorted.Count(),
                Items = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
            }, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling external image API");
            return (null, "Lỗi khi gọi API lấy ảnh");
        }
    }
}
