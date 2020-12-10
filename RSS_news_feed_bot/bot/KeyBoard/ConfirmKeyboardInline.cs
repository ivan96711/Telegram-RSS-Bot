using RSS_news_feed_bot.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace RSS_news_feed_bot.bot.KeyBoard
{
    class ConfirmKeyboardInline : IKeyboardInline
    {
        private string parameter;
        public ConfirmKeyboardInline(string RssUrlForDelete)
        {
            parameter = RssUrlForDelete;
        }

        public string KeyboardDescription => "Перечень подписок";

        public InlineKeyboardMarkup KeyboardMarkup
        {
            get
            {
                return new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Подтверждаю удаление", "/remove_сonfirm " + parameter));
            }
        }
    }
}
