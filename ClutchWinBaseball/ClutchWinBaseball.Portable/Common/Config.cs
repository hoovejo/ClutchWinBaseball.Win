using System;

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
        public const string Franchise = "http://clutchwin.com/api/v1/franchises.json?";
        public const string FranchiseSearch = "http://clutchwin.com/api/v1/games/for_team/summary.json?";
        public const string FranchiseYearSearch = "http://clutchwin.com/api/v1/games/for_team.json?";

        public const string Years = "http://clutchwin.com/api/v1/seasons.json?";
        public const string Teams = "http://clutchwin.com/api/v1/teams.json?";
        public const string RosterSearch = "http://clutchwin.com/api/v1/players.json?";
        public const string OpponentsForBatter = "http://clutchwin.com/api/v1/opponents/pitchers.json?";
        public const string PlayerPlayerSearch = "http://clutchwin.com/api/v1/events/summary.json?";
        public const string PlayerPlayerYearSearch = "http://clutchwin.com/api/v1/events/summary.json?";


        public const string PC_CacheFileKey = "playersContextViewModel.json";
        public const string PY_CacheFileKey = "playersYears.json";
        public const string PT_CacheFileKey = "playersTeams.json";
        public const string PB_CacheFileKey = "playersBatters.json";
        public const string PP_CacheFileKey = "playersPitchers.json";
        public const string PR_CacheFileKey = "playersResults.json";
        public const string PDD_CacheFileKey = "playersDrillDown.json";

        public const string TC_CacheFileKey = "teamsContextViewModel.json";
        public const string TF_CacheFileKey = "franchises.json";
        public const string TO_CacheTaskKey = "opponents.svc";
        public const string TR_CacheFileKey = "teamsResults.json";
        public const string TDD_CacheFileKey = "teamsDrillDown.json";
    }
}
