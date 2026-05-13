using System.Net;
using System.Text.RegularExpressions;
using MarkingSystemV2.Models;
using MarkingSystemV2.Services;

namespace MarkingSystemV2.ViewModels;

public sealed class LotInquiryViewModel : ObservableObject
{
    private readonly MarkingApiService _api;

    private string  _lotNo        = "";
    private bool    _isBusy       = false;
    private string? _statusMessage;
    private bool    _isError;

    // 제품정보
    private string? _carnam;
    private string? _itemnam;
    private string? _itemcod;
    private string? _rwMatItemnam;
    private string? _rwMatGrNm;

    // 생산이력
    private string? _proDate;
    private string? _proMechnam;
    private string? _engraveProDate;
    private string? _engraveMechnam;

    // 사출조건
    private ConditionRow _rowInjectTemp     = Empty();
    private ConditionRow _rowInjectPressure = Empty();
    private ConditionRow _rowInjectSpeed    = Empty();
    private ConditionRow _rowInjectTime     = Empty();

    // 계량조건
    private ConditionRow _rowGuagePressure  = Empty();
    private ConditionRow _rowGuageSpeed     = Empty();
    private ConditionRow _rowGuagePosit     = Empty();

    // 가스조건
    private ConditionRow _rowGasDelay       = Empty();
    private ConditionRow _rowGasTime        = Empty();
    private ConditionRow _rowGasPress       = Empty();
    private ConditionRow _rowGasPosit       = Empty();

    // 보압
    private ConditionRow _rowHoldPress      = Empty();
    private ConditionRow _rowHoldSpeed      = Empty();
    private ConditionRow _rowHoldTime       = Empty();

    // HOT RUNNER
    private ConditionRow _rowHotRunnerA     = Empty();
    private ConditionRow _rowHotRunnerB     = Empty();

    // 단일 행 그룹
    private ConditionRow _rowCoolTime       = Empty();
    private ConditionRow _rowKumProtect     = Empty();
    private ConditionRow _rowGasPostion     = Empty();

    // 라벨 (INJECT_RN_LABELS)
    private Dictionary<string,string>? _labels;

    public LotInquiryViewModel(MarkingApiService api)
    {
        _api         = api;
        QueryCommand = new AsyncRelayCommand(ExecuteQueryAsync,
            () => !IsBusy && !string.IsNullOrWhiteSpace(LotNo));
    }

