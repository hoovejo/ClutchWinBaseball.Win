using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ClutchWinBaseball.Portable.FeatureStateModel
{
    public class TeamsContextViewModel
    {
        [XmlIgnore]
        public bool IsHydratedObject { get; set; }

        private string lastOpponentFilterFranchiseId;

        public bool ShouldFilterOpponents(bool update, string franchiseId)
        {
            //needed for opponent filtering
            if (franchiseId == null) return false;

            bool returnValue;
            if (lastOpponentFilterFranchiseId == null || !lastOpponentFilterFranchiseId.Equals(franchiseId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                lastOpponentFilterFranchiseId = franchiseId;
            }
            return returnValue;
        }

        private string lastSearchFranchiseId;
        private string lastSearchOpponentId;

        public bool ShouldExecuteTeamResultsSearch(bool update, string franchiseId, string opponentId)
        {
            //needed for team results svc call
            if (franchiseId == null || opponentId == null) return false;

            bool returnValue;
            if (lastSearchFranchiseId == null || lastSearchOpponentId == null ||
                    !lastSearchFranchiseId.Equals(franchiseId) || !lastSearchOpponentId.Equals(opponentId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                lastSearchFranchiseId = franchiseId;
                lastSearchOpponentId = opponentId;
            }
            return returnValue;
        }

        private string lastDrillDownFranchiseId;
        private string lastDrillDownOpponentId;
        private string lastDrillDownYearId;

        public bool ShouldExecuteTeamDrillDownSearch(bool update, string franchiseId, string opponentId, string yearId)
        {
            //needed for team results svc call
            if (franchiseId == null || opponentId == null || yearId == null) return false;

            bool returnValue;
            if (lastDrillDownFranchiseId == null || lastDrillDownOpponentId == null ||
                    lastDrillDownYearId == null ||
                    !lastDrillDownFranchiseId.Equals(franchiseId) || !lastDrillDownOpponentId.Equals(opponentId) ||
                    !lastDrillDownYearId.Equals(yearId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                lastDrillDownFranchiseId = franchiseId;
                lastDrillDownOpponentId = opponentId;
                lastDrillDownYearId = yearId;
            }
            return returnValue;
        }
    }
}
