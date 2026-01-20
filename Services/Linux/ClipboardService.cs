using Avalonia.Controls;
using Avalonia.Platform;
using System;
using System.Threading.Tasks;

namespace STranslate.Avalonia.Services.Linux;

/// <summary>
/// Linux 原生剪贴板服务
/// 💡 支持 X11 PRIMARY selection (鼠标选中文本)
/// </summary>
public class ClipboardService
{
    private readonly TopLevel? _topLevel;

    public ClipboardService(TopLevel? topLevel)
    {
        _topLevel = topLevel;
    }

    /// <summary>
    /// 获取剪贴板文本
    /// </summary>
    public async Task<string?> GetTextAsync()
    {
        if (_topLevel == null) return null;

        var clipboard = _topLevel.Clipboard;
        return await clipboard?.GetTextAsync()!;
    }

    /// <summary>
    /// 设置剪贴板文本
    /// </summary>
    public async Task SetTextAsync(string text)
    {
        if (_topLevel == null) return;

        var clipboard = _topLevel.Clipboard;
        await clipboard?.SetTextAsync(text)!;
    }

    /// <summary>
    /// 清空剪贴板
    /// </summary>
    public async Task ClearAsync()
    {
        if (_topLevel == null) return;

        var clipboard = _topLevel.Clipboard;
        await clipboard?.ClearAsync()!;
    }
}
