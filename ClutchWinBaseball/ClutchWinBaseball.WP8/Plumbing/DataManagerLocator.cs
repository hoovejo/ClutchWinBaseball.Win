using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.FeatureStateModel;
using Windows.Storage;

namespace ClutchWinBaseball.WP8
{
    public static class DataManagerLocator
    {
        private static TeamsDataManager _teamsDataManager;
        private static PlayersDataManager _playersDataManager;
        private static ContextCacheManager _contextCacheManager;

        public static TeamsDataManager TeamsDataManager
        {
            get
            {
                if (_teamsDataManager == null)
                {
                    TeamsContextViewModel teamsContext = TeamsContextViewModel.Instance;
                    var tempFolder = ApplicationData.Current.LocalFolder;
                    var fileManager = new CacheFileManager(tempFolder);
                    var cacheManager = new ContextCacheManager(fileManager);
                    _teamsDataManager = new TeamsDataManager(teamsContext, ViewModelLocator.Teams, fileManager, cacheManager);
                }
                return _teamsDataManager;
            }
        }

        public static PlayersDataManager PlayersDataManager
        {
            get
            {
                if (_playersDataManager == null) 
                {
                    PlayersContextViewModel playersContext = PlayersContextViewModel.Instance;
                    var tempFolder = ApplicationData.Current.LocalFolder;
                    var fileManager = new CacheFileManager(tempFolder);
                    var cacheManager = new ContextCacheManager(fileManager);
                    _playersDataManager = new PlayersDataManager(playersContext, ViewModelLocator.Players, fileManager, cacheManager);
                }
                return _playersDataManager;
            }
        }

        public static ContextCacheManager ContextCacheManager
        {
            get
            {
                if (_contextCacheManager == null) 
                {
                    var tempFolder = ApplicationData.Current.LocalFolder;
                    var fileManager = new CacheFileManager(tempFolder);
                    _contextCacheManager = new ContextCacheManager(fileManager);
                }
                return _contextCacheManager;
            }
        }
    }
}
