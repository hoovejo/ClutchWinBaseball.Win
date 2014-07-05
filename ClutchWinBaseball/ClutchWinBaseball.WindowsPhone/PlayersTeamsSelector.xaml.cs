using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayersTeamsSelector : Page
    {
        public PlayersTeamsSelector()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private void Teams_ItemClick(object sender, ItemClickEventArgs e)
        {
            PlayersContextViewModel playersContext = PlayersContextViewModel.Instance;

            var teamId = ((PlayersTeamsViewModel)e.ClickedItem).TeamId;
            ViewModelLocator.Players.SelectedTeamId = teamId;
            playersContext.SelectedTeamId = teamId;

            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(PlayersFeature));
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.PlayersDataManager.GetTeamsAsync(isNetAvailable);

            if (!success && !isNetAvailable)
            {
                showNotification(Config.NetworkNotAvailable);
            }
            else if (!success)
            {
                showNotification(Config.Error);
            }

            // sets the items source for the zoomed out view to the group data as well
            (teamsSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsTeamItems.View.CollectionGroups;

            base.OnNavigatedTo(e);
        }

        private async void showNotification(string msg)
        {
            var dialog = new MessageDialog(msg);
            await dialog.ShowAsync();
        }

    }
}
