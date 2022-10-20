using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace Thenaniel
{
	public class Program
	{
        private static Dictionary<string, string> osuUsers = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            // parse config.json into dictionary
            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("config.json", true)
                        .Build();

            // set into environment variables
            Environment.SetEnvironmentVariable("DISCORD_TOKEN", config.GetValue<string>("discord:token"));
            Environment.SetEnvironmentVariable("DISCORD_PREFIX", config.GetValue<string>("discord:prefix"));
            // osu api v2
            Environment.SetEnvironmentVariable("OSU_ID", config.GetValue<string>("osu:id"));
            Environment.SetEnvironmentVariable("OSU_SECRET", config.GetValue<string>("osu:secret"));
            // osu api v1
            Environment.SetEnvironmentVariable("OSU_API", config.GetValue<string>("osu:api"));

            // discord start
            new Program().MainAsync().GetAwaiter().GetResult();
        }
        public async Task MainAsync()
        {
            // You should dispose a service provider created using ASP.NET
            // when you are finished using it, at the end of your app's lifetime.
            // If you use another dependency injection framework, you should inspect
            // its documentation for the best way to do this.
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();

                client.Log += Log;
                services.GetRequiredService<CommandService>().Log += Log;

                // login client
                await client.LoginAsync(Discord.TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_TOKEN"));
                await client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

        private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                 .AddSingleton(new DiscordSocketConfig
                 {
                     GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
                 })
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton(new DictionaryStorage(osuUsers))
                .BuildServiceProvider();
        }
    }
}