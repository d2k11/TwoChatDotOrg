namespace TwoChatRedux.API.Models.Message;

/// <summary>
/// Defines a chat message.
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// The ID of the message.
    /// </summary>
    public int id { get; set; } = -1;
    
    /// <summary>
    /// The channel this message was sent in.
    /// </summary>
    public string channel { get; set; } = string.Empty;
    
    /// <summary>
    /// The user that sent this message.
    /// </summary>
    public ChatUser user { get; set; } = new();
    
    /// <summary>
    /// The content of the message.
    /// </summary>
    public string content { get; set; } = string.Empty;

    /// <summary>
    /// The image associated with this message.
    /// </summary>
    public string? image { get; set; }
    
    /// <summary>
    /// The header to display with this message.
    /// </summary>
    public string header { get; set; }
    
    /// <summary>
    /// If this message is deleted.
    /// </summary>
    public bool deleted { get; set; }
    
    /// <summary>
    /// The mentions contained in this message.
    /// </summary>
    public List<ChatUserSimple>? mentions { get; set; }
    
    /// <summary>
    /// The user views of this message.
    /// </summary>
    public List<ChatUserSimple>? views { get; set; }
}