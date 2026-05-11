using MarkingSystemV2.Services;

namespace MarkingSystemV2.ViewModels;

public sealed class LoginViewModel : ObservableObject
{
    private readonly AuthService _auth;
    private readonly string _loginCompany;

    private string  _loginId     = "";
    private string  _password    = "";
    private bool    _rememberMe  = false;
    private bool    _isBusy      = false;
    private string? _errorMessage;

    public event EventHandler? LoginSucceeded;

    public LoginViewModel(AuthService auth, string loginCompany)
    {
        _auth         = auth;
        _loginCompany = loginCompany;

        if (auth.SavedLoginId != null)
        {
            _loginId    = auth.SavedLoginId;
            _rememberMe = true;
        }

        LoginCommand = new AsyncRelayCommand(ExecuteLoginAsync, () => !IsBusy);
    }

    public string LoginId
    {
        get => _loginId;
        set => SetField(ref _loginId, value);
    }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public bool RememberMe
    {
        get => _rememberMe;
        set => SetField(ref _rememberMe, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            SetField(ref _isBusy, value);
            (LoginCommand as AsyncRelayCommand)?.Invalidate();
        }
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetField(ref _errorMessage, value);
    }

    public AsyncRelayCommand LoginCommand { get; }

    private async Task ExecuteLoginAsync()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(LoginId))
        {
            ErrorMessage = "아이디를 입력하세요.";
            return;
        }
        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "비밀번호를 입력하세요.";
            return;
        }

        IsBusy = true;

        var error = await _auth.LoginAsync(_loginCompany, LoginId, Password, RememberMe);

        IsBusy = false;

        if (error == null)
            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        else
            ErrorMessage = error;
    }
}
