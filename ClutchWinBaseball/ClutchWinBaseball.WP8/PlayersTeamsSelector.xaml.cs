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
    public partial class PlayersTeamsSelector : PhoneApplicationPage
    {
        public PlayersTeamsSelector()
        {
            InitializeComponent();
        }

        private void TeamStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((PlayersTeamsViewModel)textBlock.DataContext).TeamId;
            }

            PlayersContextViewModel playersContext = PlayersContextViewModel.Instance;

            ViewModelLocator.Players.SelectedTeamId = selectedItem;
            playersContext.SelectedTeamId = selectedItem;

            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                NavigationService.Navigate(new Uri("/PlayersFeature.xaml", UriKind.Relative));
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            bool success = false;

            success = await DataManagerLocator.PlayersDataManager.GetTeamsAsync(isNetAvailable);

            if (!success && !isNetAvailable)
            {
                showNotification(Config.NetworkNotAvailable);
            }
            else if (!success)
            {
                showNotification(Config.Error);
            }

            base.OnNavigatedTo(e);
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
    }
}