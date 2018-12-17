using System;
using TellStoryTogether.Models;
using WebMatrix.WebData;

namespace TellStoryTogether.Helper
{
    public class NavPanel
    {
        private readonly int _point;
        private readonly int _countUnseenNotification;
        private readonly NotificationShow[] _allNotifications;
        private readonly int _countAllNotification;
        private readonly Article[] _userScripts;
        private readonly Article[] _userFavorites;
        private readonly int _countScripts;
        private readonly int _countFavorites;
        private readonly Genre[] _genres;
        private readonly Language[] _languages;

        public NavPanel(int take)
        {
            var dal = new DAL(WebSecurity.CurrentUserId);
            _point = dal.UserPoint();
            Tuple<int, NotificationShow[]> notification = dal.GeteNotification();
            _countUnseenNotification = notification.Item1;
            _allNotifications = notification.Item2;
            _countAllNotification = _allNotifications.Length;
            Tuple<Article[], int> scripts = dal.GetFirstNScriptArticle(take);
            _userScripts = scripts.Item1;
            _countScripts = scripts.Item2;
            Tuple<Article[], int> favorites = dal.GetFirstNFavoriteArticle(take);
            _userFavorites = favorites.Item1;
            _countFavorites = favorites.Item2;
            _genres = dal.GetGenres().ToArray();
            _languages = dal.GetLanguages();
        }

        public int UserPoints()
        {
            return _point;
        }

        public int CountUnseenNotification()
        {
            return _countUnseenNotification;
        }

        public NotificationShow[] UserAllNotifications()
        {
            return _allNotifications;
        }

        public int CountAllNotification()
        {
            return _countAllNotification;
        }

        public Article[] UserScripts()
        {
            return _userScripts;
        }

        public Article[] UserFavorites()
        {
            return _userFavorites;
        }

        public int CountScripts()
        {
            return _countScripts;
        }

        public int CountFavorites()
        {
            return _countFavorites;
        }

        public Genre[] AllGenres()
        {
            return _genres;
        }

        public Language[] AllLanguages()
        {
            return _languages;
        }
    }
}