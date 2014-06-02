using ClutchWinBaseball.Exceptions;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using System;
using System.Threading.Tasks;

namespace ClutchWinBaseball
{
    public class PlayersDataManager
    {
        private PlayersContextViewModel _playersContext;
        private PlayersFeatureViewModel _playersViewModel;
        private CacheFileManager _fileManager;
        private ContextCacheManager _cacheManager;

        public PlayersDataManager(PlayersContextViewModel pc, PlayersFeatureViewModel pvm, CacheFileManager fm, ContextCacheManager c) 
        {
            _playersContext = pc;
            _playersViewModel = pvm;
            _fileManager = fm;
            _cacheManager = c;
        }

        public async Task<bool> GetSeasonsAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            Exception exRef = null;

            try
            {
                if (!_playersContext.HasLoadedSeasonsOncePerSession)
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _playersViewModel.IsLoadingData = true;
                    returnValue = await _playersViewModel.LoadYearDataAsync(_playersContext);
                    await _cacheManager.SavePlayersContextAsync(_playersContext);

                    if (returnValue && _playersViewModel.YearItems.Count > 0)
                    {
                        var jsonString = _playersViewModel.YearsDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.PY_CacheFileKey, jsonString);
                        _playersContext.HasLoadedSeasonsOncePerSession = true;
                    }
                }
                else
                {
                    returnValue = true;
                    if (_playersViewModel.YearItems.Count <= 0)
                    {
                        _playersViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.PY_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _playersViewModel.LoadYearDataAsync(_playersContext, jsonString);
                        }
                    }
                }
            }
            catch (Exception ex) { _playersContext.HasLoadedSeasonsOncePerSession = false; exRef = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }

            if (exRef != null)
            {
                await ExceptionHandler.HandleException(exRef);
            }
            return returnValue;
        }

        public async Task<bool> GetTeamsAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            Exception exRef = null;

            try
            {
                if (_playersContext.ShouldExecuteLoadTeams(isNetAvailable))
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _playersViewModel.IsLoadingData = true;
                    returnValue = await _playersViewModel.LoadTeamDataAsync(_playersContext);
                    await _cacheManager.SavePlayersContextAsync(_playersContext);

                    if (returnValue && _playersViewModel.TeamItems.Count > 0)
                    {
                        var jsonString = _playersViewModel.TeamsDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.PT_CacheFileKey, jsonString);
                    }
                }
                else
                {
                    returnValue = true;
                    if (_playersViewModel.TeamItems.Count <= 0)
                    {
                        _playersViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.PT_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _playersViewModel.LoadTeamDataAsync(_playersContext, jsonString);
                        }
                    }
                }
            }
            catch (Exception ex) { exRef = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exRef != null)
            {
                await ExceptionHandler.HandleException(exRef);
            }
            return returnValue;
        }

        public async Task<bool> GetBattersAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            Exception exRef = null;

            try
            {
                if (_playersContext.ShouldExecuteLoadBatters(isNetAvailable))
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _playersViewModel.IsLoadingData = true;
                    returnValue = await _playersViewModel.LoadBatterDataAsync(_playersContext);
                    await _cacheManager.SavePlayersContextAsync(_playersContext);

                    if (returnValue && _playersViewModel.BatterItems.Count > 0)
                    {
                        var jsonString = _playersViewModel.BattersDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.PB_CacheFileKey, jsonString);
                    }
                }
                else
                {
                    returnValue = true;
                    if (_playersViewModel.BatterItems.Count <= 0)
                    {
                        _playersViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.PB_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _playersViewModel.LoadBatterDataAsync(_playersContext, jsonString);
                        }
                    }
                }
            }
            catch (Exception ex) { exRef = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exRef != null)
            {
                await ExceptionHandler.HandleException(exRef);
            }
            return returnValue;
        }

        public async Task<bool> GetPitchersAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            Exception exRef = null;

            try
            {
                if (_playersContext.ShouldExecuteLoadPitchers(isNetAvailable))
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _playersViewModel.IsLoadingData = true;
                    returnValue = await _playersViewModel.LoadPitcherDataAsync(_playersContext);
                    await _cacheManager.SavePlayersContextAsync(_playersContext);

                    if (returnValue && _playersViewModel.PitcherItems.Count > 0)
                    {
                        var jsonString = _playersViewModel.PitchersDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.PP_CacheFileKey, jsonString);
                    }
                }
                else
                {
                    returnValue = true;
                    if (_playersViewModel.PitcherItems.Count <= 0)
                    {
                        _playersViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.PP_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _playersViewModel.LoadPitcherDataAsync(_playersContext, jsonString);
                        }
                    }
                }
            }
            catch (Exception ex) { exRef = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exRef != null)
            {
                await ExceptionHandler.HandleException(exRef);
            }
            return returnValue;
        }

        public async Task<bool> GetPlayersResultsAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            Exception exRef = null;

            try
            {
                if (_playersContext.ShouldExecutePlayerResultsSearch(isNetAvailable))
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _playersViewModel.IsLoadingData = true;
                    returnValue = await _playersViewModel.LoadPlayerResultsDataAsync(_playersContext);
                    await _cacheManager.SavePlayersContextAsync(_playersContext);

                    if (returnValue && _playersViewModel.PlayerResultItems.Count > 0)
                    {
                        var jsonString = _playersViewModel.PlayersResultsDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.PR_CacheFileKey, jsonString);
                    }
                }
                else
                {
                    returnValue = true;
                    if (_playersViewModel.PlayerResultItems.Count <= 0)
                    {
                        _playersViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.PR_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _playersViewModel.LoadPlayerResultsDataAsync(_playersContext, jsonString);
                        }
                    }
                }
            }
            catch (Exception ex) { exRef = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exRef != null)
            {
                await ExceptionHandler.HandleException(exRef);
            }
            return returnValue;
        }

        public async Task<bool> GetPlayersDrillDownAsync(bool isNetAvailable)
        {
            bool returnValue = false;
            Exception exRef = null;

            try
            {
                if (_playersContext.ShouldExecutePlayersDrillDownSearch(isNetAvailable))
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _playersViewModel.IsLoadingData = true;
                    returnValue = await _playersViewModel.LoadPlayerDrillDownDataAsync(_playersContext);
                    await _cacheManager.SavePlayersContextAsync(_playersContext);

                    if (returnValue && _playersViewModel.PlayerDrillDownItems.Count > 0)
                    {
                        var jsonString = _playersViewModel.PlayersDrillDownDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.PDD_CacheFileKey, jsonString);
                    }
                }
                else
                {
                    returnValue = true;
                    if (_playersViewModel.PlayerDrillDownItems.Count <= 0)
                    {
                        _playersViewModel.IsLoadingData = true;
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.PDD_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _playersViewModel.LoadPlayerDrillDownDataAsync(_playersContext, jsonString);
                        }
                    }
                }
            }
            catch (Exception ex) { exRef = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exRef != null)
            {
                await ExceptionHandler.HandleException(exRef);
            }
            return returnValue;
        }
    }
}

