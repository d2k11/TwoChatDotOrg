using MudBlazor;
using TwoChatRedux.API.Models;
using TwoChatRedux.API.Systems;
using TwoChatRedux.Components.Objects.Admin;

namespace TwoChatRedux.Components.Objects.Chat;

public partial class ChatDisplay
{
    private UserInfoDialog ui_userInfoDialog { get; set; } 
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
            css += "margin-left: 55%; background: linear-gradient(180deg, rgba(2,0,36,1) 0%, rgba(98,0,238,1) 0%, rgba(180,56,255,1) 100%); color: white;";
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

    public Color ThumbsUpColor()
    {
        if(Message.likes.Where(user => user.id == ViewingUser.session.id).Any())
        {
            return Color.Primary;
        }

        return Color.Default;
    }

    public void DeleteMessage()
    {
        if (ViewingUser.flags.admin.active)
        {
            ChatApiClient.DeleteMessage(Message.id);
            Snackbar.Add("Message deleted.", Severity.Success);
        }
    }

    public void ShowUserInfo()
    {
        ui_userInfoDialog.Visible = true;
    }
}