using LAP_API.Data;
using LAP_API.Repositories.VehicleRepo;

namespace LAP_API.Repositories;

/// <summary>
/// Lớp UnitOfWork quản lý các repository và đảm bảo rằng tất cả các thao tác liên quan đến dữ liệu được thực hiện trong một đơn vị công việc duy nhất.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/11/2026 created
/// </Modified>
/// <seealso cref="LAP_API.Repositories.IUnitOfWork" />
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Vehicles = new VehicleRepository(context);
        Groups = new GroupRepository(context);
    }

    /// Các repository được quản lý bởi UnitOfWork.
    public IVehicleRepository Vehicles { get; }
    public IGroupRepository Groups { get; }

    /// Lưu tất cả các thay đổi vào cơ sở dữ liệu.
    public Task<int> SaveChangesAsync()
        => _context.SaveChangesAsync();

    /// <summary>
    /// dùng để giải phóng tài nguyên của DbContext khi UnitOfWork không còn được sử dụng nữa.
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/11/2026 created
    /// </Modified>
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
