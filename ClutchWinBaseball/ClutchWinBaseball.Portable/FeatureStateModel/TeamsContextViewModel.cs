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
            if (string.IsNullOrEmpty(SelectedTeamId)) return false;

            bool returnValue;
            if (string.IsNullOrEmpty(LastOpponentFilterFranchiseId) || !LastOpponentFilterFranchiseId.Equals(SelectedTeamId))
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

        public bool TeamsResultsServiceCallAllowed()
        {
            //needed for team results svc call
            if (string.IsNullOrEmpty(SelectedTeamId) || string.IsNullOrEmpty(SelectedOpponentId)) return false;
            else return true;
        }

        public bool ShouldExecuteTeamResultsSearch(bool update)
        {
            //needed for team results svc call
            if (string.IsNullOrEmpty(SelectedTeamId) || string.IsNullOrEmpty(SelectedOpponentId)) return false;

            bool returnValue;
            if (string.IsNullOrEmpty(LastSearchFranchiseId) || string.IsNullOrEmpty(LastSearchOpponentId) ||
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

        public bool TeamsDrillDownServiceCallAllowed()
        {
            //needed for team results svc call
            if (string.IsNullOrEmpty(SelectedTeamId) || string.IsNullOrEmpty(SelectedOpponentId) || string.IsNullOrEmpty(SelectedYearId)) return false;
            else return true;
        }

        public bool ShouldExecuteTeamDrillDownSearch(bool update)
        {
            //needed for team results svc call
            if (string.IsNullOrEmpty(SelectedTeamId) || string.IsNullOrEmpty(SelectedOpponentId) || string.IsNullOrEmpty(SelectedYearId)) return false;

            bool returnValue;
            if (string.IsNullOrEmpty(LastDrillDownFranchiseId) || string.IsNullOrEmpty(LastDrillDownOpponentId) ||
                    string.IsNullOrEmpty(LastDrillDownYearId) ||
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
