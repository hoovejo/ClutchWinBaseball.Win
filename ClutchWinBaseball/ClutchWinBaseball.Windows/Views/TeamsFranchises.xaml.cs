using ClutchWinBaseball.ItemViews;
using ClutchWinBaseball.Portable;
using ClutchWinBaseball.Portable.Common;
using ClutchWinBaseball.Portable.FeatureStateModel;
using ClutchWinBaseball.Portable.ViewModels;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClutchWinBaseball.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamsFranchises : Page
    {
        // A pointer back to the main page.
        TeamsFeature rootPage = TeamsFeature.Current;

        public TeamsFranchises()
        {
            this.InitializeComponent();
        }

        private async void Items_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ViewModelLocator.Teams.IsLoadingData) return;

            TeamsContextViewModel teamsContext = TeamsContextViewModel.Instance;
            teamsContext.SelectedTeamId = ((TeamsFranchisesViewModel)e.ClickedItem).TeamId;

            bool success = false;
            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            success = await DataManagerLocator.TeamsDataManager.GetOpponentsAsync(isNetAvailable);

            rootPage.ServiceInteractionNotify(success, isNetAvailable);

            rootPage.LoadView(typeof(TeamsOpponents));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool success = false;
            bool isNetAvailable = NetworkFunctions.GetIsNetworkAvailable();
            success = await DataManagerLocator.TeamsDataManager.GetFranchisesAsync(isNetAvailable);

            // sets the items source for the zoomed out view to the group data as well
            (semanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvsFranchiseItems.View.CollectionGroups;
            (semanticZoomSmall.ZoomedOutView as ListViewBase).ItemsSource = cvsFranchiseItems.View.CollectionGroups;
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
            TeamsFranchiseItem iv = args.ItemContainer.ContentTemplateRoot as TeamsFranchiseItem;

            if (args.InRecycleQueue == true)
            {
                iv.ClearData();
            }
            else if (args.Phase == 0)
            {
                iv.ShowPlaceholder(args.Item as TeamsFranchisesViewModel);

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
