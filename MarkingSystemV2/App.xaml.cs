using System.Windows;
using MarkingSystemV2.Services;
using MarkingSystemV2.ViewModels;
using MarkingSystemV2.Views;

namespace MarkingSystemV2;

public partial class App : Application
{
    private AppSettings     _settings  = null!;
    private AuthService     _auth      = null!;
    private MainViewModel   _mainVm    = null!;
    private LoginViewModel  _loginVm   = null!;

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        _settings = AppSettings.Load();

        _auth = new AuthService(_settings.Api.AuthBaseUrl);

        var apiClient  = new ApiClient(_settings.Api.BaseUrl, _auth);
        var markingApi = new MarkingApiService(apiClient, _settings.Api);

        _mainVm  = new MainViewModel(_auth, markingApi);
        _loginVm = new LoginViewModel(_auth, _settings.Api.LoginCompany);

        _auth.LogoutRequested    += (_, _) => ShowLogin();
        _mainVm.LogoutRequested  += (_, _) => ShowLogin();
        _loginVm.LoginSucceeded  += (_, _) => ShowMain();

        if (await _auth.TryAutoLoginAsync())
            ShowMain();
        else
            ShowLogin();
    }

    private void ShowMain()
    {
        var mainWin = new MainWindow(_mainVm);
        mainWin.Closed += (_, _) => { if (!AnyWindowOpen()) Shutdown(); };
        mainWin.Show();
        CloseOtherWindows(mainWin);
    }

    private void ShowLogin()
    {
        // LoginViewModel을 재사용해 아이디 복원 유지
        _loginVm = new LoginViewModel(_auth, _settings.Api.LoginCompany);
        _loginVm.LoginSucceeded += (_, _) => ShowMain();

        var loginWin = new LoginWindow(_loginVm);
        loginWin.Closed += (_, _) => { if (!AnyWindowOpen()) Shutdown(); };
        loginWin.Show();
        CloseOtherWindows(loginWin);
    }

    private void CloseOtherWindows(Window keep)
    {
        foreach (Window w in Windows)
        {
            if (w != keep) w.Close();
        }
    }

    private bool AnyWindowOpen() => Windows.Count > 0;
}
