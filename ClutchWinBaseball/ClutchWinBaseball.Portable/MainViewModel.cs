using ClutchWinBaseball.Portable.DataModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ClutchWinBaseball.Portable
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<FeatureViewModel>();
        }

        /// <summary>
        /// A collection for FeatureViewModel objects.
        /// </summary>
        public ObservableCollection<FeatureViewModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few FeatureViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new FeatureViewModel() { Id = 0, LineOne = "Team vs Team", LineTwo = "pick your fav team and opponent"});
            this.Items.Add(new FeatureViewModel() { Id = 1, LineOne = "Batter vs Pitcher", LineTwo = "pick your fav batter vs pitcher"});

            this.IsDataLoaded = true;
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
