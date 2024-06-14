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

    public static void UserManagerMonitor()
    {
        while (true)
        {
            List<ChatUser> updates = new();
            List<ChatUser> removals = new();

            List<ChatUser> users = Users;
            foreach (ChatUser user in users)
            {
                bool remove = false;

                if (user.flags.banned.active && user.flags.banned.expiry < DateTime.Now)
                {
                    user.flags.banned.active = false;
                }

                if (user.flags.typing.active && user.flags.typing.expiry < DateTime.Now)
                {
                    user.flags.typing.active = false;
                }

                if (user.session.expiry < DateTime.Now)
                {
                    remove = true;
                }

                TimeSpan elapsedSecond = DateTime.Now - user.session.messages.cleared_second;
                TimeSpan elapsedMinute = DateTime.Now - user.session.messages.cleared_minute;
                TimeSpan elapsedHour = DateTime.Now - user.session.messages.cleared_hour;

                if (elapsedSecond >= TimeSpan.FromSeconds(1))
                {
                    user.session.messages.sent_second = 0;
                    user.session.messages.cleared_second = DateTime.Now;
                }

                if (elapsedMinute >= TimeSpan.FromMinutes(1))
                {
                    user.session.messages.sent_minute = 0;
                    user.session.messages.cleared_minute = DateTime.Now;
                }

                if (elapsedHour >= TimeSpan.FromHours(1))
                {
                    user.session.messages.sent_hour = 0;
                    user.session.messages.cleared_hour = DateTime.Now;
                }

                user.session.lastPeriodicUpdate = DateTime.Now;

                if (!remove)
                {
                    updates.Add(user);
                }
            }
            
            lock (Users) Users = updates;
            Thread.Sleep(1000);
        }
    }

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
    public static ChatUser Put(string hash, ChatUser user, bool bypassWrite = false)
    {
        Users.Remove(Find(hash));
        Users.Add(user);
        if (!bypassWrite)
        {
            if (!Directory.Exists("users")) Directory.CreateDirectory("users");
            File.WriteAllText("users/" + hash + ".json", JsonSerializer.Serialize(user));
        }

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
            if (user is not null)
            {
                ChatUserSession? session = ChatSessionManager.Get(hash);
                user.session = session == null || session.expiry < DateTime.Now
                    ? ChatSessionManager.Add(hash)
                    : session;
                Users.Remove(user);
                Users.Add(user);
            }
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
            ChatUserSession? session = ChatSessionManager.Get(savedUser.hash);
            savedUser.session = session == null || session.expiry < DateTime.Now 
                ? ChatSessionManager.Add(hash) : session;

            return Put(hash, savedUser);
        }
        
        return Put(hash, new() { hash = hash, session = ChatSessionManager.Add(hash) });
    }
}