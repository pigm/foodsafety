﻿using Plugin.Connectivity;

namespace FoodSafety.Common.Utils
{
    public static class ValidationUtils
    {
        /// <summary>
        /// Gets the network status.
        /// </summary>
        /// <returns><c>true</c>, if network status was gotten, <c>false</c> otherwise.</returns>
        public static bool GetNetworkStatus()
        {
            return CrossConnectivity.Current.IsConnected;
        }
    }
}