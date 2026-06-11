using LAP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LAP_API.Data;

/// <summary>
/// Lớp ApplicationDbContext đại diện cho ngữ cảnh dữ liệu của ứng dụng, 
/// quản lý kết nối đến cơ sở dữ liệu và cung cấp các DbSet để truy xuất và thao tác với dữ liệu của các thực thể như Vehicle, Group, và VehicleGroup.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/11/2026 created
/// </Modified>
/// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<VehicleGroup> VehicleGroups => Set<VehicleGroup>();

    /// <summary>
    /// Override phương thức này để cấu hình mô hình dữ liệu của bạn. Bạn có thể sử dụng Fluent API để thiết lập các ràng buộc, quan hệ, và các thuộc tính khác cho các thực thể của mình.
    /// </summary>
    /// <param name="modelBuilder">Trình xây dựng được sử dụng để xây dựng mô hình cho ngữ cảnh này. Cơ sở dữ liệu (và các phần mở rộng khác) thường
    /// định nghĩa các phương thức mở rộng trên đối tượng này cho phép bạn cấu hình các khía cạnh của mô hình cụ thể
    /// cho một cơ sở dữ liệu nhất định.</param>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/11/2026 created
    /// </Modified>
    /// <remarks>
    /// <para>
    /// Nếu một mô hình được thiết lập rõ ràng trong các tùy chọn cho ngữ cảnh này 
    /// (thông qua <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
    /// thì phương thức này sẽ không được chạy. Tuy nhiên, nó vẫn sẽ chạy khi tạo một mô hình đã biên dịch.
    /// </para>
    /// <para>
    /// Xem <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> để biết thêm thông tin và ví dụ.
    /// </para>
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VehicleGroup>(e =>
        {
            e.HasKey(x => new { x.GroupId, x.VehicleId });
        });
    }
}
