using ClutchWinBaseball.ItemViews;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball.Views
{
    public sealed partial class PlayersResults : Page
    {
        // A pointer back to the main page.
        PlayersFeature rootPage = PlayersFeature.Current;

        public PlayersResults()
        {
            this.InitializeComponent();
        }

        private async void Items_ItemClick(object sender, ItemClickEventArgs e)
        {
            PlayersContextViewModel playersContext = PlayersContextViewModel.Instance;
            playersContext.SelectedGameYear = ((PlayersResultsViewModel)e.ClickedItem).GameYear;

            bool success = false;
            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            success = await DataManagerLocator.PlayersDataManager.LoadPlayersDataAsync(PlayersEndpoints.PlayerYearSearch, isNetAvailable);

            rootPage.ServiceInteractionNotify(success, isNetAvailable);

            rootPage.LoadView(typeof(PlayersDrillDown));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // sets the items source for the zoomed out view to the group data as well
            (semanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsPlayerResultItems.View.CollectionGroups;
            (semanticZoomSmall.ZoomedOutView as ListViewBase).ItemsSource = cvsPlayerResultItems.View.CollectionGroups;
        }

        #region Data Visualization
        /// <summary>
        /// We will visualize the data item in asynchronously in multiple phases for improved panning user experience 
        /// of large lists.  In this sample scneario, we will visualize different parts of the data item
        /// in the following order:
        /// 
        ///     1) Placeholders (visualized synchronously - Phase 0)
        ///     2) Labels (visualized asynchronously - Phase 1)
        ///     3) Values (visualized asynchronously - Phase 2)
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Items_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            PlayersResultsItem iv = args.ItemContainer.ContentTemplateRoot as PlayersResultsItem;

            if (args.InRecycleQueue == true)
            {
                iv.ClearData();
            }
            else if (args.Phase == 0)
            {
                iv.ShowPlaceholder(args.Item as PlayersResultsViewModel);

                // Register for async callback to visualize Title asynchronously
                args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            }
            else if (args.Phase == 1)
            {
                iv.ShowLabels();
                args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            }
            else if (args.Phase == 2)
            {
                iv.ShowValues();
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
