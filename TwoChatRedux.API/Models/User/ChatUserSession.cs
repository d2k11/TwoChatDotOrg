namespace TwoChatRedux.API.Models;

public class ChatUserSession
{
    /// <summary>
    /// The ID of this session.
    /// </summary>
    public int id { get; set; }
    
    /// <summary>
    /// The expiry of this session.
    /// </summary>
    public DateTime expiry { get; set; }
    
    /// <summary>
    /// Data regarding the messages the user has sent.
    /// </summary>
    public ChatUserMessageData messages { get; set; } = new();
    
    /// <summary>
    /// Settings for this session.
    /// </summary>
    public ChatUserSessionSettings settings { get; set; } = new();
}