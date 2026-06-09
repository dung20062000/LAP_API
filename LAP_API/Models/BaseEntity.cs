namespace LAP_API.Models;

/// <summary>
/// Lớp cơ sở cho các entity trong hệ thống, chứa các thuộc tính dùng chung.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
public abstract class BaseEntity
{
    /// Khóa chính (Primary Key).
    public int Id { get; set; }

    /// Trạng thái hoạt động của bản ghi.
    public bool IsActive { get; set; } = true;

    /// Thời điểm tạo bản ghi.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// Thời điểm cập nhật bản ghi gần nhất.
    public DateTime? UpdatedAt { get; set; }
}
