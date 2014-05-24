﻿using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.DataModel;
using ClutchWinBaseball.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

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

        private string _selectedTeamId, _selectedOpponentId, _selectedYearId;
        private bool _isDataLoaded;

        public string SelectedTeamId
        {
            get { return _selectedTeamId; }
            set { if (value != _selectedTeamId) { _selectedTeamId = value; NotifyPropertyChanged("SelectedTeamId"); } }
        }

        public string SelectedOpponentId
        {
            get { return _selectedOpponentId; }
            set { if (value != _selectedOpponentId) { _selectedOpponentId = value; NotifyPropertyChanged("SelectedOpponentId"); } }
        }

        public string SelectedYearId
        {
            get { return _selectedYearId; }
            set { if (value != _selectedYearId) { _selectedYearId = value; NotifyPropertyChanged("SelectedYearId"); } }
        }

        //Determine if we need to show loading spinner
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            private set { if (value != _isDataLoaded) { _isDataLoaded = value; NotifyPropertyChanged("IsDataLoaded"); } }
        }

        //Determine if Teams feature is ready
        public bool IsFranchiseDataLoaded { get; private set; }

        //base list to build opponent list from
        private List<TeamsFranchisesViewModel> FranchiseList = new List<TeamsFranchisesViewModel>();

        public string FranchisesDataString { get; set; }
        public string TeamsResultsDataString { get; set; }
        public string TeamsDrillDownDataString { get; set; }

        public async Task<bool> LoadFranchisesDataAsync(string cachedJson = "")
        {
            List<FranchiseModel> franchises;
            IsDataLoaded = false;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetFranchisesAsync();
                if (svcResult == null) { return false; } //no results message
                FranchisesDataString = svcResult;
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

            IsDataLoaded = true;
            return true;
        }

        public void LoadOpponentsData(string cachedJson = "")
        {
            IsDataLoaded = false;

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
                where team.TeamId != SelectedTeamId
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
            IsDataLoaded = true;
        }

        public async Task<bool> LoadTeamResultsDataAsync(string cachedJson = "")
        {
            List<TeamsResultModel> items;
            IsDataLoaded = false;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetTeamResultsAsync(SelectedTeamId, SelectedOpponentId);
                if (svcResult == null) { return false; } //no results message
                TeamsResultsDataString = svcResult;
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
            IsDataLoaded = true;
            return true;
        }

        public async Task<bool> LoadTeamDrillDownDataAsync(string cachedJson = "")
        {
            List<TeamsDrillDownModel> items;
            IsDataLoaded = false;

            if (string.IsNullOrEmpty(cachedJson))
            {
                var dataContext = new DataContext();
                var svcResult = string.Empty;

                svcResult = await dataContext.GetTeamDrillDownAsync(SelectedTeamId, SelectedOpponentId, SelectedYearId);
                if (svcResult == null) { return false; } //no results message
                TeamsDrillDownDataString = svcResult;
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
