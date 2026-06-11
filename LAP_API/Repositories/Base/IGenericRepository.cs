namespace LAP_API.Repositories;

/// <summary>
/// IGenericRepository định nghĩa các phương thức CRUD cơ bản cho một thực thể bất kỳ.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <Modified>
/// Name Date Comments
/// dungbt 6/11/2026 created
/// </Modified>
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}
