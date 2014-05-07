using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.DataModel;
using ClutchWinBaseball.Portable.ViewModels;

namespace ClutchWinBaseball.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!ViewModelLocator.Main.IsDataLoaded)
            {
                ViewModelLocator.Main.LoadData();
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {

        }

        private void SelectFeature_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = 0;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((FeatureViewModel)textBlock.DataContext).Id;
            }

            switch (selectedItem) 
            {
                case 0: NavigationService.Navigate(new Uri("/TeamsFeature.xaml", UriKind.Relative)); break;
                case 1: NavigationService.Navigate(new Uri("/PlayersFeature.xaml", UriKind.Relative)); break;
                default: NavigationService.Navigate(new Uri("/TeamsFeature.xaml", UriKind.Relative)); break;
            }
        }
    }
}