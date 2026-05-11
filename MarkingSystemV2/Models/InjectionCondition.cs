using System.Text.Json.Serialization;

namespace MarkingSystemV2.Models;

public sealed class InjectionCondition
{
    // 사출조건 — 온도
    [JsonPropertyName("INJECT_H1")] public string? InjectH1 { get; init; }
    [JsonPropertyName("INJECT_H2")] public string? InjectH2 { get; init; }
    [JsonPropertyName("INJECT_H3")] public string? InjectH3 { get; init; }
    [JsonPropertyName("INJECT_H4")] public string? InjectH4 { get; init; }
    [JsonPropertyName("INJECT_H5")] public string? InjectH5 { get; init; }

    // 사출조건 — 압력
    [JsonPropertyName("INJECT_PRESS_1")] public string? InjectPress1 { get; init; }
    [JsonPropertyName("INJECT_PRESS_2")] public string? InjectPress2 { get; init; }
    [JsonPropertyName("INJECT_PRESS_3")] public string? InjectPress3 { get; init; }
    [JsonPropertyName("INJECT_PRESS_4")] public string? InjectPress4 { get; init; }
    [JsonPropertyName("INJECT_PRESS_5")] public string? InjectPress5 { get; init; }

    // 사출조건 — 속도
    [JsonPropertyName("INJECT_SPEED_1")] public string? InjectSpeed1 { get; init; }
    [JsonPropertyName("INJECT_SPEED_2")] public string? InjectSpeed2 { get; init; }
    [JsonPropertyName("INJECT_SPEED_3")] public string? InjectSpeed3 { get; init; }
    [JsonPropertyName("INJECT_SPEED_4")] public string? InjectSpeed4 { get; init; }
    [JsonPropertyName("INJECT_SPEED_5")] public string? InjectSpeed5 { get; init; }

    // 사출조건 — 시간
    [JsonPropertyName("INJECT_TIME_1")] public string? InjectTime1 { get; init; }
    [JsonPropertyName("INJECT_TIME_2")] public string? InjectTime2 { get; init; }
    [JsonPropertyName("INJECT_TIME_3")] public string? InjectTime3 { get; init; }
    [JsonPropertyName("INJECT_TIME_4")] public string? InjectTime4 { get; init; }
    [JsonPropertyName("INJECT_TIME_5")] public string? InjectTime5 { get; init; }

    // 계량조건 — 압력
    [JsonPropertyName("GUAGE_PRESS_1")] public string? GuagePress1 { get; init; }
    [JsonPropertyName("GUAGE_PRESS_2")] public string? GuagePress2 { get; init; }
    [JsonPropertyName("GUAGE_PRESS_3")] public string? GuagePress3 { get; init; }
    [JsonPropertyName("GUAGE_PRESS_4")] public string? GuagePress4 { get; init; }
    [JsonPropertyName("GUAGE_PRESS_5")] public string? GuagePress5 { get; init; }

    // 계량조건 — 속도
    [JsonPropertyName("GUAGE_SPEED_1")] public string? GuageSpeed1 { get; init; }
    [JsonPropertyName("GUAGE_SPEED_2")] public string? GuageSpeed2 { get; init; }
    [JsonPropertyName("GUAGE_SPEED_3")] public string? GuageSpeed3 { get; init; }
    [JsonPropertyName("GUAGE_SPEED_4")] public string? GuageSpeed4 { get; init; }
    [JsonPropertyName("GUAGE_SPEED_5")] public string? GuageSpeed5 { get; init; }

    // 계량조건 — 위치
    [JsonPropertyName("GUAGE_POSIT_1")] public string? GuagePosit1 { get; init; }
    [JsonPropertyName("GUAGE_POSIT_2")] public string? GuagePosit2 { get; init; }
    [JsonPropertyName("GUAGE_POSIT_3")] public string? GuagePosit3 { get; init; }
    [JsonPropertyName("GUAGE_POSIT_4")] public string? GuagePosit4 { get; init; }
    [JsonPropertyName("GUAGE_POSIT_5")] public string? GuagePosit5 { get; init; }

    // 가스조건 — 시간
    [JsonPropertyName("GAS_TIME_1")] public string? GasTime1 { get; init; }
    [JsonPropertyName("GAS_TIME_2")] public string? GasTime2 { get; init; }
    [JsonPropertyName("GAS_TIME_3")] public string? GasTime3 { get; init; }
    [JsonPropertyName("GAS_TIME_4")] public string? GasTime4 { get; init; }
    [JsonPropertyName("GAS_TIME_5")] public string? GasTime5 { get; init; }

    // 가스조건 — 압력
    [JsonPropertyName("GAS_PRESS_1")] public string? GasPress1 { get; init; }
    [JsonPropertyName("GAS_PRESS_2")] public string? GasPress2 { get; init; }
    [JsonPropertyName("GAS_PRESS_3")] public string? GasPress3 { get; init; }
    [JsonPropertyName("GAS_PRESS_4")] public string? GasPress4 { get; init; }
    [JsonPropertyName("GAS_PRESS_5")] public string? GasPress5 { get; init; }

