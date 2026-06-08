using LAP_API.Data;
using LAP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LAP_API.Repositories.VehicleRepo;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;
    private const int CompanyId = 15076;

    public VehicleRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<IEnumerable<Vehicle>> GetByGroupIdsAsync(IEnumerable<int> groupIds)
    {
        try
        {
            var list = groupIds.ToList();
            return await _context.VehicleGroups
                .AsNoTracking()
                .Where(vg => list.Contains(vg.GroupId) && vg.IsDeleted != true && vg.CompanyId == CompanyId)
                .Join(
                    _context.Vehicles.Where(v =>
                        !v.IsLocked && v.IsDeleted != true && v.CompanyId == CompanyId),
                    vg => vg.VehicleId,
                    v => v.Id,
                    (_, v) => v)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy danh sách xe theo nhóm: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync()
    {
        try
        {
            return await _context.Vehicles
                .AsNoTracking()
                .Where(v => !v.IsLocked && v.IsDeleted != true)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy danh sách xe: {ex.Message}", ex);
        }
    }
}
