using TwoChatRedux.API.Models.Channel;

namespace TwoChatRedux.API.Models;

public class ChatUser
{
    /// <summary>
    /// The hash of this user.
    /// </summary>
    public string hash { get; set; }
    
    /// <summary>
    /// The settings of this user.
    /// </summary>
    public ChatUserSettings settings { get; set; } = new();
    
    /// <summary>
    /// The flags associated with this user.
    /// </summary>
    public ChatUserFlags flags { get; set; } = new();
    
    /// <summary>
    /// The session information for this user.
    /// </summary>
    public ChatUserSession session { get; set; } = new();
}