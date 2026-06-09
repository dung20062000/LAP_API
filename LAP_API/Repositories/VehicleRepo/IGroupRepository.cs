using LAP_API.Models;

namespace LAP_API.Repositories.VehicleRepo;


/// <summary>
/// Interface định nghĩa các thao tác truy xuất dữ liệu liên quan đến nhóm xe.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
public interface IGroupRepository
{
    Task<IEnumerable<Group>> GetAllActiveAsync();
    Task<Dictionary<int, int>> GetVehicleCountByGroupIdsAsync(IEnumerable<int> groupIds);
}
