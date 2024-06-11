namespace TwoChatRedux.API.Models;

public class ChatUserMessageData
{
    /// <summary>
    /// Messages the user has sent this second.
    /// </summary>
    public int sent_second { get; set; } = 0;
    
    /// <summary>
    /// Messages the user has sent this minute.
    /// </summary>
    public int sent_minute { get; set; } = 0;
    
    /// <summary>
    /// Messages the user has sent this hour.
    /// </summary>
    public int sent_hour { get; set; } = 0;
    
    /// <summary>
    /// Messages the user has sent this session.
    /// </summary>
    public int sent_session { get; set; } = 0;
}