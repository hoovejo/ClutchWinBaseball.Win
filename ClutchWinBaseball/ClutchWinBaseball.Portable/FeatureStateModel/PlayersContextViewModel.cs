using System.Xml.Serialization;

namespace ClutchWinBaseball.Portable.FeatureStateModel
{
    public class PlayersContextViewModel
    {
        [XmlIgnore]
        public bool IsHydratedObject { get; set; }

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
        private bool loadBatters;
        public void SetVoteLoadBatters(bool value, string teamId)
        {
            if (teamId != null && lastTeamId != teamId)
            {
                loadBatters = value;
            }
        }

        public bool ShouldExecuteLoadBatters(bool update, string teamId)
        {
            //needed for batter svc call
            if (teamId == null) return false;

            bool returnValue;
            if (loadBatters)
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
