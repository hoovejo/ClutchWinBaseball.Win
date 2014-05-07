using System;
using System.ComponentModel;

namespace ClutchWinBaseball.Portable.ViewModels
{
    public class PlayersBattersViewModel : INotifyPropertyChanged
    {
        private string _firstLetter, _lineOne, _lineTwo, _batterId;

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

        public string BatterId
        {
            get { return _batterId; }
            set { if (value != _batterId) { _batterId = value; NotifyPropertyChanged("BatterId"); } }
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
