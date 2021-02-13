
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RLKeyDiscordBot.Services
{
	public static class Extensions
	{
		public static Stream ToStream(this string str)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(str);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
	}

	public class CommandHandlingService
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _services;

		public CommandHandlingService(IServiceProvider services)
		{
			_commands = services.GetRequiredService<CommandService>();
			_client = services.GetRequiredService<DiscordSocketClient>();
			_services = services;

			// Hook CommandExecuted to handle post-command-execution logic.
			_commands.CommandExecuted += CommandExecutedAsync;
			// Hook MessageReceived so we can process each message to see
			// if it qualifies as a command.
			_client.MessageReceived += HandleCommandAsync;
		}

		public async Task InitializeAsync()
		{
			// Register modules that are public and inherit ModuleBase<T>.
			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
		}



		private async Task HandleCommandAsync(SocketMessage messageParam)
		{
			// Ignore system messages, or messages from other bots
			if (!(messageParam is SocketUserMessage message)) return;
			if (message.Source != MessageSource.User) return;

			// Create a number to track where the prefix ends and the command begins
			int argPos = 0;

			// Determine if the message is a command based on the prefix and make sure no bots trigger commands
			if (!(message.HasCharPrefix('!', ref argPos) ||
				message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
				message.Author.IsBot)
				return;

			// Create a WebSocket-based command context based on the message
			var context = new SocketCommandContext(_client, message);

			// Execute the command with the command context we just
			// created, along with the service provider for precondition checks.
			await _commands.ExecuteAsync(
				context: context,
				argPos: argPos,
				services: _services);
		}

		public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
		{
			// command is unspecified when there was a search failure (command not found); we don't care about these errors
			if (!command.IsSpecified)
				return;

			// the command was successful, we don't care about this result, unless we want to log that a command succeeded.
			if (result.IsSuccess)
				return;

			// the command failed, let's notify the user that something happened.
			await context.Channel.SendMessageAsync($"error: {result}");
		}
	}
}