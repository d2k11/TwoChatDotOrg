namespace TwoChatRedux.API.Models;

public class ChatUserTypingState
{
    /// <summary>
    /// If this user is actively typing.
    /// </summary>
    public bool active { get; set; }
    
    /// <summary>
    /// When the last typing ping expires.
    /// </summary>
    public DateTime expiry { get; set; }
}