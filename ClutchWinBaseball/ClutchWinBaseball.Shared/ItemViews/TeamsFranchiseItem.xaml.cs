using ClutchWinBaseball.Portable.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ClutchWinBaseball.ItemViews
{
    public sealed partial class TeamsFranchiseItem : UserControl
    {
        public TeamsFranchiseItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// This method visualizes the placeholder state of the data item. When 
        /// showing a placehlder, we set the opacity of other elements to zero
        /// so that stale data is not visible to the end user.  Note that we use
        /// Grid's background color as the placeholder background.
        /// </summary>
        /// <param name="item"></param>
        public void ShowPlaceholder(TeamsFranchisesViewModel item)
        {
            _item = item;
            titleTextBlock.Opacity = 0;
            categoryTextBlock.Opacity = 0;
        }

        /// <summary>
        /// Visualize the Title by updating the TextBlock for Title and setting Opacity
        /// to 1.
        /// </summary>
        public void ShowTitle()
        {
            titleTextBlock.Text = _item.LineOne;
            titleTextBlock.Opacity = 1;
        }

        /// <summary>
        /// Visualize category information by updating the correct TextBlock and 
        /// setting Opacity to 1.
        /// </summary>
        public void ShowCategory()
        {
            categoryTextBlock.Text = _item.LineTwo;
            categoryTextBlock.Opacity = 1;
        }

        /// <summary>
        /// Drop all refrences to the data item
        /// </summary>
        public void ClearData()
        {
            _item = null;
            titleTextBlock.ClearValue(TextBlock.TextProperty);
            categoryTextBlock.ClearValue(TextBlock.TextProperty);
        }

        private TeamsFranchisesViewModel _item;
    }
}
