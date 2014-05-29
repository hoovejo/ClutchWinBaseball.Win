using Newtonsoft.Json;
using System;
using System.Threading;

namespace ClutchWinBaseball.Portable.FeatureStateModel
{
    public class TeamsContextViewModel
    {
        private static readonly Lazy<TeamsContextViewModel> PrivateInstance =
            new Lazy<TeamsContextViewModel>(() => new TeamsContextViewModel(), LazyThreadSafetyMode.PublicationOnly);

        public static TeamsContextViewModel Instance { get { return PrivateInstance.Value;  } }


        public void ReHydrateMe(TeamsContextViewModel cache)
        {
            TeamsContextViewModel gospel = TeamsContextViewModel.Instance;

            gospel.LastOpponentFilterFranchiseId = cache.LastOpponentFilterFranchiseId;
            gospel.LastSearchFranchiseId = cache.LastSearchFranchiseId;
            gospel.LastSearchOpponentId = cache.LastSearchOpponentId;
            gospel.LastDrillDownFranchiseId = cache.LastDrillDownFranchiseId;
            gospel.LastDrillDownOpponentId = cache.LastDrillDownOpponentId;
            gospel.LastDrillDownYearId = cache.LastDrillDownYearId;

            gospel.SelectedTeamId = cache.SelectedTeamId;
            gospel.SelectedOpponentId = cache.SelectedOpponentId;
            gospel.SelectedYearId = cache.SelectedYearId;
        }

        [JsonIgnore]
        public bool IsHydratedObject { get; set; }
        [JsonIgnore]
        public bool HasLoadedFranchisesOncePerSession { get; set; }

        public string SelectedTeamId { get; set;}
        public string SelectedOpponentId { get; set;}
        public string SelectedYearId { get; set; }


        public string LastOpponentFilterFranchiseId { get; set; }

        public bool ShouldFilterOpponents(bool update)
        {
            //needed for opponent filtering
            if (SelectedTeamId == null) return false;

            bool returnValue;
            if (LastOpponentFilterFranchiseId == null || !LastOpponentFilterFranchiseId.Equals(SelectedTeamId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                LastOpponentFilterFranchiseId = SelectedTeamId;
            }
            return returnValue;
        }

        public string LastSearchFranchiseId { get; set; }
        public string LastSearchOpponentId { get; set; }

        public bool ShouldExecuteTeamResultsSearch(bool update)
        {
            //needed for team results svc call
            if (SelectedTeamId == null || SelectedOpponentId == null) return false;

            bool returnValue;
            if (LastSearchFranchiseId == null || LastSearchOpponentId == null ||
                    !LastSearchFranchiseId.Equals(SelectedTeamId) || !LastSearchOpponentId.Equals(SelectedOpponentId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                LastSearchFranchiseId = SelectedTeamId;
                LastSearchOpponentId = SelectedOpponentId;
            }
            return returnValue;
        }

        public string LastDrillDownFranchiseId { get; set; }
        public string LastDrillDownOpponentId { get; set; }
        public string LastDrillDownYearId { get; set; }

        public bool ShouldExecuteTeamDrillDownSearch(bool update)
        {
            //needed for team results svc call
            if (SelectedTeamId == null || SelectedOpponentId == null || SelectedYearId == null) return false;

            bool returnValue;
            if (LastDrillDownFranchiseId == null || LastDrillDownOpponentId == null ||
                    LastDrillDownYearId == null ||
                    !LastDrillDownFranchiseId.Equals(SelectedTeamId) || !LastDrillDownOpponentId.Equals(SelectedOpponentId) ||
                    !LastDrillDownYearId.Equals(SelectedYearId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                LastDrillDownFranchiseId = SelectedTeamId;
                LastDrillDownOpponentId = SelectedOpponentId;
                LastDrillDownYearId = SelectedYearId;
            }
            return returnValue;
        }
    }
}
