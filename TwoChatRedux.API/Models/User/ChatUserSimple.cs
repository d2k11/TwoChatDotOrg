namespace TwoChatRedux.API.Models;

public class ChatUserSimple
{
    /// <summary>
    /// The session ID of this user.
    /// </summary>
    public int id { get; set; }
    
    /// <summary>
    /// The screen name of this user.
    /// </summary>
    public string? screenName { get; set; }
    
    public ChatUserSimple() {}

    /// <summary>
    /// Initialize a new simple user from a full user.
    /// </summary>
    /// <param name="user">The full user.</param>
    public ChatUserSimple(ChatUser user)
    {
        id = user.session.id;
        screenName = user.session.settings.screenName;
    }
}