namespace TwoChatRedux.API.Models.Channel;

/// <summary>
/// Defines a chat channel.
/// </summary>
public class ChatChannel
{
    /// <summary>
    /// The ID of the channel.
    /// </summary>
    public int id { get; set; } = -1;
    
    /// <summary>
    /// The name of the channel.
    /// </summary>
    public string name { get; set; } = string.Empty; // like "mc"
    
    /// <summary>
    /// The display name of the channel.
    /// </summary>
    public string display { get; set; } = string.Empty; // like "minecraft"

    /// <summary>
    /// If this is a public channel that should be displayed in the channel list.
    /// </summary>
    public bool isPublic { get; set; } = true;
}