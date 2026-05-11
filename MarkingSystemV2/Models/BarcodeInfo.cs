using System.Text.Json.Serialization;

namespace MarkingSystemV2.Models;

public sealed class BarcodeInfo
{
    [JsonPropertyName("itemnam")]    public string? ItemName  { get; init; }
    [JsonPropertyName("end_i_day")] public string? EndIDay   { get; init; }
    [JsonPropertyName("end_i_cnt")] public string? EndICnt   { get; init; }
    [JsonPropertyName("lot_no_head")] public string? LotNoHead { get; init; }
    [JsonPropertyName("lot_max")]   public string? LotMax    { get; init; }
}
