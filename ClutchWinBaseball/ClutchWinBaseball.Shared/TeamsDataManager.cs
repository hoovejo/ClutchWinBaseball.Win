using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using System.Threading.Tasks;

namespace ClutchWinBaseball
{
    public enum TeamsEndpoints
    {
        Franchises,
        Opponents,
        FranchiseSearch,
        FranchiseYearSearch
    }

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

        public async Task<bool> LoadTeamsDataAsync(TeamsEndpoints endpoint, bool isNetAvailable)
        {
            bool returnValue = false;

            switch (endpoint)
            {
                case TeamsEndpoints.Franchises:
                    {
                        if (!_teamsContext.HasLoadedFranchisesOncePerSession)
                        {
                            //cache reads are allowed if no network, but svc calls not allowed
                            if (!isNetAvailable) { returnValue = false; break; }

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
                    break;
                case TeamsEndpoints.Opponents:
                    {
                        if (_teamsContext.ShouldFilterOpponents(isNetAvailable))
                        {
                            //cache reads are allowed if no network, but svc calls not allowed
                            if (!isNetAvailable) { returnValue = false; break; }

                            _teamsViewModel.LoadOpponentsData(_teamsContext);
                            await _cacheManager.SaveTeamsContextAsync(_teamsContext);
                            returnValue = true;
                        }
                        else
                        {
                            if (_teamsViewModel.OpponentsItems.Count <= 0)
                            {
                                var jsonString = await _fileManager.CacheInquiryAsync(Config.TF_CacheFileKey);
                                if (!string.IsNullOrEmpty(jsonString))
                                {
                                    _teamsViewModel.LoadOpponentsData(_teamsContext, jsonString);
                                } 
                            }
                            returnValue = true;
                        }
                    }
                    break;
                case TeamsEndpoints.FranchiseSearch:
                    {
                        if (_teamsContext.ShouldExecuteTeamResultsSearch(isNetAvailable))
                        {
                            //cache reads are allowed if no network, but svc calls not allowed
                            if (!isNetAvailable) { returnValue = false; break; }

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
                    break;
                case TeamsEndpoints.FranchiseYearSearch:
                    {
                        if (_teamsContext.ShouldExecuteTeamDrillDownSearch(isNetAvailable))
                        {
                            //cache reads are allowed if no network, but svc calls not allowed
                            if (!isNetAvailable) { returnValue = false; break; }

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
                    break;
                default:
                    break;
            }

            return returnValue;
        }
    }
}
