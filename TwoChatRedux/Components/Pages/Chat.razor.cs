using Ganss.Xss;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Models.Channel;
using TwoChatRedux.API.Models.Message;
using TwoChatRedux.API.Systems;
using TwoChatRedux.Components.Objects;
using TwoChatRedux.Components.Objects.Chat;
using TwoChatRedux.Systems;

namespace TwoChatRedux.Components.Pages;

public partial class Chat
{
    private ChatUser? currentUser { get; set; }
    private string ui_textField { get; set; } = string.Empty;
    private bool ui_error { get; set; } = false;
    private string ui_errorText { get; set; } = string.Empty;
    private string ui_messageCounts { get; set; } = string.Empty;
    private ChatSettingsDisplay ui_settingsDisplay { get; set; } = new();
    private ChatUserDisplay ui_chatUserDisplay { get; set; } = new();    
    private ChatChannelDisplay ui_chatChannelDisplay { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        currentUser =
            ChatApiClient.GetUser(ChatApiClient.GetHash(HttpContext.HttpContext.Connection.RemoteIpAddress.ToString()));
        new Thread(State).Start();
        

        ChatApiClient.SetChannel(currentUser.hash, channel);
    }
    
    public void State()
    {
        while (true)
        {
            IEnumerable<ChatUser> users = UserLoadProcess.Users.Where(user => user.hash == currentUser.hash);
            if (users.Count() == 0)
            {
                Thread.Sleep(1000);
                continue;
            }
            currentUser = users.First();
            
            bool banned = currentUser.flags.banned.active && currentUser.flags.banned.expiry > DateTime.Now;
            if (banned)
            {
                ui_error = true;
                ui_errorText = "You are <b><a href=\"/banned\">banned</a></b>. Expires: <b>" +
                               currentUser.flags.banned.expiry.ToString("MM/dd/yyyy HH:mm:ss") + ".</b>";
            }
        
            bool expired = currentUser.session.expiry < DateTime.Now;
            if (expired)
            {
                ui_error = true;
                ui_errorText = "Your session has <b>expired</b>" +
                               ". Please refresh the page.";
            }
            
            InvokeAsync(StateHasChanged);
            Thread.Sleep(1000);
        }
    }
    
    private void TextFieldKeyDown(KeyboardEventArgs obj)
    {
        ChatApiClient.SetTyping(currentUser.hash, new() { active = true, expiry = DateTime.Now.AddSeconds(5) });
        if(obj.Key == "Enter" && !obj.ShiftKey)
        {        
            SendChat();
        }
    }
    
    private async void SendChat()
    {
        if (ui_error)
        {
            Snackbar.Add("you sneaky snake, no cheating!", Severity.Error);
            return;
        }
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

        if (currentUser.session.messages.sent_second > 10)
        {
            ChatApiClient.Ban(currentUser.hash, new() { active = true, expiry = DateTime.Now.AddMinutes(15) });
        }
        if(currentUser.session.messages.sent_minute > 60)
        {
            ChatApiClient.Ban(currentUser.hash, new() { active = true, expiry = DateTime.Now.AddHours(1) });
        }
        if(currentUser.session.messages.sent_hour > 240)
        {
            ChatApiClient.Ban(currentUser.hash, new() { active = true, expiry = DateTime.Now.AddHours(24) });
        }
        
        int remainSecond = 10 - currentUser.session.messages.sent_second;
        int remainMinute = 60 - currentUser.session.messages.sent_minute;
        int remainHour = 240 - currentUser.session.messages.sent_hour;
        ui_messageCounts = remainSecond > 0 ? "<b style=\"color: green\">" + remainSecond + "</b> / " : "<b style=\"color: red\">"+remainSecond+"</b> / ";
        ui_messageCounts += remainMinute > 0 ? "<b style=\"color: green\">" + remainMinute + "</b> / " : "<b style=\"color: red\">"+remainMinute+"</b> / ";
        ui_messageCounts += remainHour > 0 ? "<b style=\"color: green\">" + remainHour + "</b>" : "<b style=\"color: red\">"+remainHour+"</b>";

        ui_textField = string.Empty;
        StateHasChanged();
    }

    private void OpenSettings()
    {
        if (!ui_error)
        {
            ui_settingsDisplay.Visible = true;
        }
    }

    private void OpenUserDisplay()
    {
        if (!ui_error)
        {
            ui_chatUserDisplay.Visible = true;
        }
    }
    
    private void OpenChannelDisplay()
    {
        if (!ui_error)
        {
            ui_chatChannelDisplay.Visible = true;
        }
    }
}