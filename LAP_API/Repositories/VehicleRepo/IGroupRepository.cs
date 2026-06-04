using LAP_API.Models;

namespace LAP_API.Repositories.VehicleRepo;

public interface IGroupRepository
{
    Task<IEnumerable<Group>> GetAllActiveAsync();
    Task<Dictionary<int, int>> GetVehicleCountByGroupIdsAsync(IEnumerable<int> groupIds);
}
