using System.Text.Json;
using AstroFramework.Objects;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.User.Properties.Session;

public class KillSessionHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.IpAddress.ToString();
        
        if(!(ip.Equals("127.0.0.1") || ip.Equals("::1") || ip.Equals("localhost"))) return Fail("Unauthorized");
        if (!Request.Query.ContainsKey("hash")) return Fail("Missing hash parameter.");
        
        ChatUser? user = ChatUserManager.Find(Request.Query["hash"]);
        if (user is null)
        {
            return Fail("User not found.");
        }
        
        if (user.flags.banned.active)
        {
            return Fail("User is banned.");
        }
        
        if(user.session.expiry <= DateTime.Now)
        {
            return Fail("Session has already expired.");
        }
        
        user.session.expiry = DateTime.Now;
        return ChatUserManager.Put(Request.Query["hash"], user, true);  
    }
}