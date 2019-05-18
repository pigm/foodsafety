using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Util;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Services.Delegate;
using FoodSafety.Common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FoodSafety.Services
{
    /// <summary>
    /// Item receiver.
    /// </summary>
    [BroadcastReceiver]
    public class ItemReceiver : BroadcastReceiver
    {
        ActivityManager am;
        ComponentName cn;
        Context ctx;
        /// <summary>
        /// Ons the receive.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="intent">Intent.</param>
        public override async void OnReceive(Context context, Intent intent)
        {

            if (ValidationUtils.GetNetworkStatus())
            {
                ctx = context;
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
            try
            {
                var productosProcesar = DataManager.RealmInstance.All<IngresaProcesaBarrasTabletRequest>().ToList();
                if (ValidationUtils.GetNetworkStatus())
                {
                    if (productosProcesar.Any())
                    {
                        foreach (IngresaProcesaBarrasTabletRequest producto in productosProcesar)
                        {
                            string postData = JsonConvert.SerializeObject(producto);
                            var enviaProducto = await ServiceDelegate.Instance.IngresoItemImage(JObject.Parse(postData));
                        
                            if (enviaProducto.Success)
                            {
                                try
                                {
                                    var codResponse = (int)enviaProducto.Response;
                                    var prodItem = producto.Item;
                                    if (codResponse == 2)
                                    {
                                        using (var trans = DataManager.RealmInstance.BeginWrite())
                                        {
                                            DataManager.RealmInstance.Remove(producto);
                                            trans.Commit();
                                        }
                                        Refreshindicator(2, prodItem);
                                    }
                                    else
                                    {
                                        using (var trans = DataManager.RealmInstance.BeginWrite())
                                        {
                                            DataManager.RealmInstance.Remove(producto);
                                            trans.Commit();
                                        }
                                        Refreshindicator(0, null);
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    Log.Info("ProcesarProductos", ex.Message);
                                    using (var trans = DataManager.RealmInstance.BeginWrite())
                                    {
                                        DataManager.RealmInstance.Remove(producto);
                                        trans.Commit();
                                    }
                                    Refreshindicator(0, null);
                                }                              
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Info("ProcesarProductos", ex.Message);
                Refreshindicator(0, null);
            }
        }


        /// <summary>
        /// Refreshindicator this instance.
        /// </summary>
        void Refreshindicator(int cod, string producto){
            if (cn.ToString().Contains("HomeAgregaProductoActivity"))
            {
                ActivityContexts.homeAddActivity.UpdateBox(cod, producto);
            }
        }
    }
}