using ClutchWinBaseball.Common;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.DataModel;
using ClutchWinBaseball.Portable.FeatureStateModel;
using System;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
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
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public HubPage()
        {
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
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

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
        #endregion

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
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
                        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;
                case 1:
                    {
                        if (!Frame.Navigate(typeof(PlayersFeature)))
                        {
                            throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                        }
                    }
                    break;
                default:
                    if (!Frame.Navigate(typeof(TeamsFeature)))
                    {
                        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;
            }
        }

        private async void HyperlinkButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private async void Footer_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private void AdControl_AdRefreshed(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (adControl != null)
            {
                adControl.Height = 50;
            }
        }

        private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.Mobile.Common.AdErrorEventArgs e)
        {
            if (adControl != null)
            {
                adControl.Height = 1;
            }
        }
    }
}