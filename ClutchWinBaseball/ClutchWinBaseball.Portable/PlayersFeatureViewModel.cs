using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.DataModel;
using ClutchWinBaseball.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;

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

        private string _selectedYearId = "select";
        private string _selectedTeamId = "select";
        private string _selectedBatterId, _selectedPitcherId, _selectedGameType, _selectedGameYear;
        private bool _isDataLoaded;

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
        public string SelectedBatterId
        {
            get { return _selectedBatterId; }
            set { if (value != _selectedBatterId) { _selectedBatterId = value; NotifyPropertyChanged("SelectedBatterId"); } }
        }
        public string SelectedPitcherId
        {
            get { return _selectedPitcherId; }
            set { if (value != _selectedPitcherId) { _selectedPitcherId = value; NotifyPropertyChanged("SelectedPitcherId"); } }
        }
        public string SelectedGameType
        {
            get { return _selectedGameType; }
            set { if (value != _selectedGameType) { _selectedGameType = value; NotifyPropertyChanged("SelectedGameType"); } }
        }
        public string SelectedGameYear
        {
            get { return _selectedGameYear; }
            set { if (value != _selectedGameYear) { _selectedGameYear = value; NotifyPropertyChanged("SelectedGameYear"); } }
        }

        //Determine if we need to show loading spinner
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            private set { if (value != _isDataLoaded) { _isDataLoaded = value; NotifyPropertyChanged("IsDataLoaded"); } }
        }

        //Determine if Players feature is ready
        public bool IsYearDataLoaded { get; private set; }

        public async Task<bool> LoadYearData()
        {
            YearItems.Clear();

            var dataContext = new DataContext();
            var years = await dataContext.GetYearsAsync();

            foreach (var year in years)
            {
                this.YearItems.Add(new PlayersYearsViewModel() { LineOne = year.Id.ToString(CultureInfo.CurrentCulture) });
            }
            IsYearDataLoaded = true;
            IsDataLoaded = true;
            return true;
        }

        public async Task<bool> LoadTeamData()
        {
            IsDataLoaded = false;
            TeamItems.Clear();

            var dataContext = new DataContext();
            List<TeamModel> teams = await dataContext.GetTeamsAsync(SelectedYearId);

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

            foreach (var viewModelItem in groupedItems)
            {
                TeamItems.Add(viewModelItem);
            }
            IsDataLoaded = true;
            return true;
        }

        public async Task<bool> LoadBatterData()
        {
            IsDataLoaded = false;
            BatterItems.Clear();

            var dataContext = new DataContext();
            List<BatterModel> batters = await dataContext.GetBattersAsync(SelectedTeamId, SelectedYearId);

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

            foreach (var viewModelItem in groupedItems)
            {
                BatterItems.Add(viewModelItem);
            }
            IsDataLoaded = true;
            return true;
        }

        public async Task<bool> LoadPitcherData()
        {
            IsDataLoaded = false;
            PitcherItems.Clear();

            var dataContext = new DataContext();
            var pitchers = await dataContext.GetPitchersAsync(SelectedBatterId, SelectedYearId);

            var groupedItems =
                from pitcher in pitchers.Rows
                orderby pitcher.FirstName
                select new PlayersPitchersViewModel
                {
                    FirstLetter = pitcher.FirstName.Substring(0, 1),
                    LineOne = pitcher.GetDisplayName(),
                    PitcherId = pitcher.RetroId
                } into list
                group list by list.FirstLetter into listByYear
                select new KeyedList<string, PlayersPitchersViewModel>(listByYear);

            foreach (var viewModelItem in groupedItems)
            {
                PitcherItems.Add(viewModelItem);
            }
            IsDataLoaded = true;
            return true;
        }

        public async Task<bool> LoadPlayerResultsData()
        {
            IsDataLoaded = false;
            PlayerResultItems.Clear();

            var dataContext = new DataContext();
            var items = await dataContext.GetPlayerResultsAsync(SelectedBatterId, SelectedPitcherId);

            var groupedItems =
                from item in items.Rows
                orderby item.Year descending
                select new PlayersResultsViewModel
                {
                    GameYear = item.Year,
                    GameType = item.Type,
                    Games = item.Games,
                    AtBat = item.AtBat,
                    Hit = item.Hit,
                    SecondBase = item.SecondBase,
                    ThirdBase = item.ThirdBase,
                    HomeRun = item.HomeRun,
                    RunBattedIn = item.RunBattedIn,
                    StrikeOut = item.StrikeOut,
                    BaseBall = item.BaseBall,
                    Average = item.Average
                } into list
                group list by list.GameYear into listByYear
                select new KeyedList<string, PlayersResultsViewModel>(listByYear);

            foreach (var viewModelItem in groupedItems)
            {
                PlayerResultItems.Add(viewModelItem);
            }
            IsDataLoaded = true;
            return true;
        }

        public async Task<bool> LoadPlayerDrillDownData()
        {
            IsDataLoaded = false;
            PlayerDrillDownItems.Clear();

            var dataContext = new DataContext();
            var items = await dataContext.GetPlayerDrillDownAsync(SelectedBatterId, SelectedPitcherId, SelectedGameYear, SelectedGameType);

            var groupedItems =
                from item in items.Rows
                orderby item.GameDate descending
                select new PlayersDrillDownViewModel
                {
                    GameDate = item.GameDate,
                    AtBat = item.AtBat,
                    Hit = item.Hit,
                    SecondBase = item.SecondBase,
                    ThirdBase = item.ThirdBase,
                    HomeRun = item.HomeRun,
                    RunBattedIn = item.RunBattedIn,
                    StrikeOut = item.StrikeOut,
                    BaseBall = item.BaseBall,
                    Average = item.Average
                } into list
                group list by list.GameDate into listByYear
                select new KeyedList<string, PlayersDrillDownViewModel>(listByYear);

            foreach (var viewModelItem in groupedItems)
            {
                PlayerDrillDownItems.Add(viewModelItem);
            }
            IsDataLoaded = true;
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
