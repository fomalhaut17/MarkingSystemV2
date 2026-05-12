using System.Text.Json;
using System.Text.Json.Serialization;
using MarkingSystemV2.Models;

namespace MarkingSystemV2.Services;

public sealed class MarkingApiService
{
    private readonly ApiClient   _client;
    private readonly ApiSettings _settings;

    public MarkingApiService(ApiClient client, ApiSettings settings)
    {
        _client   = client;
        _settings = settings;
    }

    // ── 물류 바코드 조회 ───────────────────────────────────────────────────────

    public async Task<(BarcodeInfo? info, string? error)> LookupByBarcodeAsync(string barcode)
    {
        var body = new LookupRequest(barcode, _settings.LoginCompany);
        var resp = await _client.PostAsync<LookupRequest, LookupResponse>(
            "/api/mantec/lot/lookup_by_barcode", body);

        if (resp == null)
            return (null, "서버 연결에 실패했습니다.");

        if (resp.BarcodeInfo == null || string.IsNullOrWhiteSpace(resp.BarcodeInfo.ItemName))
            return (null, "조회 결과가 없습니다.");

        return (resp.BarcodeInfo, null);
    }

    // ── 검사 결과 저장 ─────────────────────────────────────────────────────────

    public async Task<string?> SaveInspectionAsync(
        string barcode, string lotNoHead, int startSer, int endSer)
    {
        var items = Enumerable.Range(startSer, endSer - startSer + 1)
            .Select(ser => new InspectionItem(
                barcode,
                lotNoHead + ser.ToString("D6"),
                "PASS"))
            .ToList();

        var body = new SaveInspectionRequest(items, _settings.LoginCompany);
        var resp = await _client.PostAsync<SaveInspectionRequest, ApiResultResponse>(
            "/api/mantec/lot/save_inspection", body);

        if (resp == null)
            return "서버 연결에 실패했습니다.";

        if (!resp.Ok)
            return MapSaveErrorCode(resp.Message);

        return null;
    }

    // ── 생산 Lot 조회 ─────────────────────────────────────────────────────────

    public async Task<(LotContextInfo? context, InjectionCondition? condition,
                       InjectionCondition? defaults, string? error)>
        LookupByLotAsync(string barcode)
    {
        var body = new LotRequest(barcode, _settings.LoginCompany);
        var resp = await _client.PostAsync<LotRequest, LotApiResponse>(
            "/api/mantec/lot/latest_inject_by_barcode", body);

        if (resp == null)
            return (null, null, null, "서버 연결에 실패했습니다.");

        if (resp.LotContext == null || string.IsNullOrWhiteSpace(resp.LotContext.Bno))
            return (null, null, null, "조회 결과가 없습니다.");

        var condition = resp.Main?.Count > 0 ? resp.Main[0] : null;
        return (resp.LotContext, condition, resp.InjectDefaults, null);
    }

    private static string MapSaveErrorCode(string? code) => code switch
    {
        "MISSING_OR_INVALID_ITEMS" => "저장 항목이 올바르지 않습니다.",
        "EMPTY_ITEMS"              => "저장할 항목이 없습니다.",
        "SERVICE_NOT_AVAILABLE"    => "서버 오류가 발생했습니다. 잠시 후 다시 시도하세요.",
        null or "" or "OK"         => "저장에 실패했습니다.",
        _                          => $"저장에 실패했습니다. ({code})"
    };

    // ── DTOs ──────────────────────────────────────────────────────────────────

    private sealed record LookupRequest(
        [property: JsonPropertyName("barcode")]       string Barcode,
        [property: JsonPropertyName("login_company")] string LoginCompany);

    private sealed record LotRequest(
        [property: JsonPropertyName("barcode")]       string Barcode,
        [property: JsonPropertyName("login_company")] string LoginCompany);

    private sealed class LotApiResponse
    {
        [JsonPropertyName("LOT_CONTEXT")]      public LotContextInfo?            LotContext     { get; init; }
        [JsonPropertyName("MAIN")]             public List<InjectionCondition>?  Main           { get; init; }
        [JsonPropertyName("INJECT_DEFAULTS")]  public InjectionCondition?        InjectDefaults { get; init; }
        [JsonPropertyName("INJECT_RN_LABELS")] public Dictionary<string,string>? InjectRnLabels { get; init; }
    }

    private sealed class LookupResponse
    {
        [JsonPropertyName("barcode_info")] public BarcodeInfo?        BarcodeInfo { get; init; }
        [JsonPropertyName("lot_no_list")]  public List<LotNoListItem>? LotNoList   { get; init; }
    }

    private sealed record InspectionItem(
        [property: JsonPropertyName("barcode")]          string Barcode,
        [property: JsonPropertyName("lot_no")]           string LotNo,
        [property: JsonPropertyName("insp_result_code")] string InspResultCode);

    private sealed record SaveInspectionRequest(
        [property: JsonPropertyName("inspection_list")] IReadOnlyList<InspectionItem> InspectionList,
        [property: JsonPropertyName("login_company")]   string LoginCompany);

    private sealed class ApiResultResponse
    {
        [JsonPropertyName("ok")]             public bool                  Ok             { get; init; }
        [JsonPropertyName("message")]        public string?               Message        { get; init; }
        [JsonPropertyName("requestedCount")] public int?                  RequestedCount { get; init; }
        [JsonPropertyName("savedCount")]     public int?                  SavedCount     { get; init; }
        [JsonPropertyName("resultList")]     public List<JsonElement>?    ResultList     { get; init; }
    }
}
