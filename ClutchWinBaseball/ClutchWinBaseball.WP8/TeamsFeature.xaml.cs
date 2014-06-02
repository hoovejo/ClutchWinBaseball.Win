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
    public partial class TeamsFeature : PhoneApplicationPage
    {
        private TeamsContextViewModel teamsContext;

        public TeamsFeature()
        {
            InitializeComponent();

            teamsContext = TeamsContextViewModel.Instance;
        }

        // Load data for the ViewModel Items
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(Config.TeamsFeatureTabCache))
            {
                pvControl.SelectedIndex = (int)IsolatedStorageSettings.ApplicationSettings[Config.TeamsFeatureTabCache];
            }

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

            success = await DataManagerLocator.TeamsDataManager.GetFranchisesAsync(isNetAvailable);

            ServiceInteractionNotify(success, isNetAvailable);

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains(Config.TeamsFeatureTabCache))
            {
                settings.Add(Config.TeamsFeatureTabCache, pvControl.SelectedIndex);
            }
            else
            {
                settings[Config.TeamsFeatureTabCache] = pvControl.SelectedIndex;
            }
            settings.Save();

            base.OnNavigatedFrom(e);
        }

        private async void FranchisesPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModelLocator.Teams.IsLoadingData) return;

            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((TeamsFranchisesViewModel)textBlock.DataContext).TeamId;
            }

            teamsContext.SelectedTeamId = selectedItem;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.GetOpponentsAsync(isNetAvailable);

            pvControl.SelectedIndex = 1;
            if (piOpponents != null && piOpponents.ItemsSource.Count > 0)
            {
                piOpponents.ScrollTo(ViewModelLocator.Teams.OpponentsItems.First(g => g.Any()));
            }

            ServiceInteractionNotify(success, isNetAvailable);
        }

        private async void OpponentsPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModelLocator.Teams.IsLoadingData) return;

            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((TeamsOpponentsViewModel)textBlock.DataContext).TeamId;
            }

            teamsContext.SelectedOpponentId = selectedItem;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.GetTeamsResultsAsync(isNetAvailable);

            pvControl.SelectedIndex = 2;
            if (piTeamResults != null && piTeamResults.ItemsSource.Count > 0)
            {
                piTeamResults.ScrollTo(ViewModelLocator.Teams.TeamResultItems.First(g => g.Any()));
            }

            ServiceInteractionNotify(success, isNetAvailable);
        }

        private async void TeamResultsPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModelLocator.Teams.IsLoadingData) return;

            var selectedItem = string.Empty;

            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((TeamsResultsViewModel)textBlock.DataContext).Year;
            }

            teamsContext.SelectedYearId = selectedItem;

            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.TeamsDataManager.GetTeamsDrillDownAsync(isNetAvailable);

            pvControl.SelectedIndex = 3;
            if (piTeamDrillDown != null && piTeamDrillDown.ItemsSource.Count > 0)
            {
                piTeamDrillDown.ScrollTo(ViewModelLocator.Teams.TeamDrillDownItems.First(g => g.Any()));
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
                        if (ViewModelLocator.Teams.OpponentsItems.Count <= 0)
                        {
                            success = await DataManagerLocator.TeamsDataManager.GetOpponentsAsync(isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
                case 2:
                    {
                        if (ViewModelLocator.Teams.TeamResultItems.Count <= 0)
                        {
                            success = await DataManagerLocator.TeamsDataManager.GetTeamsResultsAsync(isNetAvailable);
                            neededRefresh = true;
                        }
                    }
                    break;
                case 3:
                    {
                        if (ViewModelLocator.Teams.TeamDrillDownItems.Count <= 0)
                        {
                            success = await DataManagerLocator.TeamsDataManager.GetTeamsDrillDownAsync(isNetAvailable);
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