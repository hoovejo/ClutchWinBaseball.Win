using ClutchWinBaseball.Common;
using ClutchWinBaseball.Views;
using System;
using System.Collections.Generic;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball
{
    public sealed partial class PlayersFeature : Page
    {
        public event EventHandler<PlayersViewDefinitionLoadedEventArgs> ViewDefinitionLoaded;

        public static PlayersFeature Current;
        private Frame HiddenFrame = null;
        private PlayersViewType _currentView = PlayersViewType.Batters;

        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public PlayersFeature()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            // This is a static public property that will allow downstream pages to get 
            // a handle to the TeamsFeature instance in order to call methods that are in this class.
            Current = this;
            // This frame is hidden, meaning it is never shown.  It is simply used to load
            // each scenario page and then pluck out the input and output sections and
            // place them into the UserControls on the main page.
            HiddenFrame = new Windows.UI.Xaml.Controls.Frame();
            HiddenFrame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ContentRoot.Children.Add(HiddenFrame);

            ViewDefinitionLoaded += PlayersFeature_ViewDefinitionLoaded;

            var dataSource = new PlayersFeatureIndicatorDataSource();
            ContextControl.ItemsSource = dataSource.Items;

            LoadView(typeof(PlayersBatters));

            Loaded += PageLoaded;
            Unloaded += PageUnloaded;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            Current_SizeChanged(null, null);
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (e != null)
            {
                //e.Size.Height > e.Size.Width
                if (e.Size.Width <= 700)
                    VisualStateManager.GoToState(this, "Narrow", true);
                else
                    VisualStateManager.GoToState(this, "Wide", true);

                return;
            }

            switch (ApplicationView.GetForCurrentView().Orientation)
            {
                case ApplicationViewOrientation.Landscape: 
                    VisualStateManager.GoToState(this, "Wide", true); break;
                default: 
                   VisualStateManager.GoToState(this, "Narrow", true); break;
            }
        }

        void PlayersFeature_ViewDefinitionLoaded(object sender, PlayersViewDefinitionLoadedEventArgs e)
        {
            _currentView = e.PlayersViewType;
        }

        /// <summary>
        /// This method is responsible for loading the individual input and output sections for each scenario.  This 
        /// is based on navigating a hidden Frame to the ScenarioX.xaml page and then extracting out the input
        /// and output sections into the respective UserControl on the main page.
        /// </summary>
        /// <param name="scenarioName"></param>
        public void LoadView(Type viewClass)
        {
            // Load the ScenarioX.xaml file into the Frame.
            HiddenFrame.Navigate(viewClass, this);

            // Get the top element, the Page, so we can look up the elements
            // that represent the input and output sections of the ScenarioX file.
            Page hiddenPage = HiddenFrame.Content as Page;

            // Get each element.
            UIElement input = hiddenPage.FindName("Input") as UIElement;
            UIElement output = hiddenPage.FindName("Output") as UIElement;
            UIElement outputSmall = hiddenPage.FindName("OutputSmall") as UIElement;

            // Find the LayoutRoot which parents the input and output sections in the main page.
            Panel panel = hiddenPage.FindName("LayoutRoot") as Panel;

            if (panel != null)
            {
                // Get rid of the content that is currently in the intput and output sections.
                panel.Children.Remove(input);
                panel.Children.Remove(output);
                panel.Children.Remove(outputSmall);

                // Populate the input and output sections with the newly loaded content.
                InputSection.Content = input;
                OutputSection.Content = output;
                OutputSmallSection.Content = outputSmall;
            }

            switch (viewClass.Name)
            {
                case "PlayersBatters": 
                    { 
                        if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new PlayersViewDefinitionLoadedEventArgs { PlayersViewType = PlayersViewType.Batters }); }
                        ContextControl.SelectedIndex = 0;
                        break; 
                    }
                case "PlayersPitchers": 
                    { 
                        if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new PlayersViewDefinitionLoadedEventArgs { PlayersViewType = PlayersViewType.Pitchers }); }
                        ContextControl.SelectedIndex = 1;
                        break; 
                    }
                case "PlayersResults": 
                    { 
                        if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new PlayersViewDefinitionLoadedEventArgs { PlayersViewType = PlayersViewType.Results }); }
                        ContextControl.SelectedIndex = 2;
                        break; 
                    }
                case "PlayersDrillDown": 
                    { 
                        if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new PlayersViewDefinitionLoadedEventArgs { PlayersViewType = PlayersViewType.DrillDown }); }
                        ContextControl.SelectedIndex = 3;
                        break; 
                    }
            };
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                // Use the status message style.
                case NotifyType.StatusMessage:
                    StatusBlock.Style = Resources["StatusStyle"] as Style;
                    break;
                // Use the error message style.
                case NotifyType.ErrorMessage:
                    StatusBlock.Style = Resources["ErrorStyle"] as Style;
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            if (StatusBlock.Text != String.Empty)
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        async void Footer_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentView)
            {
                case PlayersViewType.Batters: LoadView(typeof(PlayersDrillDown)); break;
                case PlayersViewType.Pitchers: LoadView(typeof(PlayersBatters)); break;
                case PlayersViewType.Results: LoadView(typeof(PlayersPitchers)); break;
                case PlayersViewType.DrillDown: LoadView(typeof(PlayersResults)); break;
            };
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentView)
            {
                case PlayersViewType.Batters: LoadView(typeof(PlayersPitchers)); break;
                case PlayersViewType.Pitchers: LoadView(typeof(PlayersResults)); break;
                case PlayersViewType.Results: LoadView(typeof(PlayersDrillDown)); break;
                case PlayersViewType.DrillDown: LoadView(typeof(PlayersBatters)); break;
            };
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HubPage));
        }

        private void batter_Click(object sender, RoutedEventArgs e)
        {
            LoadView(typeof(PlayersBatters));
            // Fire the ViewDefinitionLoaded event since we know that everything is loaded now.
            if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new PlayersViewDefinitionLoadedEventArgs { PlayersViewType = PlayersViewType.Batters }); }
        }

        private void pitcher_Click(object sender, RoutedEventArgs e)
        {
            LoadView(typeof(PlayersPitchers));
            // Fire the ViewDefinitionLoaded event since we know that everything is loaded now.
            if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new PlayersViewDefinitionLoadedEventArgs { PlayersViewType = PlayersViewType.Pitchers }); }
        }

        private void result_Click(object sender, RoutedEventArgs e)
        {
            LoadView(typeof(PlayersResults));
            // Fire the ViewDefinitionLoaded event since we know that everything is loaded now.
            if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new PlayersViewDefinitionLoadedEventArgs { PlayersViewType = PlayersViewType.Results }); }
        }

        private void detail_Click(object sender, RoutedEventArgs e)
        {
            LoadView(typeof(PlayersDrillDown));
            // Fire the ViewDefinitionLoaded event since we know that everything is loaded now.
            if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new PlayersViewDefinitionLoadedEventArgs { PlayersViewType = PlayersViewType.DrillDown }); }
        }


        private void goTo_Years(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PlayersYears));
        }


        private void goTo_Teams(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PlayersTeams));
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
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

    }

    public class PlayersViewDefinitionLoadedEventArgs : EventArgs
    {
        public PlayersViewType PlayersViewType { get; set; }
    }

    public enum PlayersViewType
    {
        Batters,
        Pitchers,
        Results,
        DrillDown
    }
}
