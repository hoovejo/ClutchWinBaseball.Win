using ClutchWinBaseball.Common;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball
{
    public sealed partial class PlayersFeature : Page
    {
        private NavigationHelper navigationHelper;
        private PlayersDataManager dataLoadManager;
        private ContextCacheManager cacheManager;
        private PlayersContextViewModel playersContext;

        public PlayersFeature()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            playersContext = PlayersContextViewModel.Instance;

            var tempFolder = ApplicationData.Current.TemporaryFolder;
            var fileManager = new CacheFileManager(tempFolder);

            cacheManager = new ContextCacheManager(fileManager);
            dataLoadManager = new PlayersDataManager(playersContext, ViewModelLocator.Players, fileManager, cacheManager);
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var tab = localSettings.Values[Config.PlayersFeatureTabCache];
            if (tab != null)
            {
                pvControl.SelectedIndex = (int)tab;
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[Config.PlayersFeatureTabCache] = pvControl.SelectedIndex;
        }

        private void goTo_Years(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PlayersYearsSelector));
        }

        private void goTo_Teams(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PlayersTeamsSelector));
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private async void Batters_ItemClick(object sender, ItemClickEventArgs e)
        {
            playersContext.SelectedBatterId = ((PlayersBattersViewModel)e.ClickedItem).BatterId;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await dataLoadManager.LoadPlayersDataAsync(PlayersEndpoints.Pitchers, isNetAvailable);

            pvControl.SelectedIndex = 1;
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private async void Pitchers_ItemClick(object sender, ItemClickEventArgs e)
        {
            playersContext.SelectedPitcherId = ((PlayersPitchersViewModel)e.ClickedItem).PitcherId;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await dataLoadManager.LoadPlayersDataAsync(PlayersEndpoints.PlayerSearch, isNetAvailable);

            pvControl.SelectedIndex = 2;
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private async void PlayerResults_ItemClick(object sender, ItemClickEventArgs e)
        {
            playersContext.SelectedGameYear = ((PlayersResultsViewModel)e.ClickedItem).GameYear;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await dataLoadManager.LoadPlayersDataAsync(PlayersEndpoints.PlayerYearSearch, isNetAvailable);

            pvControl.SelectedIndex = 3;
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            if (!playersContext.IsHydratedObject)
            {
                PlayersContextViewModel ctx = await cacheManager.ReadPlayersContextAsync();
                if (ctx != null)
                {
                    playersContext.ReHydrateMe(ctx);
                }
                playersContext.IsHydratedObject = true;

                ViewModelLocator.Players.SelectedYearId = playersContext.SelectedYearId;
                ViewModelLocator.Players.SelectedTeamId = playersContext.SelectedTeamId;
            }

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await dataLoadManager.LoadPlayersDataAsync(PlayersEndpoints.Batters, isNetAvailable);

            (battersSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsBatterItems.View.CollectionGroups;
            (pitcherSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsPitcherItems.View.CollectionGroups;
            (playerResultsSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsPlayerResults.View.CollectionGroups;
            (playerDrillDownSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsPlayerDrillDownItems.View.CollectionGroups;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void pvControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;
            bool neededRefresh = false;

            switch (pvControl.SelectedIndex)
            {
                case 0:
                    // not req'd handled by OnNavigatedTo
                    break;
                case 1:
                    {
                        if (ViewModelLocator.Players.PitcherItems.Count <= 0)
                        {
                            success = await dataLoadManager.LoadPlayersDataAsync(PlayersEndpoints.Pitchers, isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
                case 2:
                    {
                        if (ViewModelLocator.Players.PlayerResultItems.Count <= 0)
                        {
                            success = await dataLoadManager.LoadPlayersDataAsync(PlayersEndpoints.PlayerSearch, isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
                case 3:
                    {
                        if (ViewModelLocator.Players.PlayerDrillDownItems.Count <= 0)
                        {
                            success = await dataLoadManager.LoadPlayersDataAsync(PlayersEndpoints.PlayerYearSearch, isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
            }

            if (neededRefresh && !success)
            {
                // pop message
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
