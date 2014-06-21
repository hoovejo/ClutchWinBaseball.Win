using Newtonsoft.Json;
using System;
using System.Threading;

namespace ClutchWinBaseball.Portable.FeatureStateModel
{
    public class PlayersContextViewModel
    {
        private static readonly Lazy<PlayersContextViewModel> PrivateInstance =
            new Lazy<PlayersContextViewModel>(() => new PlayersContextViewModel(), LazyThreadSafetyMode.PublicationOnly);

        public static PlayersContextViewModel Instance { get { return PrivateInstance.Value; } }


        public void ReHydrateMe(PlayersContextViewModel cache)
        {
            PlayersContextViewModel gospel = PlayersContextViewModel.Instance;

            gospel.LastYearId = cache.LastYearId;
            gospel.LastTeamId = cache.LastTeamId;
            gospel.LastBatterId = cache.LastBatterId;
            gospel.LastSearchBatterId = cache.LastSearchBatterId;
            gospel.LastSearchPitcherId = cache.LastSearchPitcherId;
            gospel.LastDrillDownBatterId = cache.LastDrillDownBatterId;
            gospel.LastDrillDownPitcherId = cache.LastDrillDownPitcherId;
            gospel.LastDrillDownResultYearId = cache.LastDrillDownResultYearId;

            gospel.SelectedYearId = cache.SelectedYearId;
            gospel.SelectedTeamId = cache.SelectedTeamId;
            gospel.SelectedBatterId = cache.SelectedBatterId;
            gospel.SelectedPitcherId = cache.SelectedPitcherId;
            gospel.SelectedGameYear = cache.SelectedGameYear;
        }

        [JsonIgnore]
        public bool IsHydratedObject { get; set; }
        [JsonIgnore]
        public bool HasLoadedSeasonsOncePerSession { get; set; }


        public string SelectedYearId { get; set; }
        public string SelectedTeamId { get; set; }
        public string SelectedBatterId { get; set; }
        public string SelectedPitcherId { get; set; }
        public string SelectedGameYear { get; set; }


        public string LastYearId { get; set; }

        public bool ShouldExecuteLoadTeams(bool update)
        {
            //needed for teams svc call
            if (string.IsNullOrEmpty(SelectedYearId)) return false;

            bool returnValue;
            if (string.IsNullOrEmpty(LastYearId) || !LastYearId.Equals(SelectedYearId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                LastYearId = SelectedYearId;
            }
            return returnValue;
        }

        public string LastTeamId { get; set; }

        public bool ShouldExecuteLoadBatters(bool update)
        {
            //needed for batter svc call
            if (string.IsNullOrEmpty(SelectedTeamId)) return false;

            bool returnValue;
            if (string.IsNullOrEmpty(LastTeamId) || !LastTeamId.Equals(SelectedTeamId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                LastTeamId = SelectedTeamId;
            }
            return returnValue;
        }

        public string LastBatterId { get; set; }

        public bool PlayersPitchersServiceCallAllowed()
        {
            //needed for pitcher svc call
            if (string.IsNullOrEmpty(SelectedBatterId) || string.IsNullOrEmpty(SelectedYearId)) return false;
            else return true;
        }

        public bool ShouldExecuteLoadPitchers(bool update)
        {
            //needed for pitcher svc call
            if (string.IsNullOrEmpty(SelectedBatterId) || string.IsNullOrEmpty(SelectedYearId)) return false;

            bool returnValue;
            if (string.IsNullOrEmpty(LastBatterId) || !LastBatterId.Equals(SelectedBatterId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                LastBatterId = SelectedBatterId;
            }
            return returnValue;
        }

        public string LastSearchBatterId { get; set; }
        public string LastSearchPitcherId { get; set; }

        public bool PlayersResultsServiceCallAllowed()
        {
            //needed for results svc call
            if (string.IsNullOrEmpty(SelectedBatterId) || string.IsNullOrEmpty(SelectedPitcherId)) return false;
            else return true;
        }

        public bool ShouldExecutePlayerResultsSearch(bool update)
        {
            //needed for results svc call
            if (string.IsNullOrEmpty(SelectedBatterId) || string.IsNullOrEmpty(SelectedPitcherId)) return false;

            bool returnValue;
            if (string.IsNullOrEmpty(LastSearchBatterId) || string.IsNullOrEmpty(LastSearchPitcherId) ||
                    !LastSearchBatterId.Equals(SelectedBatterId) || !LastSearchPitcherId.Equals(SelectedPitcherId))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                LastSearchBatterId = SelectedBatterId;
                LastSearchPitcherId = SelectedPitcherId;
            }
            return returnValue;
        }

        public string LastDrillDownBatterId { get; set; }
        public string LastDrillDownPitcherId { get; set; }
        public string LastDrillDownResultYearId { get; set; }

        public bool PlayersDrillDownServiceCallAllowed()
        {
            //needed for drillDown svc call
            if (string.IsNullOrEmpty(SelectedBatterId) || string.IsNullOrEmpty(SelectedPitcherId) || string.IsNullOrEmpty(SelectedGameYear)) return false;
            else return true;
        }

        public bool ShouldExecutePlayersDrillDownSearch(bool update)
        {
            //needed for drillDown svc call
            if (string.IsNullOrEmpty(SelectedBatterId) || string.IsNullOrEmpty(SelectedPitcherId) || string.IsNullOrEmpty(SelectedGameYear)) return false;

            bool returnValue;
            if (string.IsNullOrEmpty(LastDrillDownBatterId) || string.IsNullOrEmpty(LastDrillDownPitcherId) ||
                    string.IsNullOrEmpty(LastDrillDownResultYearId) ||
                    !LastDrillDownBatterId.Equals(SelectedBatterId) || !LastDrillDownPitcherId.Equals(SelectedPitcherId) ||
                    !LastDrillDownResultYearId.Equals(SelectedGameYear))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (update)
            {
                LastDrillDownBatterId = SelectedBatterId;
                LastDrillDownPitcherId = SelectedPitcherId;
                LastDrillDownResultYearId = SelectedGameYear;
            }
            return returnValue;
        }
    }
}
