using LAP_API.Data;
using Microsoft.EntityFrameworkCore;

namespace LAP_API.Repositories;

/// <summary>
/// Triển khai repository cơ sở cung cấp các thao tác CRUD chuẩn
/// sử dụng Entity Framework Core. Tất cả repository cụ thể nên
/// kế thừa từ class này.
/// </summary>
/// <typeparam name="T">Loại thực thể quản lý bởi repository.</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    /// <summary>Instance của database context.</summary>
    protected readonly ApplicationDbContext _context;

    /// <summary>DbSet cho loại thực thể.</summary>
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// Khởi tạo một instance mới của GenericRepository.
    /// </summary>
    /// <param name="context">Database context.</param>
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <inheritdoc />
    /// dùng để lấy một thực thể theo ID. Trả về null nếu không tìm thấy.
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <inheritdoc />
    /// dùng để lấy tất cả thực thể. Trả về một danh sách rỗng nếu không có thực thể nào.
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    /// dùng để tìm kiếm thực thể theo một điều kiện cụ thể. Trả về một danh sách rỗng nếu không có thực thể nào
    public Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
    {
        return Task.FromResult(
            _dbSet.AsNoTracking().Where(predicate).AsEnumerable());
    }

    /// <inheritdoc />
    /// dùng để thêm một thực thể mới vào database. Trả về thực thể đã được thêm vào database, bao gồm ID nếu có.
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    /// dùng để cập nhật một thực thể đã tồn tại trong database. Trả về void.
    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    /// dùng để xóa một thực thể khỏi database dựa trên ID. Trả về void.
    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    /// dùng để đếm tổng số thực thể trong database. Trả về số lượng thực thể.
    public async Task<int> CountAsync()
    {
        return await _dbSet.AsNoTracking().CountAsync();
    }
}
