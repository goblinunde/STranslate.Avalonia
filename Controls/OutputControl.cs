using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System.Windows.Input;

namespace STranslate.Avalonia.Controls;

/// <summary>
/// 💡 OutputControl: 翻译输出控件 (完整版)
/// 功能: 显示翻译结果，支持词典模式(音标、词性、例句)和翻译模式(翻译结果、回译、代码格式转换)
/// </summary>
public class OutputControl : ItemsControl
{
    static OutputControl()
    {
    }

    #region 命令属性

    // 💡 CopyCommand - 复制命令
    public static readonly StyledProperty<ICommand?> CopyCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(CopyCommand));

    public ICommand? CopyCommand
    {
        get => GetValue(CopyCommandProperty);
        set => SetValue(CopyCommandProperty, value);
    }

    // 💡 InsertCommand - 插入文本命令
    public static readonly StyledProperty<ICommand?> InsertCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(InsertCommand));

    public ICommand? InsertCommand
    {
        get => GetValue(InsertCommandProperty);
        set => SetValue(InsertCommandProperty, value);
    }

    // 💡 CleanTransBackCommand - 清除回译命令
    public static readonly StyledProperty<ICommand?> CleanTransBackCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(CleanTransBackCommand));

    public ICommand? CleanTransBackCommand
    {
        get => GetValue(CleanTransBackCommandProperty);
        set => SetValue(CleanTransBackCommandProperty, value);
    }

    // 💡 RetryCommand - 重试翻译命令
    public static readonly StyledProperty<ICommand?> RetryCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(RetryCommand));

    public ICommand? RetryCommand
    {
        get => GetValue(RetryCommandProperty);
        set => SetValue(RetryCommandProperty, value);
    }

    // 💡 TransBackCommand - 回译命令
    public static readonly StyledProperty<ICommand?> TransBackCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(TransBackCommand));

    public ICommand? TransBackCommand
    {
        get => GetValue(TransBackCommandProperty);
        set => SetValue(TransBackCommandProperty, value);
    }

    // 💡 NavigateCommand - 导航到服务配置命令
    public static readonly StyledProperty<ICommand?> NavigateCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(NavigateCommand));

    public ICommand? NavigateCommand
    {
        get => GetValue(NavigateCommandProperty);
        set => SetValue(NavigateCommandProperty, value);
    }

    // 💡 PlayAudioCommand - 播放语音命令
    public static readonly StyledProperty<ICommand?> PlayAudioCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(PlayAudioCommand));

    public ICommand? PlayAudioCommand
    {
        get => GetValue(PlayAudioCommandProperty);
        set => SetValue(PlayAudioCommandProperty, value);
    }

    // 💡 PlayAudioUrlCommand - 播放URL语音命令
    public static readonly StyledProperty<ICommand?> PlayAudioUrlCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(PlayAudioUrlCommand));

    public ICommand? PlayAudioUrlCommand
    {
        get => GetValue(PlayAudioUrlCommandProperty);
        set => SetValue(PlayAudioUrlCommandProperty, value);
    }

    // 💡 ExplainCommand - 解释单词命令
    public static readonly StyledProperty<ICommand?> ExplainCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(ExplainCommand));

    public ICommand? ExplainCommand
    {
        get => GetValue(ExplainCommandProperty);
        set => SetValue(ExplainCommandProperty, value);
    }

    // 💡 CopyPascalCaseCommand - 复制为帕斯卡格式
    public static readonly StyledProperty<ICommand?> CopyPascalCaseCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(CopyPascalCaseCommand));

    public ICommand? CopyPascalCaseCommand
    {
        get => GetValue(CopyPascalCaseCommandProperty);
        set => SetValue(CopyPascalCaseCommandProperty, value);
    }

    // 💡 CopyCamelCaseCommand - 复制为驼峰格式
    public static readonly StyledProperty<ICommand?> CopyCamelCaseCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(CopyCamelCaseCommand));

    public ICommand? CopyCamelCaseCommand
    {
        get => GetValue(CopyCamelCaseCommandProperty);
        set => SetValue(CopyCamelCaseCommandProperty, value);
    }

    // 💡 CopySnakeCaseCommand - 复制为下划线格式
    public static readonly StyledProperty<ICommand?> CopySnakeCaseCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(CopySnakeCaseCommand));

    public ICommand? CopySnakeCaseCommand
    {
        get => GetValue(CopySnakeCaseCommandProperty);
        set => SetValue(CopySnakeCaseCommandProperty, value);
    }

    // 💡 SaveToVocabularyWithNoteCommand - 保存到单词本带笔记
    public static readonly StyledProperty<ICommand?> SaveToVocabularyWithNoteCommandProperty =
        AvaloniaProperty.Register<OutputControl, ICommand?>(nameof(SaveToVocabularyWithNoteCommand));

    public ICommand? SaveToVocabularyWithNoteCommand
    {
        get => GetValue(SaveToVocabularyWithNoteCommandProperty);
        set => SetValue(SaveToVocabularyWithNoteCommandProperty, value);
    }

    #endregion

    #region 布尔属性

    // 💡 ShowPrompt - 显示提示按钮
    public static readonly StyledProperty<bool> ShowPromptProperty =
        AvaloniaProperty.Register<OutputControl, bool>(nameof(ShowPrompt), true);

    public bool ShowPrompt
    {
        get => GetValue(ShowPromptProperty);
        set => SetValue(ShowPromptProperty, value);
    }

    // 💡 ShowPascalCase - 显示帕斯卡格式按钮
    public static readonly StyledProperty<bool> ShowPascalCaseProperty =
        AvaloniaProperty.Register<OutputControl, bool>(nameof(ShowPascalCase), true);

    public bool ShowPascalCase
    {
        get => GetValue(ShowPascalCaseProperty);
        set => SetValue(ShowPascalCaseProperty, value);
    }

    // 💡 ShowCamelCase - 显示驼峰格式按钮
    public static readonly StyledProperty<bool> ShowCamelCaseProperty =
        AvaloniaProperty.Register<OutputControl, bool>(nameof(ShowCamelCase), false);

    public bool ShowCamelCase
    {
        get => GetValue(ShowCamelCaseProperty);
        set => SetValue(ShowCamelCaseProperty, value);
    }

    // 💡 ShowSnakeCase - 显示下划线格式按钮
    public static readonly StyledProperty<bool> ShowSnakeCaseProperty =
        AvaloniaProperty.Register<OutputControl, bool>(nameof(ShowSnakeCase), true);

    public bool ShowSnakeCase
    {
        get => GetValue(ShowSnakeCaseProperty);
        set => SetValue(ShowSnakeCaseProperty, value);
    }

    // 💡 ShowInsert - 显示插入按钮
    public static readonly StyledProperty<bool> ShowInsertProperty =
        AvaloniaProperty.Register<OutputControl, bool>(nameof(ShowInsert), true);

    public bool ShowInsert
    {
        get => GetValue(ShowInsertProperty);
        set => SetValue(ShowInsertProperty, value);
    }

    // 💡 ShowBackTranslation - 显示回译
    public static readonly StyledProperty<bool> ShowBackTranslationProperty =
        AvaloniaProperty.Register<OutputControl, bool>(nameof(ShowBackTranslation), true);

    public bool ShowBackTranslation
    {
        get => GetValue(ShowBackTranslationProperty);
        set => SetValue(ShowBackTranslationProperty, value);
    }

    // 💡 HasActivedVocabulary - 是否激活了单词本
    public static readonly StyledProperty<bool> HasActivedVocabularyProperty =
        AvaloniaProperty.Register<OutputControl, bool>(nameof(HasActivedVocabulary), false);

    public bool HasActivedVocabulary
    {
        get => GetValue(HasActivedVocabularyProperty);
        set => SetValue(HasActivedVocabularyProperty, value);
    }

    #endregion
}
