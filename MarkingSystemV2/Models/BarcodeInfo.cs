using System.Text.Json.Serialization;

namespace MarkingSystemV2.Models;

public sealed class BarcodeInfo
{
    [JsonPropertyName("itemnam")]        public string? ItemName       { get; init; }
    [JsonPropertyName("itemcod")]        public string? ItemCode       { get; init; }
    [JsonPropertyName("end_i_day")]      public string? EndIDay        { get; init; }
    [JsonPropertyName("end_i_cnt")]      public string? EndICnt        { get; init; }
    [JsonPropertyName("lot_no_head")]    public string? LotNoHead      { get; init; }
    [JsonPropertyName("lot_min")]        public string? LotMin         { get; init; }
    [JsonPropertyName("lot_max")]        public string? LotMax         { get; init; }
    [JsonPropertyName("lot_cnt")]        public int?    LotCnt         { get; init; }
    [JsonPropertyName("lot_pass_cnt")]   public int?    LotPassCnt     { get; init; }
    [JsonPropertyName("lot_fail_cnt")]   public int?    LotFailCnt     { get; init; }
    [JsonPropertyName("bno")]            public string? Bno            { get; init; }
    [JsonPropertyName("pro_carcodenam")] public string? CarCodeName    { get; init; }
    [JsonPropertyName("pro_colornam")]   public string? ColorName      { get; init; }
}
