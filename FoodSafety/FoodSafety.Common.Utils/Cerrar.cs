﻿using System;
using Android.App;

namespace FoodSafety.Common.Utils
{
    /// <summary>
    /// Cerrar.
    /// </summary>
    public class Cerrar
    {
        /// <summary>
        /// Closes the application.
        /// </summary>
        public static void closeApplication(Activity actividad)
        {
            var activity = actividad;
            activity.FinishAffinity();
        }
    }
}
