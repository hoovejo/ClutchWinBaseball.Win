﻿using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.DataModel;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.WP8.Exceptions;
using GoogleAds;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.Storage;

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

            //ExceptionHandler.CheckForPreviousException();

            //var fileManager = new CacheFileManager(ApplicationData.Current.LocalFolder);
            //await fileManager.DeleteAllFilesAsync();
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
                case 1:
                    {
                        NavigationService.Navigate(new Uri("/PlayersFeature.xaml", UriKind.Relative)); 
                    }
                    break;
                default: NavigationService.Navigate(new Uri("/TeamsFeature.xaml", UriKind.Relative)); break;
            }
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private async void Footer_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private void OnAdReceived(object sender, AdEventArgs e)
        {
            if (adControl != null)
            {
                adControl.Height = 50;
            }
        }

        private void OnFailedToReceiveAd(object sender, AdErrorEventArgs errorCode)
        {
            if (adControl != null)
            {
                adControl.Height = 1;
            }
        }

        
        private void adControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            if (adControl != null)
            {
                adControl.Height = 1;
            }
        }

        private void adControl_AdRefreshed(object sender, EventArgs e)
        {
            if (adControl != null)
            {
                adControl.Height = 80;
            }
        }
        
    }
}