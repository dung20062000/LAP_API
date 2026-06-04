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
    [JsonPropertyName("channel")]
    public int Channel { get; set; }

    [JsonPropertyName("imageTime")]
    public DateTime ImageTime { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
}
