using LAP_API.Data;
using LAP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LAP_API.Repositories.VehicleRepo;


/// <summary>
/// Repository quản lý các truy vấn liên quan đến xe và mối quan hệ xe-nhóm.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
/// <seealso cref="LAP_API.Repositories.VehicleRepo.IVehicleRepository" />
public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;
    // Tạm thời fix cứng CompanyId cho phiên bản hiện tại
    private const int CompanyId = 15076;

    public VehicleRepository(ApplicationDbContext context)
        => _context = context;


    /// <summary>
    /// Lấy danh sách xe bằng cách join giữa bảng VehicleGroups và Vehicles.
    /// Chỉ lấy các xe không bị khóa, chưa bị xóa và thuộc đúng công ty.
    /// </summary>
    /// <param name="groupIds">danh sách id của nhóm xe.</param>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
    /// <exception cref="System.Exception">Lỗi khi lấy danh sách xe theo nhóm: {ex.Message}</exception>
    public async Task<IEnumerable<Vehicle>> GetByGroupIdsAsync(IEnumerable<int> groupIds)
    {
        try
        {
            var list = groupIds.ToList();
            return await _context.VehicleGroups
                .AsNoTracking()
                // Lọc các bản ghi quan hệ xe-nhóm
                .Where(vg => list.Contains(vg.GroupId) && vg.IsDeleted != true && vg.CompanyId == CompanyId)
                .Join(
                    // Join với bảng Vehicles để lấy thông tin chi tiết xe
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


    /// <summary>
    /// Lấy tất cả xe đang hoạt động trong hệ thống.
    /// </summary>
    /// <returns></returns>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
    /// <exception cref="System.Exception">Lỗi khi lấy danh sách xe: {ex.Message}</exception>
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
