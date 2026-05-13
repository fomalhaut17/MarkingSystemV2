using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MarkingSystemV2.Services;

/// <summary>
/// Bearer JWT 자동 주입 + 401 시 토큰 갱신 후 재시도 HTTP 클라이언트 래퍼.
/// </summary>
public sealed class ApiClient
{
    private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(15) };
    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy        = new SnakeCaseLowerWithDigitSeparator()
    };

    // API 요청/응답 로깅 (테스터·사용자 버그 진단용). 기본 ON.
    // 비활성화: ApiClient.DebugLogEnabled = false 또는 appsettings에 토글 추가 필요
    public static bool DebugLogEnabled { get; set; } = true;
    private const long DebugLogMaxBytes = 5 * 1024 * 1024;   // 5MB → .old로 회전
    private static readonly string DebugLogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ManntekMarkingSystem", "api-debug.log");
    private static readonly object DebugLogLock = new();

    private readonly string _baseUrl;
    private readonly AuthService _auth;

    public ApiClient(string baseUrl, AuthService auth)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _auth    = auth;
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string path, TRequest body)
        where TRequest  : class
        where TResponse : class
    {
        var response = await SendAsync(path, body);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var refreshed = await _auth.RefreshAsync();
            if (refreshed)
                response = await SendAsync(path, body);
        }

        if (DebugLogEnabled)
        {
            var rawBody = await response.Content.ReadAsStringAsync();
            DebugLog($"<-- {(int)response.StatusCode} {response.ReasonPhrase}\n{rawBody}\n");

            if (!response.IsSuccessStatusCode) return null;
            return JsonSerializer.Deserialize<TResponse>(rawBody, _jsonOpts);
        }

        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOpts);
    }

    private async Task<HttpResponseMessage> SendAsync<TRequest>(string path, TRequest body)
        where TRequest : class
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}{path}");
        request.Content = JsonContent.Create(body, options: _jsonOpts);

        if (_auth.AccessToken != null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth.AccessToken);

        if (DebugLogEnabled)
        {
            var bodyJson = JsonSerializer.Serialize(body, _jsonOpts);
            var token    = _auth.AccessToken is { Length: > 12 } t ? $"{t[..6]}…{t[^6..]}" : (_auth.AccessToken ?? "<none>");
            DebugLog($"--> POST {_baseUrl}{path}\nAuthorization: Bearer {token}\nContent-Type: application/json\n{bodyJson}");
        }

        return await _http.SendAsync(request);
    }

    private static void DebugLog(string text)
    {
        try
        {
            var dir = Path.GetDirectoryName(DebugLogPath);
            if (dir != null) Directory.CreateDirectory(dir);
            lock (DebugLogLock)
            {
                // 회전: 5MB 넘으면 .old로 옮기고 새 파일로 시작
                var info = new FileInfo(DebugLogPath);
                if (info.Exists && info.Length > DebugLogMaxBytes)
                {
                    var old = DebugLogPath + ".old";
                    if (File.Exists(old)) File.Delete(old);
                    File.Move(DebugLogPath, old);
                }
                File.AppendAllText(DebugLogPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {text}\n", Encoding.UTF8);
            }
        }
        catch { /* 로깅 실패는 무시 */ }
    }

    // SnakeCaseLower + 글자→숫자 경계에도 underscore 삽입 (예: InjectPress1 → inject_press_1)
    // 백엔드가 WORD_NUMBER 패턴이라 기본 SnakeCaseLower의 letter+digit 결합과 안 맞음.
    // InjectH1 같은 예외(INJECT_H1, no separator)는 모델에서 [JsonPropertyName] 명시.
    private sealed class SnakeCaseLowerWithDigitSeparator : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            var snake = SnakeCaseLower.ConvertName(name);
            var sb    = new StringBuilder(snake.Length + 4);
            for (int i = 0; i < snake.Length; i++)
            {
                if (i > 0 && char.IsDigit(snake[i]) && char.IsLetter(snake[i - 1]))
                    sb.Append('_');
                sb.Append(snake[i]);
            }
            return sb.ToString();
        }
    }
}
