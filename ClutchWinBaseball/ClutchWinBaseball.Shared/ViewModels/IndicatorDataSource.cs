using System;
using System.Collections.ObjectModel;

namespace ClutchWinBaseball
{
    /// <summary>
    /// Base class for <see cref="PlaceHolderDataItem"/> 
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class PlaceHolderDataCommon : ClutchWinBaseball.Common.BindableBase
    {
        public PlaceHolderDataCommon(String title)
        {
            this._title = title;
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class PlaceHolderDataItem : PlaceHolderDataCommon
    {
        public PlaceHolderDataItem(String title)
            : base(title)
        {
        }

    }

    public sealed class TeamsFeatureIndicatorDataSource
    {
        private ObservableCollection<object> _items = new ObservableCollection<object>();
        public ObservableCollection<object> Items
        {
            get { return this._items; }
        }

        public TeamsFeatureIndicatorDataSource()
        {
            Items.Add(new PlaceHolderDataItem("Franchises"));
            Items.Add(new PlaceHolderDataItem("Opponents"));
            Items.Add(new PlaceHolderDataItem("Results"));
            Items.Add(new PlaceHolderDataItem("DrillDown"));
        }
    }

    public sealed class PlayersFeatureIndicatorDataSource
    {
        private ObservableCollection<object> _items = new ObservableCollection<object>();
        public ObservableCollection<object> Items
        {
            get { return this._items; }
        }

        public PlayersFeatureIndicatorDataSource()
        {
            Items.Add(new PlaceHolderDataItem("Batters"));
            Items.Add(new PlaceHolderDataItem("Pitchers"));
            Items.Add(new PlaceHolderDataItem("Results"));
            Items.Add(new PlaceHolderDataItem("DrillDown"));
        }
    }
}
