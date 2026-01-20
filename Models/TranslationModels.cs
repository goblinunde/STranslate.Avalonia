using System;

namespace STranslate.Avalonia.Models;

/// <summary>
/// 翻译请求模型
/// </summary>
public class TranslationRequest
{
    public string Text { get; set; } = string.Empty;
    public string SourceLang { get; set; } = "auto";
    public string TargetLang { get; set; } = "zh";
}

/// <summary>
/// 翻译结果模型
/// </summary>
public class TranslationResult
{
    public bool IsSuccess { get; set; }
    public string TranslatedText { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public string DetectedLanguage { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
}

/// <summary>
/// 语言枚举（简化版）
/// </summary>
public enum Language
{
    Auto,       // 自动检测
    Chinese,    // 中文
    English,    // 英语
    Japanese,   // 日语
    Korean,     // 韩语
    French,     // 法语
    German,     // 德语
    Spanish,    // 西班牙语
    Russian     // 俄语
}

/// <summary>
/// 语言辅助类
/// </summary>
public static class LanguageHelper
{
    public static string ToCode(Language lang)
    {
        return lang switch
        {
            Language.Auto => "auto",
            Language.Chinese => "zh-CN",
            Language.English => "en",
            Language.Japanese => "ja",
            Language.Korean => "ko",
            Language.French => "fr",
            Language.German => "de",
            Language.Spanish => "es",
            Language.Russian => "ru",
            _ => "auto"
        };
    }

    public static string ToDisplayName(Language lang)
    {
        return lang switch
        {
            Language.Auto => "自动检测",
            Language.Chinese => "中文",
            Language.English => "英语",
            Language.Japanese => "日语",
            Language.Korean => "韩语",
            Language.French => "法语",
            Language.German => "德语",
            Language.Spanish => "西班牙语",
            Language.Russian => "俄语",
            _ => "未知"
        };
    }
}
