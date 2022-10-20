using Discord.Commands;

using OsuNet.Models.Options;
using OsuNet.Models;
using OsuNet;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Thenaniel
{
    // Commands related to osu
    [Group("osu")]
    public class OsuModule : ModuleBase<SocketCommandContext>
    {
        private static readonly OsuApi api = new OsuApi(Environment.GetEnvironmentVariable("OSU_API"));
        public DictionaryStorage osuDict { get; set; }

        // add a user to store for later use
        [Command("add")]
        [Alias("a")]
        [Summary("Add a user")]
        public async Task AddUser([Remainder] string username)
        {
            // get user who sent command
            string discordName = Context.User.Username;

            // check if user already added a profile
            if (osuDict.osuUsers.ContainsKey(discordName))
            {
                await ReplyAsync("You already have a profile");
            }
            else
            {
                // add user
                osuDict.osuUsers.Add(discordName, username);
                await ReplyAsync($"{username} has been added");
            }
        }

        // delete user profile
        [Command("delete")]
        [Alias("d")]
        [Summary("Delete a user")]
        public async Task DeleteUser()
        {
            // get user who sent command
            string discordName = Context.User.Username;

            // return error if user doesn't have a profile to delete
            if (!osuDict.osuUsers.ContainsKey(discordName))
            {
                await ReplyAsync("You don't have a profile added");
            }
            else
            {
                // delete user
                osuDict.osuUsers.Remove(discordName);
                await ReplyAsync("Your profile has been deleted");
            }
            
        }

        // get a user's info
        [Command("user")]
        [Alias("u")]
        [Summary("Get a user's info")]
        public async Task GetUser()
        {
            // get user who sent command
            string discordName = Context.User.Username;

            // return error if user isn't added
            if (!osuDict.osuUsers.ContainsKey(discordName))
            {
                await ReplyAsync("You don't have a profile added");
            }
            else
            {
                // get user and their data
                User[] user = api.GetUser(new GetUserOptions() { User = osuDict.osuUsers[discordName] });

                string username = user[0].Username;
                string country = user[0].Country;
                ulong rankGlobal = user[0].PPRank;
                ulong rankCountry = user[0].PPCountryRank;
                ulong playCount = user[0].PlayCount;
                ulong timePlayed = user[0].TotalSecondsPlayed;

                timePlayed /= 60;
                timePlayed /= 60;

                // compile data and send
                await ReplyAsync($"```Username: {username}" +
                                 $"\nCountry: {country}" +
                                 $"\nGlobal Rank: {rankGlobal}" +
                                 $"\nCountry Rank: {rankCountry}" +
                                 $"\nPlay Count: {playCount}" +
                                 $"\nTime Played: {timePlayed} hours```");
            }
        }
    }
}
