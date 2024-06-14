using Ganss.Xss;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
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
    private string ui_typing { get; set; } = string.Empty;
    private string ui_messageCounts { get; set; } = string.Empty;
    private ChatSettingsDisplay ui_settingsDisplay { get; set; } = new();
    private ChatUserDisplay ui_chatUserDisplay { get; set; } = new();    
    private ChatChannelDisplay ui_chatChannelDisplay { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        currentUser =
            ChatApiClient.GetUser(ChatApiClient.GetHash(HttpContext.HttpContext.Connection.RemoteIpAddress.ToString()));
        
        new Task(State).Start();
        

        ChatApiClient.SetChannel(currentUser.hash, channel);
    }
    
    public async void State()
    {
        while (true)
        {
            IEnumerable<ChatUser> users = UserLoadProcess.Users.Where(user => user.hash == currentUser.hash);
            if (users.Count() == 0)
            {
                await Task.Delay(1000);
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
            
            IEnumerable<ChatUser> typing = UserLoadProcess.Users.Where(user => user.session.channel.name == channel && user.flags.typing.active);
            int typingCount = typing.Count();
            if (typingCount != 0)
            {
                int counter = 0;
                ui_typing = "";
                foreach (ChatUser user in typing)
                {
                    counter++;
                    if (user.flags.typing.active)
                    {
                        if (counter == typingCount && typingCount != 1)
                        {
                            ui_typing += " and " + user.session.id;
                        }
                        else if (counter == 1)
                        {
                            ui_typing = user.session.id + "";
                        }
                        else
                        {
                            ui_typing += ", " + user.session.id;
                        }
                    }
                }
            }
            else
            {
                ui_typing = "None";
            }

            await Task.Delay(1000);
            InvokeAsync(StateHasChanged);
        }
    }
    
    private void TextFieldKeyDown(KeyboardEventArgs obj)
    {
        if (!currentUser.flags.typing.active)
        {
            ChatApiClient.SetTyping(currentUser.hash, new() { active = true, expiry = DateTime.Now.AddSeconds(3) });
        }

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
        channel = channel.ToLower();
        
        if (string.IsNullOrWhiteSpace(ui_textField) || ui_textField.Length > 512)
        {
            Snackbar.Add("Message must be between 1 and 512 characters.", Severity.Error);
            return;
        }

        ChatApiClient.SetTyping(currentUser.hash, new() { active = false });
        
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

        if (currentUser.session.messages.sent_second > 5)
        {
            ChatApiClient.Ban(currentUser.hash, new() { active = true, expiry = DateTime.Now.AddMinutes(15), reason = "[AutoMod] Spam limit exceeded for 1s" });
        }
        if(currentUser.session.messages.sent_minute > 30)
        {
            ChatApiClient.Ban(currentUser.hash, new() { active = true, expiry = DateTime.Now.AddHours(1), reason = "[AutoMod] Spam limit exceeded for 1m" });
        }
        if(currentUser.session.messages.sent_hour > 100)
        {
            ChatApiClient.Ban(currentUser.hash, new() { active = true, expiry = DateTime.Now.AddHours(24), reason = "[AutoMod] Spam limit exceeded for 1h" });
        }
        
        int remainSecond = 5 - currentUser.session.messages.sent_second;
        int remainMinute = 30 - currentUser.session.messages.sent_minute;
        int remainHour = 100 - currentUser.session.messages.sent_hour;
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

    private async void AddPrivateRoom()
    {
        ChatChannel room = await ChatApiClient.CreateRoom();
        if (room is null)
        {
            Snackbar.Add("Failed to create room.", Severity.Error);
            return;
        }

        ChatApiClient.SetChannel(currentUser.hash, room.name);
        channel = room.name;
        Snackbar.Add("Room created: " + room.name+". Copied URL to clipboard.", Severity.Success);
        CopyToClipboard("https://2chat.org/c/"+room.name);
        Navigation.NavigateTo("/c/"+room.name);
    }

    private void HandleMessageCopy(ChatMessage message)
    {
        CopyToClipboard(message.content);
        Snackbar.Add("Copied "+message.content.Length+" characters to clipboard.", Severity.Success);
    }

    private void HandleMention(ChatMessage message)
    {
        ui_textField += "@" + message.user.session.id + " ";
        StateHasChanged();
    }
    
    private async void CopyToClipboard(string copy)
    {
        await Javascript.InvokeVoidAsync("clipboardCopy.copyText", copy);
    }
}