using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.ViewModels;
using ClutchWinBaseball.Portable.Common;

namespace ClutchWinBaseball.WP8
{
    public partial class TeamsFeature : PhoneApplicationPage
    {
        public TeamsFeature()
        {
            InitializeComponent();
        }

        // Load data for the ViewModel Items
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!ViewModelLocator.Teams.IsFranchiseDataLoaded)
            {
                await ViewModelLocator.Teams.LoadFranchisesData();
            }
        }

        private void FranchisesPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((TeamsFranchisesViewModel)textBlock.DataContext).TeamId;
                ViewModelLocator.Teams.SelectedTeamId = selectedItem;
                ViewModelLocator.Teams.LoadOpponentsData();
                pvControl.SelectedIndex = 1;
                if (piOpponents != null && piOpponents.ItemsSource.Count > 0)
                {
                    piOpponents.ScrollTo(ViewModelLocator.Teams.OpponentsItems.First(g => g.Any()));
                }
            }
        }

        private async void OpponentsPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((TeamsOpponentsViewModel)textBlock.DataContext).TeamId;
                ViewModelLocator.Teams.SelectedOpponentId = selectedItem;
                await ViewModelLocator.Teams.LoadTeamResultsData();
                pvControl.SelectedIndex = 2;
                if (piTeamResults != null && piTeamResults.ItemsSource.Count > 0)
                {
                    piTeamResults.ScrollTo(ViewModelLocator.Teams.TeamResultItems.First(g => g.Any()));
                }
            }
        }

        private async void TeamResultsPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = string.Empty;
            var selectedYear = string.Empty;

            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((TeamsResultsViewModel)textBlock.DataContext).Year;
                ViewModelLocator.Teams.SelectedYearId = selectedItem;
                await ViewModelLocator.Teams.LoadTeamDrillDownData();

                pvControl.SelectedIndex = 3;
                if (piTeamDrillDown != null && piTeamDrillDown.ItemsSource.Count > 0)
                {
                    piTeamDrillDown.ScrollTo(ViewModelLocator.Teams.TeamDrillDownItems.First(g => g.Any()));
                }
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {

        }
    }
}