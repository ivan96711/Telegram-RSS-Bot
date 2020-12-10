using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_news_feed_bot.data
{
    public class AllUsers
    {
        private ObservableCollection<LoginUser> users { get; set; }
        /// <summary>
        /// Пусть сохранения.
        /// </summary>
        private string Path { get; set; }
        /// <summary>
        /// Сохранять изменения автоматически при изменении коллекции.
        /// </summary>
        public bool AutoSave { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get { return users.Count; } }

        /// <summary>
        /// AllUsers. Чтение json файла по пути Path. Если файл не найден, то создается новый. 
        /// </summary>
        /// <param name="Path">Путь к файлу.</param>
        /// <param name="AutoSave">Автоматическое сохранение. По умолчанию включено</param>
        public AllUsers(string Path, bool AutoSave = true)
        {
            this.Path = Path;
            this.AutoSave = AutoSave;
            users = File.Exists(Path) ? JsonConvert.DeserializeObject<ObservableCollection<LoginUser>>(File.ReadAllText(Path)) : new ObservableCollection<LoginUser>();
        }

        private void Save()
        {
            if (AutoSave)
                File.WriteAllText(@"data.json", JsonConvert.SerializeObject(users));
        }

        /// <summary>
        /// Возвращает или задает элемент по указанному индексу.
        /// </summary>
        /// <param name="index">Отчитываемый от нуля индекс элемента, который требуется возратить или задать.</param>
        /// <returns></returns>
        public LoginUser this[int index]
        {
            get
            {
                return users[index];
            }
        }

        public void Add(long UserId, string Username, string FirstName, string LastName)
        {
            users.Add(new LoginUser(UserId, Username, FirstName, LastName));
            Save();
        }

        public LoginUser GetUserById(long UserId)
        {
            foreach (var user in users)
                if (user.UserId == UserId)
                    return user;
            throw new ArgumentException("GetUserById: Пользователь не найден.");
        }
        /// <summary>
        /// Получение перечня ссылок пользователя по UserId.
        /// </summary>
        /// <param name="UserId">Id чата пользователя.</param>
        /// <returns></returns>
        public List<RssInfo> GetUserRss(long UserId)
        {
            return GetUserById(UserId).RssURL;
        }

        /// <summary>
        /// Проверка наличия пользователя в базе.
        /// </summary>
        /// <param name="UserId">Id чата пользователя.</param>
        /// <returns>true - пользователь найден; false - пользователь не найден.</returns>
        public bool ContainsId(long UserId)
        {
            foreach (var user in users)
                if (user.UserId == UserId)
                    return true;
            return false;
        }

        /// <summary>
        /// Добавление источника RSS к пользователю.
        /// </summary>
        /// <param name="UserId">Id чата пользователя.</param>
        /// <param name="RssURL">URL RSS источника</param>
        public void AddRssToUser(long UserId, string RssURL, string Title, string Description)
        {
            foreach (var user in users)
                if (user.UserId == UserId)
                {
                    foreach (var rss in user.RssURL)
                        if (rss.RssURL == RssURL)
                            throw new DuplicateWaitObjectException("AddRssToUser: " + RssURL + " уже присутствует в базе.");

                    user.RssURL.Add(new RssInfo(RssURL, Title, Description));
                    Save();
                    return;
                }
            throw new ArgumentException("AddRssToUser: пользователь не найден.");
        }

        public void DeleteUserRss(long UserId, string RssURL)
        {
            foreach (var user in users)
                if (user.UserId == UserId)
                {
                    for(int i = 0; i < user.RssURL.Count; i++)
                    {
                        if (user.RssURL[i].RssURL == RssURL)
                        {
                            user.RssURL.RemoveAt(i);
                            break;
                        }
                    }

                    Save();
                    return;
                }
            throw new ArgumentException("DeleteUserRss: пользователь не найден.");
        }

        public string GetLoginUserToString(long UserId)
        {
            foreach (var user in users)
                if(user.UserId == UserId)
                {
                    LoginUser userdata = user;
                    return userdata.UserId.ToString() + "/" + userdata.Username + "/" + userdata.FirstName + "/" + userdata.LastName;
                }

            throw new ArgumentException("GetLoginUserToString: пользователь не найден.");
        }
    }

    public class LoginUser
    {
        public readonly long UserId;
        public readonly string Username;
        public readonly string FirstName;
        public readonly string LastName;
        public List<RssInfo> RssURL { get; set; }

        public LoginUser(long UserId, string Username, string FirstName, string LastName)
        {
            this.UserId = UserId;
            this.Username = Username;
            this.FirstName = FirstName;
            this.LastName = LastName;
            RssURL = new List<RssInfo>();
        }
    }

    public class RssInfo
    {
        public readonly string Title;
        public readonly string RssURL;
        public readonly string Description;

        public RssInfo(string RssURL, string Title = null, string Description = null)
        {
            this.Title = Title;
            this.RssURL = RssURL;
            this.Description = Description;
        }
    }
}
