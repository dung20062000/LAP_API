using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Vehicle;

public class ImageSearchResponse
{
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("items")]
    public List<ImageItemDto> Items { get; set; } = new();
}

public class ImageItemDto
{
    [JsonPropertyName("vehiclePlate")]
    public string VehiclePlate { get; set; } = string.Empty;

    [JsonPropertyName("imageTime")]
    public DateTime ImageTime { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("speed")]
    public int Speed { get; set; }

    [JsonPropertyName("channel")]
    public int Channel { get; set; }

    [JsonPropertyName("driverName")]
    public string DriverName { get; set; } = string.Empty;
}
