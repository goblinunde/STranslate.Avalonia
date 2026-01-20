using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using System.Windows.Input;

namespace STranslate.Avalonia.Controls;

/// <summary>
/// 💡 InputControl: 翻译输入控件
/// 功能: 多行文本输入、工具栏按钮(保存单词本、语音、复制、移除换行/空格)、语言识别显示、字体大小调节
/// </summary>
public class InputControl : TemplatedControl
{
    #region Constants
    private const string PartTextBoxName = "PART_TextBox";
    private const string PartFontSizeHintBorderName = "PART_FontSizeHintBorder";
    private const string PartFontSizeTextName = "PART_FontSizeText";
    private const int FontSizeHintAnimationDurationMs = 1200;
    #endregion

    private TextBox? _textBox;
    private Border? _fontSizeHintBorder;
    private TextBlock? _fontSizeText;

    static InputControl()
    {
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // 移除旧的事件处理器
        if (_textBox != null)
        {
            _textBox.PointerWheelChanged -= OnTextBoxPointerWheelChanged;
        }

        _textBox = e.NameScope.Find<TextBox>(PartTextBoxName);
        _fontSizeHintBorder = e.NameScope.Find<Border>(PartFontSizeHintBorderName);
        _fontSizeText = e.NameScope.Find<TextBlock>(PartFontSizeTextName);

        // 💡 Avalonia: 添加鼠标滚轮事件处理 (Ctrl+滚轮调节字体大小)
        if (_textBox != null)
        {
            _textBox.PointerWheelChanged += OnTextBoxPointerWheelChanged;
        }
    }

