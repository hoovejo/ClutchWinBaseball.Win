using System;
using System.ComponentModel;

namespace ClutchWinBaseball.Portable.ViewModels
{
    public class TeamsOpponentsViewModel : INotifyPropertyChanged
    {
        private string _firstLetter, _lineOne, _lineTwo, _teamId, _location;

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

        public string Location
        {
            get { return _location; }
            set { if (value != _location) { _location = value; NotifyPropertyChanged("Location"); } }
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
