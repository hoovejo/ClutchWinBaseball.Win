using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ClutchWinBaseball.WP8
{
    public partial class PlayersYearsSelector : PhoneApplicationPage
    {
        public PlayersYearsSelector()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.PlayersDataManager.GetSeasonsAsync(isNetAvailable);

            showNotification(Config.NetworkNotAvailable);

            if (!success && !isNetAvailable)
            {
                showNotification(Config.NetworkNotAvailable);
            }
            else if (!success)
            {
                showNotification(Config.Error);
            }
        }

        private void YearStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((PlayersYearsViewModel)textBlock.DataContext).LineOne;
            }

            PlayersContextViewModel playersContext = PlayersContextViewModel.Instance;
            ViewModelLocator.Players.SelectedYearId = selectedItem;
            playersContext.SelectedYearId = selectedItem;

            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                NavigationService.Navigate(new Uri("/PlayersFeature.xaml", UriKind.Relative));
            }
        }

        private void showNotification(string msg)
        {
            var result = MessageBox.Show(msg, "Notification", MessageBoxButton.OK);
        }
    }
}