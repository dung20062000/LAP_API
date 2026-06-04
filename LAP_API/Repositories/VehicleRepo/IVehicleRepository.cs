using LAP_API.Models;

namespace LAP_API.Repositories.VehicleRepo;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetByGroupIdsAsync(IEnumerable<int> groupIds);
    Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync();
}
