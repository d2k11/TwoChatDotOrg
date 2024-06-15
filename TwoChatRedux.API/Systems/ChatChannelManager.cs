using TwoChatRedux.API.Models;
using TwoChatRedux.API.Models.Channel;

namespace TwoChatRedux.API.Systems;

public class ChatChannelManager
{
    public static List<ChatChannel> Channels = new()
    {
        new()
        {
            id = 0,
            name = "all",
            display = "all",
            isPublic = true
        },
        new()
        {
            id = 1,
            name = "gaming",
            display = "gaming",
            isPublic = true
        },
        new()
        {
            id = 2,
            name = "pol",
            display = "politics",
            isPublic = true
        },
        new()
        {
            id = 3,
            name = "nsfw",
            display = "not safe for work",
            isPublic = true
        },
        new()
        {
            id = 4,
            name = "tech",
            display = "technology",
            isPublic = true
        },
        new()
        {
            id = 5,
            name = "meta",
            display = "meta",
            isPublic = true
        },
        new()
        {
            id = 6,
            name = "admin",
            display = "administration",
            isPublic = false
        },
    };

    /// <summary>
    /// Get all channels.
    /// </summary>
    /// <returns>All channels.</returns>
    public static List<ChatChannel>? All()
    {
        return Channels;
    }
    
    /// <summary>
    /// Get all channels that match a predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <returns>All channels fitting the predicate.</returns>
    public static List<ChatChannel>? All(Func<ChatChannel, bool> predicate)
    {
        return Channels.Where(predicate).ToList();
    }
    
    /// <summary>
    /// Get a channel by ID.
    /// </summary>
    /// <param name="id">The ID of the channel to retrieve.</param>
    /// <returns>The channel.</returns>
    public static ChatChannel? Find(int id)
    {
        return Channels.FirstOrDefault(c => c.id == id);
    }
    
    /// <summary>
    /// Get a channel by name.
    /// </summary>
    /// <param name="name">The name of the channel to find.</param>
    /// <returns>The channel.</returns>
    public static ChatChannel? Find(string name)
    {
        return Channels.FirstOrDefault(c => c.name.Equals(name));
    }

    /// <summary>
    /// Get users in a channel.
    /// </summary>
    /// <param name="channel">The channel to get users from.</param>
    /// <returns>The list of users in that channel.</returns>
    public static List<ChatUser> GetUsersInChannel(string channel)
    {
        List<ChatUser>? list = ChatUserManager.All(user => user.session.channel.name.Equals(channel));
        if (list is null || list.Count == 0) return new();
        return list;
    }

    /// <summary>
    /// Get users in a channel.
    /// </summary>
    /// <param name="id">The channel to get users from.</param>
    /// <returns>The list of users in that channel.</returns>
    public static List<ChatUser> GetUsersInChannel(int id)
    {
        List<ChatUser>? list = ChatUserManager.All(user => user.session.channel.id.Equals(id));
        if (list is null || list.Count == 0) return new();
        return list;
    }
        

    /// <summary>
    /// Add a new channel.
    /// </summary>
    /// <param name="name">The name of the channel to add.</param>
    /// <returns>The channel.</returns>
    public static ChatChannel Add(ChatChannel channel)
    {
        channel.id = Channels.Count;
        Channels.Add(channel);
        return channel;
    }

    /// <summary>
    /// Create a new private room.
    /// </summary>
    /// <returns>The channel containing the private room.</returns>
    public static ChatChannel AddPrivateRoom()
    {
        string roomId = Guid.NewGuid().GetHashCode().ToString().Replace("-", "");
        ChatChannel room = new()
        {
            name = "room-" + roomId,
            display = "room-" + roomId,
            isPublic = false
        };
        return Add(room);
    }
}