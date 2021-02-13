using System.IO;
using System.Threading.Tasks;
using Discord;

namespace RLKeyDiscordBot.Services
{
    public class Fileservice
    {
        public async Task SendFile(string path, IUser socketUser)
        {
            if (File.Exists(path))
            {
                await socketUser.SendFileAsync(path, "Here you go!");
            }
            else
            {
                await socketUser.SendMessageAsync($"Error: Did not find requested file. ({path})");
            }
        }

        public async Task SendFile(string path, IMessageChannel channel)
        {
            if (File.Exists(path))
            {
                await channel.SendFileAsync(path, "Here you go!");
            }
            else
            {
                await channel.SendMessageAsync($"Error: Did not find requested file. ({path})");
            }
        }
    }
}
