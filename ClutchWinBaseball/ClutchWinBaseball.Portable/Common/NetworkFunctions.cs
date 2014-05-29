using System.Net.NetworkInformation;

namespace ClutchWinBaseball.Portable.Common
{
    public static class NetworkFunctions
    {
        public static bool GetIsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
