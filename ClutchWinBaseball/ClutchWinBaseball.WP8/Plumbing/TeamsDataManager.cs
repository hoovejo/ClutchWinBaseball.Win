using CrittercismSDK;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.WP8.Exceptions;
using System;
using System.Threading.Tasks;

namespace ClutchWinBaseball.WP8
{
    public class TeamsDataManager
    {
        private TeamsContextViewModel _teamsContext;
        private TeamsFeatureViewModel _teamsViewModel;
        private CacheFileManager _fileManager;
        private ContextCacheManager _cacheManager;
        private Exception exception;

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
                _teamsViewModel.IsLoadingData = true;

                if (!_teamsContext.HasLoadedFranchisesOncePerSession || _teamsViewModel.FranchiseItems.Count <= 0)
                {
                    if (isNetAvailable)
                    {
                        returnValue = await _teamsViewModel.LoadFranchisesDataAsync();
                        if (returnValue) { _teamsContext.HasLoadedFranchisesOncePerSession = true; }
                    }
                }
                else
                {
                    returnValue = true;
                }
            }
            catch (Exception ex) { _teamsContext.HasLoadedFranchisesOncePerSession = false; exception = ex; }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }

        public bool GetOpponents(bool isNetAvailable)
        {
            bool returnValue = false;

            try
            {
                _teamsViewModel.IsLoadingData = true;

                if (_teamsContext.ShouldFilterOpponents(isNetAvailable))
                {
                    _teamsViewModel.LoadOpponentsData(_teamsContext);
                    returnValue = true;
                    if (_teamsViewModel.OpponentsItems.Count <= 0)
                    {
                        _teamsViewModel.NoOpponents = true;
                    }
                }
                else
                {
                    returnValue = true;
                    if (string.IsNullOrEmpty(_teamsContext.SelectedTeamId) && _teamsViewModel.OpponentsItems.Count <= 0)
                    {
                        _teamsViewModel.OpponentsGoBack = true;
                    }
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }

        public async Task<bool> GetTeamsResultsAsync(bool isNetAvailable)
        {
            bool returnValue = false;

            try
            {
                _teamsViewModel.IsLoadingData = true;
                _teamsViewModel.ResultsGoBack = false;
                _teamsViewModel.NoResults = false;

                if (_teamsContext.ShouldExecuteTeamResultsSearch(isNetAvailable))
                {
                    if (isNetAvailable)
                    {
                        returnValue = await _teamsViewModel.LoadTeamResultsDataAsync(_teamsContext);
                    }

                    if (returnValue && _teamsViewModel.TeamResultItems.Count <= 0)
                    {
                        _teamsViewModel.NoResults = true;
                    }
                }
                else
                {
                    returnValue = true;
                    if (!_teamsContext.TeamsResultsServiceCallAllowed() && _teamsViewModel.TeamResultItems.Count <= 0)
                    {
                        _teamsViewModel.ResultsGoBack = true;
                    }
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }

        public async Task<bool> GetTeamsDrillDownAsync(bool isNetAvailable)
        {
            bool returnValue = false;

            try
            {
                _teamsViewModel.IsLoadingData = true;
                _teamsViewModel.NoDrillDown = false;
                _teamsViewModel.DrillDownGoBack = false;

                if (_teamsContext.ShouldExecuteTeamDrillDownSearch(isNetAvailable))
                {
                    if (isNetAvailable)
                    {
                        returnValue = await _teamsViewModel.LoadTeamDrillDownDataAsync(_teamsContext);
                    }

                    if (returnValue && _teamsViewModel.TeamDrillDownItems.Count <= 0)
                    {
                        _teamsViewModel.NoDrillDown = true;
                    }
                }
                else
                {
                    returnValue = true;
                    if (!_teamsContext.TeamsDrillDownServiceCallAllowed() && _teamsViewModel.TeamDrillDownItems.Count <= 0)
                    {
                        _teamsViewModel.DrillDownGoBack = true;
                    }
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { Crittercism.LogHandledException(exception); } catch { } exception = null; }
            return returnValue;
        }
    }
}