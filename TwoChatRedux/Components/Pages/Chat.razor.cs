using Ganss.Xss;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Models.Message;
using TwoChatRedux.API.Systems;
using TwoChatRedux.Components.Objects;

namespace TwoChatRedux.Components.Pages;

public partial class Chat
{
    private ChatUser currentUser { get; set; }
    private string ui_textField { get; set; } = string.Empty;
    private bool ui_settingsVisible { get; set; } = false;
    private ChatSettings ui_settings { get; set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        currentUser =
            ChatApiClient.GetUser(ChatApiClient.GetHash(HttpContext.HttpContext.Connection.RemoteIpAddress.ToString()));
        new Thread(State).Start();
    }
    
    private void TextFieldKeyDown(KeyboardEventArgs obj)
    {
        if(obj.Key == "Enter" && !obj.ShiftKey)
        {        
            SendChat();
        }
    }
    
    private async void SendChat()
    {
        ui_textField = new HtmlSanitizer().Sanitize(ui_textField);
        
        if (string.IsNullOrWhiteSpace(ui_textField) || ui_textField.Length > 512)
        {
            Snackbar.Add("Message must be between 1 and 512 characters.", Severity.Error);
            return;
        }
        
        StateHasChanged();
        ChatMessage? message = await ChatApiClient.SendMessage(currentUser.hash, new()
        {
            user = currentUser,
            content = ui_textField,
            channel = channel
        });
        if (message is null)
        {
            Snackbar.Add("Failed to send message.", Severity.Error);
        }

        ui_textField = string.Empty;
        StateHasChanged();
    }

    private void OpenSettings()
    {
        ui_settings.Visible = true;
    }

    public void State()
    {
        while (true)
        {
            InvokeAsync(StateHasChanged);
            Thread.Sleep(1000);
        }
    }
}