    public string LotNo
    {
        get => _lotNo;
        set { if (SetField(ref _lotNo, value)) QueryCommand.Invalidate(); }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            SetField(ref _isBusy, value);
            QueryCommand.Invalidate();
            OnPropertyChanged(nameof(DisplayStatus));
        }
    }

    public string? StatusMessage
    {
        get => _statusMessage;
        private set
        {
            SetField(ref _statusMessage, value);
            OnPropertyChanged(nameof(DisplayStatus));
        }
    }

    public bool IsError
    {
        get => _isError;
        private set => SetField(ref _isError, value);
    }

    public string? DisplayStatus => IsBusy ? "처리 중..." : StatusMessage;

    // 제품정보
    public string? Carnam       { get => _carnam;       private set => SetField(ref _carnam, value); }
    public string? Itemnam      { get => _itemnam;      private set => SetField(ref _itemnam, value); }
    public string? Itemcod      { get => _itemcod;      private set => SetField(ref _itemcod, value); }
    public string? RwMatItemnam { get => _rwMatItemnam; private set => SetField(ref _rwMatItemnam, value); }
    public string? RwMatGrNm    { get => _rwMatGrNm;    private set => SetField(ref _rwMatGrNm, value); }

    // 생산이력
    public string? ProDate        { get => _proDate;        private set => SetField(ref _proDate, value); }
    public string? ProMechnam     { get => _proMechnam;     private set => SetField(ref _proMechnam, value); }
    public string? EngraveProDate { get => _engraveProDate; private set => SetField(ref _engraveProDate, value); }
    public string? EngraveMechnam { get => _engraveMechnam; private set => SetField(ref _engraveMechnam, value); }

    // 사출조건
    public ConditionRow RowInjectTemp     { get => _rowInjectTemp;     private set => SetField(ref _rowInjectTemp, value); }
    public ConditionRow RowInjectPressure { get => _rowInjectPressure; private set => SetField(ref _rowInjectPressure, value); }
    public ConditionRow RowInjectSpeed    { get => _rowInjectSpeed;    private set => SetField(ref _rowInjectSpeed, value); }
    public ConditionRow RowInjectTime     { get => _rowInjectTime;     private set => SetField(ref _rowInjectTime, value); }

    // 계량조건
    public ConditionRow RowGuagePressure  { get => _rowGuagePressure;  private set => SetField(ref _rowGuagePressure, value); }
    public ConditionRow RowGuageSpeed     { get => _rowGuageSpeed;     private set => SetField(ref _rowGuageSpeed, value); }
    public ConditionRow RowGuagePosit     { get => _rowGuagePosit;     private set => SetField(ref _rowGuagePosit, value); }

    // 가스조건
    public ConditionRow RowGasDelay       { get => _rowGasDelay;       private set => SetField(ref _rowGasDelay, value); }
    public ConditionRow RowGasTime        { get => _rowGasTime;        private set => SetField(ref _rowGasTime, value); }
    public ConditionRow RowGasPress       { get => _rowGasPress;       private set => SetField(ref _rowGasPress, value); }
    public ConditionRow RowGasPosit       { get => _rowGasPosit;       private set => SetField(ref _rowGasPosit, value); }

    // 보압
    public ConditionRow RowHoldPress      { get => _rowHoldPress;      private set => SetField(ref _rowHoldPress, value); }
    public ConditionRow RowHoldSpeed      { get => _rowHoldSpeed;      private set => SetField(ref _rowHoldSpeed, value); }
    public ConditionRow RowHoldTime       { get => _rowHoldTime;       private set => SetField(ref _rowHoldTime, value); }

    // HOT RUNNER
    public ConditionRow RowHotRunnerA     { get => _rowHotRunnerA;     private set => SetField(ref _rowHotRunnerA, value); }
    public ConditionRow RowHotRunnerB     { get => _rowHotRunnerB;     private set => SetField(ref _rowHotRunnerB, value); }

    // 단일 행 그룹
    public ConditionRow RowCoolTime       { get => _rowCoolTime;       private set => SetField(ref _rowCoolTime, value); }
    public ConditionRow RowKumProtect     { get => _rowKumProtect;     private set => SetField(ref _rowKumProtect, value); }
    public ConditionRow RowGasPostion     { get => _rowGasPostion;     private set => SetField(ref _rowGasPostion, value); }

    // ── 라벨 (INJECT_RN_LABELS 우선, 없으면 기본 표시문구) ────────────────────
    // 그룹 라벨
    public string LblInjectGroup     => Lookup("SA_INJECT",       "사출조건");
    public string LblGuageGroup      => Lookup("SA_GUAGE",        "계량조건");
    public string LblGasGroup        => Lookup("SA_GAS",          "가스조건");
    public string LblHoldGroup       => Lookup("SA_HOLD",         "보압");
    public string LblHotRunnerGroup  => Lookup("SA_HOT_RUNNER",   "HOT RUNNER");
    public string LblCoolTimeGroup   => Lookup("SA_COOL_TIME",    "냉각시간");
    public string LblKumProtectGroup => Lookup("SA_KUM_PROTECT",  "금형보호");
    public string LblGasPostionGroup => Lookup("SA_GAS_POSTION",  "가스위치");

    // 항목 라벨
    public string LblInjectTemp   => Lookup("SA_INJECT_H",     "온도(℃)");
    public string LblInjectPress  => Lookup("SA_INJECT_PRESS", "압력(Kg/cm²)");
    public string LblInjectSpeed  => Lookup("SA_INJECT_SPEED", "속도(%)");
    public string LblInjectTime   => Lookup("SA_INJECT_TIME",  "시간(초)");
    public string LblGuagePress   => Lookup("SA_GUAGE_PRESS",  "압력(Kg/cm²)");
    public string LblGuageSpeed   => Lookup("SA_GUAGE_SPEED",  "속도(%)");
    public string LblGuagePosit   => Lookup("SA_GUAGE_POSIT",  "위치(mm)");
    public string LblGasDelay     => Lookup("SA_GAS_DELAY",    "딜레이(초)");
    public string LblGasTime      => Lookup("SA_GAS_TIME",     "시간(초)");
    public string LblGasPress     => Lookup("SA_GAS_PRESS",    "압력(Kg/cm²)");
    public string LblGasPosit     => Lookup("SA_GAS_POSIT",    "위치(mm)");
    public string LblHoldPress    => Lookup("SA_HOLD_PRESS",   "압력(Kg/cm²)");
    public string LblHoldSpeed    => Lookup("SA_HOLD_SPEED",   "속도(%)");
    public string LblHoldTime     => Lookup("SA_HOLD_TIME",    "시간(초)");
    public string LblHotRunnerA   => Lookup("SA_HOT_RUNNER_A", "A");
    public string LblHotRunnerB   => Lookup("SA_HOT_RUNNER_B", "B");

    // 온도 행 컬럼 헤더 (서버 라벨 그대로)
    public string LblH1Header => Lookup("SA_H1", "H1");
    public string LblH2Header => Lookup("SA_H2", "H2");
    public string LblH3Header => Lookup("SA_H3", "H3");
    public string LblH4Header => Lookup("SA_H4", "H4");
    public string LblH5Header => Lookup("SA_H5", "H5");

    public AsyncRelayCommand QueryCommand { get; }

    private async Task ExecuteQueryAsync()
    {
        if (string.IsNullOrWhiteSpace(LotNo)) return;

        StatusMessage = null;
        IsError       = false;
        IsBusy        = true;

        var (context, condition, defaults, labels, error) = await _api.LookupByLotAsync(InputSanitizer.Clean(LotNo));

        IsBusy = false;

        if (error != null)
        {
            StatusMessage = error;
            IsError       = true;
            ClearResult();
            return;
        }

        ApplyContext(context!);
        ApplyCondition(condition, defaults);
        ApplyLabels(labels);
    }

    private void ApplyLabels(Dictionary<string,string>? labels)
    {
        _labels = labels;
        // 모든 Lbl* 프로퍼티 갱신
        OnPropertyChanged(string.Empty);
    }

    private string Lookup(string key, string fallback)
    {
        if (_labels != null && _labels.TryGetValue(key, out var v) && !string.IsNullOrWhiteSpace(v))
            return DecodeHtmlEntities(v);
        return fallback;
    }

    // &#37 (세미콜론 누락 케이스) 보정 후 표준 HtmlDecode
    private static readonly Regex MissingSemicolon = new(@"&#(\d+)(?!;)", RegexOptions.Compiled);
    private static string DecodeHtmlEntities(string s)
        => WebUtility.HtmlDecode(MissingSemicolon.Replace(s, "&#$1;"));

    private void ApplyContext(LotContextInfo ctx)
    {
        Carnam         = ctx.Carnam;
        Itemnam        = ctx.Itemnam;
        Itemcod        = ctx.Itemcod;
        RwMatItemnam   = ctx.RwMatItemnam;
        RwMatGrNm      = ctx.RwMatGrNm;
        ProDate        = ctx.ProDate;
        ProMechnam     = ctx.ProMechnam;
        EngraveProDate = ctx.EngraveProDate;
        EngraveMechnam = ctx.EngraveMechnam;
    }

    private void ApplyCondition(InjectionCondition? c, InjectionCondition? d)
    {
        RowInjectTemp     = Row(d?.InjectH1,     d?.InjectH2,     d?.InjectH3,     d?.InjectH4,     d?.InjectH5,
                                c?.InjectH1,     c?.InjectH2,     c?.InjectH3,     c?.InjectH4,     c?.InjectH5);
        RowInjectPressure = Row(d?.InjectPress1, d?.InjectPress2, d?.InjectPress3, d?.InjectPress4, d?.InjectPress5,
                                c?.InjectPress1, c?.InjectPress2, c?.InjectPress3, c?.InjectPress4, c?.InjectPress5);
        RowInjectSpeed    = Row(d?.InjectSpeed1, d?.InjectSpeed2, d?.InjectSpeed3, d?.InjectSpeed4, d?.InjectSpeed5,
                                c?.InjectSpeed1, c?.InjectSpeed2, c?.InjectSpeed3, c?.InjectSpeed4, c?.InjectSpeed5);
        RowInjectTime     = Row(d?.InjectTime1,  d?.InjectTime2,  d?.InjectTime3,  d?.InjectTime4,  d?.InjectTime5,
                                c?.InjectTime1,  c?.InjectTime2,  c?.InjectTime3,  c?.InjectTime4,  c?.InjectTime5);

        RowGuagePressure  = Row(d?.GuagePress1,  d?.GuagePress2,  d?.GuagePress3,  d?.GuagePress4,  d?.GuagePress5,
                                c?.GuagePress1,  c?.GuagePress2,  c?.GuagePress3,  c?.GuagePress4,  c?.GuagePress5);
        RowGuageSpeed     = Row(d?.GuageSpeed1,  d?.GuageSpeed2,  d?.GuageSpeed3,  d?.GuageSpeed4,  d?.GuageSpeed5,
                                c?.GuageSpeed1,  c?.GuageSpeed2,  c?.GuageSpeed3,  c?.GuageSpeed4,  c?.GuageSpeed5);
        RowGuagePosit     = Row(d?.GuagePosit1,  d?.GuagePosit2,  d?.GuagePosit3,  d?.GuagePosit4,  d?.GuagePosit5,
                                c?.GuagePosit1,  c?.GuagePosit2,  c?.GuagePosit3,  c?.GuagePosit4,  c?.GuagePosit5);

        RowGasDelay       = Row(d?.GasDelay1,    d?.GasDelay2,    d?.GasDelay3,    d?.GasDelay4,    d?.GasDelay5,
                                c?.GasDelay1,    c?.GasDelay2,    c?.GasDelay3,    c?.GasDelay4,    c?.GasDelay5);
        RowGasTime        = Row(d?.GasTime1,     d?.GasTime2,     d?.GasTime3,     d?.GasTime4,     d?.GasTime5,
                                c?.GasTime1,     c?.GasTime2,     c?.GasTime3,     c?.GasTime4,     c?.GasTime5);
        RowGasPress       = Row(d?.GasPress1,    d?.GasPress2,    d?.GasPress3,    d?.GasPress4,    d?.GasPress5,
                                c?.GasPress1,    c?.GasPress2,    c?.GasPress3,    c?.GasPress4,    c?.GasPress5);
        RowGasPosit       = Row(d?.GasPosit1,    d?.GasPosit2,    d?.GasPosit3,    d?.GasPosit4,    d?.GasPosit5,
                                c?.GasPosit1,    c?.GasPosit2,    c?.GasPosit3,    c?.GasPosit4,    c?.GasPosit5);

        RowHoldPress      = Row(d?.HoldPress1,   d?.HoldPress2,   d?.HoldPress3,   d?.HoldPress4,   d?.HoldPress5,
                                c?.HoldPress1,   c?.HoldPress2,   c?.HoldPress3,   c?.HoldPress4,   c?.HoldPress5);
        RowHoldSpeed      = Row(d?.HoldSpeed1,   d?.HoldSpeed2,   d?.HoldSpeed3,   d?.HoldSpeed4,   d?.HoldSpeed5,
                                c?.HoldSpeed1,   c?.HoldSpeed2,   c?.HoldSpeed3,   c?.HoldSpeed4,   c?.HoldSpeed5);
        RowHoldTime       = Row(d?.HoldTime1,    d?.HoldTime2,    d?.HoldTime3,    d?.HoldTime4,    d?.HoldTime5,
                                c?.HoldTime1,    c?.HoldTime2,    c?.HoldTime3,    c?.HoldTime4,    c?.HoldTime5);

        RowHotRunnerA     = Row(d?.HotRunnerA1,  d?.HotRunnerA2,  d?.HotRunnerA3,  d?.HotRunnerA4,  d?.HotRunnerA5,
                                c?.HotRunnerA1,  c?.HotRunnerA2,  c?.HotRunnerA3,  c?.HotRunnerA4,  c?.HotRunnerA5);
        RowHotRunnerB     = Row(d?.HotRunnerB1,  d?.HotRunnerB2,  d?.HotRunnerB3,  d?.HotRunnerB4,  d?.HotRunnerB5,
                                c?.HotRunnerB1,  c?.HotRunnerB2,  c?.HotRunnerB3,  c?.HotRunnerB4,  c?.HotRunnerB5);

        RowCoolTime       = Row(d?.CoolTime1,    d?.CoolTime2,    d?.CoolTime3,    d?.CoolTime4,    d?.CoolTime5,
                                c?.CoolTime1,    c?.CoolTime2,    c?.CoolTime3,    c?.CoolTime4,    c?.CoolTime5);
        RowKumProtect     = Row(d?.KumProtect1,  d?.KumProtect2,  d?.KumProtect3,  d?.KumProtect4,  d?.KumProtect5,
                                c?.KumProtect1,  c?.KumProtect2,  c?.KumProtect3,  c?.KumProtect4,  c?.KumProtect5);
        RowGasPostion     = Row(d?.GasPostion1,  d?.GasPostion2,  d?.GasPostion3,  d?.GasPostion4,  d?.GasPostion5,
                                c?.GasPostion1,  c?.GasPostion2,  c?.GasPostion3,  c?.GasPostion4,  c?.GasPostion5);
    }

    private void ClearResult()
    {
        Carnam = Itemnam = Itemcod = RwMatItemnam = RwMatGrNm = null;
        ProDate = ProMechnam = EngraveProDate = EngraveMechnam = null;
        ApplyCondition(null, null);
        ApplyLabels(null);
    }

    private static ConditionRow Empty() => new();

    private static ConditionRow Row(string? s1, string? s2, string? s3, string? s4, string? s5,
                                    string? v1, string? v2, string? v3, string? v4, string? v5) =>
        new() { Std1 = s1, Std2 = s2, Std3 = s3, Std4 = s4, Std5 = s5,
                Val1 = v1, Val2 = v2, Val3 = v3, Val4 = v4, Val5 = v5 };
}
