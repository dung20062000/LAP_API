using LAP_API.Data;
using LAP_API.Repositories.VehicleRepo;

namespace LAP_API.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Vehicles = new VehicleRepository(context);
        Groups = new GroupRepository(context);
    }

    public IVehicleRepository Vehicles { get; }
    public IGroupRepository Groups { get; }

    public Task<int> SaveChangesAsync()
        => _context.SaveChangesAsync();

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
