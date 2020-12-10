using Newtonsoft.Json;
using RSS_news_feed_bot.bot;
using RSS_news_feed_bot.bot.actionOnMessage;
using RSS_news_feed_bot.bot.actionOnQuery;
using RSS_news_feed_bot.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RSS_news_feed_bot.bot
{
    static class Bot
    {
        public static ITelegramBotClient bot;

        private static List<IAnswer> handlers = new List<IAnswer> 
        {
            new ShowAllRss(),
            new FunnySticker()
        };
        private static NewUser newUser = new NewUser();
        private static Text text = new Text();
        private static List<IAnswerQuery> handlersQuery = new List<IAnswerQuery>
        {
            new DeleteRss(),
            new DeleteRssConfirm()
        };

        static Bot()
        {
            bot = new TelegramBotClient(Program.token) { Timeout = TimeSpan.FromSeconds(10) };

            var me = bot.GetMeAsync().Result;
            me.CanJoinGroups = false;

            Log.BotInfo(me);

            bot.OnMessage += Bot_OnMessage;
            bot.OnCallbackQuery += Bot_OnCallbackQueryReceived;
            bot.StartReceiving();

        }

        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var txt = e?.Message?.Text;

            if(e.Message.Sticker != null)
            {
                Console.WriteLine("Стикер от пользователя: " + e.Message.Sticker.FileId);
                return;
            }    

            if (txt == null)
                return;

            //Проверяем наличие пользователя. Если такого нет, то добавляем его в базу и оповещаем пользователя о вступлении.
            if (!Program.AllUsers.ContainsId(e.Message.Chat.Id))
            {
                newUser.Process(e.Message, Program.AllUsers);
                if (txt == "/start")
                    return;
            }

            foreach (IAnswer handler in handlers)
            {
                foreach(var CallCommand in handler.CallCommandList)
                {
                    if (txt == CallCommand)
                    {
                        handler.Process(e.Message, Program.AllUsers);
                        return;
                    }
                }
            }

            //Если не найдено подходящее ключевое слово, то думаем, что это попытка добавить новый RSS
            text.Process(e.Message, Program.AllUsers);
        }

        private static void Bot_OnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            CallbackQuery callbackQuery = e.CallbackQuery;

            //Проверяем наличие пользователя. Если такого нет, то добавляем его в базу и оповещаем пользователя о вступлении.
            if (!Program.AllUsers.ContainsId(callbackQuery.Message.Chat.Id))
                newUser.Process(callbackQuery.Message, Program.AllUsers);

            string command = callbackQuery.Data.Split(' ')[0];
            foreach (IAnswerQuery handler in handlersQuery)
            {
                foreach (var CallCommand in handler.CallCommandList)
                {
                    if (command == CallCommand)
                    {
                        handler.Process(callbackQuery, Program.AllUsers);
                        return;
                    }
                }
            }
        }

        public static void Bot_SendMessage(long userid, string answer, IReplyMarkup KeyboardMarkup)
        {
            try
            {
                bot.SendTextMessageAsync(
                                chatId: userid,
                                text: answer,
                                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                                replyMarkup: KeyboardMarkup
                                ).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void Bot_SendSticker(long userid, string answer, IReplyMarkup KeyboardMarkup)
        {
            try
            {
                bot.SendStickerAsync(
                                chatId: userid,
                                replyMarkup: KeyboardMarkup,
                                sticker: answer
                                ).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
