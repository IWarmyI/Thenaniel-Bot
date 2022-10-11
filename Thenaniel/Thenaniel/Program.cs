using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Thenaniel
{
	public class Program
	{
        static void Main(string[] args)
           => new Program().MainAsync().GetAwaiter().GetResult();

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

                // parse config.json into dictionary
                var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("config.json", true)
                            .Build();
                
                // set into environment variables
                Environment.SetEnvironmentVariable("TOKEN", config.GetValue<string>("token"));
                Environment.SetEnvironmentVariable("PREFIX", config.GetValue<string>("prefix"));
                Environment.SetEnvironmentVariable("APPID", config.GetValue<string>("app_id"));
                Environment.SetEnvironmentVariable("APPSECRET", config.GetValue<string>("app_secret"));

                // login client
                await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("TOKEN"));
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
                .BuildServiceProvider();
        }
    }
}