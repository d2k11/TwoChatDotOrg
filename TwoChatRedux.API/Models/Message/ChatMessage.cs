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
    /// The content of the message.
    /// </summary>
    public string content { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? image { get; set; }
    
    /// <summary>
    /// The mentions contained in this message.
    /// </summary>
    public List<ChatUser>? mentions { get; set; }
    
    /// <summary>
    /// The user views of this message.
    /// </summary>
    public List<ChatUser>? views { get; set; }
}