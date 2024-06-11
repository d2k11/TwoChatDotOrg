namespace TwoChatRedux.API.Models;

public class ChatUserFlags
{
    /// <summary>
    /// This user's ban information.
    /// </summary>
    public ChatUserBanInformation banned { get; set; } = new();

    /// <summary>
    /// This user's typing state information.
    /// </summary>
    public ChatUserTypingState typing { get; set; } = new();
}