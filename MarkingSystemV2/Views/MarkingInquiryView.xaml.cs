using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MarkingSystemV2.ViewModels;

namespace MarkingSystemV2.Views;

public partial class MarkingInquiryView : UserControl
{
    private bool _endSerialUpdating;
    private MarkingInquiryViewModel? _vm;

    public MarkingInquiryView()
    {
        InitializeComponent();
        Loaded             += OnLoaded;
        IsVisibleChanged   += OnIsVisibleChanged;
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (_vm != null)
            _vm.RequestBarcodeFocus -= FocusBarcode;

        _vm = e.NewValue as MarkingInquiryViewModel;

        if (_vm != null)
            _vm.RequestBarcodeFocus += FocusBarcode;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => FocusBarcode();

    private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (IsVisible) FocusBarcode();
    }

    private void FocusBarcode()
    {
        Dispatcher.BeginInvoke(new Action(() =>
        {
            BarcodeBox.Focus();
            Keyboard.Focus(BarcodeBox);
        }), DispatcherPriority.Input);
    }

    private void BarcodeBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        var vm = (MarkingInquiryViewModel)DataContext;
        if (vm.QueryCommand.CanExecute(null))
            vm.QueryCommand.Execute(null);
    }

    private void BarcodeBox_GotFocus(object sender, RoutedEventArgs e)
    {
        ((TextBox)sender).SelectAll();
    }

    private void EndSerialBox_GotFocus(object sender, RoutedEventArgs e)
    {
        ((TextBox)sender).SelectAll();
    }

    private void EndSerialBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_endSerialUpdating) return;
        var box = (TextBox)sender;

        var raw    = box.Text;
        var caret  = box.CaretIndex;
        var digits = new string(raw.Where(char.IsDigit).ToArray());

        string formatted;
        int    newCaret;

        if (digits.Length == 0)
        {
            formatted = "";
            newCaret  = 0;
        }
        else if (digits.Length < 6)
        {
            // 6자리 미만 (삭제 결과)
            if (caret >= raw.Length)
            {
                // 끝에서 삭제 → 좌측 0 패딩, 캐럿 끝
                formatted = digits.PadLeft(6, '0');
                newCaret  = formatted.Length;
            }
            else
            {
                // 중간에서 삭제 → 우측 0 패딩, 캐럿 위치 유지
                formatted = digits.PadRight(6, '0');
                newCaret  = Math.Min(caret, formatted.Length);
            }
        }
        else if (digits.Length == 6)
        {
            // 6자리 정상 — 캐럿 위치 유지
            formatted = digits;
            newCaret  = caret;
        }
        else if (caret >= raw.Length)
        {
            // 7자리 이상이며 끝에서 입력 → 좌측 1자리 잘림, 캐럿 끝
            formatted = digits[^6..];
            newCaret  = formatted.Length;
        }
        else
        {
            // 7자리 이상이며 중간에서 입력 → 우측 1자리 잘림, 캐럿 위치 유지
            formatted = digits[..6];
            newCaret  = Math.Min(caret, formatted.Length);
        }

        if (raw == formatted) return;

        _endSerialUpdating = true;
        box.Text       = formatted;
        box.CaretIndex = newCaret;
        _endSerialUpdating = false;
    }
}
