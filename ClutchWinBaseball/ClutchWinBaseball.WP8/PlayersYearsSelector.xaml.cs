using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.ViewModels;
using Microsoft.Phone.Controls;
using System;
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

        private async void YearStackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = string.Empty;
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock != null)
            {
                selectedItem = ((PlayersYearsViewModel)textBlock.DataContext).LineOne;
                ViewModelLocator.Players.SelectedYearId = selectedItem;
                await ViewModelLocator.Players.LoadTeamData();
            }

            NavigationService.Navigate(new Uri("/PlayersFeature.xaml", UriKind.Relative));
        }
    }
}