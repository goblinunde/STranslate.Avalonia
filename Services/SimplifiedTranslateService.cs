using STranslate.Avalonia.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace STranslate.Avalonia.Services;

/// <summary>
/// 💡 简化的翻译服务 - Stage 1
/// 不依赖Plugin系统，直接使用Google Translate非官方API
/// </summary>
public class SimplifiedTranslateService
{
    private readonly HttpClient _httpClient;
    private const string GoogleTranslateUrl = "https://translate.googleapis.com/translate_a/single";

    public SimplifiedTranslateService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
    }

    /// <summary>
    /// 执行翻译
    /// </summary>
    public async Task<TranslationResult> TranslateAsync(TranslationRequest request)
    {
        var startTime = DateTime.Now;
        var result = new TranslationResult();

        try
        {
            Debug.WriteLine($"开始翻译: {request.Text}");

            // 构建Google Translate API URL
            var url = $"{GoogleTranslateUrl}?client=gtx&sl={request.SourceLang}&tl={request.TargetLang}&dt=t&q={Uri.EscapeDataString(request.Text)}";

            // 发送请求
            var response = await _httpClient.GetStringAsync(url);
            
            Debug.WriteLine($"API响应: {response.Substring(0, Math.Min(200, response.Length))}...");

            // 解析响应
            result.TranslatedText = ParseGoogleResponse(response);
            result.IsSuccess = !string.IsNullOrEmpty(result.TranslatedText);
            result.DetectedLanguage = request.SourceLang == "auto" ? "检测中..." : request.SourceLang;
            
            Debug.WriteLine($"翻译成功: {result.TranslatedText}");
        }
        catch (HttpRequestException ex)
        {
            result.IsSuccess = false;
            result.ErrorMessage = $"网络错误: {ex.Message}";
            Debug.WriteLine($"翻译失败 (网络): {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            result.IsSuccess = false;
            result.ErrorMessage = "翻译超时";
            Debug.WriteLine("翻译失败: 超时");
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.ErrorMessage = $"翻译失败: {ex.Message}";
            Debug.WriteLine($"翻译失败 (未知): {ex.Message}");
        }

        result.Duration = DateTime.Now - startTime;
        return result;
    }

    /// <summary>
    /// 解析Google Translate API响应
    /// 响应格式: [[["翻译文本","原文本",null,null,10]],null,"en",null,null,null,null,[]]
    /// </summary>
    private string ParseGoogleResponse(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
            {
                var firstArray = root[0];
                if (firstArray.ValueKind == JsonValueKind.Array && firstArray.GetArrayLength() > 0)
                {
                    var translationArray = firstArray[0];
                    if (translationArray.ValueKind == JsonValueKind.Array && translationArray.GetArrayLength() > 0)
                    {
                        return translationArray[0].GetString() ?? string.Empty;
                    }
                }
            }

            return string.Empty;
        }
        catch
        {
            // 如果JSON解析失败，尝试简单字符串提取
            var start = json.IndexOf("\"", 3);
            var end = json.IndexOf("\"", start + 1);
            if (start > 0 && end > start)
            {
                return json.Substring(start + 1, end - start - 1);
            }
            return string.Empty;
        }
    }
}
