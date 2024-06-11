using AstroFramework.Objects;
using TwoChatRedux.API.Models.Channel;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.Channel;

public class GetChannelHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.IpAddress.ToString();
        
        if(!(ip.Equals("127.0.0.1") || ip.Equals("::1") || ip.Equals("localhost"))) return Fail("Unauthorized");
        if (!Request.Query.ContainsKey("id") && !Request.Query.ContainsKey("name")) return Fail("Missing id/name parameter.");

        string query = string.Empty;
        if(Request.Query.ContainsKey("id")) query = Request.Query["id"];
        if(Request.Query.ContainsKey("name")) query = Request.Query["name"];

        ChatChannel? channel = null;
        if(Request.Query.ContainsKey("name")) {
            channel = ChatChannelManager.Find(query);
        }
        else
        {
            channel = ChatChannelManager.Find(int.Parse(query));
        }
        
        if (channel is null)
        {
            return Fail("Channel not found.");
        }

        return channel;
    }
}