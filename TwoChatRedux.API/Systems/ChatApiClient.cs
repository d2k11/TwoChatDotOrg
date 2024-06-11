using System.Net;
using System.Text;
using System.Text.Json;
using AstroFramework.Models.Responses;
using AstroFramework.Systems;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Models.Channel;
using TwoChatRedux.API.Models.Message;

namespace TwoChatRedux.API.Systems;

public class ChatApiClient
{
    /// <summary>
    /// The path to the API server.
    /// </summary>
    public static string Path { get; set; } = "http://localhost:5555/v2";
    
    /// <summary>
    /// Get the status of the API server.
    /// </summary>
    /// <returns></returns>
    public static bool GetServerStatus()
    {
        try
        {
            return JsonSerializer.Deserialize<AstroResponse>(Get($"{Path}/status")).success;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Get a chat user.
    /// </summary>
    /// <param name="hash">The hash of the chat user.</param>
    /// <returns>The chat user, if any.</returns>
    public static ChatUser? GetUser(string hash)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatUser>(Get($"{Path}/user/get?hash={hash}"));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Get the hash of an IP.
    /// </summary>
    /// <param name="ip">The IP to hash.</param>
    /// <returns>The hash of the IP.</returns>
    public static string GetHash(string ip)
    {
        return AstroHashProvider.GetMD5(ip);
    }

    /// <summary>
    /// Apply user settings.
    /// </summary>
    /// <param name="hash">The hash to apply settings to.</param>
    /// <param name="settings">The settings to apply.</param>
    /// <returns>The user as processed by the server.</returns>
    public static async Task<ChatUser?> PutUserSettings(string hash, ChatUserSettings settings)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatUser>(await Post($"{Path}/user/set?hash={hash}",
                JsonSerializer.Serialize(settings)));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Apply user session settings.
    /// </summary>
    /// <param name="hash">The hash to apply settings to.</param>
    /// <param name="settings">The settings to apply.</param>
    /// <returns>The user as processed by the server.</returns>
    public static async Task<ChatUser?> PutSessionSettings(string hash, ChatUserSessionSettings settings)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatUser>(await Post($"{Path}/user/session?hash={hash}",
                JsonSerializer.Serialize(settings)));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Ban a user.
    /// </summary>
    /// <param name="hash">The hash of the user to ban.</param>
    /// <param name="info">The information related to the ban.</param>
    /// <returns>The user as processed by the server.</returns>
    public static async Task<ChatUser?> Ban(string hash, ChatUserBanInformation info)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatUser>(await Post($"{Path}/user/ban?hash={hash}",
                JsonSerializer.Serialize(info)));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Set a users' typing state.
    /// </summary>
    /// <param name="hash">The hash of the user to change the typing state for.</param>
    /// <param name="state">The typing state.</param>
    /// <returns>The user as processed by the server.</returns>
    public static async Task<ChatUser?> SetTyping(string hash, ChatUserTypingState state)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatUser>(await Post($"{Path}/user/typing?hash={hash}",
                JsonSerializer.Serialize(state)));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Get a users' message data.
    /// </summary>
    /// <param name="hash">The hash of the user to get message data for.</param>
    /// <returns>The message data.</returns>
    public static async Task<ChatUserMessageData?> GetMessageData(string hash)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatUserMessageData>(Get($"{Path}/user/messages?hash={hash}"));
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Set a users' channel.
    /// </summary>
    /// <param name="hash">The hash of the user to set channel for.</param>
    /// <param name="channel">The channel to move the user to.</param>
    /// <returns>The user as processed by the server.</returns>
    public static async Task<ChatUser?> SetChannel(string hash, string channel)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatUser>(Get($"{Path}/user/channel?hash={hash}&channel={channel}"));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a new channel.
    /// </summary>
    /// <param name="channel">The channel to create.</param>
    /// <returns>The created channel as processed by the server.</returns>
    public static async Task<ChatChannel?> CreateChannel(ChatChannel channel)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatChannel>(await Post($"{Path}/channel/create",
                JsonSerializer.Serialize(channel)));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a new private room.
    /// </summary>
    /// <returns>The created channel as processed by the server.</returns>
    public static async Task<ChatChannel?> CreateRoom()
    {
        try
        {
            return JsonSerializer.Deserialize<ChatChannel>(Get($"{Path}/channel/room"));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets a channel.
    /// </summary>
    /// <param name="channel">The name of the channel to get.</param>
    /// <returns>The channel.</returns>
    public static async Task<ChatChannel?> GetChannel(string channel)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatChannel>(Get($"{Path}/channel/get?name={channel}"));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets users in a channel.
    /// </summary>
    /// <param name="channel">The channel to get users in.</param>
    /// <returns>The list of users in that channel, if any.</returns>
    public static async Task<List<ChatUser>?> GetChannelUsers(string channel)
    {
        try
        {
            return JsonSerializer.Deserialize<List<ChatUser>>(Get($"{Path}/channel/users?name={channel}"));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Get a chat message.
    /// </summary>
    /// <param name="id">The ID of the message to get.</param>
    /// <returns>The chat message with the given ID, if any.</returns>
    public static ChatMessage? GetMessage(int id)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatMessage>(Get($"{Path}/chat/get?id={id}"));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Send a chat message.
    /// </summary>
    /// <param name="hash">The hash sending the message.</param>
    /// <param name="message">The message to send.</param>
    /// <returns>The message as processed by the server.</returns>
    public static async Task<ChatMessage?> SendMessage(string hash, ChatMessage message)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatMessage>(await Post($"{Path}/chat/send?hash=" + hash,
                JsonSerializer.Serialize(message)));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Delete a message.
    /// </summary>
    /// <param name="id">The ID of the message to delete.</param>
    /// <returns>The message as processed by the server.</returns>
    public static async Task<ChatMessage?> DeleteMessage(int id)
    {
        try
        {
            return JsonSerializer.Deserialize<ChatMessage>(Get($"{Path}/chat/delete?id={id}"));
        }
        catch
        {
            return null;
        }
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