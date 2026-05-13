using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MarkingSystemV2.Services;

// 스타일에서 켜는 첨부 동작: TextBox가 포커스를 받으면 텍스트 전체를 자동 선택.
// 읽기 전용 박스/셀에서 클릭만으로 Ctrl+C 복사가 가능하게 한다.
//
// PreviewMouseLeftButtonDown도 같이 가로채서, 처음 클릭으로 포커스 받을 때는
// 마우스 이벤트가 캐럿을 옮기지 않게 막아 SelectAll이 유지되도록 한다.
// 이미 포커스 받은 박스를 다시 클릭하면 정상 동작 허용 (드래그로 부분 선택 가능).
public static class SelectAllOnFocus
{
    public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(SelectAllOnFocus),
            new PropertyMetadata(false, OnIsEnabledChanged));

    public static void SetIsEnabled(DependencyObject d, bool value) => d.SetValue(IsEnabledProperty, value);
    public static bool GetIsEnabled(DependencyObject d) => (bool)d.GetValue(IsEnabledProperty);

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBox tb) return;

        if ((bool)e.NewValue)
        {
            tb.GotFocus                   += OnGotFocus;
            tb.PreviewMouseLeftButtonDown += OnPreviewMouseDown;
        }
        else
        {
            tb.GotFocus                   -= OnGotFocus;
            tb.PreviewMouseLeftButtonDown -= OnPreviewMouseDown;
        }
    }

    private static void OnGotFocus(object sender, RoutedEventArgs e) =>
        ((TextBox)sender).SelectAll();

    private static void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        var tb = (TextBox)sender;
        if (tb.IsKeyboardFocusWithin) return;   // 이미 포커스 — 정상 클릭 허용 (드래그 선택)

        tb.Focus();
        e.Handled = true;                       // 마우스 이벤트 중단 → 캐럿 재설정 방지
    }
}
