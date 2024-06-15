using MudBlazor;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.Components.Objects.Chat;

public partial class ChatSettingsDisplay
{
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (User is not null)
            {
                ui_screenName = User.session.settings.screenName ?? string.Empty;
            }
        }
        return base.OnAfterRenderAsync(firstRender);
    }

    public void SaveSettings()
    {
        ChatUserSessionSettings sessionSettings = new()
        {
            screenName = ui_screenName
        };
        ChatApiClient.PutSessionSettings(User.hash, sessionSettings);
        
        Snackbar.Add("Settings saved.", Severity.Success);
    }
}