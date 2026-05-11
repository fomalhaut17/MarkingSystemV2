using MarkingSystemV2.Services;

namespace MarkingSystemV2.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    private readonly AuthService _auth;
    private int _selectedTabIndex;

    public event EventHandler? LogoutRequested;

    public MarkingInquiryViewModel MarkingInquiry { get; }
    public LotInquiryViewModel     LotInquiry     { get; }

    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetField(ref _selectedTabIndex, value);
    }

    public string TodayDisplay => DateTime.Now.ToString("yyyy년 M월 d일");

    public RelayCommand LogoutCommand { get; }

    public MainViewModel(AuthService auth, MarkingApiService markingApi)
    {
        _auth          = auth;
        MarkingInquiry = new MarkingInquiryViewModel(markingApi);
        LotInquiry     = new LotInquiryViewModel(markingApi);

        LogoutCommand  = new RelayCommand(ExecuteLogout);

        auth.LogoutRequested += (_, _) => LogoutRequested?.Invoke(this, EventArgs.Empty);
    }

    private void ExecuteLogout()
    {
        _auth.Logout();
        LogoutRequested?.Invoke(this, EventArgs.Empty);
    }
}
