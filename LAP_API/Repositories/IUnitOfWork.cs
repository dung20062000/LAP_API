using LAP_API.Repositories.UserVehicleGroupRepo;
using LAP_API.Repositories.VehicleRepo;

namespace LAP_API.Repositories;

/// <summary>
/// IUnitOfWork định nghĩa các repository mà UnitOfWork sẽ quản lý và phương thức để lưu các thay đổi vào cơ sở dữ liệu. 
/// Nó cũng kế thừa IDisposable để đảm bảo rằng tài nguyên được giải phóng đúng cách khi không còn sử dụng nữa.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/11/2026 created
/// </Modified>
/// <seealso cref="System.IDisposable" />
public interface IUnitOfWork : IDisposable
{
    IVehicleRepository Vehicles { get; }
    IGroupRepository Groups { get; }
    IUserVehicleGroupRepository UserVehicleGroups { get; }
    Task<int> SaveChangesAsync();
}
