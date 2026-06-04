using LAP_API.Repositories.VehicleRepo;

namespace LAP_API.Repositories;

public interface IUnitOfWork : IDisposable
{
    IVehicleRepository Vehicles { get; }
    IGroupRepository Groups { get; }
    Task<int> SaveChangesAsync();
}
