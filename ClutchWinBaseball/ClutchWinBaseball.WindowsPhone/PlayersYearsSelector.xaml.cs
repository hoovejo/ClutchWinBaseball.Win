using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayersYearsSelector : Page
    {
        public PlayersYearsSelector()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Shows the details of a clicked group in the <see cref="SectionPage"/>.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Details about the click event.</param>
        private void Years_ItemClick(object sender, ItemClickEventArgs e)
        {
            PlayersContextViewModel playersContext = PlayersContextViewModel.Instance;
            var yearId = ((PlayersYearsViewModel)e.ClickedItem).LineOne;
            ViewModelLocator.Players.SelectedYearId = yearId;
            playersContext.SelectedYearId = yearId;

            Frame.Navigate(typeof(PlayersFeature));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.PlayersDataManager.LoadPlayersDataAsync(PlayersEndpoints.Seasons, isNetAvailable);

            if (!success && !isNetAvailable)
            {
                showNotification(Config.NetworkNotAvailable);
            }
            else if (!success)
            {
                showNotification(Config.Error);
            }
        }

        private void showNotification(string msg)
        {
            notifyMsg.Text = msg;
            if (!notify.IsOpen)
            {
                notify.IsOpen = true;
            }
        }

        private void btn_continue_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (notify.IsOpen)
            {
                notify.IsOpen = false;
            }
        }
    }
}
