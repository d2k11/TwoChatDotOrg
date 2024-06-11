using AstroFramework.Objects;
using AstroFramework.Systems;

namespace TwoChatRedux.API.Handlers.User;

public class GetHashHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        string ip = Request.Query.ContainsKey("ip") ? Request.Query["ip"] : Request.IpAddress.ToString();

        return new { hash = AstroHashProvider.GetMD5(ip) };
    }
}