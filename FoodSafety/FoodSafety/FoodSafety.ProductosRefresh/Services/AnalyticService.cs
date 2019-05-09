using System;
using System.Collections.Generic;
using FoodSafety.Common.Utils;
using Microsoft.AppCenter.Analytics;

namespace FoodSafety.ProductosRefresh.Services
{
    /// <summary>
    /// Analytic service.
    /// </summary>
    public class AnalyticService
    {
        /// <summary>
        /// Tracks the analytics.
        /// </summary>
        /// <param name="eventClass">Event class.</param>
        /// <param name="dictionary">Dictionary.</param>
        public static void TrackAnalytics(string eventClass, Dictionary<string, string> dictionary)
        {
            if (DataManager.appCenterActive)
                Analytics.TrackEvent(eventClass, dictionary);
        }
    }
}
