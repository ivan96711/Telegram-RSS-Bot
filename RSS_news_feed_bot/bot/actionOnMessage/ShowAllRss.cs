using RSS_news_feed_bot.bot.KeyBoard;
using RSS_news_feed_bot.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RSS_news_feed_bot.bot.actionOnMessage
{
    class ShowAllRss : IAnswer
    {
        public string PositiveAnswer => System.IO.File.ReadAllText(@"Шаблоны\ShowAllRss\Есть подписки.txt");

        public Dictionary<string, string> NegativeAnswer
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {"Nothing", System.IO.File.ReadAllText(@"Шаблоны\ShowAllRss\Нет подписок.txt") }
                };
            }
        }

        public string CommandDescription => "Отображение всех подписок пользователя";

        public List<string> CallCommandList
        {
            get { return new List<string>() { "Все подписки" }; }
        }

        public void Process(Message message, AllUsers allUsers)
        {
            var RssList = allUsers.GetUserRss(message.Chat.Id);

            Log.SentNews("Открытие перечня подписок", message.Chat.Id);

            if (RssList.Count == 0)
            {
                Bot.Bot_SendMessage(message.Chat.Id, NegativeAnswer["Nothing"], new MainKeyboard().KeyboardMarkup);
                return;
            }
            Bot.Bot_SendMessage(message.Chat.Id, PositiveAnswer, new SubscriptionsKeyboard(RssList.ToArray()).KeyboardMarkup);
        }
    }
}
