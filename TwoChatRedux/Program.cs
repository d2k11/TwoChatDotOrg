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

new Task(UserLoadProcess.UserUpdateProcess).Start();
new Task(ChatLoadProcess.ChatUpdateProcess).Start();
new Task(ChannelLoadProcess.ChannelUpdateProcess).Start();

app.Run();