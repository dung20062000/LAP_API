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
    /// <summary>
    /// Khóa chính của người dùng (GUID).
    /// </summary>
    [Key]
    [Column("PK_UserID")]
    public Guid Id { get; set; }

    /// <summary>
    /// ID công ty mà người dùng thuộc về.
    /// </summary>
    [Column("FK_CompanyID")]
    public int CompanyId { get; set; }

    /// <summary>
    /// Tên đăng nhập.
    /// </summary>
    [MaxLength(50)]
    [Column("Username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Tên hiển thị đầy đủ.
    /// </summary>
    [MaxLength(250)]
    [Column("Fullname")]
    public string Fullname { get; set; } = string.Empty;

    /// <summary>
    /// Loại người dùng: 0 = Normal, 1 = Administrator.
    /// </summary>
    [Column("UserType")]
    public byte UserType { get; set; }

    /// <summary>
    /// Trạng thái khóa tài khoản.
    /// </summary>
    [Column("IsLock")]
    public bool IsLock { get; set; }

    /// <summary>
    /// Trạng thái xóa mềm.
    /// </summary>
    [Column("IsDeleted")]
    public bool? IsDeleted { get; set; }
}
