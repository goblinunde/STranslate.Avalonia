using SharpHook;
using SharpHook.Native;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace STranslate.Avalonia.Services;

/// <summary>
/// Linux 原生全局快捷键服务 (使用 SharpHook)
/// 💡 支持 X11 和 Wayland
/// </summary>
public class HotkeyService : IDisposable
{
    private readonly TaskPoolGlobalHook _hook;
    private Action? _onOpenWindow;
    private Action? _onScreenshot;
    private Action? _onCrosswordTranslate;

    public HotkeyService()
    {
        _hook = new TaskPoolGlobalHook();
        _hook.KeyPressed += OnKeyPressed;
    }

    public async Task StartAsync()
    {
        await _hook.RunAsync();
    }

    public void RegisterOpenWindowHotkey(Action callback)
    {
        _onOpenWindow = callback;
    }

    public void RegisterScreenshotHotkey(Action callback)
    {
        _onScreenshot = callback;
    }

    public void RegisterCrosswordTranslateHotkey(Action callback)
    {
        _onCrosswordTranslate = callback;
    }

    private void OnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        // 💡 Alt+F8: 打开主窗口
        if (e.Data.KeyCode == KeyCode.VcF8 && 
            (e.RawEvent.Mask & ModifierMask.Alt) != 0)
        {
            Dispatcher.UIThread.Post(() => _onOpenWindow?.Invoke());
            return;
        }

        // 💡 Alt+A: 截图翻译
        if (e.Data.KeyCode == KeyCode.VcA && 
            (e.RawEvent.Mask & ModifierMask.Alt) != 0)
        {
            Dispatcher.UIThread.Post(() => _onScreenshot?.Invoke());
            return;
        }

        // 💡 Alt+D: 划词翻译
        if (e.Data.KeyCode == KeyCode.VcD && 
            (e.RawEvent.Mask & ModifierMask.Alt) != 0)
        {
            Dispatcher.UIThread.Post(() => _onCrosswordTranslate?.Invoke());
            return;
        }
    }

    public void Dispose()
    {
        _hook?.Dispose();
    }
}
