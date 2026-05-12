using System.Reflection;

namespace MarkingSystemV2.Services;

public static class AppVersion
{
    /// <summary>
    /// csproj &lt;Version&gt;에서 가져오는 SemVer (예: "0.1.0").
    /// </summary>
    public static string Current { get; } = LoadVersion();

    private static string LoadVersion()
    {
        var info = typeof(AppVersion).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        if (string.IsNullOrWhiteSpace(info)) return "0.0.0";

        // "0.1.0+sha" 형태에서 +sha 제거
        var plus = info.IndexOf('+');
        return plus >= 0 ? info[..plus] : info;
    }
}
