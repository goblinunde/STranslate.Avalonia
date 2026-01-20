namespace STranslate.Avalonia.Models;

/// <summary>
/// 语言选择项 - 用于ComboBox绑定
/// </summary>
public class LanguageItem
{
    public string DisplayName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Language Language { get; set; }

    public LanguageItem(Language language)
    {
        Language = language;
        DisplayName = LanguageHelper.ToDisplayName(language);
        Code = LanguageHelper.ToCode(language);
    }

    public override string ToString() => DisplayName;
}
