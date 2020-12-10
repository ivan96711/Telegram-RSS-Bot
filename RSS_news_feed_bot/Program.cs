using Newtonsoft.Json;
using RSS_news_feed_bot.bot;
using RSS_news_feed_bot.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace RSS_news_feed_bot
{
    class Program
    {
        /// <summary>
        /// Токен тестового бота
        /// </summary>
        public readonly static string token = "write bot token";
        /// <summary>
        /// Период обновления в миллисекундах
        /// </summary>
        public readonly static int updatePeriod = 300000;
        /// <summary>
        /// База всех пользователей.
        /// </summary>
        public static AllUsers AllUsers;

        public static ITelegramBotClient bot;

        static void Main(string[] args)
        {
            AllUsers = new AllUsers(@"data.json");

            bot = Bot.bot;

            if (AllUsers.Count > 0)
                ThreadManager.FirstStart(AllUsers);

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
