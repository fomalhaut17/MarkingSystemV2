using System.Windows;
using MarkingSystemV2.ViewModels;

namespace MarkingSystemV2.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
