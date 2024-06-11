using AstroFramework.Models.Responses;
using AstroFramework.Objects;

namespace TwoChatRedux.API.Handlers;

public class ServerResponseHandler : AstroHandler
{
    public override async Task<AstroResponse> Handle(HttpRequest request)
    {
        Initialize(request);

        return new AstroResponse()
        {
            success = true
        };
    }
}