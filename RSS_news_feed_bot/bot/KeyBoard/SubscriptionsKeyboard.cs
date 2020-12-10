using RSS_news_feed_bot.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace RSS_news_feed_bot.bot.KeyBoard
{
    class SubscriptionsKeyboard : IKeyboardInline
    {
        private RssInfo[] parameters;
        public SubscriptionsKeyboard(RssInfo[] parameters)
        {
            this.parameters = parameters;
        }

        public string KeyboardDescription => "Перечень подписок";

        public InlineKeyboardMarkup KeyboardMarkup
        {
            get
            {
                InlineKeyboardButton[][] inlineKeyboardButton = new InlineKeyboardButton[parameters.Length][];
                for(int i = 0; i < inlineKeyboardButton.Length; i++)
                {
                    string text = parameters[i].RssURL + " — " + parameters[i].Title + ".";
                    inlineKeyboardButton[i] = new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(text, "/remove " + parameters[i].RssURL) };
                }

                return new InlineKeyboardMarkup(inlineKeyboardButton);
            }
        }
    }
}
