using ClutchWinBaseball.Portable.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ClutchWinBaseball.ItemViews
{
    public sealed partial class TeamsResultsItem : UserControl
    {
        public TeamsResultsItem()
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
        public void ShowPlaceholder(TeamsResultsViewModel item)
        {
            _item = item;

            gamesLabel.Opacity = 0;
            teamLabel.Opacity = 0;
            oppoLabel.Opacity = 0;
            winsLabel.Opacity = 0;
            lossLabel.Opacity = 0;
            runsfLabel.Opacity = 0;
            runsaLabel.Opacity = 0;

            gamesValue.Opacity = 0;
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
            gamesLabel.Opacity = 1;
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
            gamesValue.Text = _item.Games;
            teamValue.Text = _item.Team;
            oppoValue.Text = _item.Opponent;
            winsValue.Text = _item.Wins;
            lossValue.Text = _item.Losses;
            runsfValue.Text = _item.RunsFor;
            runsaValue.Text = _item.RunsAgainst;

            gamesValue.Opacity = 1;
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

            gamesValue.ClearValue(TextBlock.TextProperty);
            teamValue.ClearValue(TextBlock.TextProperty);
            oppoValue.ClearValue(TextBlock.TextProperty);
            winsValue.ClearValue(TextBlock.TextProperty);
            lossValue.ClearValue(TextBlock.TextProperty);
            runsfValue.ClearValue(TextBlock.TextProperty);
            runsaValue.ClearValue(TextBlock.TextProperty);
        }

        private TeamsResultsViewModel _item;
    }
}
