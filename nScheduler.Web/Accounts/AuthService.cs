using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using nScheduler.Common.Models;
using nScheduler.Domain.ViewModels.Accounts;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace nScheduler.Web.Accounts;

public class AuthService
{
    private readonly HttpClient httpClient;
    private readonly AuthenticationStateProvider authenticationStateProvider;
    private readonly ILocalStorageService localStorage;

    public AuthService(HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage)
    {
        this.httpClient = httpClient;
        this.authenticationStateProvider = authenticationStateProvider;
        this.localStorage = localStorage;
    }

    public async Task<BaseResult> Login(LoginViewModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);
        var response = await httpClient.PostAsync("api/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
        var loginResult = JsonSerializer.Deserialize<BaseResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        if (!response.IsSuccessStatusCode && loginResult == null)
        {
            return new BaseResult
            {
                Code = -1,
                Msg = "网络连接失败"
            };
        }

        if (loginResult.Code == 0)
        {
            await localStorage.SetItemAsync("authToken", loginResult.Msg);

            ((ApiAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated(loginResult.Msg);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Msg);
        }

        return loginResult;
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();
        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}