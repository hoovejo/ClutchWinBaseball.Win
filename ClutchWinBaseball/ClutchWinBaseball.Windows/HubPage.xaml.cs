using ClutchWinBaseball.Common;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.DataModel;
using ClutchWinBaseball.Portable.FeatureStateModel;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;

        /// <summary>
        /// Gets the NavigationHelper used to aid in navigation and process lifetime management.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (!ViewModelLocator.Main.IsDataLoaded)
            {
                ViewModelLocator.Main.LoadData();
            }
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        /// <param name="sender">The GridView or ListView
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        private async void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //// Navigate to the appropriate destination page, configuring the new page
            var selectedItem = 0;
            var item = e.ClickedItem;
            if (item != null)
            {
                selectedItem = ((FeatureViewModel)item).Id;
            }

            switch (selectedItem)
            {
                case 0:
                    if (!Frame.Navigate(typeof(TeamsFeature)))
                    {
                        //throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;
                case 1:
                    {
                        PlayersContextViewModel playersContext = PlayersContextViewModel.Instance;
                        if (!playersContext.IsHydratedObject)
                        {
                            PlayersContextViewModel ctx = await DataManagerLocator.ContextCacheManager.ReadPlayersContextAsync();
                            if (ctx != null)
                            {
                                playersContext.ReHydrateMe(ctx);
                            }
                            playersContext.IsHydratedObject = true;

                            if (!string.IsNullOrEmpty(playersContext.SelectedYearId))
                            {
                                ViewModelLocator.Players.SelectedYearId = playersContext.SelectedYearId;
                            }
                            if (!string.IsNullOrEmpty(playersContext.SelectedTeamId))
                            {
                                ViewModelLocator.Players.SelectedTeamId = playersContext.SelectedTeamId;
                            }
                        }

                        if (!Frame.Navigate(typeof(PlayersFeature)))
                        {
                            //throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                        }
                    }
                    break;
                default:
                    if (!Frame.Navigate(typeof(TeamsFeature)))
                    {
                        //throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;
            }
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
        #endregion

        private async void HyperlinkButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

    }
}
