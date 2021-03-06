﻿using ClutchWinBaseball.Common;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamsFeature : Page
    {
        private readonly NavigationHelper navigationHelper;
        private TeamsContextViewModel teamsContext;
        private bool handledByUserAction;

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
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var tab = localSettings.Values[Config.TeamsFeatureTabCache];
            if (tab != null)
            {
                pvControl.SelectedIndex = (int)tab;
            }

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.GetFranchisesAsync(isNetAvailable);

            (teamsSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsFranchiseItems.View.CollectionGroups;
            (opponentsSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsOpponentsItems.View.CollectionGroups;
            (teamResultsSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsTeamResultItems.View.CollectionGroups;
            (teamDrillDownSemanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsTeamDrillDownItems.View.CollectionGroups;

            ServiceInteractionNotify(success, isNetAvailable);
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
        private void Teams_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ViewModelLocator.Teams.IsLoadingData) return;

            teamsContext.SelectedTeamId = ((TeamsFranchisesViewModel)e.ClickedItem).TeamId;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = DataManagerLocator.TeamsDataManager.GetOpponents(isNetAvailable);

            handledByUserAction = true;
            pvControl.SelectedIndex = 1;

            ServiceInteractionNotify(success, isNetAvailable);
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private async void Opponents_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ViewModelLocator.Teams.IsLoadingData) return;

            teamsContext.SelectedOpponentId = ((TeamsOpponentsViewModel)e.ClickedItem).TeamId;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.GetTeamsResultsAsync(isNetAvailable);

            handledByUserAction = true;
            pvControl.SelectedIndex = 2;

            ServiceInteractionNotify(success, isNetAvailable);
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private async void TeamResults_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ViewModelLocator.Teams.IsLoadingData) return;

            teamsContext.SelectedYearId = ((TeamsResultsViewModel)e.ClickedItem).Year;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.GetTeamsDrillDownAsync(isNetAvailable);

            handledByUserAction = true;
            pvControl.SelectedIndex = 3;

            ServiceInteractionNotify(success, isNetAvailable);
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void pvControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (handledByUserAction) { handledByUserAction = false; return; }

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
                        success = DataManagerLocator.TeamsDataManager.GetOpponents(isNetAvailable);
                        neededRefresh = true;
                    }
                    break;
                case 2:
                    {
                        success = await DataManagerLocator.TeamsDataManager.GetTeamsResultsAsync(isNetAvailable);
                        neededRefresh = true;
                    }
                    break;
                case 3:
                    {
                        success = await DataManagerLocator.TeamsDataManager.GetTeamsDrillDownAsync(isNetAvailable);
                        neededRefresh = true;
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

        private void ServiceInteractionNotify(bool success, bool isNetAvailable)
        {
            if (!success && !isNetAvailable)
            {
                showNotification(Config.NetworkNotAvailable);
            }
            else if (!success)
            {
                showNotification(Config.Error);
            }
        }

        private async void showNotification(string msg)
        {
            var dialog = new MessageDialog(msg);
            await dialog.ShowAsync();
        }
    }
}
