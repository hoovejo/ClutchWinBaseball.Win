﻿using ClutchWinBaseball.Portable.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ClutchWinBaseball.ItemViews
{
    public sealed partial class PlayersResultsItem : UserControl
    {
        public PlayersResultsItem()
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
        public void ShowPlaceholder(PlayersResultsViewModel item)
        {
            _item = item;

            gamesLabel.Opacity = 0;
            atBatLabel.Opacity = 0;
            hitLabel.Opacity = 0;
            secondBaseLabel.Opacity = 0;
            thirdBaseLabel.Opacity = 0;
            homeRunLabel.Opacity = 0;
            runBattedInLabel.Opacity = 0;
            strikeOutLabel.Opacity = 0;
            walksLabel.Opacity = 0;
            averageLabel.Opacity = 0;

            gamesValue.Opacity = 0;
            atBatValue.Opacity = 0;
            hitValue.Opacity = 0;
            secondBaseValue.Opacity = 0;
            thirdBaseValue.Opacity = 0;
            homeRunValue.Opacity = 0;
            runBattedInValue.Opacity = 0;
            strikeOutValue.Opacity = 0;
            walksValue.Opacity = 0;
            averageValue.Opacity = 0;
        }

        /// <summary>
        /// Visualize the labels by setting Opacity to 1.
        /// </summary>
        public void ShowLabels()
        {
            gamesLabel.Opacity = 1;
            atBatLabel.Opacity = 1;
            hitLabel.Opacity = 1;
            secondBaseLabel.Opacity = 1;
            thirdBaseLabel.Opacity = 1;
            homeRunLabel.Opacity = 1;
            runBattedInLabel.Opacity = 1;
            strikeOutLabel.Opacity = 1;
            walksLabel.Opacity = 1;
            averageLabel.Opacity = 1;
        }

        /// <summary>
        /// Visualize category information by updating the correct TextBlock and 
        /// setting Opacity to 1.
        /// </summary>
        public void ShowValues()
        {
            gamesValue.Text = _item.Games;
            atBatValue.Text = _item.AtBat;
            hitValue.Text = _item.Hit;
            secondBaseValue.Text = _item.SecondBase;
            thirdBaseValue.Text = _item.ThirdBase;
            homeRunValue.Text = _item.HomeRun;
            runBattedInValue.Text = _item.RunBattedIn;
            strikeOutValue.Text = _item.StrikeOut;
            walksValue.Text = _item.Walks;
            averageValue.Text = _item.Average;

            gamesValue.Opacity = 1;
            atBatValue.Opacity = 1;
            hitValue.Opacity = 1;
            secondBaseValue.Opacity = 1;
            thirdBaseValue.Opacity = 1;
            homeRunValue.Opacity = 1;
            runBattedInValue.Opacity = 1;
            strikeOutValue.Opacity = 1;
            walksValue.Opacity = 1;
            averageValue.Opacity = 1;
        }

        /// <summary>
        /// Drop all refrences to the data item
        /// </summary>
        public void ClearData()
        {
            _item = null;

            gamesValue.ClearValue(TextBlock.TextProperty);
            atBatValue.ClearValue(TextBlock.TextProperty);
            hitValue.ClearValue(TextBlock.TextProperty);
            secondBaseValue.ClearValue(TextBlock.TextProperty);
            thirdBaseValue.ClearValue(TextBlock.TextProperty);
            homeRunValue.ClearValue(TextBlock.TextProperty);
            runBattedInValue.ClearValue(TextBlock.TextProperty);
            strikeOutValue.ClearValue(TextBlock.TextProperty);
            walksValue.ClearValue(TextBlock.TextProperty);
            averageValue.ClearValue(TextBlock.TextProperty);
        }

        private PlayersResultsViewModel _item;
    }
}
