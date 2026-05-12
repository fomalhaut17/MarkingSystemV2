using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarkingSystemV2.Services;

internal sealed class AppModeSelector
{
    [JsonPropertyName("AppMode")] public string? AppMode { get; init; }
}

public sealed class AppSettings
{
    [JsonIgnore]              public string     AppMode { get; set; } = "local";
    [JsonPropertyName("Api")] public ApiSettings Api    { get; init; } = new();

    public static AppSettings Load()
    {
        var dir     = Path.GetDirectoryName(Environment.ProcessPath) ?? AppContext.BaseDirectory;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var modeSelectorPath = Path.Combine(dir, "appsettings.json");
        // appsettings.json 없으면 prod 폴백 — 사용자에게 전달되는 빌드가 대부분 prod이므로
        var mode = "prod";
        if (File.Exists(modeSelectorPath))
        {
            try
            {
                var selector = JsonSerializer.Deserialize<AppModeSelector>(
                    File.ReadAllText(modeSelectorPath), options);
                if (!string.IsNullOrWhiteSpace(selector?.AppMode))
                    mode = selector.AppMode.ToLowerInvariant();
            }
            catch { }
        }

        var modePath = Path.Combine(dir, $"appsettings.{mode}.json");
        if (!File.Exists(modePath)) return new AppSettings { AppMode = mode };

        try
        {
            var settings = JsonSerializer.Deserialize<AppSettings>(
                               File.ReadAllText(modePath), options)
                           ?? new AppSettings();
            settings.AppMode = mode;
            return settings;
        }
        catch
        {
            return new AppSettings { AppMode = mode };
        }
    }
}

public sealed class ApiSettings
{
    // 기본값은 prod. appsettings.{mode}.json 누락 시에도 단일 exe만으로 prod 동작 가능.
    [JsonPropertyName("BaseUrl")]      public string BaseUrl      { get; init; } = "https://wizmes.com";
    [JsonPropertyName("AuthBaseUrl")]  public string AuthBaseUrl  { get; init; } = "https://wizmes.com";
    [JsonPropertyName("LoginCompany")] public string LoginCompany { get; init; } = "DEMO";
}
