namespace MarkingSystemV2.Models;

public sealed class BarcodeInfo
{
    public string? Itemnam        { get; init; }
    public string? Itemcod        { get; init; }
    public string? EndIDay        { get; init; }
    public string? EndICnt        { get; init; }
    public string? LotNoHead      { get; init; }
    public string? LotMin         { get; init; }
    public string? LotMax         { get; init; }
    public int?    LotCnt         { get; init; }
    public int?    LotPassCnt     { get; init; }
    public int?    LotFailCnt     { get; init; }
    public string? Bno            { get; init; }
    public string? ProCarcodenam  { get; init; }
    public string? ProColornam    { get; init; }
}
