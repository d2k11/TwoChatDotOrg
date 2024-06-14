namespace TwoChatRedux.API.Models;

public class ChatUserBanInformation
{
    /// <summary>
    /// If this user's ban is active.
    /// </summary>
    public bool active { get; set; } = false;
    
    /// <summary>
    /// When this user's ban expires.
    /// </summary>
    public DateTime expiry { get; set; } = DateTime.Now;

    /// <summary>
    /// The reason this user was banned.
    /// </summary>
    public string reason { get; set; } = string.Empty;
}