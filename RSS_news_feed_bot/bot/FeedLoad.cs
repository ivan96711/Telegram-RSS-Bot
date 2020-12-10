using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RSS_news_feed_bot.bot
{
    static class FeedLoad
    {
        public static Feed GetItems(string url)
        {
            var urlsTask = FeedReader.GetFeedUrlsFromUrlAsync(url);
            var urls = urlsTask.Result;

            string feedUrl;
            if (urls == null || urls.Count() < 1)
                feedUrl = url;
            else if (urls.Count() == 1)
                feedUrl = urls.First().Url;
            else if (urls.Count() == 2)
                feedUrl = urls.First().Url;
            else
            {
                throw new Exception("This RSS is not supported");

                //Найти и настроить RSS с несколькими каналами
            }

            var readerTask = FeedReader.ReadAsync(feedUrl);
            readerTask.ConfigureAwait(false);
            return readerTask.Result;
        }
        /// <summary>
        /// Получение новых постов за последний период.
        /// </summary>
        /// <param name="url">Url Rss источника</param>
        /// <param name="updatePeriod">Период в миллисекундах.</param>
        /// <returns></returns>
        public static IEnumerable<FeedItem> GetNewItems(string url, int updatePeriod)
        {
            DateTime dateTime = DateTime.UtcNow.AddMilliseconds(-updatePeriod);
            IEnumerable<FeedItem> tmp = GetItems(url).Items.OfType<FeedItem>().Where(a => a.PublishingDate > dateTime);
            return tmp;
        }
    }
}
