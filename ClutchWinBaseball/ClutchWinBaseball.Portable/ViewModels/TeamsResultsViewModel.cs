using System;
using System.ComponentModel;

namespace ClutchWinBaseball.Portable.ViewModels
{
    public class TeamsResultsViewModel : INotifyPropertyChanged
    {
        private string _year, _games, _teams, _opponent, _wins, _losses, _runsFor, _runsAgainst;

        public string Year
        {
            get { return _year; }
            set { if (value != _year) { _year = value; NotifyPropertyChanged("Year"); } }
        }

        public string Games
        {
            get { return _games; }
            set { if (value != _games) { _games = value; NotifyPropertyChanged("Games"); } }
        }

        public string Team 
        {
            get { return _teams; }
            set { if (value != _teams) { _teams = value; NotifyPropertyChanged("Team"); } } 
        }

        public string Opponent 
        {
            get { return _opponent; }
            set { if (value != _opponent) { _opponent = value; NotifyPropertyChanged("Opponent"); } } 
        }

        public string Wins 
        { 
            get { return _wins; }
            set { if (value != _wins) { _wins = value; NotifyPropertyChanged("Wins"); } } 
        }

        public string Losses
        {
            get { return _losses; }
            set { if (value != _losses) { _losses = value; NotifyPropertyChanged("Losses"); } } 
        }

        public string RunsFor 
        { 
            get { return _runsFor; }
            set { if (value != _runsFor) { _runsFor = value; NotifyPropertyChanged("RunsFor"); } } 
        }

        public string RunsAgainst 
        {
            get { return _runsAgainst; }
            set { if (value != _runsAgainst) { _runsAgainst = value; NotifyPropertyChanged("RunsAgainst"); } } 
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
