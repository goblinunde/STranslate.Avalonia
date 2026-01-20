using Avalonia;
using ReactiveUI;
using STranslate.Avalonia.Models;
using STranslate.Avalonia.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Windows.Input;

namespace STranslate.Avalonia.ViewModels;

/// <summary>
/// 💡 MainWindowViewModel 简化版 - 让UI控件真正工作
/// 后续会逐步添加完整功能
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    #region 构造函数

    private readonly SimplifiedTranslateService _translateService;

    public MainWindowViewModel()
    {
        // 💡 初始化翻译服务
        _translateService = new SimplifiedTranslateService();

        // 💡 初始化语言列表
        SourceLanguages = new List<LanguageItem>
        {
            new(Language.Auto),
            new(Language.Chinese),
            new(Language.English),
            new(Language.Japanese),
            new(Language.Korean),
            new(Language.French),
            new(Language.German),
            new(Language.Spanish),
            new(Language.Russian)
        };

        TargetLanguages = new List<LanguageItem>
        {
            new(Language.Chinese),
            new(Language.English),
            new(Language.Japanese),
            new(Language.Korean),
            new(Language.French),
            new(Language.German),
            new(Language.Spanish),
            new(Language.Russian)
        };

        // 💡 设置默认选中语言
        _selectedSourceLanguage = SourceLanguages[0];  // Auto
        _selectedTargetLanguage = TargetLanguages[1]; // English

        InitializeCommands();
    }

    private void InitializeCommands()
    {
        _swapLanguageCommand = ReactiveCommand.Create(SwapLanguage);
        // TranslateCommand现在通过属性getter延迟初始化
        _copyCommand = ReactiveCommand.Create<string?>(CopyText);
        _playAudioCommand = ReactiveCommand.Create(PlayAudio);
        _openSettingsCommand = ReactiveCommand.Create(OpenSettings);
        _historyPreviousCommand = ReactiveCommand.Create(HistoryPrevious);
        _historyNextCommand = ReactiveCommand.Create(HistoryNext);
        _changeColorSchemeCommand = ReactiveCommand.Create(ChangeColorScheme);
        _screenshotTranslateCommand = ReactiveCommand.Create(ScreenshotTranslate);
        _ocrCommand = ReactiveCommand.Create(Ocr);
        _saveToVocabularyCommand = ReactiveCommand.Create(SaveToVocabulary);
        _removeLineBreaksCommand = ReactiveCommand.Create(RemoveLineBreaks);
        _removeSpacesCommand = ReactiveCommand.Create(RemoveSpaces);
        _singleTranslateCommand = ReactiveCommand.Create(SingleTranslate);
        _navigateCommand = ReactiveCommand.Create(Navigate);
    }

    #endregion

    #region 属性

    private string _inputText = string.Empty;
    public string InputText
    {
        get => _inputText;
        set => this.RaiseAndSetIfChanged(ref _inputText, value);
    }

    private string _identifiedLanguage = string.Empty;
    public string IdentifiedLanguage
    {
        get => _identifiedLanguage;
        set => this.RaiseAndSetIfChanged(ref _identifiedLanguage, value);
    }

    // 💡 语言列表 - 用于ComboBox绑定
    public List<LanguageItem> SourceLanguages { get; }
    public List<LanguageItem> TargetLanguages { get; }

    private LanguageItem? _selectedSourceLanguage;
    public LanguageItem? SelectedSourceLanguage
    {
        get => _selectedSourceLanguage;
        set
        {
            if (this.RaiseAndSetIfChanged(ref _selectedSourceLanguage, value) && value != null)
            {
                SourceLanguage = value.Code;
                Debug.WriteLine($"源语言切换到: {value.DisplayName} ({value.Code})");
            }
        }
    }

    private LanguageItem? _selectedTargetLanguage;
    public LanguageItem? SelectedTargetLanguage
    {
        get => _selectedTargetLanguage;
        set
        {
            if (this.RaiseAndSetIfChanged(ref _selectedTargetLanguage, value) && value != null)
            {
                TargetLanguage = value.Code;
                Debug.WriteLine($"目标语言切换到: {value.DisplayName} ({value.Code})");
            }
        }
    }

    private bool _isIdentifyProcessing = false;
    public bool IsIdentifyProcessing
    {
        get => _isIdentifyProcessing;
        set => this.RaiseAndSetIfChanged(ref _isIdentifyProcessing, value);
    }

    private bool _isAutoTranslate = false;
    public bool IsAutoTranslate
    {
        get => _isAutoTranslate;
        set => this.RaiseAndSetIfChanged(ref _isAutoTranslate, value);
    }

    private bool _isMouseHook = false;
    public bool IsMouseHook
    {
        get => _isMouseHook;
        set => this.RaiseAndSetIfChanged(ref _isMouseHook, value);
    }

    private bool _isHideInput = false;
    public bool IsHideInput
    {
        get => _isHideInput;
        set => this.RaiseAndSetIfChanged(ref _isHideInput, value);
    }

    private string _outputText = string.Empty;
    public string OutputText
    {
        get => _outputText;
        set => this.RaiseAndSetIfChanged(ref _outputText, value);
    }

    private bool _isTranslating = false;
    public bool IsTranslating
    {
        get => _isTranslating;
        set => this.RaiseAndSetIfChanged(ref _isTranslating, value);
    }

    private Language _sourceLang = Language.Auto;
    public Language SourceLanguage
    {
        get => _sourceLang;
        set => this.RaiseAndSetIfChanged(ref _sourceLang, value);
    }

    private Language _targetLang = Language.Chinese;
    public Language TargetLanguage
    {
        get => _targetLang;
        set => this.RaiseAndSetIfChanged(ref _targetLang, value);
    }

    // 💡 临时对象，后续替换为真实服务
    public object? Settings { get; } = null;
    public object? TranslateService { get; } = null;
    public object? VocabularyService { get; } = null;

    #endregion

    #region 命令字段

    private ICommand? _swapLanguageCommand;
    private ICommand? _copyCommand;
    private ICommand? _playAudioCommand;
    private ICommand? _openSettingsCommand;
    private ICommand? _historyPreviousCommand;
    private ICommand? _historyNextCommand;
    private ICommand? _changeColorSchemeCommand;
    private ICommand? _screenshotTranslateCommand;
    private ICommand? _ocrCommand;
    private ICommand? _saveToVocabularyCommand;
    private ICommand? _removeLineBreaksCommand;
    private ICommand? _removeSpacesCommand;
    private ICommand? _singleTranslateCommand;
    private ICommand? _navigateCommand;

    #endregion

    #region 命令属性 (延迟初始化)

    // 💡 翻译命令 - 执行翻译操作
    private ReactiveCommand<Unit, Unit>? _translateCommand;
    public ReactiveCommand<Unit, Unit> TranslateCommand =>
        _translateCommand ??= ReactiveCommand.CreateFromTask(async () =>
        {
            if (string.IsNullOrWhiteSpace(InputText))
            {
                System.Diagnostics.Debug.WriteLine("⚠️ 翻译失败：输入文本为空");
                OutputText = "请输入要翻译的文本";
                return;
            }

            try
            {
                IsTranslating = true;
                System.Diagnostics.Debug.WriteLine($"🔄 开始翻译: '{InputText}'");
                System.Diagnostics.Debug.WriteLine($"   源语言: {SourceLanguage}, 目标语言: {TargetLanguage}");

                // 调用翻译服务
                var result = await _translateService.TranslateAsync(new TranslationRequest
                {
                    Text = InputText,
                    SourceLang = SourceLanguage.ToString().ToLower(),
                    TargetLang = TargetLanguage.ToString().ToLower()
                });

                if (result.IsSuccess)
                {
                    OutputText = result.TranslatedText;
                    System.Diagnostics.Debug.WriteLine($"✅ 翻译成功: '{result.TranslatedText}'");
                }
                else
                {
                    OutputText = $"翻译失败: {result.ErrorMessage}";
                    System.Diagnostics.Debug.WriteLine($"❌ 翻译失败: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ 翻译错误: {ex.Message}");
                OutputText = $"翻译失败: {ex.Message}";
            }
            finally
            {
                IsTranslating = false;
            }
        });
    public ICommand SwapLanguageCommand => _swapLanguageCommand ??= ReactiveCommand.Create(SwapLanguage);
    public ICommand CopyCommand => _copyCommand ??= ReactiveCommand.Create<string?>(CopyText);
    public ICommand PlayAudioCommand => _playAudioCommand ??= ReactiveCommand.Create(PlayAudio);
    public ICommand OpenSettingsCommand => _openSettingsCommand ??= ReactiveCommand.Create(OpenSettings);
    public ICommand HistoryPreviousCommand => _historyPreviousCommand ??= ReactiveCommand.Create(HistoryPrevious);
    public ICommand HistoryNextCommand => _historyNextCommand ??= ReactiveCommand.Create(HistoryNext);
    public ICommand ChangeColorSchemeCommand => _changeColorSchemeCommand ??= ReactiveCommand.Create(ChangeColorScheme);
    public ICommand ScreenshotTranslateCommand => _screenshotTranslateCommand ??= ReactiveCommand.Create(ScreenshotTranslate);
    public ICommand OcrCommand => _ocrCommand ??= ReactiveCommand.Create(Ocr);
    public ICommand SaveToVocabularyCommand => _saveToVocabularyCommand ??= ReactiveCommand.Create(SaveToVocabulary);
    public ICommand RemoveLineBreaksCommand => _removeLineBreaksCommand ??= ReactiveCommand.Create(RemoveLineBreaks);
    public ICommand RemoveSpacesCommand => _removeSpacesCommand ??= ReactiveCommand.Create(RemoveSpaces);
    public ICommand SingleTranslateCommand => _singleTranslateCommand ??= ReactiveCommand.Create(SingleTranslate);
    public ICommand NavigateCommand => _navigateCommand ??= ReactiveCommand.Create(Navigate);
    public ICommand CancelCommand => ReactiveCommand.Create(() => Debug.WriteLine("Cancel"));
    public ICommand ExitCommand => ReactiveCommand.Create(() => Debug.WriteLine("Exit"));

    #endregion

    #region 命令实现

    private void SwapLanguage()
    {
        // 交换源语言和目标语言
        if (SelectedSourceLanguage == null || SelectedTargetLanguage == null)
        {
            Debug.WriteLine("⚠️ 无法交换：语言未选择");
            return;
        }

        if (SelectedSourceLanguage.Language == Language.Auto)
        {
            Debug.WriteLine("⚠️ 无法交换：源语言为自动检测");
            return;
        }

        // 查找目标语言在源语言列表中的对应项
        var tempTarget = SelectedTargetLanguage;
        var sourceItem = SourceLanguages.FirstOrDefault(l => l.Language == tempTarget.Language);
        
        if (sourceItem != null)
        {
            SelectedSourceLanguage = sourceItem;
            SelectedTargetLanguage = TargetLanguages.FirstOrDefault(l => l.Language == SelectedSourceLanguage.Language) 
                                      ?? TargetLanguages[0];
            
            Debug.WriteLine($"🔄 语言已交换: {SelectedSourceLanguage.DisplayName} ⇆ {SelectedTargetLanguage.DisplayName}");
        }
        else
        {
            Debug.WriteLine("⚠️ 无法交换：目标语言不在源语言列表中");
        }
    }

    private void CopyText(string? text)
    {
        // 💡 TODO: 实现剪贴板功能需要TopLevel/Window引用
        // 暂时只输出debug信息
        if (string.IsNullOrEmpty(text))
            text = InputText;

        System.Diagnostics.Debug.WriteLine($"Copy: {text}");
    }

    private void PlayAudio()
    {
        // TODO: 实现语音播放
        System.Diagnostics.Debug.WriteLine("PlayAudio clicked");
    }

    private void OpenSettings()
    {
        // TODO: 打开设置窗口
        System.Diagnostics.Debug.WriteLine("OpenSettings clicked");
    }

    private void HistoryPrevious()
    {
        System.Diagnostics.Debug.WriteLine("HistoryPrevious clicked");
    }

    private void HistoryNext()
    {
        System.Diagnostics.Debug.WriteLine("HistoryNext clicked");
    }

    private void ChangeColorScheme()
    {
        System.Diagnostics.Debug.WriteLine("ChangeColorScheme clicked");
    }

    private void ScreenshotTranslate()
    {
        System.Diagnostics.Debug.WriteLine("ScreenshotTranslate clicked");
    }

    private void Ocr()
    {
        System.Diagnostics.Debug.WriteLine("Ocr clicked");
    }

    private void SaveToVocabulary()
    {
        System.Diagnostics.Debug.WriteLine("SaveToVocabulary clicked");
    }

    private void RemoveLineBreaks()
    {
        // 移除换行符
        if (!string.IsNullOrEmpty(InputText))
        {
            InputText = InputText.Replace("\n", " ").Replace("\r", "");
        }
    }

    private void RemoveSpaces()
    {
        // 移除空格
        if (!string.IsNullOrEmpty(InputText))
        {
            InputText = InputText.Replace(" ", "");
        }
    }

    private void SingleTranslate()
    {
        System.Diagnostics.Debug.WriteLine("SingleTranslate clicked");
    }

    private void Navigate()
    {
        System.Diagnostics.Debug.WriteLine("Navigate clicked");
    }

    #endregion
}
