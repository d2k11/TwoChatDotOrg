using System.Text.Json;
using AstroFramework.Objects;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.User;

public class SetUserAdminHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.IpAddress.ToString();
        
        if(!(ip.Equals("127.0.0.1") || ip.Equals("::1") || ip.Equals("localhost"))) return Fail("Unauthorized");
        if (!Request.Query.ContainsKey("hash")) return Fail("Missing hash parameter");
        
        string hash = Request.Query["hash"];
        ChatUserAdminState body = JsonSerializer.Deserialize<ChatUserAdminState>(Request.Body);
        
        ChatUser? user = ChatUserManager.Find(hash);
        if (user is null)
        {
            return Fail("User not found.");
        }

        user.flags.admin = body;
        return ChatUserManager.Put(hash, user);
    }
}