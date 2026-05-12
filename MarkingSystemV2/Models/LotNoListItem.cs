using System.Text.Json.Serialization;

namespace MarkingSystemV2.Models;

public sealed class LotNoListItem
{
    [JsonPropertyName("LOT_NO")]           public string? LotNo          { get; init; }
    [JsonPropertyName("BARCODE")]          public string? Barcode        { get; init; }
    [JsonPropertyName("INSP_RESULT_CODE")] public string? InspResultCode { get; init; }
    [JsonPropertyName("CRE_DATE")]         public string? CreatedDate    { get; init; }
    [JsonPropertyName("UP_DATE")]          public string? UpdatedDate    { get; init; }
    [JsonPropertyName("USER_ID")]          public string? UserId         { get; init; }
    [JsonPropertyName("USER_NM")]          public string? UserName       { get; init; }
}
