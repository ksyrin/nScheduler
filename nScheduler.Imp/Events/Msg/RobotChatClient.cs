using System.Net.Http.Json;

namespace nScheduler.Imp.Events.Msg;

public class RobotChatClient
{
    public static async Task SendTextMessage(string url, string text)
    {
        var body = new
        {
            msgtype = "text",
            text = new
            {
                content = text
            }
        };

        var client = new HttpClient();
        await client.PostAsync(url, JsonContent.Create(body));
    }
}