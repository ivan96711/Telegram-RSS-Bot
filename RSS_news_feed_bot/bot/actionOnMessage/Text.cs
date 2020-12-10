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
    class Text : IAnswer
    {
        public string PositiveAnswer => System.IO.File.ReadAllText(@"Шаблоны\Text\Добавлен RSS адрес.txt");

        public Dictionary<string, string> NegativeAnswer
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {"Text5", System.IO.File.ReadAllText(@"Шаблоны\Text\Добавлен повторяющийся RSS адрес.txt") },
                    {"Text2", System.IO.File.ReadAllText(@"Шаблоны\Text\Неверная ссылка.txt") },
                    {"Text3", System.IO.File.ReadAllText(@"Шаблоны\Text\Неподдерживаемые RSS.txt") },
                    {"Text1", System.IO.File.ReadAllText(@"Шаблоны\Text\Нераспознанный текст.txt") }
                };
            }
        }

        public string CommandDescription => "Отработка текста, если нет ключевых свойств";

        public List<string> CallCommandList
        {
            get { return new List<string>(); }
        }

        public void Process(Message message, AllUsers allUsers)
        {
            //Проверка является ли присланное сообщение текстом без ссылки
            if (message.Entities == null || message.Entities.Length > 1 || message.Entities[0].Type != Telegram.Bot.Types.Enums.MessageEntityType.Url)
            {
                Log.WriteLineUserMessage("Сообщение от пользователя:", message.Text, message.Chat.Id);
                Bot.Bot_SendMessage(message.Chat.Id, NegativeAnswer["Text1"], new MainKeyboard().KeyboardMarkup);
                return;
            }

            try
            {
                var sas = FeedLoad.GetItems(message.Text);
                allUsers.AddRssToUser(message.Chat.Id, message.Text, sas.Title, sas.Description);
                ThreadManager.NewRssLink(allUsers.GetUserById(message.Chat.Id));

                Log.WriteLineUserMessage("Пользователь добавил новый RSS:", message.Text, message.Chat.Id);
                string answer = sas.Title != null ? PositiveAnswer.Replace("<Source>", "\"" + sas.Title + "\"") : PositiveAnswer.Replace("<Source>", "");
                Bot.Bot_SendMessage(message.Chat.Id, answer, new MainKeyboard().KeyboardMarkup);
            }
            //Обработка случая добавления дубликата.
            catch (DuplicateWaitObjectException)
            {
                Log.WriteLineUserMessage("Пользователь пытался добавить дубликат RSS:", message.Text, message.Chat.Id);
                Bot.Bot_SendMessage(message.Chat.Id, NegativeAnswer["Text5"], new MainKeyboard().KeyboardMarkup);
            }
            //Обработка случая для совмещенных RSS.
            catch (ArgumentException)
            {
                Log.WriteLineUserMessage("Пользователь пытался добавить неподдерживаемый RSS:", message.Text, message.Chat.Id);
                Bot.Bot_SendMessage(message.Chat.Id, NegativeAnswer["Text3"], new MainKeyboard().KeyboardMarkup);
            }
            //Обработка случая некорректного RSS.
            catch
            {
                Log.WriteLineUserMessage("Пользователь пытался добавить некорректный RSS:", message.Text, message.Chat.Id);
                Bot.Bot_SendMessage(message.Chat.Id, NegativeAnswer["Text2"], new MainKeyboard().KeyboardMarkup);
            }
        }
    }
}