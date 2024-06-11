using AstroFramework.Objects;
using TwoChatRedux.API.Models.Channel;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.User;

public class GetAllChannelChatsHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.IpAddress.ToString();
        
        if(!(ip.Equals("127.0.0.1") || ip.Equals("::1") || ip.Equals("localhost"))) return Fail("Unauthorized");
        if(!Request.Query.ContainsKey("name")) return Fail("Missing name parameter");
        
        string name = Request.Query["name"];
        return ChatManager.All(msg => msg.channel == name);
    }
}