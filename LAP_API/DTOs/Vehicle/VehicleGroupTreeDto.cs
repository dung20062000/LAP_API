using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Vehicle;

public class VehicleGroupTreeDto
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public string Data { get; set; } = string.Empty;

    [JsonPropertyName("children")]
    public List<VehicleGroupTreeDto> Children { get; set; } = new();
}
