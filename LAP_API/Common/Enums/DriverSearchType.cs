namespace LAP_API.Common.Enums;

/// <summary>
/// Enum tìm kiếm cho danh sách lái xe.
/// </summary>
/// <Modified>
/// Name       Date        Comments
/// dungbt     6/26/2026   created
/// </Modified>
public enum DriverSearchType : byte
{
    /// <summary>
    /// Tìm theo tên lái xe.
    /// </summary>
    Name = 0,

    /// <summary>
    /// Tìm theo số giấy phép lái xe.
    /// </summary>
    DriverLicense = 1
}
