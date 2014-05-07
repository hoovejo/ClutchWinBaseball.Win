using System;
using System.ComponentModel;

namespace ClutchWinBaseball.Portable.ViewModels
{
    public class PlayersDrillDownViewModel : INotifyPropertyChanged
    {
        private string _gameDate, _gameYear, _atBat, _hit, _secondBase, 
            _thirdBase, _homeRun, _runBattedIn, _strikeOut, _baseBall, _average;

        public string GameDate
        {
            get { return _gameDate; }
            set { if (value != _gameDate) { _gameDate = value; NotifyPropertyChanged("GameDate"); } }
        }

        public string GameYear 
        { 
            get { return _gameYear; } 
            set { if (value != _gameYear) { _gameYear = value; NotifyPropertyChanged("GameYear"); } } 
        }

        public string AtBat 
        { 
            get { return _atBat; }
            set { if (value != _atBat) { _atBat = value; NotifyPropertyChanged("AtBat"); } } 
        }

        public string Hit 
        { 
            get { return _hit; }
            set { if (value != _hit) { _hit = value; NotifyPropertyChanged("Hit"); } } 
        }

        public string SecondBase 
        { 
            get { return _secondBase; } 
            set { if (value != _secondBase) { _secondBase = value; NotifyPropertyChanged("SecondBase"); } } 
        }

        public string ThirdBase 
        { 
            get { return _thirdBase; } 
            set { if (value != _thirdBase) { _thirdBase = value; NotifyPropertyChanged("ThirdBase"); } } 
        }

        public string HomeRun 
        { 
            get { return _homeRun; }
            set { if (value != _homeRun) { _homeRun = value; NotifyPropertyChanged("HomeRun"); } } 
        }

        public string RunBattedIn 
        { 
            get { return _runBattedIn; }
            set { if (value != _runBattedIn) { _runBattedIn = value; NotifyPropertyChanged("RunBattedIn"); } } 
        }

        public string StrikeOut 
        { 
            get { return _strikeOut; }
            set { if (value != _strikeOut) { _strikeOut = value; NotifyPropertyChanged("StrikeOut"); } } 
        }

        public string BaseBall 
        { 
            get { return _baseBall; }
            set { if (value != _baseBall) { _baseBall = value; NotifyPropertyChanged("BaseBall"); } } 
        }

        public string Average 
        { 
            get { return _average; }
            set { if (value != _average) { _average = value; NotifyPropertyChanged("Average"); } } 
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
