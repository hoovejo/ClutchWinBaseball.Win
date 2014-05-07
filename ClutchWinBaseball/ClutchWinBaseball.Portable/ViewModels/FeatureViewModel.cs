using System;
using System.ComponentModel;

namespace ClutchWinBaseball.Portable.DataModel
{
    public class FeatureViewModel : INotifyPropertyChanged
    {
        private string _lineOne, _lineTwo;
        private int _id;

        public int Id
        {
            get { return _id; }
            set { if (value != _id) { _id = value; NotifyPropertyChanged("Id"); } }
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
