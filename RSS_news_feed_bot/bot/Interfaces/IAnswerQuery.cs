﻿using RSS_news_feed_bot.data;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace RSS_news_feed_bot.bot
{
    public interface IAnswerQuery
    {
        /// <summary>
        /// Текст ответа по умолчанию
        /// </summary>
        string PositiveAnswer { get; }
        /// <summary>
        /// Текст отрицательного ответа
        /// </summary>
        Dictionary<string, string> NegativeAnswer { get; }
        /// <summary>
        /// Описание команды
        /// </summary>
        string CommandDescription { get; }
        /// <summary>
        /// Список имен с вызовом команды
        /// </summary>
        List<string> CallCommandList { get; }
        /// <summary>
        /// Обработчик
        /// </summary>
        /// <param name="message"></param>
        /// <param name="allUsers"></param>
        void Process(CallbackQuery callbackQuery, AllUsers allUsers);
    }
}
