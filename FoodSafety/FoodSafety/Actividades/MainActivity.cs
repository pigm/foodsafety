using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Widget;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Services.Delegate;
using FoodSafety.Common.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSafety.Services;
using Plugin.Geolocator;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.IO;

namespace FoodSafety
{
    /// <summary>
    /// Main activity.
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true)]
    public class MainActivity : Activity, ILocationListener, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        Intent i;
        LocationManager locMgr;
        ProgressDialog _progressDialog;
        const int RequestLocationId = 0;

        /// <summary>
        /// The permissions location.
        /// </summary>
        readonly string[] PermissionsLocation =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.AccessLocationExtraCommands,
            Manifest.Permission.AccessMockLocation,
            Manifest.Permission.AccessNetworkState,
            Manifest.Permission.AccessNotificationPolicy,
            Manifest.Permission.AccountManager,
            Manifest.Permission.Camera,
            Manifest.Permission.ChangeNetworkState,
            Manifest.Permission.Internet,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage
        };

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);        
            BroadcastProduct();
            DataManager.FechaHoy = DateTime.Now.DayOfYear;
            SetContentView(Resource.Layout.main_activity);
            if (ValidationUtils.GetNetworkStatus())
            {
                try{
                    AppCenter.Start("c72dc3eb-e784-4e20-930f-5b4095f95ed0", typeof(Analytics), typeof(Crashes));
                    DataManager.appCenterActive = true;
                }catch{
                    DataManager.appCenterActive = false;
                }

                await GetLocationCompatAsync();
                loading();
                await PerfilarAsync();
            }
            else
            {
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("Trazabilidad de carnes");
                builder.SetIcon(Resource.Mipmap.ic_trazabilidad);
                builder.SetCancelable(false);
                builder.SetMessage(" Estimado usuario, al iniciar trazabilidad de carnes debes tener acceso a internet. Luego podrás ocupar la aplicación sin problemas incluso cuando no tengas conexión a la red.\n Debes tener en cuenta que si cierras las app, debes estar conectado a internet para abrirla nuevamente.");
                builder.SetPositiveButton("Aceptar", delegate { 
                    closeApplication();
                });
                builder.Show();
                AnalyticService.TrackAnalytics("Main Activity", new Dictionary<string, string> {
                    { "Category", "Sin conexion a internet al iniciar la app" },
                    { "Action", "Validacion de conexion"}
                });

            }
        }

        /// <summary>
        /// Loading this instance.
        /// </summary>
        public void loading()
        {
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progressDialog.SetTitle("Obteniendo productos...");
            _progressDialog.SetMessage("Por favor, espera un momento");
            _progressDialog.Show();
            _progressDialog.SetCancelable(false);
        }

        /// <summary>
        /// Perfilars the async.
        /// </summary>
        /// <returns>The async.</returns>
        async Task PerfilarAsync()
        {
            var frigorificosWalmart = DataManager.RealmInstance.All<EntidadFrigorifico>().ToList();
            if (ValidationUtils.GetNetworkStatus())
            {
                var existenFrigorificos = DataManager.RealmInstance.All<EntidadFrigorifico>().Where(x => x.DayOfCharge == DataManager.FechaHoy).ToList();
                if (!existenFrigorificos.Any())
                {                 
                    var frigorificos = await ServiceDelegate.Instance.ObtieneFrigorificos();
                    if (frigorificos.Success)
                    {
                        using (var transfri = DataManager.RealmInstance.BeginWrite())
                        {
                            DataManager.RealmInstance.RemoveAll("EntidadFrigorifico");
                            transfri.Commit();
                        }
                        DataManager.RealmInstance.Write(() =>
                        {
                            var frigorificosResponse = frigorificos.Response as List<EntidadFrigorifico>;
                            foreach (EntidadFrigorifico frigorifico in frigorificosResponse)
                            {
                                DataManager.RealmInstance.Add(frigorifico);
                            }
                        });
                    }
                }
            }

            var itemBalanzaWalmart = DataManager.RealmInstance.All<EntidadItem>().ToList();
            if (ValidationUtils.GetNetworkStatus())
            {
                var existenItemsBalanza = DataManager.RealmInstance.All<EntidadItem>().Where(x => x.DayOfCharge == DataManager.FechaHoy).ToList();
                if (!existenItemsBalanza.Any())
                {
                    var itemsBalanza = await ServiceDelegate.Instance.ObtieneItemBalanza();
                    if (itemsBalanza.Success)
                    {
                        using (var transbal = DataManager.RealmInstance.BeginWrite())
                        {
                            DataManager.RealmInstance.RemoveAll("EntidadItem");
                            transbal.Commit();
                        }
                        DataManager.RealmInstance.Write(() =>
                        {
                            var itemsBalanzaResponse = itemsBalanza.Response as List<EntidadItem>;
                            foreach (EntidadItem itemBalanza in itemsBalanzaResponse)
                            {
                                DataManager.RealmInstance.Add(itemBalanza);
                            }
                        });
                    }
                }
            }

            var procedenciasWalmart = DataManager.RealmInstance.All<EntidadProcedencia>().ToList();
            if (ValidationUtils.GetNetworkStatus())
            {
                var existenProcedencias = DataManager.RealmInstance.All<EntidadProcedencia>().Where(x => x.DayOfCharge == DataManager.FechaHoy).ToList();
                if (!existenProcedencias.Any())
                {                 
                    var procedencias = await ServiceDelegate.Instance.ObtieneProcedencias();
                    if (procedencias.Success)
                    {
                        using (var transproc = DataManager.RealmInstance.BeginWrite())
                        {
                            DataManager.RealmInstance.RemoveAll("EntidadProcedencia");
                            transproc.Commit();
                        }
                        DataManager.RealmInstance.Write(() =>
                        {
                            var procedenciasResponse = procedencias.Response as List<EntidadProcedencia>;
                            foreach (EntidadProcedencia procedencia in procedenciasResponse)
                            {
                                DataManager.RealmInstance.Add(procedencia);
                            }
                        });
                    }
                }
            }
            StreamReader reader = new StreamReader(Assets.Open("BD/locales.json"));
            string json = reader.ReadToEnd();
            var sucursales = await ServiceDelegate.Instance.GetLocales(json);
            if (sucursales.Success)
            {
                DataManager.Sucursales = sucursales.Response as List<Sucursal>;
                try
                {
                    OpenMaps(false);
                }
                catch (Exception ex)
                {
                    dialogWarning("Trazabilidad de carnes", "Ups!, se produjo un error inesperado. Por favor intente más tarde.", "aceptar");
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                dialogWarning("Trazabilidad de carnes", "Ups!, se produjo un error inesperado. Por favor intente más tarde.", "aceptar");
            }
        }

        /// <summary>
        /// Opens the maps.
        /// </summary>
        /// <param name="activeLocal">If set to <c>true</c> active local.</param>
        public void OpenMaps(bool activeLocal = false)
        {
            locMgr = GetSystemService(Context.LocationService) as LocationManager;

            if (locMgr.AllProviders.Contains(LocationManager.GpsProvider)
                && locMgr.IsProviderEnabled(LocationManager.GpsProvider))
            {
                if (ActivityCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessFineLocation) ==
                    Permission.Granted &&
                    ActivityCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessCoarseLocation) ==
                    Permission.Granted)
                {
                    OpenSucursal(activeLocal);
                }
                else
                {
                    RequestPermissions(
                        new String[] { Android.Manifest.Permission.AccessCoarseLocation, Android.Manifest.Permission.AccessFineLocation }
                        , 0
                    );

                    OpenMaps(activeLocal);
                }
            }
            else
            {
                dialogWarning("Trazabilidad de carnes", "No hay acceso a gps. Por favor, habilitar.", "aceptar");
                AnalyticService.TrackAnalytics("Main Activity", new Dictionary<string, string> {
                     { "Category", "Sin acceso al gps" },
                    { "Action", "debe tener conexion a gps"}
                });
            }
        }

        /// <summary>
        /// Opens the sucursal.
        /// </summary>
        public async void OpenSucursal(bool seleccionada)
        {
            //locMgr.RequestLocationUpdates(LocationManager.GpsProvider, 0, 1, this);
            //Android.Locations.Location location = locMgr.GetLastKnownLocation(LocationManager.GpsProvider);
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100; //100 is new default 
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(15));
                Console.WriteLine("Position Status: {0}", position.Timestamp);
                Console.WriteLine("Position Latitude: {0}", position.Latitude);
                Console.WriteLine("Position Longitude: {0}", position.Longitude);

                string lat = position.Latitude.ToString().Replace(',', '.');
                string lon = position.Longitude.ToString().Replace(',', '.');

                geolocalizacionWalmart(lat, lon);
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                Crashes.TrackError(ex);
                geolocalizacionWalmart("-33.537091", "-70.5716911");
            }
        }

        /// <summary>
        /// Geolocalizacions the walmart.
        /// </summary>
        /// <param name="lat">Lat.</param>
        /// <param name="lon">Lon.</param>
        async void geolocalizacionWalmart (string lat, string lon){
            DataManager.actualLatitud = lat;
            DataManager.actualLongitud = lon;
            var existeSucursalSeleccionada = DataManager.RealmInstance.All<SucursalTienda>();
            if (!existeSucursalSeleccionada.Any())
            {
                i = new Intent(this, typeof(UbicacionTiendaActivity));
                StartActivity(i);
                AnalyticService.TrackAnalytics("Main Activity", new Dictionary<string, string> {
                    { "Category", "App, no tiene sucursal asignada" },
                    { "Action", "redireccionado a mapa"}
                });
            }
            else
            {
                i = new Intent(this, typeof(HomeAgregaProductoActivity));
                StartActivity(i);
                AnalyticService.TrackAnalytics("Main Activity", new Dictionary<string, string> {
                    { "Category", "App, tiene sucursal asignada" },
                    { "Action", "redireccionado desde el home"}
                });
            }
        }

        /// <summary>
        /// Ons the stop.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
        }

        /// <summary>
        /// Ons the destroy.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            loading();
        }
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// Ons the start.
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();
        }

        /// <summary>
        /// Ons the restart.
        /// </summary>
        protected override void OnRestart()
        {
            base.OnRestart();
        }

        /// <summary>
        /// Ons the location changed.
        /// </summary>
        /// <param name="location">Location.</param>
        public void OnLocationChanged(Android.Locations.Location location)
        {
            return;
        }

        /// <summary>
        /// Ons the provider disabled.
        /// </summary>
        /// <param name="provider">Provider.</param>
        public void OnProviderDisabled(string provider)
        {
            return;
        }

        /// <summary>
        /// Ons the provider enabled.
        /// </summary>
        /// <param name="provider">Provider.</param>
        public void OnProviderEnabled(string provider)
        {
            return;
        }

        /// <summary>
        /// Ons the status changed.
        /// </summary>
        /// <param name="provider">Provider.</param>
        /// <param name="status">Status.</param>
        /// <param name="extras">Extras.</param>
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            return;
        }

        /// <summary>
        /// Gets the location compat async.
        /// </summary>
        /// <returns>The location compat async.</returns>
        async Task GetLocationCompatAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;
            if (ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted)
            {
                return;
            }

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
            {
                var dialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle("Acción requerida")
                                            .SetMessage("Esta aplicación necesita acceso a la localización del dispositivo")
                .SetPositiveButton("Aceptar", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsLocation, RequestLocationId);
                })
                .Show();
                return;
            }

            RequestPermissions(PermissionsLocation, RequestLocationId);
        }

        /// <summary>
        /// Ons the request permissions result.
        /// </summary>
        /// <param name="requestCode">Request code.</param>
        /// <param name="permissions">Permissions.</param>
        /// <param name="grantResults">Grant results.</param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Permission.Granted)
                        {
                            var toast = Toast.MakeText(this, "Permisos asignados",
                                                       ToastLength.Short);
                            toast.Show();
                        }
                        else
                        {
                            var toast = Toast.MakeText(this, "Los permisos no fueron otorgados, la aplicación no se puede utilizar",
                                                      ToastLength.Long);
                            toast.Show();
                            closeApplication();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        public void closeApplication()
        {
            var activity = this;
            activity.FinishAffinity();
        }

        public void dialogWarning(string title, string message, string positiveButton)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle(title);
            builder.SetIcon(Resource.Mipmap.ic_trazabilidad);
            builder.SetMessage(message);
            builder.SetCancelable(false);
            builder.SetPositiveButton(positiveButton, delegate {
                closeApplication();
            });
            builder.Show();
            
        }

        /// <summary>
        /// Mensajes the item padre no coincide.
        /// </summary>
        /// <param name="itemProducto">Item producto.</param>
        public void mensajeItemPadreNoCoincide()
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Trazabilidad de carnes");
            builder.SetMessage("No existe item padre, para el item");
            builder.SetCancelable(false);
            builder.SetPositiveButton("ACEPTAR", delegate {
               
            });
            builder.Show();
        }

        /// <summary>
        /// Broadcasts the product.
        /// </summary>
        void BroadcastProduct()
        {
            Intent i = new Intent(this, typeof(ItemReceiver));
            PendingIntent pi = PendingIntent.GetBroadcast(this, 0, i, 0);
            AlarmManager alarmManager = (AlarmManager)GetSystemService(AlarmService);
            alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtime, SystemClock.CurrentThreadTimeMillis(),
             30000, pi);
        }
    }
}