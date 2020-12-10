using Telegram.Bot.Types.ReplyMarkups;

namespace RSS_news_feed_bot.bot
{
    interface IKeyboardInline
    {
        string KeyboardDescription { get; }
        InlineKeyboardMarkup KeyboardMarkup { get; }
    }
}
