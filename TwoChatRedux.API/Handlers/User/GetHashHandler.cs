using AstroFramework.Objects;
using AstroFramework.Systems;

namespace TwoChatRedux.API.Handlers.User;

public class GetHashHandler : AstroHandler
{
    public override async Task<object> Handle(HttpRequest request)
    {
        Initialize(request);

        return new { hash = AstroHashProvider.GetMD5(Request.IpAddress.ToString()) };
    }
}