    // 가스조건 — 위치 (POSIT)
    [JsonPropertyName("GAS_POSIT_1")] public string? GasPosit1 { get; init; }
    [JsonPropertyName("GAS_POSIT_2")] public string? GasPosit2 { get; init; }
    [JsonPropertyName("GAS_POSIT_3")] public string? GasPosit3 { get; init; }
    [JsonPropertyName("GAS_POSIT_4")] public string? GasPosit4 { get; init; }
    [JsonPropertyName("GAS_POSIT_5")] public string? GasPosit5 { get; init; }

    // 가스조건 — 가스위치 (DB 철자 오류: POSTION)
    [JsonPropertyName("GAS_POSTION_1")] public string? GasPostion1 { get; init; }
    [JsonPropertyName("GAS_POSTION_2")] public string? GasPostion2 { get; init; }
    [JsonPropertyName("GAS_POSTION_3")] public string? GasPostion3 { get; init; }
    [JsonPropertyName("GAS_POSTION_4")] public string? GasPostion4 { get; init; }
    [JsonPropertyName("GAS_POSTION_5")] public string? GasPostion5 { get; init; }

    // 보압조건 — 압력
    [JsonPropertyName("HOLD_PRESS_1")] public string? HoldPress1 { get; init; }
    [JsonPropertyName("HOLD_PRESS_2")] public string? HoldPress2 { get; init; }
    [JsonPropertyName("HOLD_PRESS_3")] public string? HoldPress3 { get; init; }
    [JsonPropertyName("HOLD_PRESS_4")] public string? HoldPress4 { get; init; }
    [JsonPropertyName("HOLD_PRESS_5")] public string? HoldPress5 { get; init; }

    // 보압조건 — 속도
    [JsonPropertyName("HOLD_SPEED_1")] public string? HoldSpeed1 { get; init; }
    [JsonPropertyName("HOLD_SPEED_2")] public string? HoldSpeed2 { get; init; }
    [JsonPropertyName("HOLD_SPEED_3")] public string? HoldSpeed3 { get; init; }
    [JsonPropertyName("HOLD_SPEED_4")] public string? HoldSpeed4 { get; init; }
    [JsonPropertyName("HOLD_SPEED_5")] public string? HoldSpeed5 { get; init; }

    // 보압조건 — 시간
    [JsonPropertyName("HOLD_TIME_1")] public string? HoldTime1 { get; init; }
    [JsonPropertyName("HOLD_TIME_2")] public string? HoldTime2 { get; init; }
    [JsonPropertyName("HOLD_TIME_3")] public string? HoldTime3 { get; init; }
    [JsonPropertyName("HOLD_TIME_4")] public string? HoldTime4 { get; init; }
    [JsonPropertyName("HOLD_TIME_5")] public string? HoldTime5 { get; init; }

    // HOT RUNNER — A
    [JsonPropertyName("HOT_RUNNER_A_1")] public string? HotRunnerA1 { get; init; }
    [JsonPropertyName("HOT_RUNNER_A_2")] public string? HotRunnerA2 { get; init; }
    [JsonPropertyName("HOT_RUNNER_A_3")] public string? HotRunnerA3 { get; init; }
    [JsonPropertyName("HOT_RUNNER_A_4")] public string? HotRunnerA4 { get; init; }
    [JsonPropertyName("HOT_RUNNER_A_5")] public string? HotRunnerA5 { get; init; }

    // HOT RUNNER — B
    [JsonPropertyName("HOT_RUNNER_B_1")] public string? HotRunnerB1 { get; init; }
    [JsonPropertyName("HOT_RUNNER_B_2")] public string? HotRunnerB2 { get; init; }
    [JsonPropertyName("HOT_RUNNER_B_3")] public string? HotRunnerB3 { get; init; }
    [JsonPropertyName("HOT_RUNNER_B_4")] public string? HotRunnerB4 { get; init; }
    [JsonPropertyName("HOT_RUNNER_B_5")] public string? HotRunnerB5 { get; init; }

    // 냉각시간
    [JsonPropertyName("COOL_TIME_1")] public string? CoolTime1 { get; init; }
    [JsonPropertyName("COOL_TIME_2")] public string? CoolTime2 { get; init; }
    [JsonPropertyName("COOL_TIME_3")] public string? CoolTime3 { get; init; }
    [JsonPropertyName("COOL_TIME_4")] public string? CoolTime4 { get; init; }
    [JsonPropertyName("COOL_TIME_5")] public string? CoolTime5 { get; init; }

    // 금형보호
    [JsonPropertyName("KUM_PROTECT_1")] public string? KumProtect1 { get; init; }
    [JsonPropertyName("KUM_PROTECT_2")] public string? KumProtect2 { get; init; }
    [JsonPropertyName("KUM_PROTECT_3")] public string? KumProtect3 { get; init; }
    [JsonPropertyName("KUM_PROTECT_4")] public string? KumProtect4 { get; init; }
    [JsonPropertyName("KUM_PROTECT_5")] public string? KumProtect5 { get; init; }
}
