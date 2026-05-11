using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace MarkingSystemV2.Services;

/// <summary>
/// Bearer JWT 자동 주입 + 401 시 토큰 갱신 후 재시도 HTTP 클라이언트 래퍼.
/// </summary>
public sealed class ApiClient
{
    private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(15) };
    private static readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

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

        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOpts);
    }

    private async Task<HttpResponseMessage> SendAsync<TRequest>(string path, TRequest body)
        where TRequest : class
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}{path}");
        request.Content = JsonContent.Create(body);

        if (_auth.AccessToken != null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth.AccessToken);

        return await _http.SendAsync(request);
    }
}
