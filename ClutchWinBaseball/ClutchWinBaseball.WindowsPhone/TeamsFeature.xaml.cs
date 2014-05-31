using ClutchWinBaseball.Common;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamsFeature : Page
    {
        private NavigationHelper navigationHelper;
        private TeamsContextViewModel teamsContext;

        public TeamsFeature()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            teamsContext = TeamsContextViewModel.Instance;
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
            var tab = localSettings.Values[Config.TeamsFeatureTabCache];
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
            localSettings.Values[Config.TeamsFeatureTabCache] = pvControl.SelectedIndex;
        }

        /// <summary>
        /// Shows the details of a clicked group in the <see cref="SectionPage"/>.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Details about the click event.</param>
        private async void Teams_ItemClick(object sender, ItemClickEventArgs e)
        {
            teamsContext.SelectedTeamId = ((TeamsFranchisesViewModel)e.ClickedItem).TeamId;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.LoadTeamsDataAsync(TeamsEndpoints.Opponents, isNetAvailable);

            pvControl.SelectedIndex = 1;
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private async void Opponents_ItemClick(object sender, ItemClickEventArgs e)
        {
            teamsContext.SelectedOpponentId = ((TeamsOpponentsViewModel)e.ClickedItem).TeamId;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.LoadTeamsDataAsync(TeamsEndpoints.FranchiseSearch, isNetAvailable);

            pvControl.SelectedIndex = 2;
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private async void TeamResults_ItemClick(object sender, ItemClickEventArgs e)
        {
            teamsContext.SelectedYearId = ((TeamsResultsViewModel)e.ClickedItem).Year;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.LoadTeamsDataAsync(TeamsEndpoints.FranchiseYearSearch, isNetAvailable);

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

            if (!teamsContext.IsHydratedObject)
            {
                TeamsContextViewModel ctx = await DataManagerLocator.ContextCacheManager.ReadTeamsContextAsync();
                if (ctx != null)
                {
                    teamsContext.ReHydrateMe(ctx);
                }
                teamsContext.IsHydratedObject = true;
            }

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.LoadTeamsDataAsync(TeamsEndpoints.Franchises, isNetAvailable);

            (teamsSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsFranchiseItems.View.CollectionGroups;
            (opponentsSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsOpponentsItems.View.CollectionGroups;
            (teamResultsSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsTeamResultItems.View.CollectionGroups;
            (teamDrillDownSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsTeamDrillDownItems.View.CollectionGroups;

            if (!success && !isNetAvailable)
            {
                showNotification(Config.NetworkNotAvailable);
            }
            else if (!success)
            {
                showNotification(Config.Error);
            }
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
                        if (ViewModelLocator.Teams.OpponentsItems.Count <= 0)
                        {
                            success = await DataManagerLocator.TeamsDataManager.LoadTeamsDataAsync(TeamsEndpoints.Opponents, isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
                case 2:
                    {
                        if (ViewModelLocator.Teams.TeamResultItems.Count <= 0)
                        {
                            success = await DataManagerLocator.TeamsDataManager.LoadTeamsDataAsync(TeamsEndpoints.FranchiseSearch, isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
                case 3:
                    {
                        if (ViewModelLocator.Teams.TeamDrillDownItems.Count <= 0)
                        {
                            success = await DataManagerLocator.TeamsDataManager.LoadTeamsDataAsync(TeamsEndpoints.FranchiseYearSearch, isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
            }

            if (neededRefresh && !success && !isNetAvailable)
            {
                showNotification(Config.NetworkNotAvailable);
            }
            else if (neededRefresh && !success)
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
