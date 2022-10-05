using System.Threading.Tasks;
using System.Reflection;

using Microsoft.Extensions.Configuration;

using Discord.WebSocket;
using Discord.Commands;

public class CommandHandler
{
    private readonly DiscordSocketClient client;
    private readonly CommandService commands;
    private readonly IConfigurationRoot config;

    // Retrieve client and CommandService instance
    public CommandHandler(DiscordSocketClient _client, CommandService _commands, IConfigurationRoot _config)
    {
        client = _client;
        commands = _commands;
        config = _config;
    }

    public async Task InstallCommandsAsync()
    {
        // Hook the MessageReceived event into our command handler
        client.MessageReceived += HandleCommandAsync;

        // Here we discover all of the command modules in the entry 
        // assembly and load them. Starting from Discord.NET 2.0, a
        // service provider is required to be passed into the
        // module registration method to inject the 
        // required dependencies.
        //
        // If you do not use Dependency Injection, pass null.
        // See Dependency Injection guide for more information.
        await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        // Don't process the command if it was a system message
        SocketUserMessage message = messageParam as SocketUserMessage;
        if (message == null) return;

        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;

        var prefix = config.GetValue<char>("DiscordConfig:prefix");

        if (message.HasCharPrefix(prefix, ref argPos))
        {
            return;
        }

        // Determine if the message is a command based on the prefix and make sure no bots trigger commands
        if (message.HasCharPrefix(config.GetValue<char>("DiscordConfig:prefix"), ref argPos) ||
            message.HasMentionPrefix(client.CurrentUser, ref argPos) ||
            message.Author.IsBot)
        {
            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }
    }
}