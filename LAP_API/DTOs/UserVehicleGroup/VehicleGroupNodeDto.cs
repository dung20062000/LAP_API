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
    /// Key của node (ID nhóm xe dưới dạng string, dùng cho PrimeNG).
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    /// Nhãn hiển thị trên cây (tên nhóm xe).
    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    /// ID nhóm xe (số nguyên).
    [JsonPropertyName("data")]
    public int Data { get; set; }

    /// ID nhóm xe cha (null nếu là node gốc).
    [JsonPropertyName("parentId")]
    public int? ParentId { get; set; }

    /// Danh sách node con.
    [JsonPropertyName("children")]
    public List<VehicleGroupNodeDto> Children { get; set; } = new();

    /// Có phải là node lá hay không (không có con).
    [JsonPropertyName("leaf")]
    public bool Leaf => Children.Count == 0;
}
