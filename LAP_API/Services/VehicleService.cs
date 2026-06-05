using LAP_API.DTOs.Vehicle;
using LAP_API.Repositories;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace LAP_API.Services;

/// <summary>
/// 
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/4/2026 created
/// </Modified>
/// <seealso cref="LAP_API.Services.BaseService" />
/// <seealso cref="LAP_API.Services.IVehicleService" />
public class VehicleService : BaseService, IVehicleService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const int CompanyId = 15076;
    private const int MaxImageSearchDays = 30;

    public VehicleService(
        IUnitOfWork unitOfWork,
        IHttpClientFactory httpClientFactory,
        ILogger<VehicleService> logger)
        : base(unitOfWork, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<VehicleGroupTreeDto>> GetGroupsTreeAsync()
    {
        var groups = (await _unitOfWork.Groups.GetAllActiveAsync()).ToList();
        if (!groups.Any())
            return new List<VehicleGroupTreeDto>();

        var vehicleCounts = await _unitOfWork.Groups
            .GetVehicleCountByGroupIdsAsync(groups.Select(g => g.Id));

        return groups.Select(g => new VehicleGroupTreeDto
        {
            Key = g.Id.ToString(),
            Label = $"{g.GroupName} ({vehicleCounts.GetValueOrDefault(g.Id, 0)})",
            Data = g.Id.ToString(),
        }).ToList();
    }

    public async Task<List<VehicleDto>> GetVehiclesByGroupIdsAsync(List<int> groupIds)
    {
        var vehicles = groupIds.Count == 0
            ? await _unitOfWork.Vehicles.GetActiveVehiclesAsync()
            : await _unitOfWork.Vehicles.GetByGroupIdsAsync(groupIds);

        return vehicles.Select(v => new VehicleDto
        {
            Id = v.Id,
            VehiclePlate = v.VehiclePlate,
            PrivateCode = v.PrivateCode,
            DisplayName = v.PrivateCode != v.VehiclePlate
                ? $"{v.PrivateCode} ({v.VehiclePlate})"
                : v.VehiclePlate,
        }).ToList();
    }

    public async Task<(ImageSearchResponse? response, string? error)> SearchImagesAsync(
        ImageSearchRequest request)
    {
        // Validation
        if (request.StartTime >= request.EndTime)
            return (null, "StartTime phải nhỏ hơn EndTime");

        if ((request.EndTime - request.StartTime).TotalDays > MaxImageSearchDays)
            return (null, $"Khoảng thời gian không được vượt quá {MaxImageSearchDays} ngày");

        if (request.EndTime > DateTime.UtcNow)
            return (null, "EndTime không được vượt quá ngày hiện tại");

        var pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
        var pageSize = request.PageSize > 0 ? request.PageSize : 20;

        var searchBody = new
        {
            CustomerId = request.CustomerId,
            VehicleName = request.VehiclePlate,
            Channels = request.Channels,
            StartTime = request.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            EndTime = request.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            Frequency = 5,
            StorageTime = 90,
        };

        try
        {
            var client = _httpClientFactory.CreateClient();
            var content = JsonContent.Create(searchBody);

            var response = await client.PostAsync(
                "http://centraldb.bagroup.io/api/v2/images/ImagesFrequency",
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
            else if (doc.RootElement.TryGetProperty("items", out dataEl))
                arr = dataEl;
            else if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                arr = doc.RootElement;

            if (arr.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var item in arr.EnumerateArray())
                {
                    items.Add(new ImageItemDto
                    {
                        // Mapping short keys from API response: k -> channel, c -> time, u -> url
                        Channel = item.TryGetProperty("k", out var ch) ? ch.GetInt32() : 0,
                        ImageTime = item.TryGetProperty("c", out var it) ? it.GetDateTime() : DateTime.MinValue,
                        Url = item.TryGetProperty("u", out var u) ? u.GetString() ?? "" : "",
                        Latitude = item.TryGetProperty("lat", out var lat) ? lat.GetDouble() : null,
                        Longitude = item.TryGetProperty("lng", out var lng) ? lng.GetDouble() : null,
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
