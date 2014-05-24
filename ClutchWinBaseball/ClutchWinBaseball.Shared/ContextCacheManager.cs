using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClutchWinBaseball
{
    public class ContextCacheManager
    {
        private CacheFileManager _fileManager;

        public ContextCacheManager(CacheFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public async Task<bool> SaveTeamsContextAsync(TeamsContextViewModel context)
        {
            bool returnValue = true;
            var jsonString = JsonConvert.SerializeObject(context);
            returnValue = await _fileManager.CacheUpdateAsync(Config.TC_CacheFileKey, jsonString);
            return returnValue;
        }

        public async Task<TeamsContextViewModel> ReadTeamsContextAsync()
        {
            TeamsContextViewModel vm = null;
            var jsonString = await _fileManager.CacheInquiryAsync(Config.TC_CacheFileKey);
            if (!string.IsNullOrEmpty(jsonString))
            {
                vm = JsonConvert.DeserializeObject<TeamsContextViewModel>(jsonString);
            }
            return vm;
        }

        public async Task<bool> SavePlayersContextAsync(PlayersContextViewModel context)
        {
            bool returnValue = true;
            var jsonString = JsonConvert.SerializeObject(context);
            returnValue = await _fileManager.CacheUpdateAsync(Config.PC_CacheFileKey, jsonString);
            return returnValue;
        }

        public async Task<PlayersContextViewModel> ReadPlayersContextAsync()
        {
            PlayersContextViewModel vm = null;
            var jsonString = await _fileManager.CacheInquiryAsync(Config.PC_CacheFileKey);
            if (!string.IsNullOrEmpty(jsonString))
            {
                vm = JsonConvert.DeserializeObject<PlayersContextViewModel>(jsonString);
            }
            return vm;
        }
    }
}
