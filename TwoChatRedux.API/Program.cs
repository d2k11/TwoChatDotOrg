using AstroFramework.Models.Objects;
using AstroFramework.Objects;
using TwoChatRedux.API.Handlers;
using TwoChatRedux.API.Handlers.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

AstroRouter router = new(app);

router.AddRoute("/v2/status", AstroHttpMethod.GET, new ServerResponseHandler());

// /v2/user
router.AddRoute("/v2/user/get", AstroHttpMethod.GET, new GetUserHandler());
router.AddRoute("/v2/user/getHash", AstroHttpMethod.GET, new GetHashHandler());
router.AddRoute("/v2/user/set", AstroHttpMethod.POST, new PutUserSettingsHandler());

app.Run();