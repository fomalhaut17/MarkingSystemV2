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
    private string? _carModel;
    private string? _itemName;
    private string? _itemCode;
    private string? _materialName;
    private string? _grade;

    // 생산이력
    private string? _productionDate;
    private string? _injectionEquip;
    private string? _markingDate;
    private string? _markingEquip;

    // 사출조건
    private ConditionRow _rowInjectTemp     = Empty();
    private ConditionRow _rowInjectEmpty    = Empty();
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
    public string? CarModel     { get => _carModel;     private set => SetField(ref _carModel, value); }
    public string? ItemName     { get => _itemName;     private set => SetField(ref _itemName, value); }
    public string? ItemCode     { get => _itemCode;     private set => SetField(ref _itemCode, value); }
    public string? MaterialName { get => _materialName; private set => SetField(ref _materialName, value); }
    public string? Grade        { get => _grade;        private set => SetField(ref _grade, value); }

    // 생산이력
    public string? ProductionDate { get => _productionDate; private set => SetField(ref _productionDate, value); }
    public string? InjectionEquip { get => _injectionEquip; private set => SetField(ref _injectionEquip, value); }
    public string? MarkingDate    { get => _markingDate;    private set => SetField(ref _markingDate, value); }
    public string? MarkingEquip   { get => _markingEquip;   private set => SetField(ref _markingEquip, value); }

    // 사출조건
    public ConditionRow RowInjectTemp     { get => _rowInjectTemp;     private set => SetField(ref _rowInjectTemp, value); }
    public ConditionRow RowInjectEmpty    { get => _rowInjectEmpty;    private set => SetField(ref _rowInjectEmpty, value); }
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

    public AsyncRelayCommand QueryCommand { get; }

    private async Task ExecuteQueryAsync()
    {
        if (string.IsNullOrWhiteSpace(LotNo)) return;

        StatusMessage = null;
        IsError       = false;
        IsBusy        = true;

        var (context, condition, defaults, error) = await _api.LookupByLotAsync(LotNo.Trim());

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
    }

    private void ApplyContext(LotContextInfo ctx)
    {
        CarModel      = ctx.CarName;
        ItemName      = ctx.ItemName;
        ItemCode      = ctx.ItemCode;
        MaterialName  = ctx.MaterialName;
        Grade         = ctx.Grade;
        ProductionDate = ctx.ProductionDate;
        InjectionEquip = ctx.InjectionEquip;
        MarkingDate   = ctx.MarkingDate;
        MarkingEquip  = ctx.MarkingEquip;
    }

    private void ApplyCondition(InjectionCondition? c, InjectionCondition? d)
    {
        RowInjectTemp     = Row(d?.InjectH1,     d?.InjectH2,     d?.InjectH3,     d?.InjectH4,     d?.InjectH5,
                                c?.InjectH1,     c?.InjectH2,     c?.InjectH3,     c?.InjectH4,     c?.InjectH5);
        RowInjectEmpty    = Empty();
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
        CarModel = ItemName = ItemCode = MaterialName = Grade = null;
        ProductionDate = InjectionEquip = MarkingDate = MarkingEquip = null;
        ApplyCondition(null, null);
    }

    private static ConditionRow Empty() => new();

    private static ConditionRow Row(string? s1, string? s2, string? s3, string? s4, string? s5,
                                    string? v1, string? v2, string? v3, string? v4, string? v5) =>
        new() { Std1 = s1, Std2 = s2, Std3 = s3, Std4 = s4, Std5 = s5,
                Val1 = v1, Val2 = v2, Val3 = v3, Val4 = v4, Val5 = v5 };
}
