using ClutchWinBaseball.Portable.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ClutchWinBaseball.ItemViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamsDrillDownItem : UserControl
    {
        public TeamsDrillDownItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// This method visualizes the placeholder state of the data item. When 
        /// showing a placehlder, we set the opacity of other elements to zero
        /// so that stale data is not visible to the end user.  Note that we use
        /// Grid's background color as the placeholder background.
        /// </summary>
        /// <param name="item"></param>
        public void ShowPlaceholder(TeamsDrillDownViewModel item)
        {
            _item = item;

            teamLabel.Opacity = 0;
            oppoLabel.Opacity = 0;
            winsLabel.Opacity = 0;
            lossLabel.Opacity = 0;
            runsfLabel.Opacity = 0;
            runsaLabel.Opacity = 0;

            teamValue.Opacity = 0;
            oppoValue.Opacity = 0;
            winsValue.Opacity = 0;
            lossValue.Opacity = 0;
            runsfValue.Opacity = 0;
            runsaValue.Opacity = 0;
        }

        /// <summary>
        /// Visualize the labels by setting Opacity to 1.
        /// </summary>
        public void ShowLabels()
        {
            teamLabel.Opacity = 1;
            oppoLabel.Opacity = 1;
            winsLabel.Opacity = 1;
            lossLabel.Opacity = 1;
            runsfLabel.Opacity = 1;
            runsaLabel.Opacity = 1;
        }

        /// <summary>
        /// Visualize category information by updating the correct TextBlock and 
        /// setting Opacity to 1.
        /// </summary>
        public void ShowValues()
        {
            teamValue.Text = _item.Team;
            oppoValue.Text = _item.Opponent;
            winsValue.Text = _item.Win;
            lossValue.Text = _item.Loss;
            runsfValue.Text = _item.RunsFor;
            runsaValue.Text = _item.RunsAgainst;

            teamValue.Opacity = 1;
            oppoValue.Opacity = 1;
            winsValue.Opacity = 1;
            lossValue.Opacity = 1;
            runsfValue.Opacity = 1;
            runsaValue.Opacity = 1;
        }

        /// <summary>
        /// Drop all refrences to the data item
        /// </summary>
        public void ClearData()
        {
            _item = null;

            teamValue.ClearValue(TextBlock.TextProperty);
            oppoValue.ClearValue(TextBlock.TextProperty);
            winsValue.ClearValue(TextBlock.TextProperty);
            lossValue.ClearValue(TextBlock.TextProperty);
            runsfValue.ClearValue(TextBlock.TextProperty);
            runsaValue.ClearValue(TextBlock.TextProperty);
        }

        private TeamsDrillDownViewModel _item;
    }
}
