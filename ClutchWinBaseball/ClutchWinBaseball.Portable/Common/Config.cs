using System;
using System.Collections.Generic;
using System.Text;

namespace ClutchWinBaseball.Portable.Common
{
    public class Config
    {
        public const string AccessTokenKey = "&access_token=";
        public const string AccessTokenValue = "joe";

        public const string FranchiseIdKey = "&franchise_abbr=";
        public const string OpponentIdKey = "&opp_franchise_abbr=";
        public const string TeamIdKey = "&team_abbr=";
        public const string FranchiseSearchKeyValue = "&group=season,team_abbr,opp_abbr&fieldset=basic";
        public const string SeasonIdKey = "&season=";
        public const string FieldSetBasicKeyValue = "&fieldset=basic";

        public const string BatterIdKey = "&bat_id=";
        public const string PitcherIdKey = "&pit_id=";
        public const string GroupSeasonKeyValue = "&group=season";
        public const string GroupGameDateKeyValue = "&group=game_date";

        public const string Space = " ";
        public const string Slash = "/";
        public const string Franchise = "http://clutchwin.com/api/v1/franchises.json?";
        public const string FranchiseSearch = "http://clutchwin.com/api/v1/games/for_team/summary.json?";
        public const string FranchiseYearSearch = "http://clutchwin.com/api/v1/games/for_team.json?";

        public const string Years = "http://clutchwin.com/api/v1/seasons.json?";
        public const string Teams = "http://clutchwin.com/api/v1/teams.json?";
        public const string RosterSearch = "http://clutchwin.com/api/v1/players.json?";
        public const string OpponentsForBatter = "http://clutchwin.com/api/v1/opponents/pitchers.json?";
        public const string PlayerPlayerSearch = "http://clutchwin.com/api/v1/events/summary.json?";
        public const string PlayerPlayerYearSearch = "http://clutchwin.com/api/v1/events/summary.json?";

    }
}
