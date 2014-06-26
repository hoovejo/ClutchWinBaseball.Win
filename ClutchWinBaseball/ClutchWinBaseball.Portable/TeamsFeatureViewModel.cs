using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.DataModel;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ClutchWinBaseball.Portable
{
    public class TeamsFeatureViewModel : INotifyPropertyChanged
    {
        public TeamsFeatureViewModel()
        {
            FranchiseItems = new ObservableCollection<KeyedList<string, TeamsFranchisesViewModel>>();
            OpponentsItems = new ObservableCollection<KeyedList<string, TeamsOpponentsViewModel>>();
            TeamResultItems = new ObservableCollection<KeyedList<string, TeamsResultsViewModel>>();
            TeamDrillDownItems = new ObservableCollection<KeyedList<string, TeamsDrillDownViewModel>>();
        }

        public ObservableCollection<KeyedList<string, TeamsFranchisesViewModel>> FranchiseItems { get; private set; }
        public ObservableCollection<KeyedList<string, TeamsOpponentsViewModel>> OpponentsItems { get; private set; }
        public ObservableCollection<KeyedList<string, TeamsResultsViewModel>> TeamResultItems { get; private set; }
        public ObservableCollection<KeyedList<string, TeamsDrillDownViewModel>> TeamDrillDownItems { get; private set; }

        private bool _isLoadingData, _noOpponents, _opponentsGoBack, _noResults, _resultsGoBack, _noDrillDown, _drillDownGoBack;

        //Determine if we need to show loading spinner
        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set { if (value != _isLoadingData) { _isLoadingData = value; NotifyPropertyChanged("IsLoadingData"); } }
        }

        public bool NoOpponents
        {
            get { return _noOpponents; }
            set { if (value != _noOpponents) { _noOpponents = value; NotifyPropertyChanged("NoOpponents"); } }
        }

        public bool OpponentsGoBack
        {
            get { return _opponentsGoBack; }
            set { if (value != _opponentsGoBack) { _opponentsGoBack = value; NotifyPropertyChanged("OpponentsGoBack"); } }
        }

        public bool NoResults
        {
            get { return _noResults; }
            set { if (value != _noResults) { _noResults = value; NotifyPropertyChanged("NoResults"); } }
        }

        public bool ResultsGoBack
        {
            get { return _resultsGoBack; }
            set { if (value != _resultsGoBack) { _resultsGoBack = value; NotifyPropertyChanged("ResultsGoBack"); } }
        }
        
        public bool NoDrillDown
        {
            get { return _noDrillDown; }
            set { if (value != _noDrillDown) { _noDrillDown = value; NotifyPropertyChanged("NoDrillDown"); } }
        }

        public bool DrillDownGoBack
        {
            get { return _drillDownGoBack; }
            set { if (value != _drillDownGoBack) { _drillDownGoBack = value; NotifyPropertyChanged("DrillDownGoBack"); } }
        }

        //base list to build opponent list from
        private List<TeamsFranchisesViewModel> FranchiseList = new List<TeamsFranchisesViewModel>();

        /*
        public string FranchisesDataString { get; set; }
        public string TeamsResultsDataString { get; set; }
        public string TeamsDrillDownDataString { get; set; }
        */

        public async Task<bool> LoadFranchisesDataAsync(string cachedJson = "")
        {
            List<FranchiseModel> franchises;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetFranchisesAsync();
                if (svcResult == null) { return false; } //no results message
                //FranchisesDataString = svcResult;
                franchises = JsonConvert.DeserializeObject<List<FranchiseModel>>(svcResult);
            }
            else
            {
                franchises = JsonConvert.DeserializeObject<List<FranchiseModel>>(cachedJson);
            }

            var groupedItems =
                from franchise in franchises
                orderby franchise.Location
                select new TeamsFranchisesViewModel
                {
                    FirstLetter = franchise.Location.Substring(0, 1),
                    LineOne = franchise.GetDisplayName(),
                    LineTwo = franchise.GetDetail(),
                    TeamId = franchise.RetroId,
                    Location = franchise.Location
                } into list
                group list by list.FirstLetter into listByYear
                select new KeyedList<string, TeamsFranchisesViewModel>(listByYear);

            if (FranchiseList.Count <= 0)
            {
                foreach (var franchise in franchises)
                {
                    FranchiseList.Add(new TeamsFranchisesViewModel()
                    {
                        LineOne = franchise.GetDisplayName(),
                        LineTwo = franchise.GetDetail(),
                        TeamId = franchise.RetroId,
                        Location = franchise.Location
                    });
                }
            }

            FranchiseItems.Clear();
            foreach (var viewModelItem in groupedItems)
            {
                FranchiseItems.Add(viewModelItem);
            }

            return true;
        }

        public void LoadOpponentsData(TeamsContextViewModel context, string cachedJson = "")
        {
            if (!string.IsNullOrEmpty(cachedJson) && FranchiseList.Count <= 0)
            {
                var franchises = JsonConvert.DeserializeObject<List<FranchiseModel>>(cachedJson);
                foreach (var franchise in franchises)
                {
                    FranchiseList.Add(new TeamsFranchisesViewModel()
                    {
                        LineOne = franchise.GetDisplayName(),
                        LineTwo = franchise.GetDetail(),
                        TeamId = franchise.RetroId,
                        Location = franchise.Location
                    });
                }
            }

            var groupedItems =
                from team in FranchiseList
                where team.TeamId != context.SelectedTeamId
                orderby team.Location
                select new TeamsOpponentsViewModel
                {
                    FirstLetter = team.Location.Substring(0, 1), 
                    LineOne = team.LineOne, 
                    LineTwo = team.LineTwo, 
                    TeamId = team.TeamId, 
                    Location = team.Location
                } into list
                group list by list.FirstLetter into listByYear
                select new KeyedList<string, TeamsOpponentsViewModel>(listByYear);

            OpponentsItems.Clear();
            foreach (var viewModelItem in groupedItems)
            {
                OpponentsItems.Add(viewModelItem);
            }
        }

        public async Task<bool> LoadTeamResultsDataAsync(TeamsContextViewModel context, string cachedJson = "")
        {
            List<TeamsResultModel> items;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetTeamResultsAsync(context.SelectedTeamId, context.SelectedOpponentId);
                if (svcResult == null) { return false; } //no results message
                //TeamsResultsDataString = svcResult;
                items = JsonConvert.DeserializeObject<List<TeamsResultModel>>(svcResult);
            }
            else
            {
                items = JsonConvert.DeserializeObject<List<TeamsResultModel>>(cachedJson);
            }

            var groupedItems =
                from item in items
                orderby item.Year descending
                select new TeamsResultsViewModel
                {
                    Year = item.Year,
                    Games = (int.Parse(item.Wins) + int.Parse(item.Losses)).ToString(),
                    Team = item.Team,
                    Opponent = item.Opponent,
                    Wins = item.Wins,
                    Losses = item.Losses,
                    RunsFor = item.RunsFor,
                    RunsAgainst = item.RunsAgainst
                } into list
                group list by list.Year into listByYear
                select new KeyedList<string, TeamsResultsViewModel>(listByYear);

            TeamResultItems.Clear();
            foreach (var viewModelItem in groupedItems)
            {
                TeamResultItems.Add(viewModelItem);
            }
            return true;
        }

        public async Task<bool> LoadTeamDrillDownDataAsync(TeamsContextViewModel context, string cachedJson = "")
        {
            List<TeamsDrillDownModel> items;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetTeamDrillDownAsync(context.SelectedTeamId, context.SelectedOpponentId, context.SelectedYearId);
                if (svcResult == null) { return false; } //no results message
                //TeamsDrillDownDataString = svcResult;
                items = JsonConvert.DeserializeObject<List<TeamsDrillDownModel>>(svcResult);
            }
            else
            {
                items = JsonConvert.DeserializeObject<List<TeamsDrillDownModel>>(cachedJson);
            }

            var groupedItems =
                from item in items
                orderby item.GameDate descending
                select new TeamsDrillDownViewModel
                {
                    GameDate = item.GameDate,
                    Team = item.Team,
                    Opponent = item.Opponent,
                    Win = item.Win,
                    Loss = item.Loss,
                    RunsFor = item.RunsFor,
                    RunsAgainst = item.RunsAgainst
                } into list
                group list by list.GameDate into listByYear
                select new KeyedList<string, TeamsDrillDownViewModel>(listByYear);

            TeamDrillDownItems.Clear();
            foreach (var viewModelItem in groupedItems)
            {
                TeamDrillDownItems.Add(viewModelItem);
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
