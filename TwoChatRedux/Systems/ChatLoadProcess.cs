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
                    chats.Reverse();
                    Chats = chats;
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
}