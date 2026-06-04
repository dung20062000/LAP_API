using LAP_API.DTOs.Vehicle;

namespace LAP_API.Services;

public interface IVehicleService
{
    Task<List<VehicleGroupTreeDto>> GetGroupsTreeAsync();
    Task<List<VehicleDto>> GetVehiclesByGroupIdsAsync(List<int> groupIds);
    Task<(ImageSearchResponse? response, string? error)> SearchImagesAsync(ImageSearchRequest request);
}
