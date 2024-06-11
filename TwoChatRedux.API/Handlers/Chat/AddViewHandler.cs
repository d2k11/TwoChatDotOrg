using AstroFramework.Objects;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Models.Message;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.User;

public class AddViewHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.IpAddress.ToString();
        
        if(!(ip.Equals("127.0.0.1") || ip.Equals("::1") || ip.Equals("localhost"))) return Fail("Unauthorized");
        if (!Request.Query.ContainsKey("id")) return Fail("Missing id parameter.");
        if (!Request.Query.ContainsKey("hash")) return Fail("Missing hash parameter.");
        
        int id = int.Parse(request.Query["id"]);
        ChatMessage? message = ChatManager.Get(id);
        ChatUser? user = ChatUserManager.Find(Request.Query["hash"]);

        if (message is null)
        {
            return Fail("Message not found.");
        }

        if (user is null)
        {
            return Fail("User not found.");
        }
        
        if (user.flags.banned.active)
        {
            return Fail("User is banned.");
        }
        
        message.views.Add(new(user));
        return ChatManager.Put(message.id, message);
    }
}