﻿using ClutchWinBaseball.Common;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball
{
    public sealed partial class PlayersFeature : Page
    {
        private NavigationHelper navigationHelper;

        public PlayersFeature()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
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
            ViewModelLocator.Players.SelectedBatterId = ((PlayersBattersViewModel)e.ClickedItem).BatterId;
            //TODO: check if the tapped value is the same as previous
            await ViewModelLocator.Players.LoadPitcherData();
            pvControl.SelectedIndex = 1;
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private async void Pitchers_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModelLocator.Players.SelectedPitcherId = ((PlayersPitchersViewModel)e.ClickedItem).PitcherId;
            //TODO: check if the tapped value is the same as previous
            await ViewModelLocator.Players.LoadPlayerResultsData();
            pvControl.SelectedIndex = 2;
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private async void PlayerResults_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModelLocator.Players.SelectedGameType = ((PlayersResultsViewModel)e.ClickedItem).GameType;
            ViewModelLocator.Players.SelectedGameYear = ((PlayersResultsViewModel)e.ClickedItem).GameYear;
            //TODO: check if the tapped value is the same as previous
            await ViewModelLocator.Players.LoadPlayerDrillDownData();
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

            if (!ViewModelLocator.Players.IsYearDataLoaded)
            {
                await ViewModelLocator.Players.LoadYearData();
            }

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
        
    }
}
