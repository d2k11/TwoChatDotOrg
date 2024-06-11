using System.Net;
using System.Text;
using System.Text.Json;
using AstroFramework.Models.Responses;
using TwoChatRedux.API.Models.Message;

namespace TwoChatRedux.API.Systems;

public class ChatApiClient
{
    public static string Path { get; set; } = "http://localhost:5555/v2";
    
    public static bool GetServerStatus()
    {
        return JsonSerializer.Deserialize<AstroResponse>(Get($"{Path}/status")).success;    
    }
    
    // Helper Methods
    
    private static void Hit(string url)
    {
        new WebClient().DownloadString(url);
    }
    
    private static string Get(string url)
    {
        return new WebClient().DownloadString(url);
    }
    
    private static async Task<string> Post(string url, string body)
    {
        HttpResponseMessage msg = await new HttpClient().PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
        string reply = await new StreamReader(await msg.Content.ReadAsStreamAsync()).ReadToEndAsync();
        return reply;
    }
}