using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace STranslate.Avalonia.Services.Linux;

/// <summary>
/// Linux 原生截图服务
/// 💡 使用 gnome-screenshot 或 scrot
/// </summary>
public class ScreenshotService
{
    private readonly string _tempPath = Path.Combine(Path.GetTempPath(), "stranslate_screenshot.png");

    /// <summary>
    /// 截取屏幕区域
    /// </summary>
    public async Task<string?> CaptureRegionAsync()
    {
        // 💡 删除旧截图
        if (File.Exists(_tempPath))
        {
            File.Delete(_tempPath);
        }

        // 💡 尝试使用 gnome-screenshot (Fedora/GNOME 默认)
        if (await TryGnomeScreenshotAsync())
        {
            return File.Exists(_tempPath) ? _tempPath : null;
        }

        // 💡 备选方案: 使用 scrot
        if (await TryScrotAsync())
        {
            return File.Exists(_tempPath) ? _tempPath : null;
        }

        return null;
    }

    private async Task<bool> TryGnomeScreenshotAsync()
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "gnome-screenshot",
                    Arguments = $"-a -f {_tempPath}", // -a = 区域选择
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                }
            };

            process.Start();
            await process.WaitForExitAsync();
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> TryScrotAsync()
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "scrot",
                    Arguments = $"-s {_tempPath}", // -s = select area
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            await process.WaitForExitAsync();
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
