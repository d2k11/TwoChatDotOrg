namespace TwoChatRedux.Components.Objects;

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
}