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
        var mode = "local";
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
    [JsonPropertyName("BaseUrl")]      public string BaseUrl      { get; init; } = "http://localhost:47300";
    [JsonPropertyName("AuthBaseUrl")]  public string AuthBaseUrl  { get; init; } = "http://localhost:47300";
    [JsonPropertyName("LoginCompany")] public string LoginCompany { get; init; } = "DEMO";
}
