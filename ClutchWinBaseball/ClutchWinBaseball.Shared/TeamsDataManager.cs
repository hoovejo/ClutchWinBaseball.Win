using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using System.Threading.Tasks;

namespace ClutchWinBaseball
{
    public class TeamsDataManager
    {
        private TeamsContextViewModel _teamsContext;
        private TeamsFeatureViewModel _teamsViewModel;
        private CacheFileManager _fileManager;
        private ContextCacheManager _cacheManager;

        public TeamsDataManager(TeamsContextViewModel tc, TeamsFeatureViewModel tvm, CacheFileManager fm, ContextCacheManager c) 
        {
            _teamsContext = tc;
            _teamsViewModel = tvm;
            _fileManager = fm;
            _cacheManager = c;
        }

        public async Task<bool> GetFranchisesAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            try
            {
                if (!_teamsContext.HasLoadedFranchisesOncePerSession)
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _teamsViewModel.IsLoadingData = true;
                    returnValue = await _teamsViewModel.LoadFranchisesDataAsync();
                    await _cacheManager.SaveTeamsContextAsync(_teamsContext);

                    if (returnValue && _teamsViewModel.FranchiseItems.Count > 0)
                    {
                        var jsonString = _teamsViewModel.FranchisesDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.TF_CacheFileKey, jsonString);
                        _teamsContext.HasLoadedFranchisesOncePerSession = true;
                    }
                }
                else
                {
                    if (_teamsViewModel.FranchiseItems.Count <= 0)
                    {
                        _teamsViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.TF_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _teamsViewModel.LoadFranchisesDataAsync(jsonString);
                        }
                    }
                    else
                    {
                        returnValue = true;
                    }
                }
            }
            catch { _teamsContext.HasLoadedFranchisesOncePerSession = false; }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            return returnValue;
        }

        public async Task<bool> GetOpponentsAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            try
            {
                if (_teamsContext.ShouldFilterOpponents(isNetAvailable))
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _teamsViewModel.IsLoadingData = true;
                    _teamsViewModel.LoadOpponentsData(_teamsContext);
                    await _cacheManager.SaveTeamsContextAsync(_teamsContext);
                    returnValue = true;
                }
                else
                {
                    if (_teamsViewModel.OpponentsItems.Count <= 0)
                    {
                        _teamsViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.TF_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            _teamsViewModel.LoadOpponentsData(_teamsContext, jsonString);
                        }
                    }
                    returnValue = true;
                }
            }
            catch { }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            return returnValue;
        }

        public async Task<bool> GetTeamsResultsAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            try
            {
                if (_teamsContext.ShouldExecuteTeamResultsSearch(isNetAvailable))
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _teamsViewModel.IsLoadingData = true;
                    returnValue = await _teamsViewModel.LoadTeamResultsDataAsync(_teamsContext);
                    await _cacheManager.SaveTeamsContextAsync(_teamsContext);

                    if (returnValue && _teamsViewModel.TeamResultItems.Count > 0)
                    {
                        var jsonString = _teamsViewModel.TeamsResultsDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.TR_CacheFileKey, jsonString);
                    }
                }
                else
                {
                    if (_teamsViewModel.TeamResultItems.Count <= 0)
                    {
                        _teamsViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.TR_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _teamsViewModel.LoadTeamResultsDataAsync(_teamsContext, jsonString);
                        }
                    }
                    else
                    {
                        returnValue = true;
                    }
                }
            }
            catch { }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            return returnValue;
        }

        public async Task<bool> GetTeamsDrillDownAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            try
            {
                if (_teamsContext.ShouldExecuteTeamDrillDownSearch(isNetAvailable))
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _teamsViewModel.IsLoadingData = true;
                    returnValue = await _teamsViewModel.LoadTeamDrillDownDataAsync(_teamsContext);
                    await _cacheManager.SaveTeamsContextAsync(_teamsContext);

                    if (returnValue && _teamsViewModel.TeamDrillDownItems.Count > 0)
                    {
                        var jsonString = _teamsViewModel.TeamsDrillDownDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.TDD_CacheFileKey, jsonString);
                    }
                }
                else
                {
                    if (_teamsViewModel.TeamDrillDownItems.Count <= 0)
                    {
                        _teamsViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.TDD_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _teamsViewModel.LoadTeamDrillDownDataAsync(_teamsContext, jsonString);
                        }
                    }
                    else
                    {
                        returnValue = true;
                    }
                }
            }
            catch { }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            return returnValue;
        }
    }
}
