using Discord.Commands;

using OsuNet;
using OsuNet.Models;
using OsuNet.Models.Options;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;

namespace Thenaniel
{
	public class CommandModule : ModuleBase<SocketCommandContext>
	{
        [Command("8ball")]
        [Alias("8b")]
        [Summary("Ask the Magic 8 Ball")]
        public async Task EightBall([Remainder] string question)
        {
        	// randomly select response
        	Random magicBall = new Random();

        	int num = magicBall.Next(0, 10);

        	string[] response =
        	{
        		"```Definitely Not```",
        		"```Maybe```",
        		"```God No```",
        		"```Sure```",
        		"```Go For It Champ```",
        		"```Ask Again Later```",
        		"```I Doubt It```",
        		"```Why Not```",
        		"```Yes```",
        		"```Most Likely```",
        	};

        await ReplyAsync(response[num]);
        }

        [Command("prefix")]
        [Alias("p")]
        [Summary("Change Bot Prefix")]
        public async Task SetPrefix(char prefix)
        {
            Environment.SetEnvironmentVariable("DISCORD_PREFIX", prefix.ToString());

            await ReplyAsync($"Prefix has been set to { prefix }");
        }

        [Command("help")]
        [Alias("h")]
        [Summary("Display All Available Commands")]
        public async Task AllCommands()
        {
            await ReplyAsync("```General Commands:\n" + 
                             "(h)elp\t\tShow all available commands and their short-hands\n" +
                             "(p)refix\t\tChange bot\'s prefix\n" +
                             "(8b)all\t\tAsk a question and get a randomized response\n\n" + 

                             "Osu! Commands (Use \"(o)su\" along with your command):\n" + 
                             "(a)dd\t\tAdd an Osu! profile (You can only have one at a time)\n" + 
                             "(d)elete\t\tRemove the connection between your Osu! profile and Discord\n" + 
                             "(u)ser\t\tGet stats of your Osu! profile\n\n" +
                             "More to Come!```");
        }
    }

	// Commands related to league
	[Group("lol")]
	public class LeagueModule : ModuleBase<SocketCommandContext>
	{
		
	}
}