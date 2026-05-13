using System.Windows.Threading;
using MarkingSystemV2.Services;

namespace MarkingSystemV2.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    private readonly AuthService _auth;
    private readonly DispatcherTimer _clockTimer;
    private int _selectedTabIndex;

    public event EventHandler? LogoutRequested;

    public MarkingInquiryViewModel MarkingInquiry { get; }
    public LotInquiryViewModel     LotInquiry     { get; }

    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetField(ref _selectedTabIndex, value);
    }

    public string TodayDisplay => DateTime.Now.ToString("yyyy년 M월 d일 HH:mm:ss");

    public string CurrentVersion => $"v{AppVersion.Current}";

    public RelayCommand LogoutCommand { get; }

    public MainViewModel(AuthService auth, MarkingApiService markingApi)
    {
        _auth          = auth;
        MarkingInquiry = new MarkingInquiryViewModel(markingApi);
        LotInquiry     = new LotInquiryViewModel(markingApi);

        LogoutCommand  = new RelayCommand(ExecuteLogout);

        auth.LogoutRequested += (_, _) => LogoutRequested?.Invoke(this, EventArgs.Empty);

        _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clockTimer.Tick += (_, _) => OnPropertyChanged(nameof(TodayDisplay));
        _clockTimer.Start();
    }

    private void ExecuteLogout()
    {
        _auth.Logout();
        LogoutRequested?.Invoke(this, EventArgs.Empty);
    }
}
