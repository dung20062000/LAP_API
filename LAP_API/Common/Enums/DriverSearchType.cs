namespace LAP_API.Common.Enums;

/// <summary>
/// Lo?i tìm ki?m cho danh sách lái xe.
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
    /// Tìm theo s? gi?y phép lái xe.
    /// </summary>
    DriverLicense = 1
}
