using System;
using System.ComponentModel;

namespace ClutchWinBaseball.Portable.ViewModels
{
    public class TeamsDrillDownViewModel : INotifyPropertyChanged
    {
        private string _gameDate, _teams, _opponent, _win, _loss, _runsFor, _runsAgainst;

        public string GameDate
        {
            get { return _gameDate; }
            set { if (value != _gameDate) { _gameDate = value; NotifyPropertyChanged("Games"); } }
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

        public string Win
        {
            get { return _win; }
            set { if (value != _win) { _win = value; NotifyPropertyChanged("Wins"); } }
        }

        public string Loss
        {
            get { return _loss; }
            set { if (value != _loss) { _loss = value; NotifyPropertyChanged("Losses"); } }
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
