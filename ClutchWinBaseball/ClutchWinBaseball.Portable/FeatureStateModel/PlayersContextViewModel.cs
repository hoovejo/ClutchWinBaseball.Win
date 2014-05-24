using Newtonsoft.Json;
using System;
using System.Threading;
using System.Xml.Serialization;

namespace ClutchWinBaseball.Portable.FeatureStateModel
{
    public class PlayersContextViewModel
    {
        private static readonly Lazy<PlayersContextViewModel> PrivateInstance =
            new Lazy<PlayersContextViewModel>(() => new PlayersContextViewModel(), LazyThreadSafetyMode.PublicationOnly);

        public static PlayersContextViewModel Instance { get { return PrivateInstance.Value; } }

        [JsonIgnore]
        public bool IsHydratedObject { get; set; }

        public void ReHydrateMe(PlayersContextViewModel cache)
        {
            PlayersContextViewModel gospel = PlayersContextViewModel.Instance;

            gospel.lastYearId = cache.lastYearId;
            gospel.lastTeamId = cache.lastTeamId;
            gospel.lastBatterId = cache.lastBatterId;
            gospel.lastSearchBatterId = cache.lastSearchBatterId;
            gospel.lastSearchPitcherId = cache.lastSearchPitcherId;
            gospel.lastDrillDownBatterId = cache.lastDrillDownBatterId;
            gospel.lastDrillDownPitcherId = cache.lastDrillDownPitcherId;
            gospel.lastDrillDownResultYearId = cache.lastDrillDownResultYearId;
        }

        public bool HasLoadedSessionOncePerSession { get; set; }

        private string lastYearId;

        public bool ShouldExecuteLoadTeams(bool update, string yearId)
        {
            //needed for teams svc call
            if (yearId == null) return false;

            bool returnValue;
            if (lastYearId == null || !lastYearId.Equals(yearId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                lastYearId = yearId;
            }
            return returnValue;
        }

        private string lastTeamId;

        public bool ShouldExecuteLoadBatters(bool update, string teamId)
        {
            //needed for batter svc call
            if (teamId == null) return false;

            bool returnValue;
            if (lastTeamId == null || !lastTeamId.Equals(teamId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                lastTeamId = teamId;
            }
            return returnValue;
        }

        private string lastBatterId;

        public bool ShouldExecuteLoadPitchers(bool update, string batterId, string yearId)
        {
            //needed for pitcher svc call
            if (batterId == null || yearId == null) return false;

            bool returnValue;
            if (lastBatterId == null || !lastBatterId.Equals(batterId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                lastBatterId = batterId;
            }
            return returnValue;
        }

        private string lastSearchBatterId;
        private string lastSearchPitcherId;

        public bool ShouldExecutePlayerResultsSearch(bool update, string batterId, string pitcherId)
        {
            //needed for results svc call
            if (batterId == null || pitcherId == null) return false;

            bool returnValue;
            if (lastSearchBatterId == null || lastSearchPitcherId == null ||
                    !lastSearchBatterId.Equals(batterId) || !lastSearchPitcherId.Equals(pitcherId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                lastSearchBatterId = batterId;
                lastSearchPitcherId = pitcherId;
            }
            return returnValue;
        }

        private string lastDrillDownBatterId;
        private string lastDrillDownPitcherId;
        private string lastDrillDownResultYearId;

        public bool ShouldExecutePlayersDrillDownSearch(bool update, string batterId, string pitcherId, string resultYearId)
        {
            //needed for drillDown svc call
            if (batterId == null || pitcherId == null || resultYearId == null) return false;

            bool returnValue;
            if (lastDrillDownBatterId == null || lastDrillDownPitcherId == null ||
                    lastDrillDownResultYearId == null ||
                    !lastDrillDownBatterId.Equals(batterId) || !lastDrillDownPitcherId.Equals(pitcherId) ||
                    !lastDrillDownResultYearId.Equals(resultYearId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                lastDrillDownBatterId = batterId;
                lastDrillDownPitcherId = pitcherId;
                lastDrillDownResultYearId = resultYearId;
            }
            return returnValue;
        }
    }
}
