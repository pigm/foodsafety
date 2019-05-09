using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Utils;
namespace FoodSafety.ProductosRefresh.Services
{
    [BroadcastReceiver]
    public class CheckPendingProducts : BroadcastReceiver
    {
        ActivityManager am;
        ComponentName cn;
        Context context;
 
        /// <summary>
        /// Ons the receive.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="intent">Intent.</param>
        public override async void OnReceive(Context context, Intent intent)
        {
            if (ValidationUtils.GetNetworkStatus())
            {                this.context = context;
                am = (ActivityManager)context.GetSystemService(Context.ActivityService);
                cn = am.GetRunningTasks(1).ElementAt(0).TopActivity;
                Refreshindicator();
            }
        }

      

        /// <summary>
        /// Refreshindicator this instance.
        /// </summary>
        void Refreshindicator()
        {
            if (cn.ToString().Contains("HomeAgregaProductoActivity"))
            {
                ActivityContexts.homeAddActivity.CheckStatusAsync();
            }
        }
    }
}
