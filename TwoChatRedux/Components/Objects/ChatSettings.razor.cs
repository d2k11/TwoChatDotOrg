using MudBlazor;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.Components.Objects;

public partial class ChatSettings
{
    private string ui_screenName { get; set; } = string.Empty;
    public void Close()
    {
        Visible = false;
        StateHasChanged();
    }

    public void Save()
    {
        // Publish session settings
        ChatUserSessionSettings session = new()
        {
            screenName = ui_screenName == string.Empty ? null : ui_screenName
        };
        ChatApiClient.PutSessionSettings(User.hash, session);
        
        Snackbar.Add("Settings saved.", Severity.Success);
    }
}