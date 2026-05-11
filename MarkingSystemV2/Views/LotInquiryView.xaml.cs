using System.Windows.Controls;
using System.Windows.Input;
using MarkingSystemV2.ViewModels;

namespace MarkingSystemV2.Views;

public partial class LotInquiryView : UserControl
{
    public LotInquiryView()
    {
        InitializeComponent();
    }

    private void LotNoBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        var vm = (LotInquiryViewModel)DataContext;
        if (vm.QueryCommand.CanExecute(null))
            vm.QueryCommand.Execute(null);
    }
}
