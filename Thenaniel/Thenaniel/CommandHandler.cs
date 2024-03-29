﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

public class CommandHandler
{
    private readonly DiscordSocketClient discordClient;
    private readonly CommandService commands;
    private readonly IServiceProvider services;

    // Retrieve client and CommandService instance
    public CommandHandler(IServiceProvider _services)
    {
        discordClient = _services.GetRequiredService<DiscordSocketClient>();
        commands = _services.GetRequiredService<CommandService>();
        services = _services;

        // Hook CommandExecuted to handle post-command-execution logic.
        commands.CommandExecuted += CommandExecutedAsync;
        // Hook MessageReceived so we can process each message to see
        // if it qualifies as a command.
        discordClient.MessageReceived += MessageReceivedAsync;
    }
    public async Task InitializeAsync()
    {
        // Register modules that are public and inherit ModuleBase<T>.
        await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
    }

    public async Task MessageReceivedAsync(SocketMessage rawMessage)
    {
        // Ignore system messages, or messages from other bots
        if (!(rawMessage is SocketUserMessage message))
            return;
        if (message.Source != MessageSource.User)
            return;

        char[] prefix = Environment.GetEnvironmentVariable("DISCORD_PREFIX").ToCharArray();

        // This value holds the offset where the prefix ends
        var argPos = 0;
        if (!message.HasCharPrefix(prefix[0], ref argPos))
        {
            Console.WriteLine(message.Content);
            return;
        }
        var context = new SocketCommandContext(discordClient, message);
        // Perform the execution of the command. In this method,
        // the command service will perform precondition and parsing check
        // then execute the command if one is matched.
        await commands.ExecuteAsync(context, argPos, services);
        // Note that normally a result will be returned by this format, but here
        // we will handle the result in CommandExecutedAsync,
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