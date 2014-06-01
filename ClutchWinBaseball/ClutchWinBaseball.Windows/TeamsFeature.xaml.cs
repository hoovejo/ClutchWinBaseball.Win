using ClutchWinBaseball.Common;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Views;
using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball
{
    public sealed partial class TeamsFeature : Page
    {
        public event EventHandler<TeamsViewDefinitionLoadedEventArgs> ViewDefinitionLoaded;

        public static TeamsFeature Current;
        private Frame HiddenFrame = null;
        private TeamsViewType _currentView = TeamsViewType.Franchises;

        private readonly NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public TeamsFeature()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            // This is a static public property that will allow downstream pages to get 
            // a handle to the TeamsFeature instance in order to call methods that are in this class.
            Current = this;
            // This frame is hidden, meaning it is never shown.  It is simply used to load
            // each scenario page and then pluck out the input and output sections and
            // place them into the UserControls on the main page.
            HiddenFrame = new Windows.UI.Xaml.Controls.Frame();
            HiddenFrame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ContentRoot.Children.Add(HiddenFrame);

            ViewDefinitionLoaded += TeamsFeature_ViewDefinitionLoaded;

            var dataSource = new TeamsFeatureIndicatorDataSource();
            ContextControl.ItemsSource = dataSource.Items;

            Loaded += PageLoaded;
            Unloaded += PageUnloaded;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            TeamsContextViewModel teamsContext = TeamsContextViewModel.Instance;

            if (!teamsContext.IsHydratedObject)
            {
                TeamsContextViewModel ctx = await DataManagerLocator.ContextCacheManager.ReadTeamsContextAsync();
                if (ctx != null)
                {
                    teamsContext.ReHydrateMe(ctx);
                }
                teamsContext.IsHydratedObject = true;
            }

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var view = localSettings.Values[Config.TeamsFeatureTabCache] as string;
            if (view != null)
            {
                TeamsViewType savedView = (TeamsViewType)Enum.Parse(typeof(TeamsViewType), view);

                switch (savedView)
                {
                    case TeamsViewType.Franchises: LoadView(typeof(TeamsFranchises), true); break;
                    case TeamsViewType.Opponents: LoadView(typeof(TeamsOpponents), true); break;
                    case TeamsViewType.Results: LoadView(typeof(TeamsResults), true); break;
                    case TeamsViewType.DrillDown: LoadView(typeof(TeamsDrillDown), true); break;
                    default: LoadView(typeof(PlayersBatters)); break;
                }
            }
            else
            {
                LoadView(typeof(TeamsFranchises));
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[Config.TeamsFeatureTabCache] = _currentView.ToString();
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            ViewDefinitionLoaded -= TeamsFeature_ViewDefinitionLoaded;
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

        async void TeamsFeature_ViewDefinitionLoaded(object sender, TeamsViewDefinitionLoadedEventArgs e)
        {
            _currentView = e.TeamsViewType;

            //Handle restart/reentrance
            if (e.CheckDataLoad)
            {
                bool success = false;
                bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();

                switch (e.TeamsViewType)
                {
                    case TeamsViewType.Franchises: success = await DataManagerLocator.TeamsDataManager.GetFranchisesAsync(isNetAvailable); break;
                    case TeamsViewType.Opponents: success = await DataManagerLocator.TeamsDataManager.GetOpponentsAsync(isNetAvailable); break;
                    case TeamsViewType.Results: success = await DataManagerLocator.TeamsDataManager.GetTeamsResultsAsync(isNetAvailable); break;
                    case TeamsViewType.DrillDown: success = await DataManagerLocator.TeamsDataManager.GetTeamsDrillDownAsync(isNetAvailable); break;
                }

                ServiceInteractionNotify(success, isNetAvailable);
            }
        }

        /// <summary>
        /// This method is responsible for loading the individual input and output sections for each scenario.  This 
        /// is based on navigating a hidden Frame to the ScenarioX.xaml page and then extracting out the input
        /// and output sections into the respective UserControl on the main page.
        /// </summary>
        /// <param name="scenarioName"></param>
        public void LoadView(Type viewClass, bool checkDataLoad = false)
        {
            NotifyUser(string.Empty, NotifyType.StatusMessage);
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
                case "TeamsFranchises": 
                    {
                        if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this,
                            new TeamsViewDefinitionLoadedEventArgs { TeamsViewType = TeamsViewType.Franchises, CheckDataLoad = checkDataLoad });
                        }
                        ContextControl.SelectedIndex = 0;
                        break; 
                    }
                case "TeamsOpponents": 
                    {
                        if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this,
                            new TeamsViewDefinitionLoadedEventArgs { TeamsViewType = TeamsViewType.Opponents, CheckDataLoad = checkDataLoad });
                        }
                        ContextControl.SelectedIndex = 1;
                        break; 
                    }
                case "TeamsResults": 
                    {
                        if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this,
                            new TeamsViewDefinitionLoadedEventArgs { TeamsViewType = TeamsViewType.Results, CheckDataLoad = checkDataLoad });
                        }
                        ContextControl.SelectedIndex = 2;
                        break; 
                    }
                case "TeamsDrillDown": 
                    {
                        if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this,
                            new TeamsViewDefinitionLoadedEventArgs { TeamsViewType = TeamsViewType.DrillDown, CheckDataLoad = checkDataLoad });
                        }
                        ContextControl.SelectedIndex = 3;
                        break; 
                    }
            };
        }

        public void ServiceInteractionNotify(bool success, bool isNetAvailable)
        {
            if (!success && !isNetAvailable)
            {
                NotifyUser(Config.NetworkNotAvailable, NotifyType.ErrorMessage);
            }
            else if (!success)
            {
                NotifyUser(Config.Error, NotifyType.ErrorMessage);
            }
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
                case TeamsViewType.Franchises: LoadView(typeof(TeamsDrillDown), true); break;
                case TeamsViewType.Opponents: LoadView(typeof(TeamsFranchises), true); break;
                case TeamsViewType.Results: LoadView(typeof(TeamsOpponents), true); break;
                case TeamsViewType.DrillDown: LoadView(typeof(TeamsResults), true); break;
            };
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentView)
            {
                case TeamsViewType.Franchises: LoadView(typeof(TeamsOpponents), true); break;
                case TeamsViewType.Opponents: LoadView(typeof(TeamsResults), true); break;
                case TeamsViewType.Results: LoadView(typeof(TeamsDrillDown), true); break;
                case TeamsViewType.DrillDown: LoadView(typeof(TeamsFranchises), true); break;
            };
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(HubPage));
            }            
        }

        private void team_Click(object sender, RoutedEventArgs e)
        {
            LoadView(typeof(TeamsFranchises), true);
            // Fire the ViewDefinitionLoaded event since we know that everything is loaded now.
            if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new TeamsViewDefinitionLoadedEventArgs { TeamsViewType = TeamsViewType.Franchises }); }
        }

        private void opponent_Click(object sender, RoutedEventArgs e)
        {
            LoadView(typeof(TeamsOpponents), true);
            // Fire the ViewDefinitionLoaded event since we know that everything is loaded now.
            if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new TeamsViewDefinitionLoadedEventArgs { TeamsViewType = TeamsViewType.Opponents }); }
        }

        private void result_Click(object sender, RoutedEventArgs e)
        {
            LoadView(typeof(TeamsResults), true);
            // Fire the ViewDefinitionLoaded event since we know that everything is loaded now.
            if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new TeamsViewDefinitionLoadedEventArgs { TeamsViewType = TeamsViewType.Results }); }
        }

        private void detail_Click(object sender, RoutedEventArgs e)
        {
            LoadView(typeof(TeamsDrillDown), true);
            // Fire the ViewDefinitionLoaded event since we know that everything is loaded now.
            if (ViewDefinitionLoaded != null) { ViewDefinitionLoaded(this, new TeamsViewDefinitionLoadedEventArgs { TeamsViewType = TeamsViewType.DrillDown }); }
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="LoadState"/>
        /// and <see cref="SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

    }

    public class TeamsViewDefinitionLoadedEventArgs : EventArgs
    {
        public TeamsViewType TeamsViewType { get; set; }
        public bool CheckDataLoad { get; set; }
    }

    public enum TeamsViewType
    {
        Franchises,
        Opponents,
        Results,
        DrillDown
    }

    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };
}
