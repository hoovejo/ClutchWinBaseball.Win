using BugSense;
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

                if (!_teamsContext.HasLoadedFranchisesOncePerSession)
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    returnValue = await _teamsViewModel.LoadFranchisesDataAsync();
                    await _cacheManager.SaveTeamsContextAsync(_teamsContext);

                    if (returnValue && _teamsViewModel.FranchiseItems.Count > 0)
                    {
                        var jsonString = _teamsViewModel.FranchisesDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.TF_CacheFileKey, jsonString);
                        _teamsContext.HasLoadedFranchisesOncePerSession = true;
                    }
                    else { await _fileManager.DeleteFileAsync(Config.TF_CacheFileKey); }
                }
                else
                {
                    returnValue = true;
                    if (_teamsViewModel.FranchiseItems.Count <= 0)
                    {
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.TF_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _teamsViewModel.LoadFranchisesDataAsync(jsonString);
                        }
                    }
                }
            }
            catch (Exception ex) { _teamsContext.HasLoadedFranchisesOncePerSession = false; exception = ex; }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { var result = await BugSenseHandler.Instance.LogExceptionAsync(exception); } catch { } exception = null; }
            return returnValue;
        }

        public async Task<bool> GetOpponentsAsync(bool isNetAvailable)
        {
            bool returnValue = false;

            try
            {
                _teamsViewModel.IsLoadingData = true;

                if (_teamsContext.ShouldFilterOpponents(isNetAvailable))
                {
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    _teamsViewModel.LoadOpponentsData(_teamsContext);
                    await _cacheManager.SaveTeamsContextAsync(_teamsContext);
                    returnValue = true;
                }
                else
                {
                    returnValue = true;
                    if (_teamsViewModel.OpponentsItems.Count <= 0 && !string.IsNullOrEmpty(_teamsContext.SelectedTeamId))
                    {
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.TF_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            _teamsViewModel.LoadOpponentsData(_teamsContext, jsonString);
                        }
                    }
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { var result = await BugSenseHandler.Instance.LogExceptionAsync(exception); } catch { } exception = null; }
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
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    returnValue = await _teamsViewModel.LoadTeamResultsDataAsync(_teamsContext);
                    await _cacheManager.SaveTeamsContextAsync(_teamsContext);

                    if (returnValue && _teamsViewModel.TeamResultItems.Count > 0)
                    {
                        var jsonString = _teamsViewModel.TeamsResultsDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.TR_CacheFileKey, jsonString);
                    }
                    else
                    {
                        await _fileManager.DeleteFileAsync(Config.TR_CacheFileKey);
                        _teamsViewModel.NoResults = true;
                    }
                }
                else
                {
                    returnValue = true;
                    if (_teamsViewModel.TeamResultItems.Count <= 0)
                    {
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.TR_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _teamsViewModel.LoadTeamResultsDataAsync(_teamsContext, jsonString);
                        }
                        else { _teamsViewModel.ResultsGoBack = true; }
                    }
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { var result = await BugSenseHandler.Instance.LogExceptionAsync(exception); } catch { } exception = null; }
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
                    //cache reads are allowed if no network, but svc calls not allowed
                    if (!isNetAvailable) { returnValue = false; }

                    returnValue = await _teamsViewModel.LoadTeamDrillDownDataAsync(_teamsContext);
                    await _cacheManager.SaveTeamsContextAsync(_teamsContext);

                    if (returnValue && _teamsViewModel.TeamDrillDownItems.Count > 0)
                    {
                        var jsonString = _teamsViewModel.TeamsDrillDownDataString;
                        returnValue = await _fileManager.CacheUpdateAsync(Config.TDD_CacheFileKey, jsonString);
                    }
                    else
                    {
                        await _fileManager.DeleteFileAsync(Config.TDD_CacheFileKey);
                        _teamsViewModel.NoDrillDown = true;
                    }
                }
                else
                {
                    returnValue = true;
                    if (_teamsViewModel.TeamDrillDownItems.Count <= 0)
                    {
                        var jsonString = await _fileManager.CacheInquiryAsync(Config.TDD_CacheFileKey);
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            returnValue = await _teamsViewModel.LoadTeamDrillDownDataAsync(_teamsContext, jsonString);
                        }
                        else { _teamsViewModel.DrillDownGoBack = true; }
                    }
                }
            }
            catch (Exception ex) { exception = ex; }
            finally
            {
                _teamsViewModel.IsLoadingData = false;
            }
            if (exception != null) { try { var result = await BugSenseHandler.Instance.LogExceptionAsync(exception); } catch { } exception = null; }
            return returnValue;
        }
    }
}
