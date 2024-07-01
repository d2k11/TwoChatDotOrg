using TwoChatRedux.API.Models;

namespace TwoChatRedux.API.Systems;

public class ChatSessionManager
{
    public static List<ChatUserSession> Sessions = new();

    public static ChatUserSession Add(string hash)
    {
        int id = int.Parse(Guid.NewGuid().GetHashCode().ToString().Replace("-", "").Substring(0, 6));
        if(Sessions.Where(user => user.id == id).Count() > 0) return Add(hash);
        ChatUserSession newSession = new()
        {
            id = int.Parse(Guid.NewGuid().GetHashCode().ToString().Replace("-", "").Substring(0, 6)),
            expiry = DateTime.Now.AddMinutes(5),
            sessionStart = DateTime.Now,
            lastPeriodicUpdate = DateTime.Now,
            messages = new(),
            settings = new(),
        };
        Sessions.Add(newSession);
        ChatManager.SendSystemChat(newSession.channel.name, "Anonymous #"+newSession.id+" has joined the chat.");
        return newSession;
    }

    public static ChatUserSession? Get(string hash)
    {
        IEnumerable<ChatUser> users = ChatUserManager.Users.Where(user => user.hash == hash);
        if(users.Count() == 0) return null;
        ChatUser user = users.First();
        return user.session;
    }
}