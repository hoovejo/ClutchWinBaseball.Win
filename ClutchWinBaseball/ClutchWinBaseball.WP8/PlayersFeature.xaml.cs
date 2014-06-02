using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using Microsoft.Phone.Controls;
using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ClutchWinBaseball.WP8
{
    public partial class PlayersFeature : PhoneApplicationPage
    {
        private PlayersContextViewModel playersContext;

        public PlayersFeature()
        {
            InitializeComponent();

            playersContext = PlayersContextViewModel.Instance;
        }

        // Load data for the ViewModel Items
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(Config.PlayersFeatureTabCache))
            {
                pvControl.SelectedIndex = (int)IsolatedStorageSettings.ApplicationSettings[Config.PlayersFeatureTabCache];
            }

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.PlayersDataManager.GetBattersAsync(isNetAvailable);

            ServiceInteractionNotify(success, isNetAvailable);

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains(Config.PlayersFeatureTabCache))
            {
                settings.Add(Config.PlayersFeatureTabCache, pvControl.SelectedIndex);
            }
            else
            {
                settings[Config.PlayersFeatureTabCache] = pvControl.SelectedIndex;
            }
            settings.Save();

            base.OnNavigatedFrom(e);
        }

        private void goTo_Years(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PlayersYearsSelector.xaml", UriKind.Relative));
        }

        private void goTo_Teams(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PlayersTeamsSelector.xaml", UriKind.Relative));
        }

        private async void BatterStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModelLocator.Players.IsLoadingData) return;

            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((PlayersBattersViewModel)textBlock.DataContext).BatterId;
            }

            playersContext.SelectedBatterId = selectedItem;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.PlayersDataManager.GetPitchersAsync(isNetAvailable);

            pvControl.SelectedIndex = 1;
            if (piPitchers != null && piPitchers.ItemsSource.Count > 0)
            {
                piPitchers.ScrollTo(ViewModelLocator.Players.PitcherItems.First(g => g.Any()));
            }

            ServiceInteractionNotify(success, isNetAvailable);
        }

        private async void PitcherStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModelLocator.Players.IsLoadingData) return;

            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((PlayersPitchersViewModel)textBlock.DataContext).PitcherId;
            }

            playersContext.SelectedPitcherId = selectedItem;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.PlayersDataManager.GetPlayersResultsAsync(isNetAvailable);

            pvControl.SelectedIndex = 2;
            if (piPlayerResults != null && piPlayerResults.ItemsSource.Count > 0)
            {
                piPlayerResults.ScrollTo(ViewModelLocator.Players.PlayerResultItems.First(g => g.Any()));
            }

            ServiceInteractionNotify(success, isNetAvailable);
        }

        private async void ResultsStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModelLocator.Players.IsLoadingData) return;

            var selectedYear = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedYear = ((PlayersResultsViewModel)textBlock.DataContext).GameYear;
            }

            playersContext.SelectedGameYear = selectedYear;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.PlayersDataManager.GetPlayersDrillDownAsync(isNetAvailable);

            pvControl.SelectedIndex = 3;
            if (piPlayerDrillDown != null && piPlayerDrillDown.ItemsSource.Count > 0)
            {
                piPlayerDrillDown.ScrollTo(ViewModelLocator.Players.PlayerDrillDownItems.First(g => g.Any()));
            }

            ServiceInteractionNotify(success, isNetAvailable);
        }

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
                            success = await DataManagerLocator.PlayersDataManager.GetPitchersAsync(isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
                case 2:
                    {
                        if (ViewModelLocator.Players.PlayerResultItems.Count <= 0)
                        {
                            success = await DataManagerLocator.PlayersDataManager.GetPlayersResultsAsync(isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
                case 3:
                    {
                        if (ViewModelLocator.Players.PlayerDrillDownItems.Count <= 0)
                        {
                            success = await DataManagerLocator.PlayersDataManager.GetPlayersDrillDownAsync(isNetAvailable);
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

        private void showNotification(string msg)
        {
            notifyMsg.Text = msg;
            if (!notify.IsOpen)
            {
                notify.IsOpen = true;
            }
        }

        private void btn_continue_Click(object sender, RoutedEventArgs e)
        {
            if (notify.IsOpen)
            {
                notify.IsOpen = false;
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {

        }
    }
}