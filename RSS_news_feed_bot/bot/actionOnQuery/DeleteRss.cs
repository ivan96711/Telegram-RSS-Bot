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
    class DeleteRss : IAnswerQuery
    {
        public string PositiveAnswer => System.IO.File.ReadAllText(@"Шаблоны\DeleteRss\Подтверждение удаления.txt");

        public Dictionary<string, string> NegativeAnswer => new Dictionary<string, string>();

        public string CommandDescription => "Подтверждение удаления";

        public List<string> CallCommandList
        {
            get { return new List<string>() { "/remove" }; }
        }

        public void Process(CallbackQuery callbackQuery, AllUsers allUsers)
        {
            string rss = callbackQuery.Data.Split(' ')[1];
            Bot.Bot_SendMessage(callbackQuery.Message.Chat.Id, PositiveAnswer.Replace("<RssUrl>", rss), new ConfirmKeyboardInline(rss).KeyboardMarkup);
        }
    }
}
