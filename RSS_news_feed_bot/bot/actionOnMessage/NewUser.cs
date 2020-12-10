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
    class NewUser : IAnswer
    {
        public string PositiveAnswer => System.IO.File.ReadAllText(@"Шаблоны\NewUser\Приветствие.txt");

        public Dictionary<string, string> NegativeAnswer
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {"OldUser", System.IO.File.ReadAllText(@"Шаблоны\NewUser\Пользователь был добавлен ранее.txt") }
                };
            }
        }

        public string CommandDescription => "Добавление нового пользователя";

        public List<string> CallCommandList
        {
            get { return new List<string>(); }
        }

        public void Process(Message message, AllUsers allUsers)
        {
            if (allUsers.ContainsId(message.Chat.Id))
            {
                Bot.Bot_SendMessage(message.Chat.Id, NegativeAnswer["OldUser"], new MainKeyboard().KeyboardMarkup);
                return;
            }

            allUsers.Add(message.Chat.Id, message.Chat.Username, message.Chat.FirstName, message.Chat.LastName);
            string answer = message.Chat.FirstName != null ? PositiveAnswer.Replace("<FirstName>", message.Chat.FirstName) : PositiveAnswer.Replace(", <FirstName>", message.Chat.FirstName);
            Bot.Bot_SendMessage(message.Chat.Id, answer, new MainKeyboard().KeyboardMarkup);
            new FunnySticker().Process(message, allUsers);
            Log.WriteLineNewUser(message.Chat.Id);
        }
    }
}
