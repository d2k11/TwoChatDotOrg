using TwoChatRedux.API.Models.Channel;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.API.Models;

public class ChatUserSession
{
    /// <summary>
    /// The ID of this session.
    /// </summary>
    public int id { get; set; }

    /// <summary>
    /// The channel of this user.
    /// </summary>
    public ChatChannel channel { get; set; } = ChatChannelManager.Channels[0];
    
    /// <summary>
    /// The expiry of this session.
    /// </summary>
    public DateTime expiry { get; set; }

    /// <summary>
    /// The start of this session.
    /// </summary>
    public DateTime sessionStart { get; set; } = DateTime.Now;
    
    /// <summary>
    /// The last periodic update.
    /// </summary>
    public DateTime lastPeriodicUpdate { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Data regarding the messages the user has sent.
    /// </summary>
    public ChatUserMessageData messages { get; set; } = new();
    
    /// <summary>
    /// Settings for this session.
    /// </summary>
    public ChatUserSessionSettings settings { get; set; } = new();
}