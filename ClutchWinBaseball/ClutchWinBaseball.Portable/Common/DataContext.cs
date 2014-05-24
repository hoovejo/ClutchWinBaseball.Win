using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClutchWinBaseball.Portable.Common
{
    public class DataContext
    {
        public async Task<string> GetFranchisesAsync()
        {
            // http://clutchwin.com/api/v1/franchises.json?
            // &access_token=abc
            var uri = new StringBuilder(Config.Franchise)
                .Append(Config.AccessTokenKey).Append(Config.AccessTokenValue);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return content;
        }

        public async Task<string> GetTeamResultsAsync(string teamId, string opponentId)
        {
            // http://clutchwin.com/api/v1/games/for_team/summary.json?
            // &access_token=abc&franchise_abbr=TOR&opp_franchise_abbr=BAL&group=season,team_abbr,opp_abbr&fieldset=basic
            var uri = new StringBuilder(Config.FranchiseSearch)
                    .Append(Config.AccessTokenKey).Append(Config.AccessTokenValue)
                    .Append(Config.FranchiseIdKey).Append(teamId)
                    .Append(Config.OpponentIdKey).Append(opponentId)
                    .Append(Config.FranchiseSearchKeyValue);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return content;
        }

        public async Task<string> GetTeamDrillDownAsync(string teamId, string opponentId, string yearId)
        {
            // http://clutchwin.com/api/v1/games/for_team.json?
            // &access_token=abc&franchise_abbr=TOR&opp_franchise_abbr=BAL&season=2013&fieldset=basic
            var uri = new StringBuilder(Config.FranchiseYearSearch)
                .Append(Config.AccessTokenKey).Append(Config.AccessTokenValue)
                .Append(Config.FranchiseIdKey).Append(teamId)
                .Append(Config.OpponentIdKey).Append(opponentId)
                .Append(Config.SeasonIdKey).Append(yearId)
                .Append(Config.FieldSetBasicKeyValue);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return content;
        }

        public async Task<string> GetYearsAsync()
        {
            // http://clutchwin.com/api/v1/seasons.json?
            // &access_token=abc
            var uri = new StringBuilder(Config.Years)
                .Append(Config.AccessTokenKey).Append(Config.AccessTokenValue);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return content;
        }

        public async Task<string> GetTeamsAsync(string yearId)
        {
            // http://clutchwin.com/api/v1/teams.json?
            // &access_token=abc&season=2013
            var uri = new StringBuilder(Config.Teams)
                .Append(Config.AccessTokenKey).Append(Config.AccessTokenValue)
                .Append(Config.SeasonIdKey).Append(yearId);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return content;
        }

        public async Task<string> GetBattersAsync(string teamId, string yearId)
        {
            // http://clutchwin.com/api/v1/players.json?
            // &access_token=abc&team_abbr=BAL&season=2013
            var uri = new StringBuilder(Config.RosterSearch)
                    .Append(Config.AccessTokenKey).Append(Config.AccessTokenValue)
                    .Append(Config.TeamIdKey).Append(teamId)
                    .Append(Config.SeasonIdKey).Append(yearId);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return content;
        }

        public async Task<string> GetPitchersAsync(string batterId, string yearId)
        {
            // http://clutchwin.com/api/v1/opponents/pitchers.json?
            // &access_token=abc&bat_id=aybae001&season=2013&fieldset=basic
            var uri = new StringBuilder(Config.OpponentsForBatter)
                    .Append(Config.AccessTokenKey).Append(Config.AccessTokenValue)
                    .Append(Config.BatterIdKey).Append(batterId)
                    .Append(Config.SeasonIdKey).Append(yearId)
                    .Append(Config.FieldSetBasicKeyValue);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return content;
        }

        public async Task<string> GetPlayerResultsAsync(string batterId, string pitcherId)
        {
            // http://clutchwin.com/api/v1/events/summary.json?
            // &access_token=abc&bat_id=aybae001&pit_id=parkj001&group=season
            var uri = new StringBuilder(Config.PlayerPlayerSearch)
                    .Append(Config.AccessTokenKey).Append(Config.AccessTokenValue)
                    .Append(Config.BatterIdKey).Append(batterId)
                    .Append(Config.PitcherIdKey).Append(pitcherId)
                    .Append(Config.GroupSeasonKeyValue);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return content;
        }

        public async Task<string> GetPlayerDrillDownAsync(string batterId, string pitcherId, string yearId)
        {
            // http://clutchwin.com/api/v1/events/summary.json?
            // &access_token=abc&bat_id=aybae001&pit_id=parkj001&season=2013&group=game_date
            var uri = new StringBuilder(Config.PlayerPlayerYearSearch)
                    .Append(Config.AccessTokenKey).Append(Config.AccessTokenValue)
                    .Append(Config.BatterIdKey).Append(batterId)
                    .Append(Config.PitcherIdKey).Append(pitcherId)
                    .Append(Config.SeasonIdKey).Append(yearId)
                    .Append(Config.GroupGameDateKeyValue);

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return content;
        }
    }
}
