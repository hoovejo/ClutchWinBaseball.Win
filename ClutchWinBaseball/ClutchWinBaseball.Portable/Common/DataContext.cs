using ClutchWinBaseball.Portable.DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClutchWinBaseball.Portable.Common
{
    public class DataContext
    {
        public async Task<List<FranchiseModel>> GetFranchisesAsync()
        {
            // /franchises.json
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(Config.Franchise);
            return JsonConvert.DeserializeObject<List<FranchiseModel>>(content);
        }

        public async Task<TeamsResultModel> GetTeamResultsAsync(string teamId, string opponentId)
        {
            // /search/franchise_vs_franchise/ATL/BAL.json
            var uri = new StringBuilder().Append(Config.FranchiseSearch).Append(teamId)
                .Append(Config.Slash).Append(opponentId).Append(Config.JsonSuffix);
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TeamsResultModel>(content);
        }

        public async Task<TeamsDrillDownModel> GetTeamDrillDownAsync(string teamId, string opponentId, string yearId)
        {
            // /search/franchise_vs_franchise_by_year/ATL/BOS/2013.json
            var uri = new StringBuilder().Append(Config.FranchiseYearSearch).Append(teamId)
                .Append(Config.Slash).Append(opponentId)
                .Append(Config.Slash).Append(yearId)
                .Append(Config.JsonSuffix);
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TeamsDrillDownModel>(content);
        }

        public async Task<List<YearModel>> GetYearsAsync()
        {
            // /years.json
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(Config.Years);
            return JsonConvert.DeserializeObject<List<YearModel>>(content);
        }

        public async Task<List<TeamModel>> GetTeamsAsync(string yearId)
        {
            // /teams/2013.json
            var uri = new StringBuilder().Append(Config.Teams).Append(yearId).Append(Config.JsonSuffix);
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return JsonConvert.DeserializeObject<List<TeamModel>>(content);
        }

        public async Task<List<BatterModel>> GetBattersAsync(string teamId, string yearId)
        {
            // /search/roster_for_team_and_year/ATL/2013.json
            var uri = new StringBuilder().Append(Config.RosterSearch).Append(teamId)
                .Append(Config.Slash).Append(yearId).Append(Config.JsonSuffix);
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return JsonConvert.DeserializeObject<List<BatterModel>>(content);
        }

        public async Task<PitcherModel> GetPitchersAsync(string batterId, string yearId)
        {
            // /search/opponents_for_batter/aybae001/2013.json
            var uri = new StringBuilder().Append(Config.OpponentsForBatter).Append(batterId)
                .Append(Config.Slash).Append(yearId).Append(Config.JsonSuffix);
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PitcherModel>(content);
        }

        public async Task<PlayersResultModel> GetPlayerResultsAsync(string batterId, string pitcherId)
        {
            // /search/player_vs_player/aybae001/parkj001.json
            var uri = new StringBuilder().Append(Config.PlayerPlayerSearch).Append(batterId)
                .Append(Config.Slash).Append(pitcherId).Append(Config.JsonSuffix);
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PlayersResultModel>(content);
        }

        public async Task<PlayersDrillDownModel> GetPlayerDrillDownAsync(string batterId, string pitcherId, string yearId, string gameType)
        {
            // /search/player_vs_player_by_year/aybae001/parkj001/2013/regular.json
            var uri = new StringBuilder().Append(Config.PlayerPlayerYearSearch).Append(batterId)
                .Append(Config.Slash).Append(pitcherId)
                .Append(Config.Slash).Append(yearId)
                .Append(Config.Slash).Append(gameType)
                .Append(Config.JsonSuffix);
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri.ToString());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PlayersDrillDownModel>(content);
        }
    }
}
