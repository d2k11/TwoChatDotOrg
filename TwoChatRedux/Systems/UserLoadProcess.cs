using TwoChatRedux.API.Models;
using TwoChatRedux.API.Models.Channel;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.Systems;

public class UserLoadProcess
{
    public static List<ChatUser> Users { get; set; } = new();

    public static Dictionary<string, List<ChatUser>> UsersByChannel = new();

    /// <summary>
    /// The user update process.
    /// </summary>
    public static async void UserUpdateProcess()
    {
        while (true)
        {
            if (Users is not null)
            {
                lock (Users)
                {
                    Users = ChatApiClient.GetAllUsers();
                }
            }

            List<ChatChannel> channels = await ChatApiClient.GetAllChannels();
            if (channels is not null && channels.Count > 0 && Users is not null)
            {
                foreach (ChatChannel channel in channels)
                {
                    lock (UsersByChannel)
                    {
                        if (!UsersByChannel.ContainsKey(channel.name)) UsersByChannel.Add(channel.name, new());
                        UsersByChannel[channel.name] = Users.Where(x => x.session.channel.id == channel.id).ToList();
                    }
                }
            }

            await Task.Delay(1000);
        }
    }

    /// <summary>
    /// Get all online users.
    /// </summary>
    /// <returns>A list of online users.</returns>
    public static List<ChatUser> GetUsers()
    {
        return Users is not null ? Users : new();
    }

    /// <summary>
    /// Get all online users in a given channel.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<ChatUser> GetUsersInChannel(string name)
    {
        return UsersByChannel.ContainsKey(name) ? UsersByChannel[name] : new();
    }
}