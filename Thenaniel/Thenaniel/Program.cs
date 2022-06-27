using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace Thenaniel
{
	public class Program
	{
		//client
		private static DiscordSocketClient client;
		//command service
		private static CommandService commands;
		//command handler
		private CommandHandler handler;

		public static Task Main(string[] args) => new Program().MainAsync();

		public async Task MainAsync()
		{
			//link client logs
			client = new DiscordSocketClient();
			client.Log += Log;

			//get config
			var config = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("config.json").Build();

			commands = new CommandService();

			handler = new CommandHandler(client, commands, config);
			await handler.InstallCommandsAsync();

			await client.LoginAsync(TokenType.Bot, config.GetValue<string>("DiscordConfig:token"));
			await client.StartAsync();

			await Task.Delay(-1);
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}