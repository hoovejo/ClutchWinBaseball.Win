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

        public TeamsDataManager(TeamsContextViewModel teamsContext, TeamsFeatureViewModel teamsViewModel, CacheFileManager fileManager) 
        {
            _teamsContext = teamsContext;
            _teamsViewModel = teamsViewModel;
            _fileManager = fileManager;
        }

        public async Task<bool> LoadTeamsDataAsync(TeamsEndpoints endpoint, bool isNetworkAvailable)
        {
            bool returnValue = false;

            switch (endpoint)
            {
                case TeamsEndpoints.Franchises:
                    {
                        if (!_teamsContext.HasLoadedFranchisesOncePerSession)
                        {
                            //cache reads are allowed if no network, but svc calls not allowed
                            if (!isNetworkAvailable) { returnValue = false; break; }

                            returnValue = await _teamsViewModel.LoadFranchisesDataAsync();

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
                        if (_teamsContext.ShouldFilterOpponents(isNetworkAvailable, _teamsViewModel.SelectedTeamId))
                        {
                            //cache reads are allowed if no network, but svc calls not allowed
                            if (!isNetworkAvailable) { returnValue = false; break; }

                            _teamsViewModel.LoadOpponentsData();
                            returnValue = true;
                        }
                        else
                        {
                            if (_teamsViewModel.OpponentsItems.Count <= 0)
                            {
                                var jsonString = await _fileManager.CacheInquiryAsync(Config.TF_CacheFileKey);
                                if (!string.IsNullOrEmpty(jsonString))
                                {
                                    _teamsViewModel.LoadOpponentsData(jsonString);
                                } 
                            }
                            returnValue = true;
                        }
                    }
                    break;
                case TeamsEndpoints.FranchiseSearch:
                    {
                        if (_teamsContext.ShouldExecuteTeamResultsSearch(isNetworkAvailable, _teamsViewModel.SelectedTeamId, 
                            _teamsViewModel.SelectedOpponentId))
                        {
                            //cache reads are allowed if no network, but svc calls not allowed
                            if (!isNetworkAvailable) { returnValue = false; break; }

                            returnValue = await _teamsViewModel.LoadTeamResultsDataAsync();

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
                                    returnValue = await _teamsViewModel.LoadTeamResultsDataAsync(jsonString);
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
                        if (_teamsContext.ShouldExecuteTeamDrillDownSearch(isNetworkAvailable, _teamsViewModel.SelectedTeamId, 
                            _teamsViewModel.SelectedOpponentId, _teamsViewModel.SelectedYearId))
                        {
                            //cache reads are allowed if no network, but svc calls not allowed
                            if (!isNetworkAvailable) { returnValue = false; break; }

                            returnValue = await _teamsViewModel.LoadTeamDrillDownDataAsync();

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
                                    returnValue = await _teamsViewModel.LoadTeamDrillDownDataAsync(jsonString); 
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
