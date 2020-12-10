using CodeHollow.FeedReader;
using RSS_news_feed_bot.bot.KeyBoard;
using RSS_news_feed_bot.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RSS_news_feed_bot.bot
{
    static class ThreadManager
    {
        public readonly static int updatePeriod = Program.updatePeriod;
        private static Dictionary<long, Thread> threadDictionary = new Dictionary<long, Thread>();
        /// <summary>
        /// Запуск потоков на каждого пользователя при запуске.
        /// </summary>
        /// <param name="AllUsers"></param>
        public static void FirstStart(AllUsers AllUsers)
        {
            
            for(int i = 0; i < AllUsers.Count; i++)
            {
                if (AllUsers[i].RssURL != null && AllUsers[i].RssURL.Count == 0)
                    continue;
                Thread myThread = new Thread(new ParameterizedThreadStart(LoadNewsThread));
                myThread.Name = AllUsers[i].UserId.ToString();
                myThread.Start(AllUsers[i]);

                threadDictionary.Add(AllUsers[i].UserId, myThread);
            }
        }

        /// <summary>
        /// Добавление новой ссылки у пользователя.
        /// </summary>
        /// <param name="loginUser"></param>
        public static void NewRssLink(LoginUser loginUser)
        {
            //Флаг, на случай, если пользователь отсутствует в базе, но при этом от него прилетел url.
            bool newUser = false;
            if (!threadDictionary.ContainsKey(loginUser.UserId))
                newUser = true;
            else
                //Останавливаем запущенный поток.
                threadDictionary[loginUser.UserId].Abort();

            //Запускаем поток с новыми параметрами
            Thread myThread = new Thread(new ParameterizedThreadStart(LoadNewsThread));
            myThread.Name = loginUser.UserId.ToString();
            myThread.Start(loginUser);

            //Добавляем пользователя, если он отсутствовал.
            if(newUser)
                threadDictionary.Add(loginUser.UserId, myThread);
        }

        private static void LoadNewsThread(object user)
        {
            LoginUser loginUser = (LoginUser)user;
            while (true)
            {
                //Перебор всех источников и их загрузка.
                for (int i = 0; i < loginUser.RssURL.Count; i++)
                {
                    try
                    {
                        //Получаем список с постами за последний период.
                        var items = FeedLoad.GetNewItems(loginUser.RssURL[i].RssURL, updatePeriod);
                        bool itemsNotNull = false;
                        foreach (var item in items)
                        {
                            itemsNotNull = true;

                            string text = Message(item, loginUser.RssURL[i]);

                            //Отправка сообщения.
                            Bot.Bot_SendMessage(loginUser.UserId, text, new MainKeyboard().KeyboardMarkup);
                        }

                        if(itemsNotNull)
                            Log.SentNews("Последние посты источника " + loginUser.RssURL[i].RssURL + " были отправлены", loginUser.UserId);
                    }
                    catch
                    {
                        Log.SentNews("SubThread: Ошибка при загрузке новостей из: " + loginUser.RssURL[i].RssURL, loginUser.UserId);
                    }
                }
                Thread.Sleep(updatePeriod);
            }
        }

        /// <summary>
        /// Создание текста сообщение
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rss"></param>
        /// <returns></returns>
        private static string Message(FeedItem item, RssInfo rss)
        {
            //Заменяем </p> на перенос строки, т.к. ParseMode телеги не знает данный тег.
            string description = item.Description.Replace("</p>", "\n").Replace("\t", "");

            //Удаляем абсолютно все html теги, кроме тегов ссылок (<a href> и </a>), используя регулярку.
            description = Regex.Replace(description, @"<(?!a href)+(?!\/a)(.*?)>", string.Empty);

            string name = "<i>" + item.Link.Split('/')[2] + " — " + rss + "</i>";
            string maintitle = "<i>" + rss + "</i>";
            string title = "<b>" + item.Title + "</b>";
            string link = "<a href=\"" + item.Link + "\"><ins>Читать полностью</ins></a>";

            string text = name + '\n' +
                title + '\n' +
                " " + '\n' +
                HttpUtility.HtmlDecode(description) + '\n' +
                " " + '\n' +
                link + '\n' +
                "#" + item.Link.Split('/')[2].Replace(".", "_");

            return text;
        }
    }
}
