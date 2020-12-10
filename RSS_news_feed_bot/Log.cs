using RSS_news_feed_bot.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_news_feed_bot
{
    static class Log
    {
        private readonly static AllUsers AllUsers;
        static Log()
        {
            AllUsers = Program.AllUsers;
        }

        public static void BotInfo(Telegram.Bot.Types.User me)
        {
            Console.WriteLine(DateTime.Now.ToString() + '\n' +
                $"Bot id:{me.Id}. Bot name: {me.FirstName}" + '\n' +
                $"Users in base: {AllUsers.Count}" + '\n' +
                "=================================================================================");
        }

        public static void WriteLineUserMessage(string Message, string UserText, long UserId)
        {
            Console.WriteLine(DateTime.Now.ToString() + " " + AllUsers.GetLoginUserToString(UserId) + '\n' +
                    Message + '\n' +
                    UserText + '\n' +
                    "=================================================================================");
        }

        public static void SentNews(string Message, long UserId)
        {
            Console.WriteLine(DateTime.Now.ToString() + " " + AllUsers.GetLoginUserToString(UserId) + '\n' +
                    Message + '\n' +
                    "=================================================================================");
        }

        public static void WriteLineNewUser(long UserId)
        {
            LoginUser userdata = AllUsers.GetUserById(UserId);

            Console.WriteLine(DateTime.Now.ToString() + '\n' +
                $"New user: " + '\n' +
                $"Id - {userdata.UserId}," + '\n' +
                $"Username - {userdata.Username}" + '\n' +
                $"FirstName - {userdata.FirstName}" + '\n' +
                $"LastName - {userdata.LastName}" + '\n' +
                "=================================================================================");
        }
    }
}
