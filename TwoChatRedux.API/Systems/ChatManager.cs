using TwoChatRedux.API.Models;
using TwoChatRedux.API.Models.Message;

namespace TwoChatRedux.API.Systems;

public class ChatManager
{
    /// <summary>
    /// All messages sent on this server.
    /// </summary>
    public static List<ChatMessage> Messages = new();

    /// <summary>
    /// All messages sent on this server.
    /// </summary>
    /// <returns>A list of all messages sent on this server.</returns>
    public static List<ChatMessage> All()
    {
        return Messages;
    }

    /// <summary>
    /// All messages fitting a certain condition.
    /// </summary>
    /// <param name="predicate">The condition to fit.</param>
    /// <returns>All messages fitting the given condition.</returns>
    public static List<ChatMessage> All(Func<ChatMessage,bool> predicate)
    {
        return Messages.Where(predicate).ToList();
    }

    /// <summary>
    /// Add a message to the server.
    /// </summary>
    /// <param name="hash">The hash of the user publishing the message.</param>
    /// <param name="channel">The channel the message is being published in.</param>
    /// <param name="message">The message being published.</param>
    /// <returns>The message as the application interprets it.</returns>
    public static ChatMessage Add(string hash, string channel, string message)
    {
        return Add(ChatUserManager.Find(hash), channel, message);
    }
    
    /// <summary>
    /// Add a message to the server.
    /// </summary>
    /// <param name="user">The user publishing the message.</param>
    /// <param name="channel">The channel the message is being published in.</param>
    /// <param name="message">The message being published.</param>
    /// <returns>The message as the application interprets it.</returns>
    public static ChatMessage Add(ChatUser user, string channel, string message)
    {
        lock (Messages)
        {
            int id = 0;
            if (Messages.Count != 0)
            {
                id = Messages.OrderByDescending(msg => msg.id).First().id + 1;
            }
            ChatMessage msg = new()
            {
                id = id,
                channel = channel,
                content = message,
                user = user,
                mentions = new()
            };

            // [img]base64:...[/img]
            if (msg.content.Contains("[img]"))
            {
                msg.image = msg.content.Split("[img]")[1].Split("[/img]")[0];
                msg.content = message.Replace("[img]" + msg.image + "[/img]", "");
            }

            // @465012
            if (msg.content.Contains("@"))
            {
                // Only other users in this channel
                foreach (ChatUser onlineUser in ChatUserManager.All(usr => usr.session.channel == user.session.channel))
                {
                    // Contains their ID or screen name
                    if (msg.content.Contains("@" + onlineUser.session.id) ||
                        msg.content.Contains("@ "+onlineUser.session.settings.screenName))
                    {
                        msg.mentions.Add(new(onlineUser));
                    }
                }
            }
            
            // Determine header
            msg.header = user.hash.Equals("System") ? "System" : user.session.settings.screenName != null ? 
                user.session.settings.screenName + " (#" + user.session.id + "-"+msg.id + ")" :
                "Anonymous #"+user.session.id+"-"+msg.id;

            if (msg.mentions.Count != 0)
            {
                msg.header += " mentions ";
                foreach (ChatUserSimple onlineUser in msg.mentions)
                {
                    msg.header += "#" + onlineUser.id + ", ";
                }
                msg.header = msg.header.Substring(0, msg.header.Length - 2);
            }

            msg.views = new() { new(user) };
            
            Messages.Add(msg);
            if(Messages.Count() > 200) Messages.RemoveAt(0);

            return msg;
        }
    }

    /// <summary>
    /// Get a chat message.
    /// </summary>
    /// <param name="id">The ID of the chat message.</param>
    /// <returns>The chat message.</returns>
    public static ChatMessage? Get(int id)
    {
        IEnumerable<ChatMessage> messages = Messages.Where(msg => msg.id == id);
        return messages.Count() != 0 ? messages.First() : null;
    }

    /// <summary>
    /// Get the latest chat message.
    /// </summary>
    /// <returns>The latest chat message, if any are present.</returns>
    public static ChatMessage? GetLatest()
    {
        if(Messages.Count == 0) return null;
        return Messages.Last();
    }

    /// <summary>
    /// Delete a chat message.
    /// </summary>
    /// <param name="id">The ID of the chat message.</param>
    /// <returns>The chat message.</returns>
    public static ChatMessage Delete(int id)
    {
        ChatMessage msg = Messages.Where(msg => msg.id == id).First();
        msg.deleted = true;
        Messages.Remove(Messages.Where(msg => msg.id == id).First());
        Messages.Add(msg);
        return msg;
    }

    /// <summary>
    /// Edit a chat message.
    /// </summary>
    /// <param name="id">The ID of the message to edit.</param>
    /// <param name="message">The message to edit.</param>
    /// <returns>The message as processed by the server.</returns>
    public static ChatMessage Put(int id, ChatMessage message)
    {
        Messages.Remove(Get(id));
        Messages.Add(message);
        return Get(id);
    }

    public static ChatMessage SendSystemChat(string channel, string chat)
    {
        return Add(new ChatUser()
        {
            flags = new()
            {
                banned = new() { active = false },
                typing = new() { active = false }
            },
            session = new()
            {
                channel = ChatChannelManager.Find(channel),
                expiry = DateTime.Now.AddMinutes(30),
                id = 0,
                lastPeriodicUpdate = DateTime.Now,
                messages = new(),
                sessionStart = DateTime.Now,
                settings = new()
            },
            hash = "System",
        }, channel, chat);
    }
}