using AstroFramework.Objects;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Handlers.User;

public class DeleteChatHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.IpAddress.ToString();
        if(!(ip.Equals("127.0.0.1") || ip.Equals("::1") || ip.Equals("localhost"))) return Fail("Unauthorized");
        if (!Request.Query.ContainsKey("id")) return Fail("Missing id parameter.");
        
        int id = int.Parse(request.Query["id"]);
        ChatManager.Delete(id);
        return ChatManager.Get(id);
    }
}