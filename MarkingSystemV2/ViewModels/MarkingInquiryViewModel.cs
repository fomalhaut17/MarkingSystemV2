using System.Windows;
using MarkingSystemV2.Models;
using MarkingSystemV2.Services;

namespace MarkingSystemV2.ViewModels;

public sealed class MarkingInquiryViewModel : ObservableObject
{
    private readonly MarkingApiService _api;

    private string  _barcode        = "";
    private bool    _isBusy         = false;
    private string? _statusMessage;
    private bool    _isError        = false;

    private BarcodeInfo? _barcodeInfo;
    private string? _itemnam;
    private string? _endIDay;
    private string? _endICnt;
    private string? _lotNoHead;
    private string? _lotMax;
    private string? _startSerial;
    private int     _startSerialInt;
    private string  _endSerial = "";

    public MarkingInquiryViewModel(MarkingApiService api)
    {
        _api = api;

        QueryCommand        = new AsyncRelayCommand(ExecuteQueryAsync,
            () => !IsBusy && !string.IsNullOrWhiteSpace(Barcode));

        SaveCommand         = new AsyncRelayCommand(ExecuteSaveAsync,
            () => !IsBusy && _barcodeInfo != null && IsEndSerialValid());

        CopyLotNoHeadCommand    = new RelayCommand(
            () => TrySetClipboard(LotNoHead),
            () => !string.IsNullOrEmpty(LotNoHead));

        CopyStartSerialCommand  = new RelayCommand(
            () => TrySetClipboard(StartSerial),
            () => !string.IsNullOrEmpty(StartSerial));
    }

    public string Barcode
    {
        get => _barcode;
        set
        {
            if (SetField(ref _barcode, value))
                QueryCommand.Invalidate();
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            SetField(ref _isBusy, value);
            QueryCommand.Invalidate();
            SaveCommand.Invalidate();
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

    public string? Itemnam
    {
        get => _itemnam;
        private set => SetField(ref _itemnam, value);
    }

    public string? EndIDay
    {
        get => _endIDay;
        private set => SetField(ref _endIDay, value);
    }

    public string? EndICnt
    {
        get => _endICnt;
        private set => SetField(ref _endICnt, value);
    }

    public string? LotNoHead
    {
        get => _lotNoHead;
        private set
        {
            SetField(ref _lotNoHead, value);
            CopyLotNoHeadCommand.Invalidate();
        }
    }

    public string? LotMax
    {
        get => _lotMax;
        private set => SetField(ref _lotMax, value);
    }

    public string? StartSerial
    {
        get => _startSerial;
        private set
        {
            SetField(ref _startSerial, value);
            CopyStartSerialCommand.Invalidate();
        }
    }

    public bool HasResult => _barcodeInfo != null;

    public string EndSerial
    {
        get => _endSerial;
        set
        {
            if (SetField(ref _endSerial, value))
                SaveCommand.Invalidate();
        }
    }

    public AsyncRelayCommand QueryCommand       { get; }
    public AsyncRelayCommand SaveCommand        { get; }
    public RelayCommand      CopyLotNoHeadCommand   { get; }
    public RelayCommand      CopyStartSerialCommand { get; }

    public event Action? RequestBarcodeFocus;

    // ── 조회 ──────────────────────────────────────────────────────────────────

    private async Task ExecuteQueryAsync()
    {
        StatusMessage = null;
        IsError       = false;
        IsBusy        = true;

        ClearResult();

        var (info, error) = await _api.LookupByBarcodeAsync(Barcode.Trim());

        IsBusy = false;

        if (error != null)
        {
            StatusMessage = error;
            IsError       = true;
            return;
        }

        ApplyResult(info!);
    }

    private void ApplyResult(BarcodeInfo info)
    {
        _barcodeInfo    = info;
        Itemnam         = info.Itemnam;
        EndIDay         = info.EndIDay;
        EndICnt         = info.EndICnt;
        LotNoHead       = info.LotNoHead?.TrimStart();
        LotMax          = info.LotMax;

        int currentMax  = int.TryParse(info.LotMax, out var v) ? v : 0;
        _startSerialInt = currentMax + 1;
        StartSerial     = _startSerialInt.ToString("D6");

        EndSerial = "000000";
        OnPropertyChanged(nameof(HasResult));
        SaveCommand.Invalidate();
    }

    private void ClearResult()
    {
        _barcodeInfo    = null;
        Itemnam         = null;
        EndIDay         = null;
        EndICnt         = null;
        LotNoHead       = null;
        LotMax          = null;
        StartSerial     = null;
        _startSerialInt = 0;
        EndSerial       = "000000";
        OnPropertyChanged(nameof(HasResult));
    }

    // ── 저장 ──────────────────────────────────────────────────────────────────

    private async Task ExecuteSaveAsync()
    {
        if (!IsEndSerialValid()) return;

        StatusMessage = null;
        IsError       = false;
        IsBusy        = true;

        int endSerialInt = int.Parse(EndSerial);
        var error = await _api.SaveInspectionAsync(
            Barcode.Trim(), _barcodeInfo!.LotNoHead!, _startSerialInt, endSerialInt);

        if (error != null)
        {
            IsBusy        = false;
            StatusMessage = error;
            IsError       = true;
            return;
        }

        StatusMessage = $"저장 완료 — {endSerialInt - _startSerialInt + 1}건 ({_startSerialInt:D6} ~ {endSerialInt:D6})";
        IsError       = false;

        // 같은 바코드로 자동 재조회 (StatusMessage 유지)
        var (info, queryError) = await _api.LookupByBarcodeAsync(Barcode.Trim());

        IsBusy = false;

        if (queryError != null)
        {
            StatusMessage = queryError;
            IsError       = true;
            ClearResult();
            RequestBarcodeFocus?.Invoke();
            return;
        }

        ApplyResult(info!);
        RequestBarcodeFocus?.Invoke();
    }

    private bool IsEndSerialValid()
    {
        if (string.IsNullOrWhiteSpace(EndSerial)) return false;
        if (!int.TryParse(EndSerial, out var endInt)) return false;
        return endInt >= _startSerialInt;
    }

    private static void TrySetClipboard(string? text)
    {
        if (!string.IsNullOrEmpty(text))
            Clipboard.SetText(text);
    }
}
