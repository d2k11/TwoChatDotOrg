using AstroFramework.Objects;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.User.Properties;

public class GetAllUsersHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.IpAddress.ToString();
        
        if(!(ip.Equals("127.0.0.1") || ip.Equals("::1") || ip.Equals("localhost"))) return Fail("Unauthorized");

        return ChatUserManager.All();
    }
}