namespace MarkingSystemV2.Models;

public sealed class LotContextInfo
{
    // ── 기본 ───────────────────────────────────────────────────────────────────
    public string? Bno            { get; init; }
    public string? StoreBarcode   { get; init; }
    public string? Itemnam        { get; init; }
    public string? Itemcod        { get; init; }
    public string? Carnam         { get; init; }
    public string? Carcode        { get; init; }
    public string? StrFromBarcode { get; init; }
    public string? ProDate        { get; init; }
    public string? ProMechnam     { get; init; }
    public string? ProMechcod     { get; init; }
    public string? RwLinkNo       { get; init; }
    public string? RwBarcode      { get; init; }
    public string? ProLinkNo      { get; init; }

    // ── 원재료 스펙 (RWRAWN05 계열) ─────────────────────────────────────────────
    public string? RwMatItemcod   { get; init; }
    public string? RwMatItemnam   { get; init; }
    public string? RwMatQtNm      { get; init; }
    public string? RwMatGrNm      { get; init; }

    // ── 원자재 바코드 PRINT_LIST ───────────────────────────────────────────────
    public string? RwItemcod      { get; init; }
    public string? RwItemnam      { get; init; }
    public string? RwBno          { get; init; }
    public string? RwPdType       { get; init; }
    public string? RwGradeCd      { get; init; }
    public string? RwGradeNm      { get; init; }

    // ── 소모 연계 / 각인 ───────────────────────────────────────────────────────
    public string? ConsumeOriginBarcode { get; init; }
    public string? EngraveProDate       { get; init; }
    public string? EngraveMechcod       { get; init; }
    public string? EngraveMechnam       { get; init; }
}
