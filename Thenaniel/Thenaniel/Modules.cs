using System;
using System.Threading.Tasks;

using Discord.Commands;

namespace Thenaniel
{
	public class CommandModule : ModuleBase<SocketCommandContext>
	{
        [Command("8ball")]
        [Summary("Ask the Magic 8 Ball")]
        public async Task AskAsync([Remainder] string question)
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
    }

	// Commands related to league
	[Group("lol")]
	public class LeagueModule : ModuleBase<SocketCommandContext>
	{
		
	}

	// Commands related to osu
	[Group("osu")]
	public class OsuModule : ModuleBase<SocketCommandContext>
	{

	}
}