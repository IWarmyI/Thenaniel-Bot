	using System;
using System.Threading.Tasks;

using Discord.WebSocket;
using Discord.Commands;

namespace Thenaniel
{
	public class InfoModule : ModuleBase<SocketCommandContext>
	{
		[Command("n8ball")]
		[Summary("Ask the Magic 8 Ball")]
		public Task EightBall([Remainder][Summary("Ask the Magic 8 Ball")] string question)
		{
			// randomly select response
			Random magicBall = new Random();

			int num = magicBall.Next(0, 10);

			string[] response =
			{
				"```Definitely...Not```",
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

			return ReplyAsync(response[num]);
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