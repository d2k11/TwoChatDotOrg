using AstroFramework.Objects;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Models.Channel;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.User.Properties;

public class UserSwitchChannelHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.IpAddress.ToString();
        
        if(!(ip.Equals("127.0.0.1") || ip.Equals("::1") || ip.Equals("localhost"))) return Fail("Unauthorized");
        if (!Request.Query.ContainsKey("hash")) return Fail("Missing hash parameter");
        string hash = Request.Query["hash"];
        if (!Request.Query.ContainsKey("channel")) return Fail("Missing channel/channelId parameter");
        string channel = Request.Query["channel"];
        
        ChatUser? user = ChatUserManager.Find(hash);
        if (user is null)
        {
            return Fail("User not found");
        }
        
        if (user.flags.banned.active)
        {
            return Fail("User is banned");
        }
        
        ChatChannel? chatChannel = ChatChannelManager.Find(channel);
        if (chatChannel is null)
        {
            return Fail("Channel not found");
        }

        user.session.channel = chatChannel;
        return ChatUserManager.Put(hash, user, true);
    }
}