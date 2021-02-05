using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template.Services;

namespace Template.Modules
{
    public class InteractiveCommands : InteractiveBase
    {
        [Command("dm.account.create", RunMode = RunMode.Async)]
        public async Task dmaccountcreate1step()
        {
            bool isCreated = SqlDB.DiscordIDAvail(Context.User.Id);
            if (isCreated)
            {
                await ReplyAsync("Enter username");
                var username = await NextMessageAsync();
                if (username != null)
                {
                    await ReplyAsync($"Enter password");
                    var password = await NextMessageAsync();
                    if (password != null)
                    {
                        if (isCreated)
                        {
                            SqlDB.CreateUser(Convert.ToString(username), Convert.ToString(password), Context.User.Id);
                            Context.Channel.SendMessageAsync($@"Your account has been created successfully: {username} | {password}");
                        }
                        else
                        {
                            Context.Channel.SendMessageAsync($@"You have already account");
                        }
                    }
                    else
                    {
                        await ReplyAsync("You did not reply before the timeout");
                    }
                }
                else
                {
                    await ReplyAsync("You did not reply before the timeout");
                }
            }
            else
            {
                Context.Channel.SendMessageAsync($@"You have already account");
            }
        }
    }
}