    /// <summary>
    /// 💡 处理 TextBox 的鼠标滚轮事件，实现 Ctrl+鼠标滚轮调节字体大小
    /// </summary>
    private void OnTextBoxPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        // 检查是否按下了 Ctrl 键
        if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            try
            {
                var currentFontSize = CurrentFontSize;
                
                // 根据滚轮方向调整字体大小
                var delta = e.Delta.Y > 0 ? 1 : -1;
                var newFontSize = currentFontSize + delta;

                // 限制字体大小范围 (10-20)
                newFontSize = Math.Max(10, Math.Min(20, newFontSize));

                if (Math.Abs(newFontSize - currentFontSize) > 0.01)
                {
                    CurrentFontSize = newFontSize;
                    ShowFontSizeHint();
                }

                e.Handled = true;
            }
            catch
            {
                e.Handled = false;
            }
        }
    }

    /// <summary>
    /// 显示字体大小调节提示 (淡入淡出动画)
    /// </summary>
    private void ShowFontSizeHint()
    {
        if (_fontSizeHintBorder == null)
            return;

        // 💡 Avalonia: 使用Transitions实现淡入淡出效果
        _fontSizeHintBorder.IsVisible = true;
        _fontSizeHintBorder.Opacity = 1.0;

        // 延迟后淡出
        DispatcherTimer.RunOnce(() =>
        {
            if (_fontSizeHintBorder != null)
            {
                _fontSizeHintBorder.Opacity = 0;
                DispatcherTimer.RunOnce(() =>
                {
                    if (_fontSizeHintBorder != null)
                        _fontSizeHintBorder.IsVisible = false;
                }, TimeSpan.FromMilliseconds(FontSizeHintAnimationDurationMs));
            }
        }, TimeSpan.FromMilliseconds(200));
    }

    /// <summary>
    /// 选择所有文本
    /// </summary>
    public void SelectAll() => _textBox?.SelectAll();

    /// <summary>
    /// 设置光标位置
    /// </summary>
    public void SetCaretIndex(int index)
    {
        if (_textBox != null)
            _textBox.CaretIndex = index;
    }

    #region Properties

    // 💡 Text - 输入文本内容
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<InputControl, string>(
            nameof(Text),
            string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // 💡 IdentifiedLanguage - 识别出的语言
    public static readonly StyledProperty<string> IdentifiedLanguageProperty =
        AvaloniaProperty.Register<InputControl, string>(
            nameof(IdentifiedLanguage),
            string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

    public string IdentifiedLanguage
    {
        get => GetValue(IdentifiedLanguageProperty);
        set => SetValue(IdentifiedLanguageProperty, value);
    }

    // 💡 IsIdentify - 是否正在识别语言
    public static readonly StyledProperty<bool> IsIdentifyProperty =
        AvaloniaProperty.Register<InputControl, bool>(
            nameof(IsIdentify),
            false);

    public bool IsIdentify
    {
        get => GetValue(IsIdentifyProperty);
        set => SetValue(IsIdentifyProperty, value);
    }

    // 💡 TranslateOnPaste - 粘贴后自动翻译
    public static readonly StyledProperty<bool> TranslateOnPasteProperty =
        AvaloniaProperty.Register<InputControl, bool>(
            nameof(TranslateOnPaste),
            true);

    public bool TranslateOnPaste
    {
        get => GetValue(TranslateOnPasteProperty);
        set => SetValue(TranslateOnPasteProperty, value);
    }

    // 💡 CurrentFontSize - 当前字体大小
    public static readonly StyledProperty<double> CurrentFontSizeProperty =
        AvaloniaProperty.Register<InputControl, double>(
            nameof(CurrentFontSize),
            14.0,
            defaultBindingMode: BindingMode.TwoWay);

    public double CurrentFontSize
    {
        get => GetValue(CurrentFontSizeProperty);
        set => SetValue(CurrentFontSizeProperty, value);
    }

    // 💡 ExecuteCommand - Enter键执行翻译命令
    public static readonly StyledProperty<ICommand?> ExecuteCommandProperty =
        AvaloniaProperty.Register<InputControl, ICommand?>(
            nameof(ExecuteCommand));

    public ICommand? ExecuteCommand
    {
        get => GetValue(ExecuteCommandProperty);
        set => SetValue(ExecuteCommandProperty, value);
    }

    // 💡 ForceExecuteCommand - Ctrl+Enter强制执行命令
    public static readonly StyledProperty<ICommand?> ForceExecuteCommandProperty =
        AvaloniaProperty.Register<InputControl, ICommand?>(
            nameof(ForceExecuteCommand));

    public ICommand? ForceExecuteCommand
    {
        get => GetValue(ForceExecuteCommandProperty);
        set => SetValue(ForceExecuteCommandProperty, value);
    }

    // 💡 SaveToVocabularyCommand - 保存到单词本
    public static readonly StyledProperty<ICommand?> SaveToVocabularyCommandProperty =
        AvaloniaProperty.Register<InputControl, ICommand?>(
            nameof(SaveToVocabularyCommand));

    public ICommand? SaveToVocabularyCommand
    {
        get => GetValue(SaveToVocabularyCommandProperty);
        set => SetValue(SaveToVocabularyCommandProperty, value);
    }

    // 💡 HasActivedVocabulary - 是否激活了单词本
    public static readonly StyledProperty<bool> HasActivedVocabularyProperty =
        AvaloniaProperty.Register<InputControl, bool>(
            nameof(HasActivedVocabulary),
            false);

    public bool HasActivedVocabulary
    {
        get => GetValue(HasActivedVocabularyProperty);
        set => SetValue(HasActivedVocabularyProperty, value);
    }

    // 💡 PlayAudioCommand - 播放语音
    public static readonly StyledProperty<ICommand?> PlayAudioCommandProperty =
        AvaloniaProperty.Register<InputControl, ICommand?>(
            nameof(PlayAudioCommand));

    public ICommand? PlayAudioCommand
    {
        get => GetValue(PlayAudioCommandProperty);
        set => SetValue(PlayAudioCommandProperty, value);
    }

    // 💡 CopyCommand - 复制命令
    public static readonly StyledProperty<ICommand?> CopyCommandProperty =
        AvaloniaProperty.Register<InputControl, ICommand?>(
            nameof(CopyCommand));

    public ICommand? CopyCommand
    {
        get => GetValue(CopyCommandProperty);
        set => SetValue(CopyCommandProperty, value);
    }

    // 💡 RemoveLineBreaksCommand - 移除换行
    public static readonly StyledProperty<ICommand?> RemoveLineBreaksCommandProperty =
        AvaloniaProperty.Register<InputControl, ICommand?>(
            nameof(RemoveLineBreaksCommand));

    public ICommand? RemoveLineBreaksCommand
    {
        get => GetValue(RemoveLineBreaksCommandProperty);
        set => SetValue(RemoveLineBreaksCommandProperty, value);
    }

    // 💡 RemoveSpacesCommand - 移除空格
    public static readonly StyledProperty<ICommand?> RemoveSpacesCommandProperty =
        AvaloniaProperty.Register<InputControl, ICommand?>(
            nameof(RemoveSpacesCommand));

    public ICommand? RemoveSpacesCommand
    {
        get => GetValue(RemoveSpacesCommandProperty);
        set => SetValue(RemoveSpacesCommandProperty, value);
    }

    #endregion
}
