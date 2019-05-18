using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Services.Delegate;
using FoodSafety.Common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FoodSafety.ProductosRefresh.Services
{
    [BroadcastReceiver]
    public class ItemReceiver : BroadcastReceiver
    {
        ActivityManager am;
        ComponentName cn;
        Context ctx;

        public override async void OnReceive(Context context, Intent intent)
        {  
            if (ValidationUtils.GetNetworkStatus())
            {
                am = (ActivityManager)context.GetSystemService(Context.ActivityService);
                cn = am.GetRunningTasks(1).ElementAt(0).TopActivity;
                await ProcesarProductos();
            }
        }

        /// <summary>
        /// Procesars the denuncias.
        /// </summary>
        /// <returns>The denuncias.</returns>
        async Task ProcesarProductos()
        {
            var productosProcesar = DataManager.RealmInstance.All<IngresoItemRefreshRequest>().ToList();
            if (ValidationUtils.GetNetworkStatus())
            {
                if (productosProcesar.Any())
                {
                    foreach (IngresoItemRefreshRequest producto in productosProcesar)
                    {
                        string postData = JsonConvert.SerializeObject(producto);
                        var enviaProducto = await ServiceDelegate.Instance.IngresoItemRefreshImg(JObject.Parse(postData));
                        if (enviaProducto.Success)
                        {
                            using (var trans = DataManager.RealmInstance.BeginWrite())
                            {
                                DataManager.RealmInstance.Remove(producto);
                                trans.Commit();
                            }
                            Refreshindicator();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Refreshindicator this instance.
        /// </summary>
        void Refreshindicator()
        {
            if (cn.ToString().Contains("HomeAgregaProductoActivity"))
            {
                ActivityContexts.homeAddActivity.UpdateBox();
            }
        }
    }
}
