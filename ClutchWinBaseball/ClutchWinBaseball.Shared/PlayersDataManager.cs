using CrittercismSDK;
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
        private Exception exception;

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

            try
            {
                _playersViewModel.IsLoadingData = true;

                if (!_playersContext.HasLoadedSeasonsOncePerSession || _playersViewModel.YearItems.Count <= 0)
                {
                    if (isNetAvailable)
                    {
                        returnValue = await _playersViewModel.LoadYearDataAsync(_playersContext);
                        if (returnValue) { _playersContext.HasLoadedSeasonsOncePerSession = true; }
                    }
                }
                else
                {
                    returnValue = true;
                }
            }
            catch (Exception ex) { _playersContext.HasLoadedSeasonsOncePerSession = false; exception = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }

        public async Task<bool> GetTeamsAsync(bool isNetAvailable)
        {
            bool returnValue = false;

            try
            {
                _playersViewModel.IsLoadingData = true;

                if (_playersContext.ShouldExecuteLoadTeams(isNetAvailable))
                {
                    if (isNetAvailable)
                    {
                        returnValue = await _playersViewModel.LoadTeamDataAsync(_playersContext);
                    }
                }
                else
                {
                    returnValue = true;
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }

        public async Task<bool> GetBattersAsync(bool isNetAvailable)
        {
            bool returnValue = false;

            try
            {
                _playersViewModel.IsLoadingData = true;

                if (_playersContext.ShouldExecuteLoadBatters(isNetAvailable))
                {
                    if (isNetAvailable)
                    {
                        returnValue = await _playersViewModel.LoadBatterDataAsync(_playersContext);
                    }
                }
                else 
                {
                    returnValue = true;
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }

        public async Task<bool> GetPitchersAsync(bool isNetAvailable)
        {
            bool returnValue = false;

            try
            {
                _playersViewModel.IsLoadingData = true;
                _playersViewModel.NoPitchers = false;
                _playersViewModel.PitchersGoBack = false;

                if (_playersContext.ShouldExecuteLoadPitchers(isNetAvailable))
                {
                    if (isNetAvailable) 
                    {
                        returnValue = await _playersViewModel.LoadPitcherDataAsync(_playersContext);
                    }

                    if (returnValue && _playersViewModel.PitcherItems.Count <= 0)
                    {
                        _playersViewModel.NoPitchers = true;
                    }
                }
                else
                {
                    returnValue = true;
                    if (!_playersContext.PlayersPitchersServiceCallAllowed() && _playersViewModel.PitcherItems.Count <= 0)
                    {
                        _playersViewModel.PitchersGoBack = true;
                    }
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }

        public async Task<bool> GetPlayersResultsAsync(bool isNetAvailable)
        {
            bool returnValue = false;

            try
            {
                _playersViewModel.IsLoadingData = true;
                _playersViewModel.NoResults = false;
                _playersViewModel.ResultsGoBack = false;

                if (_playersContext.ShouldExecutePlayerResultsSearch(isNetAvailable))
                {
                    if (isNetAvailable) 
                    {
                        returnValue = await _playersViewModel.LoadPlayerResultsDataAsync(_playersContext);
                    }

                    if (returnValue && _playersViewModel.PlayerResultItems.Count <= 0)
                    {
                        _playersViewModel.NoResults = true;
                    }
                }
                else
                {
                    returnValue = true;
                    if (!_playersContext.PlayersResultsServiceCallAllowed() && _playersViewModel.PlayerResultItems.Count <= 0)
                    {
                        _playersViewModel.ResultsGoBack = true;
                    }
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }

        public async Task<bool> GetPlayersDrillDownAsync(bool isNetAvailable)
        {
            bool returnValue = false;

            try
            {
                _playersViewModel.IsLoadingData = true;
                _playersViewModel.NoDrillDown = false;
                _playersViewModel.DrillDownGoBack = false;

                if (_playersContext.ShouldExecutePlayersDrillDownSearch(isNetAvailable))
                {
                    if (isNetAvailable)
                    {
                        returnValue = await _playersViewModel.LoadPlayerDrillDownDataAsync(_playersContext);
                    }

                    if (returnValue && _playersViewModel.PlayerDrillDownItems.Count <= 0)
                    {
                        _playersViewModel.NoDrillDown = true;
                    }
                }
                else
                {
                    returnValue = true;
                    if (!_playersContext.PlayersDrillDownServiceCallAllowed() && _playersViewModel.PlayerDrillDownItems.Count <= 0)
                    {
                        _playersViewModel.DrillDownGoBack = true;
                    }
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _playersViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }
    }
}