using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Techno_Bot.Module
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("hi")]
        [Alias("hello")]
        public async Task Hi()
        {
            await ReplyAsync("Hello!");
        }

        [Command("tick")]
        public async Task Tick()
        {
            await ReplyAsync("Tack");
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You don't have permission to ban members ``ban_member``!")]
        public async Task BanMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please speacify a user!");
                return;
            }
            if (reason == null) reason = "Not specified";

            await Context.Guild.AddBanAsync(user, 1, reason);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark:{user.Mention} was banned\n**Reason** {reason}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Ban Log")
                    .WithIconUrl("http://www.fastkashmir.com/wp-content/uploads/2017/03/ban-2.png");

                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(743741286674202726) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
                .WithDescription($"{user.Mention} was banned\n**Reason** {reason}\n **Moderator**{Context.User.Mention}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Ban Log")
                    .WithIconUrl("http://www.fastkashmir.com/wp-content/uploads/2017/03/ban-2.png");

                });
            Embed embedLog = EmbedBuilderLog.Build();
            logChannel.SendMessageAsync(embed: embedLog);
        }

        [Command("unban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You don't have permission to unban members ``unban_member``!")]
        public async Task UnBanMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please speacify a user!");
                return;
            }
            if (reason == null) reason = "Not specified";

            await Context.Guild.RemoveBanAsync(user);
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You don't have permission to kick members ``kick_member``!")]
        public async Task KickMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please speacify a user!");
                return;
            }
            if (reason == null) reason = "Not specified";

            await user.KickAsync(reason);
        }

        [Command("dm")]
        public async Task Dm(IGuildUser user = null, [Remainder] string chat = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please specify whom you want to dm");
                return;
            }
            if (chat == null) chat = $"{Context.User.Mention} just dm you nothing as the person to send somthing";

            await user.SendMessageAsync(chat);
        }

        [Command("help")]
        public async Task Help()
        {
            await ReplyAsync("Hi I am Techno Bot and my prefix is '!'\nmy commnads are\nhi(!hi)\nhello(!hello)\ntick(just for fun '!tick')\nkick(prank you friends by kicking '!kick <mention> <reason>')\nflip(flip a coin '!flip @Techno Bot')\ndog(send your friends dog photo '!dog <mention user>')\nhardwarenews(latest news about hardwares '!hardwarenews')\nmeme(get memes only 1 per day '!meme')\ndm(send direct message using the bot'!dm <mention user><chat>')");
        }

        [Command("purge")]
        public async Task Purge(int max)
        {

            var messages = Context.Channel.GetMessagesAsync(max).Flatten();
            foreach (var h in await messages.ToArrayAsync())
            {
                await this.Context.Channel.DeleteMessageAsync(h);
            }
        }

        [Command("bye")]
        public async Task Bye()
        {
            await ReplyAsync($"Bye {Context.User.Mention}");
        }

    }

}
