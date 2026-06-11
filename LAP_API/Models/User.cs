using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAP_API.Models;

/// <summary>
/// Entity đại diện cho bảng người dùng (Admin.Users) trong database.
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
[Table("Admin.Users")]
public class User
{
    /// Khóa chính của người dùng (GUID).
    [Key]
    [Column("PK_UserID")]
    public Guid Id { get; set; }

    /// ID công ty mà người dùng thuộc về.
    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    /// Tên đăng nhập.
    [MaxLength(50)]
    [Column("Username")]
    public string Username { get; set; } = string.Empty;

    /// Tên hiển thị đầy đủ.
    [MaxLength(250)]
    [Column("Fullname")]
    public string Fullname { get; set; } = string.Empty;

    /// Loại người dùng: 0 = Normal, 1 = Administrator.
    [Column("UserType")]
    public byte UserType { get; set; }

    /// Trạng thái khóa tài khoản.
    [Column("IsLock")]
    public bool IsLock { get; set; }

    /// Trạng thái xóa mềm.
    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }

    /// Trạng thái kích hoạt.
    [Column("IsActived")]
    public bool IsActived { get; set; }

    /// Email người dùng.
    [MaxLength(50)]
    [Column("Email")]
    public string? Email { get; set; }

    /// Số điện thoại.
    [MaxLength(50)]
    [Column("PhoneNumber")]
    public string? PhoneNumber { get; set; }

    /// Thời điểm đăng nhập gần nhất.
    [Column("LastLoginDate")]
    public DateTime? LastLoginDate { get; set; }

    /// Ngày tạo tài khoản.
    [Column("CreatedDate")]
    public DateTime? CreatedDate { get; set; }
}
