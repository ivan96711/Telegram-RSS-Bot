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
    class FunnySticker : IAnswer
    {
        public string PositiveAnswer => string.Empty;

        public Dictionary<string, string> NegativeAnswer => new Dictionary<string, string>();

        public string CommandDescription => "Ответ стикерами на смайл";

        public List<string> CallCommandList
        {
            get { return new List<string>() { "👋" }; }
        }

        public void Process(Message message, AllUsers allUsers)
        {
            string[] stickers = System.IO.File.ReadAllLines(@"Шаблоны\FunnySticker\Список стикеров.txt");
            Random rnd = new Random();

            Log.WriteLineUserMessage("Отправлен стикер пользователю:", message.Text, message.Chat.Id);
            Bot.Bot_SendSticker(message.Chat.Id, stickers[rnd.Next(0, stickers.Length)], new MainKeyboard().KeyboardMarkup);
        }
    }
}
