using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Services.Delegate;
using FoodSafety.Common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace FoodSafety.ProductosRefresh.Services
{
    [BroadcastReceiver]
    public class NotificacionAlert : BroadcastReceiver
    {
        ActivityManager am;
        ComponentName cn;
        Context context;
        NotificationChannel chan1;
        public const string CHANNEL_ID = "foodsafety-refresh-group001";
        public const string MESSAGE_BODY = "Hay un producto refresh que ya ha cumplido su tiempo de espera para tomar temperatura.";
        public const int NOTIFY_ID = 1100;


        public override async void OnReceive(Context context, Intent intent)
        {
            if (ValidationUtils.GetNetworkStatus())
            {
                this.context = context;
                am = (ActivityManager)context.GetSystemService(Context.ActivityService);
                cn = am.GetRunningTasks(1).ElementAt(0).TopActivity;
                await ProcesarNotificacion();
            }
        }

        /// <summary>
        /// Procesars the denuncias.
        /// </summary>
        /// <returns>The denuncias.</returns>
        async Task ProcesarNotificacion()
        {
            var productosPendientesArevisar = DataManager.RealmInstance.All<ProductoRefreshPendiente>().
                                                         Where(f => f.EstadoAlertaNotificacion == false).ToList();

            var dateTimeNow = DateTime.Now;
            var horaActual = String.Format("{0:HH:mm}", dateTimeNow);
            if (productosPendientesArevisar.Any())
                {
                    foreach (ProductoRefreshPendiente producto in productosPendientesArevisar)
                    {
                        if (producto.TiempoRestante.Equals(horaActual))
                        {
                            DataManager.RealmInstance.Write(() => { 
                                var dataUp = DataManager.RealmInstance.All<ProductoRefreshPendiente>().
                                                        Where(f => f.ItemPadre == producto.ItemPadre).FirstOrDefault();

                                dataUp.EstadoAlertaNotificacion = true;
                            });
                            SendNotification(MESSAGE_BODY);
                            Refreshindicator();
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
                ActivityContexts.homeAddActivity.alertNotificacionRevisarProducto();
            }
        }

        void SendNotification(string messageBody)
        {
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(context, CHANNEL_ID)
                                                        .SetContentTitle("Listo")
                                                            .SetSmallIcon(Resource.Mipmap.ic_refresh)
                                                        .SetContentText(messageBody)
                                                        .SetAutoCancel(true)
                                                        .SetShowWhen(true)
                                                        .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
                                                        .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(context);
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.Build.VERSION_CODES.O)
            {
                chan1 = new NotificationChannel(CHANNEL_ID,
                                                context.GetString(Resource.String.btnAgregar), NotificationImportance.High);
                chan1.LightColor = Color.Pink;
                chan1.LockscreenVisibility = NotificationVisibility.Public;
                notificationManager.CreateNotificationChannel(chan1);
                notificationBuilder.SetChannelId(CHANNEL_ID);
            }

            notificationManager.Notify(NOTIFY_ID, notificationBuilder.Build());
        }
    }
}
