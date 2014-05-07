using System;
using System.Collections.Generic;
using System.Text;

namespace ClutchWinBaseball.Portable
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            _main = new MainViewModel();
            _teams = new TeamsFeatureViewModel();
            _players = new PlayersFeatureViewModel();
        }

        private static MainViewModel _main;
        private static TeamsFeatureViewModel _teams;
        private static PlayersFeatureViewModel _players;

        public static MainViewModel Main
        {
            get
            {
                if (_main == null) { _main = new MainViewModel(); }
                return _main;
            }
        }

        public static TeamsFeatureViewModel Teams
        {
            get
            {
                if (_teams == null) { _teams = new TeamsFeatureViewModel(); }
                return _teams;
            }
        }

        public static PlayersFeatureViewModel Players
        {
            get
            {
                if (_players == null) { _players = new PlayersFeatureViewModel(); }
                return _players;
            }
        }
    }

}
