using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.VisualTree;
using System.Windows.Input;

namespace STranslate.Avalonia.Controls;

/// <summary>
/// 💡 HeaderControl: 主窗口顶部工具栏控件
/// 包含: 置顶、设置、历史导航、隐藏输入、主题切换、鼠标钩子、截图翻译、OCR、自动翻译等功能按钮
/// </summary>
public class HeaderControl : TemplatedControl
{
    static HeaderControl()
    {
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // 💡 实现窗口拖动功能
        var border = e.NameScope.Find<Border>("PART_Border");
        if (border != null)
        {
            border.PointerPressed += (s, args) =>
            {
                // 只在左键按下时拖动窗口
                if (args.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                {
                    // 💡 使用 VisualTree 查找父级 Window
                    var window = this.FindAncestorOfType<Window>();
                    window?.BeginMoveDrag(args);
                }
            };
        }
    }

    #region IsTopmost - 置顶状态
    public static readonly StyledProperty<bool> IsTopmostProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsTopmost),
            false,
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsTopmost
    {
        get => GetValue(IsTopmostProperty);
        set => SetValue(IsTopmostProperty, value);
    }
    #endregion

    #region Setting - 设置按钮
    public static readonly StyledProperty<bool> IsSettingVisibleProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsSettingVisible),
            true);

    public bool IsSettingVisible
    {
        get => GetValue(IsSettingVisibleProperty);
        set => SetValue(IsSettingVisibleProperty, value);
    }

    public static readonly StyledProperty<ICommand?> SettingCommandProperty =
        AvaloniaProperty.Register<HeaderControl, ICommand?>(
            nameof(SettingCommand));

    public ICommand? SettingCommand
    {
        get => GetValue(SettingCommandProperty);
        set => SetValue(SettingCommandProperty, value);
    }
    #endregion

    #region HideInput - 隐藏输入框
    public static readonly StyledProperty<bool> IsHideInputProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsHideInput),
            false,
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsHideInput
    {
        get => GetValue(IsHideInputProperty);
        set => SetValue(IsHideInputProperty, value);
    }

    public static readonly StyledProperty<bool> IsHideInputVisibleProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsHideInputVisible),
            true);

    public bool IsHideInputVisible
    {
        get => GetValue(IsHideInputVisibleProperty);
        set => SetValue(IsHideInputVisibleProperty, value);
    }
    #endregion

    #region ScreenshotTranslateInImage - 截图翻译
    public static readonly StyledProperty<bool> ScreenshotTranslateInImageProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(ScreenshotTranslateInImage),
            false,
            defaultBindingMode: BindingMode.TwoWay);

    public bool ScreenshotTranslateInImage
    {
        get => GetValue(ScreenshotTranslateInImageProperty);
        set => SetValue(ScreenshotTranslateInImageProperty, value);
    }

    public static readonly StyledProperty<bool> IsScreenshotTranslateInImageVisibleProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsScreenshotTranslateInImageVisible),
            true);

    public bool IsScreenshotTranslateInImageVisible
    {
        get => GetValue(IsScreenshotTranslateInImageVisibleProperty);
        set => SetValue(IsScreenshotTranslateInImageVisibleProperty, value);
    }

    public static readonly StyledProperty<ICommand?> ScreenshotTranslateCommandProperty =
        AvaloniaProperty.Register<HeaderControl, ICommand?>(
            nameof(ScreenshotTranslateCommand));

    public ICommand? ScreenshotTranslateCommand
    {
        get => GetValue(ScreenshotTranslateCommandProperty);
        set => SetValue(ScreenshotTranslateCommandProperty, value);
    }
    #endregion

    #region ColorScheme - 主题切换
    public static readonly StyledProperty<bool> IsColorSchemeVisibleProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsColorSchemeVisible),
            true);

    public bool IsColorSchemeVisible
    {
        get => GetValue(IsColorSchemeVisibleProperty);
        set => SetValue(IsColorSchemeVisibleProperty, value);
    }

    public static readonly StyledProperty<ICommand?> ColorSchemeCommandProperty =
        AvaloniaProperty.Register<HeaderControl, ICommand?>(
            nameof(ColorSchemeCommand));

    public ICommand? ColorSchemeCommand
    {
        get => GetValue(ColorSchemeCommandProperty);
        set => SetValue(ColorSchemeCommandProperty, value);
    }
    #endregion

    #region MouseHook - 鼠标钩子翻译
    public static readonly StyledProperty<bool> IsMouseHookProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsMouseHook),
            false,
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsMouseHook
    {
        get => GetValue(IsMouseHookProperty);
        set => SetValue(IsMouseHookProperty, value);
    }

    public static readonly StyledProperty<bool> IsMouseHookVisibleProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsMouseHookVisible),
            true);

    public bool IsMouseHookVisible
    {
        get => GetValue(IsMouseHookVisibleProperty);
        set => SetValue(IsMouseHookVisibleProperty, value);
    }
    #endregion

    #region History - 历史导航
    public static readonly StyledProperty<bool> IsHistoryNavigationVisibleProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsHistoryNavigationVisible),
            true);

    public bool IsHistoryNavigationVisible
    {
        get => GetValue(IsHistoryNavigationVisibleProperty);
        set => SetValue(IsHistoryNavigationVisibleProperty, value);
    }

    public static readonly StyledProperty<ICommand?> HistoryPreviousCommandProperty =
        AvaloniaProperty.Register<HeaderControl, ICommand?>(
            nameof(HistoryPreviousCommand));

    public ICommand? HistoryPreviousCommand
    {
        get => GetValue(HistoryPreviousCommandProperty);
        set => SetValue(HistoryPreviousCommandProperty, value);
    }

    public static readonly StyledProperty<ICommand?> HistoryNextCommandProperty =
        AvaloniaProperty.Register<HeaderControl, ICommand?>(
            nameof(HistoryNextCommand));

    public ICommand? HistoryNextCommand
    {
        get => GetValue(HistoryNextCommandProperty);
        set => SetValue(HistoryNextCommandProperty, value);
    }
    #endregion

    #region OCR - 文字识别
    public static readonly StyledProperty<bool> IsOcrVisibleProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsOcrVisible),
            true);

    public bool IsOcrVisible
    {
        get => GetValue(IsOcrVisibleProperty);
        set => SetValue(IsOcrVisibleProperty, value);
    }

    public static readonly StyledProperty<ICommand?> OcrCommandProperty =
        AvaloniaProperty.Register<HeaderControl, ICommand?>(
            nameof(OcrCommand));

    public ICommand? OcrCommand
    {
        get => GetValue(OcrCommandProperty);
        set => SetValue(OcrCommandProperty, value);
    }
    #endregion

    #region AutoTranslate - 自动翻译
    public static readonly StyledProperty<bool> IsAutoTranslateProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsAutoTranslate),
            false,
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsAutoTranslate
    {
        get => GetValue(IsAutoTranslateProperty);
        set => SetValue(IsAutoTranslateProperty, value);
    }

    public static readonly StyledProperty<bool> IsAutoTranslateVisibleProperty =
        AvaloniaProperty.Register<HeaderControl, bool>(
            nameof(IsAutoTranslateVisible),
            true);

    public bool IsAutoTranslateVisible
    {
        get => GetValue(IsAutoTranslateVisibleProperty);
        set => SetValue(IsAutoTranslateVisibleProperty, value);
    }
    #endregion
}
