using TwoChatRedux.Components;
using MudBlazor.Services;
using TwoChatRedux.API.Systems;
using TwoChatRedux.Systems;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

while (!ChatApiClient.GetServerStatus())
{
}

new Thread(UserLoadProcess.UserUpdateProcess).Start();
new Thread(ChatLoadProcess.ChatUpdateProcess).Start();
new Thread(ChannelLoadProcess.ChannelUpdateProcess).Start();

app.Run();