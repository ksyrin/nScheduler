using nScheduler.Common.Models;
using System.Net.Http.Json;
using System.Text;

namespace nScheduler.Common.Extensions;

public class HttpRequest
{
    private readonly HttpClient client;

    public HttpRequest(HttpClient client) => this.client = client;

    public Task<T?> List<T>(string url)
    {
        return client.GetFromJsonAsync<T>(url);
    }

    public Task<QueryResult<T>?> List<T>(string url, string seachText)
    {
        string reqUrl = url + (seachText.IsEmpty() ? "" : "?" + seachText);
        return client.GetFromJsonAsync<QueryResult<T>>(reqUrl);
    }

    public Task<QueryResult<T>?> Page<T>(string url, int page, int size, string seachText)
    {
        string reqUrl = url + "?page=" + page + "&size=" + size + seachText;
        return client.GetFromJsonAsync<QueryResult<T>>(reqUrl);
    }

    public async Task<QueryResult<T>> Page<T>(string url, int page, int size, object obj)
    {
        string reqUrl = url + "?page=" + page + "&size=" + size;
        var content = await client.PostAsync(reqUrl, JsonContent.Create(obj));
        return (await content.Content.ReadAsStringAsync()).ToObject<QueryResult<T>>();
    }

    public async Task<QueryResult<T>> Page<T>(string url, object obj)
    {
        var content = await client.PostAsync(url, JsonContent.Create(obj));
        return (await content.Content.ReadAsStringAsync()).ToObject<QueryResult<T>>();
    }

    public async Task<bool> Json(string url, object obj)
    {
        var content = await client.PostAsync(url, JsonContent.Create(obj));
        var result = (await content.Content.ReadAsStringAsync()).ToObject<BaseResult>();
        return result.Code == 0 ? true : throw new Exception(result.Msg);
    }

    public Task<T?> Get<T>(string url)
    {
        return client.GetFromJsonAsync<T>(url);
    }

    public async Task<bool> From(string url, string body)
    {
        var content = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded"));
        var result = (await content.Content.ReadAsStringAsync()).ToObject<BaseResult>();
        return result.Code == 0 ? true : throw new Exception(result.Msg);
    }
}