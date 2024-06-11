using System.Text.Json;
using TwoChatRedux.API.Models;

namespace TwoChatRedux.API.Systems;

/// <summary>
/// The manager for all chat users.
/// </summary>
public class ChatUserManager
{
    /// <summary>
    /// All chat users currently connected to the server.
    /// </summary>
    public static List<ChatUser> Users = new();

    /// <summary>
    /// Get all chat users.
    /// </summary>
    /// <returns>All users.</returns>
    public static List<ChatUser> All()
    {
        return Users;
    }
    
    /// <summary>
    /// Get all chat users matching a condition.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <returns>All users matching the provided condition.</returns>
    public static List<ChatUser> All(Func<ChatUser, bool> predicate)
    {
        return Users.Count != 0 ? Users.Where(predicate).ToList() : new();
    }
    
    /// <summary>
    /// Put a user matching a hash.
    /// </summary>
    /// <param name="hash">The hash to match.</param>
    /// <param name="user">The user to upload.</param>
    /// <returns>The modified user.</returns>
    public static ChatUser Put(string hash, ChatUser user)
    {
        Users.Remove(Find(hash));
        Users.Add(user);
        if(!Directory.Exists("users")) Directory.CreateDirectory("users");
        File.WriteAllText("users/"+hash+".json", JsonSerializer.Serialize(user));
        return Find(hash);
    }

    /// <summary>
    /// Find a chat user by their hash.
    /// </summary>
    /// <param name="hash">The hash to find.</param>
    /// <returns>The associated user information</returns>
    public static ChatUser? Find(string hash)
    {
        ChatUser? user = All(user => user.hash == hash).Count != 0 ? All(user => user.hash == hash).First() : null;
        if (user is null)
        {
            user = File.Exists("users/"+hash+".json") ? JsonSerializer.Deserialize<ChatUser>(File.ReadAllText("users/"+hash+".json")) : null;
            if(user is not null) { user.session = ChatSessionManager.Add(hash); }
        }
        return user;
    }

    
    /// <summary>
    /// Adds a new user to the chat.
    /// </summary>
    /// <param name="hash">The hash to add.</param>
    /// <returns>The new user.</returns>
    public static ChatUser Add(string hash)
    {
        if (Find(hash) != null) return Find(hash);
        
        if(!Directory.Exists("users")) Directory.CreateDirectory("users");
        if(File.Exists("users/"+hash+".json"))
        {
            ChatUser savedUser = JsonSerializer.Deserialize<ChatUser>(File.ReadAllText("users/"+hash+".json"));
            savedUser.session = new();
            return Put(hash, savedUser);
        }
        
        return Put(hash, new() { hash = hash, session = ChatSessionManager.Add(hash) });
    }
}