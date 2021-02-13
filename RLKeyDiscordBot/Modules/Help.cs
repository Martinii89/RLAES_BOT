using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLKeyDiscordBot.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
	{

		public CommandService _commands { get; set; }

		[Command("Help")]
		[Summary("Prints this")]
		public async Task HelpCommand()
		{
			List<CommandInfo> commands = _commands.Commands.ToList();
			EmbedBuilder embedBuilder = new EmbedBuilder();

			foreach (CommandInfo command in commands)
			{
				if (command.Name == "Help") continue;
				// Get the command Summary attribute information
				string embedFieldText = command.Summary ?? "No description available\n";

				embedBuilder.AddField(command.Name, embedFieldText);
			}

			await ReplyAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
			if (Context.Channel is SocketTextChannel cmdChannel && GetPermission(cmdChannel).ManageMessages)
			{
				await cmdChannel.DeleteMessageAsync(Context.Message.Id);
			}
			
		}

		public ChannelPermissions GetPermission(SocketTextChannel channel)
		{
			var guildUser = channel.Guild.GetUser(Context.Client.CurrentUser.Id);
			return guildUser.GetPermissions(channel);
		}
	}
}
