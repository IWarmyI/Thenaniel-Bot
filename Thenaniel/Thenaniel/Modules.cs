using System;
using System.Threading.Tasks;

using Discord.Commands;

namespace Thenaniel
{
	public class SayModule : ModuleBase<SocketCommandContext>
	{
        //[Command("ask")]
        //[Summary("Ask the Magic 8 Ball")]
        //public async Task AskAsync()
        //{
        //	// randomly select response
        //	Random magicBall = new Random();

        //	int num = magicBall.Next(0, 10);

        //	string[] response =
        //	{
        //		"```Definitely Not```",
        //		"```Maybe```",
        //		"```God No```",
        //		"```Sure```",
        //		"```Go For It Champ```",
        //		"```Ask Again Later```",
        //		"```I Doubt It```",
        //		"```Why Not```",
        //		"```Yes```",
        //		"```Most Likely```",
        //	};

        //await ReplyAsync(response[num]);
        //}
        // ~say hello world -> hello world
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
            => ReplyAsync(echo);

        // ReplyAsync is a method on ModuleBase
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