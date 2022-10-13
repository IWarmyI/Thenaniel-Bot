using Discord.Commands;
using OsuNet;
using OsuNet.Models;
using OsuNet.Models.Options;

using System;
using System.Threading.Tasks;

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
        private static readonly OsuApi api = new OsuApi(Environment.GetEnvironmentVariable("OSU_API"));

        [Command("user")]
        [Alias("u")]
        [Summary("Get user info")]
        public async Task GetUser([Remainder] string username)
        {
            User[] user = api.GetUser(new GetUserOptions() { User = username });

            ulong rankGlobal = user[0].PPRank;
            ulong rankCountry = user[0].PPCountryRank;
            ulong playCount = user[0].PlayCount;
            ulong timePlayed = user[0].TotalSecondsPlayed;

            timePlayed /= 60;
            timePlayed /= 60;

            string data = username +
                          "\n```Global Rank: " + rankGlobal +
                          "\nCountry Rank: " + rankCountry +
                          "\nPlay Count: " + playCount +
                          "\nTime Played: " + timePlayed + " hours```";

            ReplyAsync(data);
        }
    }
}