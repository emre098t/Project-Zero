using System.Data;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Template.Services;


namespace Template.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<Commands> _logger;

        public Commands(ILogger<Commands> logger)
            => _logger = logger;

        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Pong!");
        }

        [Command("echo")]
        public async Task EchoAsync([Remainder] string text)
        {
            await ReplyAsync(text);
        }
        [Command("math")]
        public async Task MathAsync([Remainder] string math)
        {
            var dt = new DataTable();
            var result = dt.Compute(math, null);

            await ReplyAsync($"Result: {result}");
        }
        [Command("bogdan")]
        public async Task bogdan()
        {
            Context.Channel.SendMessageAsync($@"Bogdan je simp {MentionUtils.MentionUser(207145047651516416)}");
        }
        [Command("dm.account.info")]
        public async Task dmaccountinfo([Remainder] SocketGuildUser user = null)
        {
            if (user == null)
            {
                bool isCreated = SqlDB.isCreated(Context.User.Id);
                if (isCreated)
                {
                    var builder2 = new EmbedBuilder()
                       .WithTitle("Account Info")
                       .AddField("Username:", "TEST", true)
                       .AddField("Characters", "TEST", true)
                       .WithColor(Color.DarkBlue)
                       .WithCurrentTimestamp();
                    var embed2 = builder2.Build();
                    Context.Channel.SendMessageAsync(embed: embed2);
                }
                else
                {
                    var builder3 = new EmbedBuilder()
                        .WithTitle("Account Info")
                        .AddField("Username:", SqlDB.GetUsername(Context.User.Id), true)
                        .AddField("Characters", SqlDB.GetNumChars((uint)SqlDB.GetAccountID(Context.User.Id)), true)
                        .WithColor(Color.DarkBlue)
                        .WithCurrentTimestamp();
                    var embed3 = builder3.Build();
                    Context.Channel.SendMessageAsync(embed: embed3);
                }
            }
            else
            {
                bool isCreated = SqlDB.isCreated(user.Id);
                if (isCreated)
                {
                    var builder2 = new EmbedBuilder()
                       .WithTitle("Account Info")
                       .AddField("Username:", "TEST", true)
                       .AddField("Characters", "TEST", true)
                       .WithColor(Color.DarkBlue)
                       .WithCurrentTimestamp();
                    var embed2 = builder2.Build();
                    Context.Channel.SendMessageAsync(embed: embed2);
                }
                else
                {
                    var builder = new EmbedBuilder()
                       .WithTitle("Account Info")
                        .AddField("Username:", SqlDB.GetUsername(user.Id), true)
                        .AddField("Characters", SqlDB.GetNumChars((uint)SqlDB.GetAccountID(user.Id)), true)
                         .WithColor(Color.DarkBlue)
                          .WithCurrentTimestamp();
                    var embed = builder.Build();
                    Context.Channel.SendMessageAsync(embed: embed);
                }
            }
        }
        [Command("dm.account.deletemyaccount")]
        public async Task dmaccountdelete()
        {
            string accountid = SqlDB.DeleteDiscordAccount(Context.User.Id);
            Context.Channel.SendMessageAsync($"Your account has been deleted successfully");
        }


        [Command("dm.account.changepwd")]
        public async Task dmaccountchangepwd([Remainder] string password = null)
        {
            if (password == null)
            {
                Context.Channel.SendMessageAsync("Please enter password after command | Example: +dm.account.changepwd Test");
            }
            else
            {
                SqlDB.SetNewPassword(Context.User.Id, password);
                Context.Channel.SendMessageAsync($@"Your password has been changed successfully: {password}");
            }
        }

        [Command("dm.account.changeusername")]
        public async Task dmaccountchangeusername([Remainder] string username = null)
        {
            if (username == null)
            {
                Context.Channel.SendMessageAsync("Please enter username after command | Example: +dm.account.changeusername Test");
            }
            else
            {
                SqlDB.SetNewUsername(Context.User.Id, username);
                Context.Channel.SendMessageAsync($@"Your username has been changed successfully: {username}");
            }
        }
        [Command("dm.account.infowuser")]
        public async Task dmaccountinfowuser([Remainder] string user = null)
        {
            if (user == null)
            {
                Context.Channel.SendMessageAsync("Example: +dm.account.infowuser TestUser");
            }
            else
            {
                bool isCreated = SqlDB.isCreatedUser(user);
                if (isCreated)
                {
                    var builder2 = new EmbedBuilder()
                       .WithTitle("Account Info")
                       .AddField("Username:", "TEST", true)
                       .AddField("Characters", "TEST", true)
                       .WithColor(Color.DarkBlue)
                       .WithCurrentTimestamp();
                    var embed2 = builder2.Build();
                    Context.Channel.SendMessageAsync(embed: embed2);
                }
                else
                {
                    var builder = new EmbedBuilder()
                       .WithTitle("Account Info")
                        .AddField("Username:", user, true)
                        .AddField("Characters", SqlDB.GetNumChars((uint)SqlDB.GetAccountIDwUser(user)), true)
                         .WithColor(Color.DarkBlue)
                          .WithCurrentTimestamp();
                    var embed = builder.Build();
                    Context.Channel.SendMessageAsync(embed: embed);
                }
            }
        }
    }
}