using TwoChatRedux.API.Models.Channel;
using TwoChatRedux.API.Systems;

namespace TwoChatRedux.Systems;

public class ChannelLoadProcess
{
    public static List<ChatChannel> Channels { get; set; } = new();
    public static async void ChannelUpdateProcess()
    {
        while (true)
        {
            List<ChatChannel> channels = await ChatApiClient.GetAllChannels();
            if (channels is not null)
            {
                lock (Channels)
                {
                    channels.Reverse();
                    Channels = channels;
                }
            }
            
            await Task.Delay(250);
        }
    }
}