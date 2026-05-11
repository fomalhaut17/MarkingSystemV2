namespace MarkingSystemV2.Models;

public sealed class ConditionRow
{
    public bool    IsHeader { get; init; }
    public string  Label    { get; init; } = "";
    public string  Unit     { get; init; } = "";
    public string? Std1     { get; init; }
    public string? Val1     { get; init; }
    public string? Std2     { get; init; }
    public string? Val2     { get; init; }
    public string? Std3     { get; init; }
    public string? Val3     { get; init; }
    public string? Std4     { get; init; }
    public string? Val4     { get; init; }
    public string? Std5     { get; init; }
    public string? Val5     { get; init; }
}
