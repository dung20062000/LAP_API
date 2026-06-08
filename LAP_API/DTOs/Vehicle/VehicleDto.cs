using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Vehicle;

public class VehicleDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("vehiclePlate")]
    public string VehiclePlate { get; set; } = string.Empty;

    [JsonPropertyName("XNCode")]
    public int XNCode { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

}
