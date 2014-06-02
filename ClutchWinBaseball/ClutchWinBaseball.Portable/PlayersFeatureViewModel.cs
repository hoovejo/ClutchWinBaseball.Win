using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.DataModel;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ClutchWinBaseball.Portable
{
    public class PlayersFeatureViewModel : INotifyPropertyChanged
    {
        public PlayersFeatureViewModel()
        {
            YearItems = new ObservableCollection<PlayersYearsViewModel>();
            TeamItems = new ObservableCollection<KeyedList<string, PlayersTeamsViewModel>>();
            BatterItems = new ObservableCollection<KeyedList<string, PlayersBattersViewModel>>();
            PitcherItems = new ObservableCollection<KeyedList<string, PlayersPitchersViewModel>>();
            PlayerResultItems = new ObservableCollection<KeyedList<string, PlayersResultsViewModel>>();
            PlayerDrillDownItems = new ObservableCollection<KeyedList<string, PlayersDrillDownViewModel>>();
        }

        public ObservableCollection<PlayersYearsViewModel> YearItems { get; private set; }
        public ObservableCollection<KeyedList<string, PlayersTeamsViewModel>> TeamItems { get; private set; }
        public ObservableCollection<KeyedList<string, PlayersBattersViewModel>> BatterItems { get; private set; }
        public ObservableCollection<KeyedList<string, PlayersPitchersViewModel>> PitcherItems { get; private set; }
        public ObservableCollection<KeyedList<string, PlayersResultsViewModel>> PlayerResultItems { get; private set; }
        public ObservableCollection<KeyedList<string, PlayersDrillDownViewModel>> PlayerDrillDownItems { get; private set; }

        private string _selectedYearId = "season";
        private string _selectedTeamId = "team";
        private bool _isLoadingData, _isShowingBatters;

        public string SelectedYearId
        {
            get { return _selectedYearId; }
            set { if (value != _selectedYearId) { _selectedYearId = value; NotifyPropertyChanged("SelectedYearId"); } }
        }

        public string SelectedTeamId
        {
            get { return _selectedTeamId; }
            set { if (value != _selectedTeamId) { _selectedTeamId = value; NotifyPropertyChanged("SelectedTeamId"); } }
        }

        public bool IsShowingBatters
        {
            get { return _isShowingBatters; }
            set { if (value != _isShowingBatters) { _isShowingBatters = value; NotifyPropertyChanged("IsShowingBatters"); } }
        }

        //Determine if we need to show loading spinner
        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set { if (value != _isLoadingData) { _isLoadingData = value; NotifyPropertyChanged("IsLoadingData"); } }
        }

        public string YearsDataString { get; set; }
        public string TeamsDataString { get; set; }
        public string BattersDataString { get; set; }
        public string PitchersDataString { get; set; }
        public string PlayersResultsDataString { get; set; }
        public string PlayersDrillDownDataString { get; set; }

        public async Task<bool> LoadYearDataAsync(PlayersContextViewModel context, string cachedJson = "")
        {
            List<YearModel> years;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetYearsAsync();
                if (svcResult == null) { return false; } //no results message
                YearsDataString = svcResult;
                years = JsonConvert.DeserializeObject<List<YearModel>>(svcResult);
            }
            else
            {
                years = JsonConvert.DeserializeObject<List<YearModel>>(cachedJson);
            }

            YearItems.Clear();
            foreach (var year in years)
            {
                this.YearItems.Add(new PlayersYearsViewModel() { LineOne = year.Id.ToString(CultureInfo.CurrentCulture) });
            }
            return true;
        }

        public async Task<bool> LoadTeamDataAsync(PlayersContextViewModel context, string cachedJson = "")
        {
            List<TeamModel> teams;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetTeamsAsync(context.SelectedYearId);
                if (svcResult == null) { return false; } //no results message
                TeamsDataString = svcResult;
                teams = JsonConvert.DeserializeObject<List<TeamModel>>(svcResult);
            }
            else
            {
                teams = JsonConvert.DeserializeObject<List<TeamModel>>(cachedJson);
            } 
            
            var groupedItems =
                from team in teams
                orderby team.Location
                select new PlayersTeamsViewModel
                {
                    FirstLetter = team.Location.Substring(0, 1),
                    LineOne = team.GetDisplayName(),
                    LineTwo = team.GetDetail(),
                    TeamId = team.TeamId
                } into list
                group list by list.FirstLetter into listByYear
                select new KeyedList<string, PlayersTeamsViewModel>(listByYear);

            TeamItems.Clear();
            foreach (var viewModelItem in groupedItems)
            {
                TeamItems.Add(viewModelItem);
            }
            return true;
        }

        public async Task<bool> LoadBatterDataAsync(PlayersContextViewModel context, string cachedJson = "")
        {
            List<BatterModel> batters;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetBattersAsync(context.SelectedTeamId, context.SelectedYearId);
                if (svcResult == null) { return false; } //no results message
                BattersDataString = svcResult;
                batters = JsonConvert.DeserializeObject<List<BatterModel>>(svcResult);
            }
            else
            {
                batters = JsonConvert.DeserializeObject<List<BatterModel>>(cachedJson);
            } 
            
            var groupedItems =
                from batter in batters
                orderby batter.FirstName
                select new PlayersBattersViewModel
                {
                    FirstLetter = batter.FirstName.Substring(0, 1),
                    LineOne = batter.GetDisplayName(),
                    BatterId = batter.RetroPlayerId
                } into list
                group list by list.FirstLetter into listByYear
                select new KeyedList<string, PlayersBattersViewModel>(listByYear);

            BatterItems.Clear();
            foreach (var viewModelItem in groupedItems)
            {
                BatterItems.Add(viewModelItem);
            }
            return true;
        }

        public async Task<bool> LoadPitcherDataAsync(PlayersContextViewModel context, string cachedJson = "")
        {
            List<PitcherModel> pitchers;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetPitchersAsync(context.SelectedBatterId, context.SelectedYearId);
                if (svcResult == null) { return false; } //no results message
                PitchersDataString = svcResult;
                pitchers = JsonConvert.DeserializeObject<List<PitcherModel>>(svcResult);
            }
            else
            {
                pitchers = JsonConvert.DeserializeObject<List<PitcherModel>>(cachedJson);
            } 

            var groupedItems =
                from pitcher in pitchers
                orderby pitcher.FirstName
                select new PlayersPitchersViewModel
                {
                    FirstLetter = pitcher.FirstName.Substring(0, 1),
                    LineOne = pitcher.GetDisplayName(),
                    PitcherId = pitcher.RetroId
                } into list
                group list by list.FirstLetter into listByYear
                select new KeyedList<string, PlayersPitchersViewModel>(listByYear);

            PitcherItems.Clear();
            foreach (var viewModelItem in groupedItems)
            {
                PitcherItems.Add(viewModelItem);
            }
            return true;
        }

        public async Task<bool> LoadPlayerResultsDataAsync(PlayersContextViewModel context, string cachedJson = "")
        {
            List<PlayersResultModel> items;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetPlayerResultsAsync(context.SelectedBatterId, context.SelectedPitcherId);
                if (svcResult == null) { return false; } //no results message
                PlayersResultsDataString = svcResult;
                items = JsonConvert.DeserializeObject<List<PlayersResultModel>>(svcResult);
            }
            else
            {
                items = JsonConvert.DeserializeObject<List<PlayersResultModel>>(cachedJson);
            } 

            var groupedItems =
                from item in items
                orderby item.Year descending
                select new PlayersResultsViewModel
                {
                    GameYear = item.Year,
                    Games = item.Games,
                    AtBat = item.AtBat,
                    Hit = item.Hit,
                    Walks = item.Walks,
                    SecondBase = item.SecondBase,
                    ThirdBase = item.ThirdBase,
                    HomeRun = item.HomeRun,
                    RunBattedIn = item.RunBattedIn,
                    StrikeOut = item.StrikeOut,
                    Average = item.Average.ToString("N3", CultureInfo.InvariantCulture)
                } into list
                group list by list.GameYear into listByYear
                select new KeyedList<string, PlayersResultsViewModel>(listByYear);

            PlayerResultItems.Clear();
            foreach (var viewModelItem in groupedItems)
            {
                PlayerResultItems.Add(viewModelItem);
            }
            return true;
        }

        public async Task<bool> LoadPlayerDrillDownDataAsync(PlayersContextViewModel context, string cachedJson = "")
        {
            List<PlayersDrillDownModel> items;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetPlayerDrillDownAsync(context.SelectedBatterId, context.SelectedPitcherId, context.SelectedGameYear);
                if (svcResult == null) { return false; } //no results message
                PlayersDrillDownDataString = svcResult;
                items = JsonConvert.DeserializeObject<List<PlayersDrillDownModel>>(svcResult);
            }
            else
            {
                items = JsonConvert.DeserializeObject<List<PlayersDrillDownModel>>(cachedJson);
            } 

            var groupedItems =
                from item in items
                orderby item.GameDate descending
                select new PlayersDrillDownViewModel
                {
                    GameDate = item.GameDate,
                    AtBat = item.AtBat,
                    Hit = item.Hit,
                    Walks = item.Walks,
                    SecondBase = item.SecondBase,
                    ThirdBase = item.ThirdBase,
                    HomeRun = item.HomeRun,
                    RunBattedIn = item.RunBattedIn,
                    StrikeOut = item.StrikeOut,
                    Average = item.Average.ToString("N3", CultureInfo.InvariantCulture)
                } into list
                group list by list.GameDate into listByYear
                select new KeyedList<string, PlayersDrillDownViewModel>(listByYear);

            PlayerDrillDownItems.Clear();
            foreach (var viewModelItem in groupedItems)
            {
                PlayerDrillDownItems.Add(viewModelItem);
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
