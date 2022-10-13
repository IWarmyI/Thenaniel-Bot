using System;
using System.Threading.Tasks;

using OsuSharp;

using Discord.Commands;
using OsuSharp.Domain;
using OsuSharp.Interfaces;
using System.Collections.Generic;
using System.Threading;

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
        private IOsuClient client;

        public OsuModule (IOsuClient _client)
        {
            client = _client;
        }

        [Command("recent")]
        [Alias("r")]
        [Summary("Get user's most recent play")]
        public async Task Recent([Remainder] string username)
        {
            await Recent(1, username);
        }

        [Command("recent")]
        [Alias("r")]
        [Summary("Get user's most recent play")]
        public async Task Recent(int count, [Remainder] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                await ReplyAsync("Username is null/empty, command usage is \"a.recent <count=1> [username]\"");
                return;
            }
            var user = await client.GetUserByNameAsync(username);
            if (user == null) return;
            var recentScores = await client.GetUserRecentAndBeatmapByUsernameAsync(username, limit: 1);
            if (recentScores.Count < 1) return;
            if (count == 1)
            {
                await ReplyAsync($"**Most recent play for {user.Username}:**", embed: CreateSinglePlayEmbed(user, recentScores[0].UserRecent, recentScores[0].Beatmap));
            }
            else
            {
                await ReplyAsync($"**Most recent plays for {user.Username}:**", embed: CreateMultiplePlayEmbed(user, recentScores));
            }
        }
    }
}