namespace TwoChatRedux.API.Models;

public class ChatUserAdminState
{
    /// <summary>
    /// If this user is an administrator or not.
    /// </summary>
    public bool active { get; set; } = false;
    
    /// <summary>
    /// When this users' administrative access expires.
    /// </summary>
    public DateTime expiry { get; set; } = DateTime.Now;
}