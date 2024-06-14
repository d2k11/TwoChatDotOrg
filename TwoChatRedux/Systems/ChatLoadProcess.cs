using TwoChatRedux.API.Models.Channel;
using TwoChatRedux.API.Models.Message;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.Systems;

public class ChatLoadProcess
{
    public static List<ChatMessage> Chats = new();
    public static Dictionary<string, List<ChatMessage>> ChatsByChannel = new();

    public static async void ChatUpdateProcess()
    {
        while (true)
        {
            List<ChatMessage> chats = await ChatApiClient.GetAllMessages();
            if (chats is not null)
            {
                lock (Chats)
                {
                    Chats = chats.Where(chat => chat.deleted == false).ToList().OrderByDescending(chat => chat.id)
                        .ToList();
                }
            }

            List<ChatChannel> channels = await ChatApiClient.GetAllChannels();
            foreach (ChatChannel channel in channels)
            {
                lock (ChatsByChannel)
                {
                    if (!ChatsByChannel.ContainsKey(channel.name)) ChatsByChannel.Add(channel.name, new());
                    ChatsByChannel[channel.name] = Chats.Where(x => x.channel == channel.name).ToList();
                }
            }
            
            await Task.Delay(250);
        }
    }
    
    public static List<ChatMessage> GetChats()
    {
        return Chats is not null ? Chats : new();
    }
    
    public static List<ChatMessage> GetChatsInChannel(string name)
    {
        return ChatsByChannel.ContainsKey(name) ? ChatsByChannel[name] : new();
    }
}