using MudBlazor;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Systems;
using TwoChatRedux.Components.Objects.Admin;

namespace TwoChatRedux.Components.Objects.Chat;

public partial class ChatDisplay
{
    private BanDialog ui_banDialog { get; set; } 
    protected override async Task OnInitializedAsync()
    {
        ChatApiClient.AddMessageView(ViewingUser.hash, Message);
        await Task.CompletedTask;
    }
    
    public string Style()
    {
        bool self = ViewingUser.hash == Message.user.hash;
        bool system = Message.header == "System";
        bool mentions = Message.mentions.Where(usr => usr.id == ViewingUser.session.id).Any();

        string css = "";
        if (self)
        {
            css += "margin-left: 55%; background-color: #6200EE; color: white;";
        }
        else
        {
            css += "margin-right: 55%;";
        }

        if (mentions)
        {
            css += "border: 1px solid yellow;";
        }
        
        if (system)
        {
            css += "margin-left: 25%; margin-right: 25%;";
        }

        return css;
    }

    public Color AvatarColor()
    {
        if(Message.header == "System")
        {
            return Color.Default;
        }

        if (Message.user.flags.admin.active)
        {
            return Color.Error;
        }

        return Color.Primary;
    }

    public void DeleteMessage()
    {
        if (ViewingUser.flags.admin.active)
        {
            ChatApiClient.DeleteMessage(Message.id);
            Snackbar.Add("Message deleted.", Severity.Success);
        }
    }

    public void BanUser()
    {
        if (ViewingUser.flags.admin.active)
        {
            if (Message.user.hash == ViewingUser.hash)
            {
                Snackbar.Add("You cannot ban yourself.", Severity.Error);
                return;
            }
            // TODO: ban dialog
            /*
            ChatApiClient.Ban(Message.user.hash,
                new ChatUserBanInformation() { active = true, expiry = DateTime.Now.AddHours(1) });
            Snackbar.Add("User banned.", Severity.Success);
            */
            ui_banDialog.Visible = true;
        }
    }
}