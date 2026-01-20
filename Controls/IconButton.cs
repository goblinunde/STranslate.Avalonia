using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using System.Windows.Input;

namespace STranslate.Avalonia.Controls;

/// <summary>
/// 💡 IconButton: 支持图标的按钮控件，可在普通Button和ToggleButton之间切换
/// 从WPF版本迁移到Avalonia，保持完全相同的功能
/// </summary>
public class IconButton : TemplatedControl
{
    public enum IconButtonType
    {
        /// <summary>
        /// 一次性按钮
        /// </summary>
        Once,
        /// <summary>
        /// 切换按钮
        /// </summary>
        Toggle
    }

    static IconButton()
    {
        // 💡 Avalonia使用StyledProperty替代WPF的DependencyProperty
    }

    private ToggleButton? _toggleButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // 移除旧的事件处理器
        if (_toggleButton != null)
        {
            _toggleButton.PointerPressed -= OnToggleButtonPointerPressed;
        }

        // 获取模板中的 ToggleButton
        _toggleButton = e.NameScope.Find<ToggleButton>("PART_ToggleButton");

        // 添加新的事件处理器
        if (_toggleButton != null)
        {
            _toggleButton.PointerPressed += OnToggleButtonPointerPressed;
        }
    }

    private void OnToggleButtonPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // 仅在启用 RequireCtrlToToggle 属性时才应用特殊逻辑
        if (!RequireCtrlToToggle || Type != IconButtonType.Toggle)
        {
            return;
        }

        // 💡 Avalonia: 使用KeyModifiers替代WPF的ModifierKeys
        var keyModifiers = e.KeyModifiers;
        bool isCtrlPressed = keyModifiers.HasFlag(KeyModifiers.Control);

        if (!isCtrlPressed)
        {
            // 普通点击：执行命令，但阻止切换状态
            e.Handled = true;

            if (Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
            }
        }
        // Ctrl + 点击：让默认行为发生（切换状态），不执行命令
    }

    // 💡 Type属性 - 控制按钮类型(Once/Toggle)
    public static readonly StyledProperty<IconButtonType> TypeProperty =
        AvaloniaProperty.Register<IconButton, IconButtonType>(
            nameof(Type),
            IconButtonType.Once);

    public IconButtonType Type
    {
        get => GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    // 💡 Icon属性 - 图标内容(支持FluentIcon枚举或字符串)
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<IconButton, object?>(
            nameof(Icon),
            defaultBindingMode: BindingMode.TwoWay);

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    // 💡 IconSize属性 - 图标大小
    public static readonly StyledProperty<double> IconSizeProperty =
        AvaloniaProperty.Register<IconButton, double>(
            nameof(IconSize),
            16.0);

    public double IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    // 💡 Command属性
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<IconButton, ICommand?>(
            nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    // 💡 IsOn属性 - Toggle状态
    public static readonly StyledProperty<bool> IsOnProperty =
        AvaloniaProperty.Register<IconButton, bool>(
            nameof(IsOn),
            false,
            defaultBindingMode: BindingMode.TwoWay);

    public bool IsOn
    {
        get => GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    // 💡 CommandParameter属性
    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<IconButton, object?>(
            nameof(CommandParameter));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// 获取或设置是否需要按住 Ctrl 键才能切换状态。
    /// 当为 true 时，普通点击执行 Command，Ctrl + 点击切换 IsOn 状态。
    /// 默认值为 false，保持原有的 Toggle 行为。
    /// </summary>
    public static readonly StyledProperty<bool> RequireCtrlToToggleProperty =
        AvaloniaProperty.Register<IconButton, bool>(
            nameof(RequireCtrlToToggle),
            false);

    public bool RequireCtrlToToggle
    {
        get => GetValue(RequireCtrlToToggleProperty);
        set => SetValue(RequireCtrlToToggleProperty, value);
    }
}
