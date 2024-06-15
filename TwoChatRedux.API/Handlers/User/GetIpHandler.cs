using AstroFramework.Objects;

namespace TwoChatRedux.API.Handlers.User;

public class GetIpHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);
        
        return new { ip = Request.IpAddress.ToString() };
    }
}