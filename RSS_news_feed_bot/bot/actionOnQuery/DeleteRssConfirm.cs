using RSS_news_feed_bot.bot.KeyBoard;
using RSS_news_feed_bot.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RSS_news_feed_bot.bot.actionOnQuery
{
    class DeleteRssConfirm : IAnswerQuery
    {
        public string PositiveAnswer => System.IO.File.ReadAllText(@"Шаблоны\DeleteRssConfirm\Удалено.txt");

        public Dictionary<string, string> NegativeAnswer => new Dictionary<string, string>();

        public string CommandDescription => "Удаление RSS";

        public List<string> CallCommandList
        {
            get { return new List<string>() { "/remove_сonfirm" }; }
        }

        public void Process(CallbackQuery callbackQuery, AllUsers allUsers)
        {
            string rss = callbackQuery.Data.Split(' ')[1];

            Program.AllUsers.DeleteUserRss(callbackQuery.Message.Chat.Id, rss);
            ThreadManager.NewRssLink(allUsers.GetUserById(callbackQuery.Message.Chat.Id));

            Log.WriteLineUserMessage("Пользователь удалил RSS:", rss, callbackQuery.Message.Chat.Id);
            Bot.Bot_SendMessage(callbackQuery.Message.Chat.Id, PositiveAnswer.Replace("<RssUrl>", rss), null);
        }
    }
}
