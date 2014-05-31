using ClutchWinBaseball.ItemViews;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PlayersYears : Page
    {
        public PlayersYears()
        {
            this.InitializeComponent();

            Loaded += PlayersYears_Loaded;
            Unloaded += PlayersYears_Unloaded;
        }

        void PlayersYears_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        void PlayersYears_Loaded(object sender, RoutedEventArgs e)
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
            if (StatusBlock.Text != string.Empty)
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void Items_ItemClick(object sender, ItemClickEventArgs e)
        {
            PlayersContextViewModel playersContext = PlayersContextViewModel.Instance;

            var yearId = ((PlayersYearsViewModel)e.ClickedItem).LineOne;
            ViewModelLocator.Players.SelectedYearId = yearId;
            playersContext.SelectedYearId = yearId;

            Frame.Navigate(typeof(PlayersFeature));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            PlayersContextViewModel playersContext = PlayersContextViewModel.Instance;

            bool success = false;
            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            success = await DataManagerLocator.PlayersDataManager.LoadPlayersDataAsync(PlayersEndpoints.Seasons, isNetAvailable);

            ServiceInteractionNotify(success, isNetAvailable);
        }

        #region Data Visualization
        /// <summary>
        /// We will visualize the data item in asynchronously in multiple phases for improved panning user experience 
        /// of large lists.  In this sample scneario, we will visualize different parts of the data item
        /// in the following order:
        /// 
        ///     1) Placeholders (visualized synchronously - Phase 0)
        ///     2) Tilte (visualized asynchronously - Phase 1)
        ///     3) Image (visualized asynchronously - Phase 2)
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Items_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            PlayersYearItem iv = args.ItemContainer.ContentTemplateRoot as PlayersYearItem;

            if (args.InRecycleQueue == true)
            {
                iv.ClearData();
            }
            else if (args.Phase == 0)
            {
                iv.ShowPlaceholder(args.Item as PlayersYearsViewModel);

                // Register for async callback to visualize Title asynchronously
                args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            }
            else if (args.Phase == 1)
            {
                iv.ShowTitle();
                args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            }
            else if (args.Phase == 2)
            {
                iv.ShowCategory();
                args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            }
            // For improved performance, set Handled to true since app is visualizing the data item
            args.Handled = true;
        }

        /// <summary>
        /// Managing delegate creation to ensure we instantiate a single instance for 
        /// optimal performance. 
        /// </summary>
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ContainerContentChangingDelegate
        {
            get
            {
                if (_delegate == null)
                {
                    _delegate = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(Items_ContainerContentChanging);
                }
                return _delegate;
            }
        }
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _delegate;

        #endregion //Data Visualization
    }
}
