using AstroFramework.Models.Responses;
using AstroFramework.Objects;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.User;

public class GetUserHandler : AstroHandler
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

        return user;
    }
}