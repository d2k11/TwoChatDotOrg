namespace TwoChatRedux.API.Models;

public class ChatUserTypingState
{
    public bool active { get; set; }
    public DateTime expiry { get; set; }
}