using System.Text.Json.Serialization;

namespace MarkingSystemV2.Models;

public sealed class LotContextInfo
{
    [JsonPropertyName("bno")]              public string? Bno            { get; init; }
    [JsonPropertyName("carnam")]           public string? CarName        { get; init; }
    [JsonPropertyName("itemnam")]          public string? ItemName       { get; init; }
    [JsonPropertyName("itemcod")]          public string? ItemCode       { get; init; }
    [JsonPropertyName("rw_mat_itemnam")]   public string? MaterialName   { get; init; }
    [JsonPropertyName("rw_mat_gr_nm")]     public string? Grade          { get; init; }
    [JsonPropertyName("pro_date")]         public string? ProductionDate { get; init; }
    [JsonPropertyName("pro_mechnam")]      public string? InjectionEquip { get; init; }
    [JsonPropertyName("engrave_pro_date")] public string? MarkingDate    { get; init; }
    [JsonPropertyName("engrave_mechnam")]  public string? MarkingEquip   { get; init; }
}
