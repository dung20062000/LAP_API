using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LAP_API.DTOs.UserVehicleGroup;

/// <summary>
/// Request body cho API lưu gán nhóm xe cho người dùng.
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
public class AssignGroupsRequest
{
    /// <summary>
    /// Danh sách ID nhóm xe cần gán (danh sách ID đang được gán sau khi người dùng thao tác).
    /// </summary>
    [Required]
    [JsonPropertyName("groupIds")]
    public List<int> GroupIds { get; set; } = new();
}
