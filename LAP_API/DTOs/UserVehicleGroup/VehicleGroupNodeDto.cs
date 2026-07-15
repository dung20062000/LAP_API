using System.Text.Json.Serialization;

namespace LAP_API.DTOs.UserVehicleGroup;

/// <summary>
/// DTO đại diện cho một nút trong cây nhóm xe (PrimeNG TreeNode).
/// Sử dụng chung cho cả danh sách nhóm đã gán và chưa gán.
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
public class VehicleGroupNodeDto
{
    /// <summary>
    /// Key của node (ID nhóm xe dưới dạng string, dùng cho PrimeNG).
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Nhãn hiển thị trên cây (tên nhóm xe).
    /// </summary>
    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// ID nhóm xe (số nguyên).
    /// </summary>
    [JsonPropertyName("data")]
    public int Data { get; set; }

    /// <summary>
    /// ID nhóm xe cha (null nếu là node gốc).
    /// </summary>
    [JsonPropertyName("parentId")]
    public int? ParentId { get; set; }

    /// <summary>
    /// Danh sách node con.
    /// </summary>
    [JsonPropertyName("children")]
    public List<VehicleGroupNodeDto> Children { get; set; } = new();

    /// <summary>
    /// Có phải là node lá hay không (không có con).
    /// </summary>
    [JsonPropertyName("leaf")]
    public bool Leaf => Children.Count == 0;
}
