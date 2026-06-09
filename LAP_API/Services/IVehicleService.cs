using LAP_API.DTOs.Vehicle;

namespace LAP_API.Services;


/// <summary>
/// Interface định nghĩa các nghiệp vụ liên quan đến xe.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
public interface IVehicleService
{
    Task<List<VehicleGroupTreeDto>> GetGroupsTreeAsync();
    Task<List<VehicleDto>> GetVehiclesByGroupIdsAsync(List<int> groupIds);
    Task<(ImageSearchResponse? response, string? error)> SearchImagesAsync(ImageSearchRequest request);
}
