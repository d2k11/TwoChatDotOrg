using TwoChatRedux.API.Models;

namespace TwoChatRedux.API.Systems;

public class ChatSessionManager
{
    public static List<ChatUserSession> Sessions = new();

    public static ChatUserSession Add(string hash)
    {
        ChatUserSession newSession = new()
        {
            id = int.Parse(Guid.NewGuid().GetHashCode().ToString().Replace("-", "").Substring(0, 6)),
            expiry = DateTime.Now.AddMinutes(30),
            messages = new(),
            settings = new(),
        };
        Sessions.Add(newSession);
        return newSession;
    }
}