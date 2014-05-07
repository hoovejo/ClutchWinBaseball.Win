﻿using System;
using System.ComponentModel;

namespace ClutchWinBaseball.Portable.ViewModels
{
    public class PlayersTeamsViewModel : INotifyPropertyChanged
    {
        private string _firstLetter, _lineOne, _lineTwo, _teamId;

        public string FirstLetter
        {
            get { return _firstLetter; }
            set { if (value != _firstLetter) { _firstLetter = value; NotifyPropertyChanged("FirstLetter"); } }
        }

        public string LineOne
        {
            get { return _lineOne; }
            set { if (value != _lineOne) { _lineOne = value; NotifyPropertyChanged("LineOne"); } }
        }

        public string LineTwo
        { 
            get { return _lineTwo; }
            set { if (value != _lineTwo) { _lineTwo = value; NotifyPropertyChanged("LineTwo"); } }
        }

        public string TeamId
        {
            get { return _teamId; }
            set { if (value != _teamId) { _teamId = value; NotifyPropertyChanged("TeamId"); } }
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
