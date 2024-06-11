using System.Text.Json;
using AstroFramework.Objects;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Models.Message;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.User;

public class SendChatHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.IpAddress.ToString();
        
        if(!(ip.Equals("127.0.0.1") || ip.Equals("::1") || ip.Equals("localhost"))) return Fail("Unauthorized");
        if (!Request.Query.ContainsKey("hash")) return Fail("Missing hash parameter.");
        
        ChatUser? user = ChatUserManager.Find(Request.Query["hash"]);
        if (user is null || (user is not null && user.session.expiry < DateTime.Now))
        {
            user = ChatUserManager.Add(Request.Query["hash"]);
        }

        if (user.flags.banned.active)
        {
            return Fail("User is banned.");
        }

        user.session.messages.sent_session++;
        user.session.messages.sent_second++;
        user.session.messages.sent_minute++;
        user.session.messages.sent_hour++;
        
        ChatMessage body = JsonSerializer.Deserialize<ChatMessage>(Request.Body);
        ChatMessage returned = ChatManager.Add(user, body.channel, body.content);
        return returned;
    }
}