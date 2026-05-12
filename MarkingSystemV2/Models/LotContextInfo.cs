using System.Text.Json.Serialization;

namespace MarkingSystemV2.Models;

public sealed class LotContextInfo
{
    // ── 기본 ───────────────────────────────────────────────────────────────────
    [JsonPropertyName("bno")]              public string? Bno             { get; init; }
    [JsonPropertyName("store_barcode")]    public string? StoreBarcode    { get; init; }
    [JsonPropertyName("itemnam")]          public string? ItemName        { get; init; }
    [JsonPropertyName("itemcod")]          public string? ItemCode        { get; init; }
    [JsonPropertyName("carnam")]           public string? CarName         { get; init; }
    [JsonPropertyName("carcode")]          public string? CarCode         { get; init; }
    [JsonPropertyName("str_from_barcode")] public string? StrFromBarcode  { get; init; }
    [JsonPropertyName("pro_date")]         public string? ProductionDate  { get; init; }
    [JsonPropertyName("pro_mechnam")]      public string? InjectionEquip  { get; init; }
    [JsonPropertyName("pro_mechcod")]      public string? InjectionEquipCode { get; init; }
    [JsonPropertyName("rw_link_no")]       public string? RwLinkNo        { get; init; }
    [JsonPropertyName("rw_barcode")]       public string? RwBarcode       { get; init; }
    [JsonPropertyName("pro_link_no")]      public string? ProLinkNo       { get; init; }

    // ── 원재료 스펙 (RWRAWN05 계열) ─────────────────────────────────────────────
    [JsonPropertyName("rw_mat_itemcod")]   public string? MaterialItemCode { get; init; }
    [JsonPropertyName("rw_mat_itemnam")]   public string? MaterialName     { get; init; }
    [JsonPropertyName("rw_mat_qt_nm")]     public string? MaterialQtName   { get; init; }
    [JsonPropertyName("rw_mat_gr_nm")]     public string? Grade            { get; init; }

    // ── 원자재 바코드 PRINT_LIST ───────────────────────────────────────────────
    [JsonPropertyName("rw_itemcod")]       public string? RwItemCode      { get; init; }
    [JsonPropertyName("rw_itemnam")]       public string? RwItemName      { get; init; }
    [JsonPropertyName("rw_bno")]           public string? RwBno           { get; init; }
    [JsonPropertyName("rw_pd_type")]       public string? RwPdType        { get; init; }
    [JsonPropertyName("rw_grade_cd")]      public string? RwGradeCode     { get; init; }
    [JsonPropertyName("rw_grade_nm")]      public string? RwGradeName     { get; init; }

    // ── 소모 연계 / 각인 ───────────────────────────────────────────────────────
    [JsonPropertyName("consume_origin_barcode")] public string? ConsumeOriginBarcode { get; init; }
    [JsonPropertyName("engrave_pro_date")]       public string? MarkingDate          { get; init; }
    [JsonPropertyName("engrave_mechcod")]        public string? MarkingEquipCode     { get; init; }
    [JsonPropertyName("engrave_mechnam")]        public string? MarkingEquip         { get; init; }
}
