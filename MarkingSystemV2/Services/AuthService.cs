using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarkingSystemV2.Services;

/// <summary>
/// JWT 인증 서비스.
/// Access Token  : 항상 메모리에만 보관 (앱 종료 시 소멸).
/// Refresh Token : rememberMe=true 시 파일(%APPDATA%\ManntekMarkingSystem\auth.json) 저장,
///                 false 시 메모리에만 보관.
/// </summary>
public sealed class AuthService
{
    private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(10) };

    private readonly string _authBaseUrl;
    private readonly string _tokenFilePath;

    private string? _accessToken;
    private string? _refreshToken;
    private bool    _rememberMe;

    public string? SavedLoginId { get; private set; }

    public event EventHandler? LogoutRequested;

    public AuthService(string authBaseUrl)
    {
        _authBaseUrl   = authBaseUrl.TrimEnd('/');
        _tokenFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ManntekMarkingSystem", "auth.json");
    }

    public string? AccessToken => _accessToken;

    // ── 자동 로그인 ────────────────────────────────────────────────────────────

    public async Task<bool> TryAutoLoginAsync()
    {
        LoadFromFile();
        if (string.IsNullOrEmpty(_refreshToken)) return false;

        _rememberMe = true;
        return await RefreshAsync();
    }

    // ── 로그인 ─────────────────────────────────────────────────────────────────

    public async Task<string?> LoginAsync(string loginCompany, string loginId, string loginPassword, bool rememberMe)
    {
        try
        {
            var body     = new { login_company = loginCompany, login_id = loginId, login_password = loginPassword };
            var response = await _http.PostAsJsonAsync($"{_authBaseUrl}/api/login", body);
            var result   = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (response.IsSuccessStatusCode && result?.Ok == true &&
                result.AccessToken != null && result.RefreshToken != null)
            {
                _accessToken  = result.AccessToken;
                _refreshToken = result.RefreshToken;
                _rememberMe   = rememberMe;

                if (rememberMe)
                {
                    SavedLoginId = loginId;
                    SaveToFile(loginCompany, loginId);
                }
                else
                {
                    SavedLoginId = null;
                    DeleteTokenFile();
                }

                return null;
            }

            if (!string.IsNullOrWhiteSpace(result?.Message))
                return MapServerErrorCode(result.Message);

            return (int)response.StatusCode switch
            {
                >= 500 => "서버 오류가 발생했습니다. 잠시 후 다시 시도하세요.",
                _      => "로그인에 실패했습니다."
            };
        }
        catch (TaskCanceledException)
        {
            return "서버 응답이 없습니다. 잠시 후 다시 시도하세요.";
        }
        catch (HttpRequestException)
        {
            return "서버에 연결할 수 없습니다. 네트워크를 확인하세요.";
        }
        catch (Exception ex)
        {
            return $"오류가 발생했습니다: {ex.Message}";
        }
    }

    // ── 토큰 갱신 ──────────────────────────────────────────────────────────────

    public async Task<bool> RefreshAsync()
    {
        if (string.IsNullOrEmpty(_refreshToken)) return false;

        try
        {
            var body     = new { refreshToken = _refreshToken };
            var response = await _http.PostAsJsonAsync($"{_authBaseUrl}/api/auth/refresh", body);
            var result   = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (response.IsSuccessStatusCode && result?.Ok == true &&
                result.AccessToken != null && result.RefreshToken != null)
            {
                _accessToken  = result.AccessToken;
                _refreshToken = result.RefreshToken;

                if (_rememberMe) SaveToFile(null, null);

                return true;
            }
        }
        catch { }

        Logout();
        LogoutRequested?.Invoke(this, EventArgs.Empty);
        return false;
    }

    // ── 로그아웃 ───────────────────────────────────────────────────────────────

    public void Logout()
    {
        _accessToken  = null;
        _refreshToken = null;
        _rememberMe   = false;
        DeleteTokenFile();
    }

    // ── 에러 코드 변환 ─────────────────────────────────────────────────────────

    private static string MapServerErrorCode(string code) => code switch
    {
        "INVALID_LOGIN" => "아이디 또는 비밀번호가 올바르지 않습니다.",
        _               => $"로그인에 실패했습니다. ({code})"
    };

    // ── 파일 I/O ──────────────────────────────────────────────────────────────

    private void LoadFromFile()
    {
        try
        {
            if (!File.Exists(_tokenFilePath)) return;
            var data = JsonSerializer.Deserialize<StoredToken>(File.ReadAllText(_tokenFilePath));
            _refreshToken = data?.RefreshToken;
            SavedLoginId  = data?.LoginId;
        }
        catch { }
    }

    private void SaveToFile(string? loginCompany, string? loginId)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_tokenFilePath)!);
            var stored = new StoredToken(_refreshToken, loginCompany ?? "", loginId ?? SavedLoginId ?? "");
            File.WriteAllText(_tokenFilePath, JsonSerializer.Serialize(stored));
        }
        catch { }
    }

    private void DeleteTokenFile()
    {
        try { if (File.Exists(_tokenFilePath)) File.Delete(_tokenFilePath); }
        catch { }
    }

    // ── DTOs ──────────────────────────────────────────────────────────────────

    private sealed record StoredToken(
        [property: JsonPropertyName("refreshToken")]  string? RefreshToken,
        [property: JsonPropertyName("login_company")] string? LoginCompany,
        [property: JsonPropertyName("login_id")]      string? LoginId);

    private sealed class LoginResponse
    {
        [JsonPropertyName("ok")]           public bool    Ok           { get; init; }
        [JsonPropertyName("message")]      public string? Message      { get; init; }
        [JsonPropertyName("accessToken")]  public string? AccessToken  { get; init; }
        [JsonPropertyName("refreshToken")] public string? RefreshToken { get; init; }
    }
}
