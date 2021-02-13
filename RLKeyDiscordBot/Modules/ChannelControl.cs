using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace RLKeyDiscordBot.Modules
{
    public class ChannelControl : ModuleBase<SocketCommandContext>
    {
        [Command("purge")]
        [Name("purge <amount>")]
        [Summary("Deletes a specified amount of messages")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task DelMesAsync(int delnum)
        {
            if (!(Context.Channel is SocketTextChannel cmdChannel))
            {
                Console.WriteLine("Failed to get the bot channel");
                return;
            }
            var items = await cmdChannel.GetMessagesAsync(delnum + 1).FlattenAsync();
            await cmdChannel.DeleteMessagesAsync(items);
        }

    }
}
