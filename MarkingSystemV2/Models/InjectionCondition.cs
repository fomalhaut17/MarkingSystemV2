using System.Text.Json.Serialization;

namespace MarkingSystemV2.Models;

public sealed class InjectionCondition
{
    // ── 바코드·검사 헤더 ───────────────────────────────────────────────────────
    public string? Barcode     { get; init; }
    public string? BcItemcod   { get; init; }
    public string? BcBno       { get; init; }
    public string? BcPdType    { get; init; }
    public string? BcMechcod   { get; init; }
    public string? BcProdate   { get; init; }
    public string? BcProtime   { get; init; }
    public string? BcWrkpart   { get; init; }
    public string? CompanyCd   { get; init; }
    public string? Chkdocno    { get; init; }
    public string? WorkDay     { get; init; }
    public string? ChkNo       { get; init; }
    public string? FmeCd       { get; init; }
    public string? WorkPart    { get; init; }
    public string? CheckTime   { get; init; }
    public string? Mechcod     { get; init; }
    public string? KumCod      { get; init; }
    public string? GongCo      { get; init; }

    // ── 사출조건 ─ 온도/압력/속도/시간 ─────────────────────────────────────────
    // INJECT_H{N}: H와 숫자 사이에 underscore 없음 (백엔드 예외 패턴)
    [JsonPropertyName("INJECT_H1")] public string? InjectH1 { get; init; }
    [JsonPropertyName("INJECT_H2")] public string? InjectH2 { get; init; }
    [JsonPropertyName("INJECT_H3")] public string? InjectH3 { get; init; }
    [JsonPropertyName("INJECT_H4")] public string? InjectH4 { get; init; }
    [JsonPropertyName("INJECT_H5")] public string? InjectH5 { get; init; }

    public string? InjectPress1 { get; init; }
    public string? InjectPress2 { get; init; }
    public string? InjectPress3 { get; init; }
    public string? InjectPress4 { get; init; }
    public string? InjectPress5 { get; init; }

    public string? InjectSpeed1 { get; init; }
    public string? InjectSpeed2 { get; init; }
    public string? InjectSpeed3 { get; init; }
    public string? InjectSpeed4 { get; init; }
    public string? InjectSpeed5 { get; init; }

    public string? InjectTime1 { get; init; }
    public string? InjectTime2 { get; init; }
    public string? InjectTime3 { get; init; }
    public string? InjectTime4 { get; init; }
    public string? InjectTime5 { get; init; }

    // ── 계량조건 ─ 압력/속도/위치 ──────────────────────────────────────────────
    public string? GuagePress1 { get; init; }
    public string? GuagePress2 { get; init; }
    public string? GuagePress3 { get; init; }
    public string? GuagePress4 { get; init; }
    public string? GuagePress5 { get; init; }

    public string? GuageSpeed1 { get; init; }
    public string? GuageSpeed2 { get; init; }
    public string? GuageSpeed3 { get; init; }
    public string? GuageSpeed4 { get; init; }
    public string? GuageSpeed5 { get; init; }

    public string? GuagePosit1 { get; init; }
    public string? GuagePosit2 { get; init; }
    public string? GuagePosit3 { get; init; }
    public string? GuagePosit4 { get; init; }
    public string? GuagePosit5 { get; init; }

    // ── 가스조건 ─ 딜레이/시간/압력/위치(POSIT) ────────────────────────────────
    public string? GasDelay1 { get; init; }
    public string? GasDelay2 { get; init; }
    public string? GasDelay3 { get; init; }
    public string? GasDelay4 { get; init; }
    public string? GasDelay5 { get; init; }

    public string? GasTime1 { get; init; }
    public string? GasTime2 { get; init; }
    public string? GasTime3 { get; init; }
    public string? GasTime4 { get; init; }
    public string? GasTime5 { get; init; }

    public string? GasPress1 { get; init; }
    public string? GasPress2 { get; init; }
    public string? GasPress3 { get; init; }
    public string? GasPress4 { get; init; }
    public string? GasPress5 { get; init; }

    public string? GasPosit1 { get; init; }
    public string? GasPosit2 { get; init; }
    public string? GasPosit3 { get; init; }
    public string? GasPosit4 { get; init; }
    public string? GasPosit5 { get; init; }

    // ── 보압조건 ─ 압력/속도/시간 ──────────────────────────────────────────────
    public string? HoldPress1 { get; init; }
    public string? HoldPress2 { get; init; }
    public string? HoldPress3 { get; init; }
    public string? HoldPress4 { get; init; }
    public string? HoldPress5 { get; init; }

    public string? HoldSpeed1 { get; init; }
    public string? HoldSpeed2 { get; init; }
    public string? HoldSpeed3 { get; init; }
    public string? HoldSpeed4 { get; init; }
    public string? HoldSpeed5 { get; init; }

    public string? HoldTime1 { get; init; }
    public string? HoldTime2 { get; init; }
    public string? HoldTime3 { get; init; }
    public string? HoldTime4 { get; init; }
    public string? HoldTime5 { get; init; }

    // ── HOT RUNNER A/B ─────────────────────────────────────────────────────────
    public string? HotRunnerA1 { get; init; }
    public string? HotRunnerA2 { get; init; }
    public string? HotRunnerA3 { get; init; }
    public string? HotRunnerA4 { get; init; }
    public string? HotRunnerA5 { get; init; }

    public string? HotRunnerB1 { get; init; }
    public string? HotRunnerB2 { get; init; }
    public string? HotRunnerB3 { get; init; }
    public string? HotRunnerB4 { get; init; }
    public string? HotRunnerB5 { get; init; }

    // ── 냉각시간 ───────────────────────────────────────────────────────────────
    public string? CoolTime1 { get; init; }
    public string? CoolTime2 { get; init; }
    public string? CoolTime3 { get; init; }
    public string? CoolTime4 { get; init; }
    public string? CoolTime5 { get; init; }

    // ── 금형보호 ───────────────────────────────────────────────────────────────
    public string? KumProtect1 { get; init; }
    public string? KumProtect2 { get; init; }
    public string? KumProtect3 { get; init; }
    public string? KumProtect4 { get; init; }
    public string? KumProtect5 { get; init; }

    // ── 가스위치 (DB 철자 POSTION) ─────────────────────────────────────────────
    public string? GasPostion1 { get; init; }
    public string? GasPostion2 { get; init; }
    public string? GasPostion3 { get; init; }
    public string? GasPostion4 { get; init; }
    public string? GasPostion5 { get; init; }

    // ── 감사 ───────────────────────────────────────────────────────────────────
    public string? UserId  { get; init; }
    public string? CreDate { get; init; }
    public string? UpDate  { get; init; }
}
