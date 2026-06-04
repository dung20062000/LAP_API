using LAP_API.Data;
using LAP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LAP_API.Repositories.VehicleRepo;

public class GroupRepository : IGroupRepository
{
    private readonly ApplicationDbContext _context;

    public GroupRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<IEnumerable<Group>> GetAllActiveAsync()
    {
        try
        {
            return await _context.Groups
                .AsNoTracking()
                .Where(g => g.Status && g.IsDeleted != true)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy danh sách nhóm: {ex.Message}", ex);
        }
    }

    public async Task<Dictionary<int, int>> GetVehicleCountByGroupIdsAsync(IEnumerable<int> groupIds)
    {
        try
        {
            var list = groupIds.ToList();
            var result = await _context.VehicleGroups
                .AsNoTracking()
                .Where(vg => list.Contains(vg.GroupId) && vg.IsDeleted != true)
                .Join(
                    _context.Vehicles.Where(v =>
                        !v.IsLocked && v.IsDeleted != true),
                    vg => vg.VehicleId,
                    v => v.Id,
                    (vg, _) => vg.GroupId)
                .GroupBy(id => id)
                .Select(g => new { GroupId = g.Key, Count = g.Count() })
                .ToListAsync();

            return result.ToDictionary(x => x.GroupId, x => x.Count);
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi đếm xe theo nhóm: {ex.Message}", ex);
        }
    }
}
