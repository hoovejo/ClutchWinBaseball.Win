using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.ViewModels;
using Microsoft.Phone.Controls;
using System;
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

        private async void TeamStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((PlayersTeamsViewModel)textBlock.DataContext).TeamId;
                ViewModelLocator.Players.SelectedTeamId = selectedItem;
                await ViewModelLocator.Players.LoadBatterData();
            }

            NavigationService.Navigate(new Uri("/PlayersFeature.xaml", UriKind.Relative));
        }
    }
}