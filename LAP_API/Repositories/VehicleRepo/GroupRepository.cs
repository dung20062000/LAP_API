using LAP_API.Data;
using LAP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LAP_API.Repositories.VehicleRepo;


/// <summary>
/// Repository quản lý các truy vấn liên quan đến bảng Group trong cơ sở dữ liệu.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
/// <seealso cref="LAP_API.Repositories.VehicleRepo.IGroupRepository" />
public class GroupRepository : IGroupRepository
{
    private readonly ApplicationDbContext _context;

    // fix cứng CompanyId vì hiện tại chỉ có 1 công ty, sau này nếu có nhiều công ty thì sẽ cần thay đổi
    private const int CompanyId = 15076;

    public GroupRepository(ApplicationDbContext context)
        => _context = context;


    /// <summary>
    /// Truy vấn danh sách nhóm hoạt động.
    /// Lọc theo CompanyId và trạng thái Status/IsDeleted.
    /// </summary>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
    /// <exception cref="System.Exception">Lỗi khi lấy danh sách nhóm: {ex.Message}</exception>
    public async Task<IEnumerable<Group>> GetAllActiveAsync()
    {
        try
        {
            return await _context.Groups
                .AsNoTracking()
                .Where(g => g.Status && g.IsDeleted != true && g.CompanyId == CompanyId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy danh sách nhóm: {ex.Message}", ex);
        }
    }


    /// <summary>
    /// Đếm số lượng xe cho từng nhóm ID được truyền vào.
    /// Thực hiện Join giữa VehicleGroups và Vehicles để đảm bảo xe còn tồn tại và không bị khóa.
    /// </summary>
    /// <param name="groupIds">The group ids.</param>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
    /// <exception cref="System.Exception">Lỗi khi đếm xe theo nhóm: {ex.Message}</exception>
    public async Task<Dictionary<int, int>> GetVehicleCountByGroupIdsAsync(IEnumerable<int> groupIds)
    {
        try
        {
            var list = groupIds.ToList();
            var result = await _context.VehicleGroups
                .AsNoTracking()
                // Lọc VehicleGroups theo danh sách ID và CompanyId
                .Where(vg => list.Contains(vg.GroupId) && vg.IsDeleted != true && vg.CompanyId == CompanyId)
                .Join(
                    // Chỉ join với các xe đang hoạt động
                    _context.Vehicles.Where(v =>
                        !v.IsLocked && v.IsDeleted != true && v.CompanyId == CompanyId),
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
