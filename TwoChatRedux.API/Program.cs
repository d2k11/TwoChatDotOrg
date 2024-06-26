using AstroFramework.Models.Objects;
using AstroFramework.Objects;
using Microsoft.AspNetCore.HttpOverrides;
using TwoChatRedux.API.Handlers;
using TwoChatRedux.API.Handlers.Channel;
using TwoChatRedux.API.Handlers.User;
using TwoChatRedux.API.Handlers.User.Properties;
using TwoChatRedux.API.Handlers.User.Properties.Session;
using TwoChatRedux.API.Systems;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | 
        ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

app.UseHttpsRedirection();
app.UseForwardedHeaders();

AstroRouter router = new(app);

router.AddRoute("/v2/status", AstroHttpMethod.GET, new ServerResponseHandler());

// /v2/user
router.AddRoute("/v2/user/get", AstroHttpMethod.GET, new GetUserHandler());
router.AddRoute("/v2/user/all", AstroHttpMethod.GET, new GetAllUsersHandler());
router.AddRoute("/v2/user/hash", AstroHttpMethod.GET, new GetHashHandler());
router.AddRoute("/v2/user/set", AstroHttpMethod.POST, new PutUserSettingsHandler());
router.AddRoute("/v2/user/session", AstroHttpMethod.POST, new PutSessionSettingsHandler());
router.AddRoute("/v2/user/bump", AstroHttpMethod.GET, new BumpSessionHandler());
router.AddRoute("/v2/user/kill", AstroHttpMethod.GET, new KillSessionHandler());
router.AddRoute("/v2/user/messages", AstroHttpMethod.GET, new UserMessageDataHandler());
router.AddRoute("/v2/user/channel", AstroHttpMethod.GET, new UserSwitchChannelHandler());
router.AddRoute("/v2/user/ban", AstroHttpMethod.POST, new SetUserBannedHandler());
router.AddRoute("/v2/user/typing", AstroHttpMethod.POST, new SetUserTypingHandler());
router.AddRoute("/v2/user/admin", AstroHttpMethod.POST, new SetUserAdminHandler());
router.AddRoute("/v2/user/ip", AstroHttpMethod.GET, new GetIpHandler());

// /v2/chat
router.AddRoute("/v2/chat/get", AstroHttpMethod.GET, new GetChatHandler());
router.AddRoute("/v2/chat/send", AstroHttpMethod.POST, new SendChatHandler());
router.AddRoute("/v2/chat/delete", AstroHttpMethod.GET, new DeleteChatHandler());
router.AddRoute("/v2/chat/latest", AstroHttpMethod.GET, new GetLatestChatHandler());
router.AddRoute("/v2/chat/all", AstroHttpMethod.GET, new GetAllChatsHandler());
router.AddRoute("/v2/chat/channel", AstroHttpMethod.GET, new GetAllChannelChatsHandler());
router.AddRoute("/v2/chat/view", AstroHttpMethod.GET, new AddViewHandler());
router.AddRoute("/v2/chat/like", AstroHttpMethod.GET, new AddRemoveLikeHandler());

// /v2/channel
router.AddRoute("/v2/channel/get", AstroHttpMethod.GET, new GetChannelHandler());
router.AddRoute("/v2/channel/users", AstroHttpMethod.GET, new GetChannelUsersHandler());
router.AddRoute("/v2/channel/create", AstroHttpMethod.POST, new CreateChannelHandler());
router.AddRoute("/v2/channel/room", AstroHttpMethod.GET, new CreateRoomHandler());
router.AddRoute("/v2/channel/all", AstroHttpMethod.GET, new GetAllChannelsHandler());

// Start User Handler
new Thread(ChatUserManager.UserManagerMonitor).Start();

app.Run();