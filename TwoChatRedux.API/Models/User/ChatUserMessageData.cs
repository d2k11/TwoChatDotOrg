using System.Runtime.InteropServices.JavaScript;

namespace TwoChatRedux.API.Models;

public class ChatUserMessageData
{
    /// <summary>
    /// Messages the user has sent this second.
    /// </summary>
    public int sent_second { get; set; } = 0;
    
    /// <summary>
    /// Last time the sent_second was cleared.
    /// </summary>
    public DateTime cleared_second { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Messages the user has sent this minute.
    /// </summary>
    public int sent_minute { get; set; } = 0;

    /// <summary>
    /// Last time the sent_minute was cleared.
    /// </summary>
    public DateTime cleared_minute { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Messages the user has sent this hour.
    /// </summary>
    public int sent_hour { get; set; } = 0;

    /// <summary>
    /// Last time the sent_hour was cleared.
    /// </summary>
    public DateTime cleared_hour { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Messages the user has sent this session.
    /// </summary>
    public int sent_session { get; set; } = 0;
}