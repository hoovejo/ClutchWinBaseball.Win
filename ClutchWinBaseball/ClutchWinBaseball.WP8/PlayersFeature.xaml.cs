using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.ViewModels;
using Microsoft.Phone.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ClutchWinBaseball.WP8
{
    public partial class PlayersFeature : PhoneApplicationPage
    {
        public PlayersFeature()
        {
            InitializeComponent();
        }

        // Load data for the ViewModel Items
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!ViewModelLocator.Players.IsYearDataLoaded)
            {
                await ViewModelLocator.Players.LoadYearDataAsync();
            }
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
            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((PlayersBattersViewModel)textBlock.DataContext).BatterId;
                ViewModelLocator.Players.SelectedBatterId = selectedItem;
                await ViewModelLocator.Players.LoadPitcherDataAsync();
                pvControl.SelectedIndex = 1;
                if (piPitchers != null && piPitchers.ItemsSource.Count > 0)
                {
                    piPitchers.ScrollTo(ViewModelLocator.Players.PitcherItems.First(g => g.Any()));
                }
            }
        }

        private async void PitcherStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((PlayersPitchersViewModel)textBlock.DataContext).PitcherId;
                ViewModelLocator.Players.SelectedPitcherId = selectedItem;
                await ViewModelLocator.Players.LoadPlayerResultsDataAsync();
                pvControl.SelectedIndex = 2;
                if (piPlayerResults != null && piPlayerResults.ItemsSource.Count > 0)
                {
                    piPlayerResults.ScrollTo(ViewModelLocator.Players.PlayerResultItems.First(g => g.Any()));
                }
            }
        }

        private async void ResultsStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = string.Empty;
            var selectedYear = string.Empty;

            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((PlayersResultsViewModel)textBlock.DataContext).GameType;
                selectedYear = ((PlayersResultsViewModel)textBlock.DataContext).GameYear;
                ViewModelLocator.Players.SelectedGameType = selectedItem;
                ViewModelLocator.Players.SelectedGameYear = selectedYear;
                await ViewModelLocator.Players.LoadPlayerDrillDownDataAsync();

                pvControl.SelectedIndex = 3;
                if (piPlayerDrillDown != null && piPlayerDrillDown.ItemsSource.Count > 0)
                {
                    piPlayerDrillDown.ScrollTo(ViewModelLocator.Players.PlayerDrillDownItems.First(g => g.Any()));
                }
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {

        }
    }
}