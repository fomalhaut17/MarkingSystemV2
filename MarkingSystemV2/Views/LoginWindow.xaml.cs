using System.Windows;
using System.Windows.Input;
using MarkingSystemV2.ViewModels;

namespace MarkingSystemV2.Views;

public partial class LoginWindow : Window
{
    public LoginWindow(LoginViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;

        Loaded += (_, _) =>
        {
            if (!string.IsNullOrEmpty(vm.LoginId))
                PasswordBox.Focus();
            else
                IdBox.Focus();
        };
    }

    private void Input_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        SyncPassword();
        var vm = (LoginViewModel)DataContext;
        if (vm.LoginCommand.CanExecute(null))
            vm.LoginCommand.Execute(null);
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        => SyncPassword();

    private void LoginButton_Click(object sender, RoutedEventArgs e)
        => SyncPassword();

    private void SyncPassword()
        => ((LoginViewModel)DataContext).Password = PasswordBox.Password;
}
