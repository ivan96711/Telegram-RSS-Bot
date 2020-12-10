using Telegram.Bot.Types.ReplyMarkups;

namespace RSS_news_feed_bot.bot
{
    interface IKeyboard
    {
        string KeyboardDescription { get; }
        ReplyKeyboardMarkup KeyboardMarkup { get; }
    }
}
