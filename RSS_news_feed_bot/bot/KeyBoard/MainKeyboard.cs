using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace RSS_news_feed_bot.bot.KeyBoard
{
    class MainKeyboard : IKeyboard
    {
        public string KeyboardDescription => "Клавиатура, открытая по умолчанию";

        public ReplyKeyboardMarkup KeyboardMarkup => 
            new ReplyKeyboardMarkup(
                new KeyboardButton[][]
                {
                    new KeyboardButton[] { "Все подписки", "👋" }
                },
                resizeKeyboard: true
            );
    }
}
