using LAP_API.Data;
using LAP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LAP_API.Repositories.UserVehicleGroupRepo;

/// <summary>
/// Triển khai repository cho các thao tác truy xuất dữ liệu
/// liên quan đến Admin.Users và Admin.UserVehicleGroup.
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
public class UserVehicleGroupRepository : IUserVehicleGroupRepository
{
    private readonly ApplicationDbContext _context;

    // Fix cứng CompanyId vì hiện tại chỉ phục vụ 1 công ty
    private const int CompanyId = 15076;

    public UserVehicleGroupRepository(ApplicationDbContext context)
        => _context = context;


    /// <summary>
    /// Lấy danh sách người dùng đang hoạt động của công ty.
    /// Lọc theo CompanyId, IsLock = false, IsDeleted != true.
    /// Sắp xếp theo Fullname.
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        try
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u =>
                    u.CompanyId == CompanyId &&
                    !u.IsLock &&
                    u.IsDeleted != true)
                .OrderBy(u => u.Fullname)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy danh sách người dùng: {ex.Message}", ex);
        }
    }


    /// <summary>
    /// Lấy danh sách ID nhóm xe đã gán cho người dùng (không bị xóa mềm).
    /// </summary>
    /// <param name="userId">ID người dùng.</param>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    public async Task<IEnumerable<int>> GetAssignedGroupIdsAsync(Guid userId)
    {
        try
        {
            return await _context.UserVehicleGroups
                .AsNoTracking()
                .Where(uvg => uvg.UserId == userId && uvg.IsDeleted != true)
                .Select(uvg => uvg.VehicleGroupId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy nhóm xe đã gán: {ex.Message}", ex);
        }
    }


    /// <summary>
    /// Cập nhật danh sách gán nhóm xe cho người dùng theo chiến lược replace-all.
    /// - Soft-delete các bản ghi cũ không còn trong danh sách mới.
    /// - Thêm mới hoặc khôi phục (IsDeleted=false) các bản ghi còn thiếu.
    /// </summary>
    /// <param name="userId">ID người dùng.</param>
    /// <param name="groupIds">Danh sách ID nhóm xe sau khi cập nhật.</param>
    /// <param name="groups">Danh sách Group để tra cứu ParentVehicleGroupID.</param>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    public async Task UpdateAssignedGroupsAsync(Guid userId, List<int> groupIds, IEnumerable<Models.Group> groups)
    {
        try
        {
            var now = DateTime.Now;

            // Lấy tất cả bản ghi hiện tại của user (kể cả đã xóa mềm)
            var existing = await _context.UserVehicleGroups
                .Where(uvg => uvg.UserId == userId)
                .ToListAsync();

            // dùng GroupBy + ToDictionary để tránh lỗi duplicate key nếu có nhiều bản ghi cùng VehicleGroupId (do xóa mềm) -> crash trương trình
            var existingDict = existing.GroupBy(uvg => uvg.VehicleGroupId).ToDictionary(g => g.Key, g => g.First());
            var groupDict = groups.ToDictionary(g => g.Id);

            // Soft-delete các nhóm không còn trong danh sách mới
            var toRemove = existing
                .Where(uvg => uvg.IsDeleted != true && !groupIds.Contains(uvg.VehicleGroupId))
                .ToList();

            foreach (var uvg in toRemove)
            {
                uvg.IsDeleted = true;
                uvg.UpdatedDate = now;
            }

            // Thêm mới hoặc khôi phục các nhóm trong danh sách mới
            foreach (var groupId in groupIds)
            {
                if (existingDict.TryGetValue(groupId, out var existingRecord))
                {
                    // Khôi phục nếu đang bị xóa mềm
                    if (existingRecord.IsDeleted == true)
                    {
                        existingRecord.IsDeleted = false;
                        existingRecord.UpdatedDate = now;
                        existingRecord.ParentVehicleGroupId = groupDict.TryGetValue(groupId, out var g)
                            ? g.ParentVehicleGroupID
                            : null;
                    }
                }
                else
                {
                    // Thêm mới
                    var parentId = groupDict.TryGetValue(groupId, out var grp)
                        ? grp.ParentVehicleGroupID
                        : null;

                    _context.UserVehicleGroups.Add(new UserVehicleGroup
                    {
                        UserId = userId,
                        VehicleGroupId = groupId,
                        ParentVehicleGroupId = parentId,
                        CreatedDate = now,
                        UpdatedDate = now,
                        IsDeleted = false
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi cập nhật gán nhóm xe: {ex.Message}", ex);
        }
    }
}
