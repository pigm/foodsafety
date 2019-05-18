
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FoodSafety.ProductosRefresh
{
    [Activity(Label = "Notificacion producto pendiente a revisar", Theme = "@style/ThemeNoActionBarAzul")]
    public class NotificacionAlertDialogActivity : Activity
    {
        static Dialog customDialog = null;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.notificacion_alert_dialog);
        }

        /// <summary>
        /// Views the formulario user.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public static void viewAlertProductosPendientes(Activity activity)
        {
            customDialog = new Dialog(activity, Resource.Style.Theme_Dialog_Translucent);
            customDialog.SetCancelable(true);
            customDialog.SetContentView(Resource.Layout.notificacion_alert_dialog);       
            Button btnVer = customDialog.FindViewById<Button>(Resource.Id.btnVer);
            btnVer.Click += delegate {
                Intent intentAlistaPendientes = new Intent(activity, typeof(ProductosPendientesActivity));
                activity.StartActivity(intentAlistaPendientes);
                customDialog.Dismiss();
            };
            customDialog.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            customDialog.Show();
        }
    }
}