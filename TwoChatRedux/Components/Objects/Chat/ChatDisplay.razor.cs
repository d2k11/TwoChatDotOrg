using MudBlazor;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.Components.Objects.Chat;

public partial class ChatDisplay
{
    public string Style()
    {
        bool self = ViewingUser.hash == Message.user.hash;
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

        return css;
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
            ChatApiClient.Ban(Message.user.hash,
                new ChatUserBanInformation() { active = true, expiry = DateTime.Now.AddHours(1) });
            Snackbar.Add("User banned.", Severity.Success);
        }
    }
}