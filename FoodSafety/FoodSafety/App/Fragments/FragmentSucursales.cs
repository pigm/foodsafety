using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Utils;
using System;
using System.Threading;
using TrazabilidadCarnes.Adapters;
using Android.Content;
using System.Linq;
using Microsoft.AppCenter.Crashes;
using System.Threading.Tasks;

namespace FoodSafety
{
    /// <summary>
    /// Fragment sucursales.
    /// </summary>
    public class FragmentSucursales : Fragment, IOnMapReadyCallback, GoogleMap.IOnMarkerClickListener, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        AdapterSucursal adapter;
        BottomSheetBehavior bottomSheetBehavior_suc;
        bool isVertical;
        CoordinatorLayout cordinatorSucursales;
        float scale;
        FragmentManager fm;
        GoogleApiClient client;
        GoogleMap map;
        ImageButton btnLogout;
        ImageView btnVolver;
        List<Marker> markerList;
        LinearLayout linear_mapa;
        LinearLayout sheet_suc;
        LinearLayout linearLayoutBtnIrAMapasDeTienda;
        LinearLayoutManager layoutManager;
        public LinearLayout listaVacia;
        RecyclerView recycler;
        SupportMapFragment mf;
        TextView lblNombreActivity;
        ViewGroup v;

        public FragmentSucursales(FragmentManager fm, bool isVertical = true)
        {
            this.fm = fm;
            this.isVertical = isVertical;
        }

        public void OnConnected(Bundle connectionHint)
        {
            return;
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            return;
        }

        public void OnConnectionSuspended(int cause)
        {
            return;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);    
            RetainInstance = true;
        }            

        /// <summary>
        /// Ons the create view.
        /// </summary>
        /// <returns>The create view.</returns>
        /// <param name="inflater">Inflater.</param>
        /// <param name="container">Container.</param>
        /// <param name="savedInstanceState">Saved instance state.</param>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            scale = Context.Resources.DisplayMetrics.Density;
            int pixels = 0;
            if (v == null)
            {
                v = (ViewGroup)inflater.Inflate(Resource.Layout.sucursales_fragment, container, false);              
            }
            cordinatorSucursales = v.FindViewById<CoordinatorLayout>(Resource.Id.cordinatorSucursales);
            btnLogout = v.FindViewById<ImageButton>(Resource.Id.btnLogout);
            btnVolver = v.FindViewById<ImageView>(Resource.Id.btnVolver);
            linearLayoutBtnIrAMapasDeTienda = v.FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnIrAMapasDeTienda);
            lblNombreActivity = v.FindViewById<TextView>(Resource.Id.lblNombreActivity);
            btnVolver.Visibility = ViewStates.Invisible;
            btnLogout.Visibility = ViewStates.Invisible;
            linearLayoutBtnIrAMapasDeTienda.Visibility = ViewStates.Invisible;
            lblNombreActivity.Text = "¿En qué tienda estás?";
            sheet_suc = v.FindViewById<LinearLayout>(Resource.Id.linear_sheet_sucursales);
             linear_mapa = v.FindViewById<LinearLayout>(Resource.Id.linear_mapa);
            listaVacia = v.FindViewById<LinearLayout>(Resource.Id.sucursal_lista_vacia);
            CoordinatorLayout.LayoutParams linearLayoutParams = new CoordinatorLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
            if (isVertical)
            {
                pixels = (int)(360 * scale + 0.5f);
                bottomSheetBehavior_suc = BottomSheetBehavior.From(sheet_suc);
                bottomSheetBehavior_suc.PeekHeight = pixels;
            }
            else
            {
                ViewGroup.LayoutParams parameters = (ViewGroup.LayoutParams)sheet_suc.LayoutParameters;
                parameters.Width = 500;
                parameters.Height = 660;
                if(parameters is ViewGroup.MarginLayoutParams){
                    ((ViewGroup.MarginLayoutParams)parameters).LeftMargin = 780;
                }
                sheet_suc.LayoutParameters = parameters;
                
                pixels = (int)(0 * scale + 0.5f);

            }
            linearLayoutParams.SetMargins(0, 0, 0, pixels);
            linear_mapa.LayoutParameters = linearLayoutParams;
            recycler = v.FindViewById<RecyclerView>(Resource.Id.mi_recycler_view_sucursales);
            adapter = new AdapterSucursal(DataManager.Sucursales, this);
            adapter.itemClick += OnItemClick;
            recycler.SetAdapter(adapter);
            layoutManager = new LinearLayoutManager(Activity);
            recycler.SetLayoutManager(layoutManager);
            
            mf = (SupportMapFragment)ChildFragmentManager.FindFragmentById(Resource.Id.map_sucursales);
            mf.GetMapAsync(this);
            ThreadPool.QueueUserWorkItem(o => cargaServicios());
            RetainInstance = true;
            return v;
        }
       
        /// <summary>
        /// Ons the marker click.
        /// </summary>
        /// <returns><c>true</c>, if marker click was oned, <c>false</c> otherwise.</returns>
        /// <param name="marker">Marker.</param>
        public bool OnMarkerClick(Marker marker)
        {

            if (map != null)
            {
                if (DataManager.Sucursales != null)
                {
                    var sucursalTiendaWalmart = DataManager.RealmInstance.All<SucursalTienda>().ToList();
                    if (ValidationUtils.GetNetworkStatus())
                    {
                        if (sucursalTiendaWalmart.Any())
                        {
                            foreach (SucursalTienda sucursalSeleccionada in sucursalTiendaWalmart)
                            {
                                using (var trans = DataManager.RealmInstance.BeginWrite())
                                {
                                    DataManager.RealmInstance.Remove(sucursalSeleccionada);
                                    trans.Commit();
                                }
                            }
                        }
                    }

                    var sucursal = DataManager.Sucursales.Where(x => x.location.address == marker.Snippet).FirstOrDefault();
                    DataManager.sucursalSeleccionada = sucursal;

                    var existeSucursalSelecionada = DataManager.RealmInstance.All<SucursalTienda>();
                    if (!existeSucursalSelecionada.Any())
                    {   ///cada vez que escribamos en la bd debemos borrar el registro antiguo.
                        DataManager.RealmInstance.Write(() =>
                        {
                            SucursalTienda sucursalTiendaSeleccionada = new SucursalTienda
                            {
                                id = DataManager.sucursalSeleccionada.id,
                                name = DataManager.sucursalSeleccionada.name,
                                format = DataManager.sucursalSeleccionada.format,
                                address = DataManager.sucursalSeleccionada.location.address
                            };
                            DataManager.RealmInstance.Add(sucursalTiendaSeleccionada);

                        });
                    }
                    Intent intentAHomeAgregaProducto = new Intent(Activity, typeof(HomeAgregaProductoActivity));
                    StartActivity(intentAHomeAgregaProducto);
                    DataManager.estadoBusquedaProducto = string.Empty;
                }
            }
            return true;
        }

        /// <summary>
        /// Cargas the servicios.
        /// </summary>
        private void cargaServicios()
        {
            try
            {
                Activity.RunOnUiThread(async () => await saveServices());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Activity.Finish();
            }
        }

        /// <summary>
        /// Saves the services.
        /// </summary>
        /// <param name="sucursales">Sucursales.</param>
        private async Task saveServices()
        {
            if (map != null)
            {
                drawMarker(map);
                adapter.refreshAdapter(DataManager.Sucursales, map);
            }
        }

        /// <summary>
        /// Draws the marker.
        /// </summary>
        /// <param name="map2">Map2.</param>
        public void drawMarker(GoogleMap map2)
        {
            BitmapDescriptor mapIcon;

            if (map2 != null)
            {
                markerList = new List<Marker>();
                if (DataManager.Sucursales != null && DataManager.Sucursales.Count > 0)
                {
                    for (int i = 0; i < DataManager.Sucursales.Count; i++)
                    {
                        mapIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.liderubicacion);

                        if (DataManager.Sucursales[i].HasValidCoordinates())
                        {
                            string nombre = DataManager.Sucursales[i].name;

                            map2.MyLocationEnabled = true;
                            var point = new LatLng(DataManager.Sucursales[i].location.position.coordinates[1], DataManager.Sucursales[i].location.position.coordinates[0]);
                            MarkerOptions markerOpt1 = new MarkerOptions();
                            markerOpt1.SetPosition(point);
                            markerOpt1.SetTitle(nombre);
                            markerOpt1.SetIcon(mapIcon);
                            markerOpt1.SetSnippet(DataManager.Sucursales[i].location.address);
                            markerList.Add(map2.AddMarker(markerOpt1));

                            if (DataManager.actualLatitud != null)
                            {
                                var lat = DataManager.actualLatitud.Replace("\r", "").Replace(',', '.');
                                var lon = DataManager.actualLongitud.Replace("\r", "").Replace(',', '.');
                                var latitude = double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture);
                                var longitud = double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture);
                                double diff = Geolocation.
                                                         CalculateDistance(latitude, longitud, DataManager.Sucursales[i].location.position.coordinates[1], DataManager.Sucursales[i].location.position.coordinates[0]);
                                DataManager.Sucursales[i].UserDistance = diff;
                                DataManager.Sucursales[i].HasUserDistance = true;
                            }
                        }
                    }
                }
                else
                {
                    Toast.MakeText(Activity, "Lista de sucursales vacia", ToastLength.Short).Show();
                }
            }
        }

        /// <summary>
        /// Aplis the client build.
        /// </summary>
        public void ApliClientBuild()
        {
            client = new GoogleApiClient.Builder(Activity)
                                                        .AddConnectionCallbacks(this)
                                                        .AddOnConnectionFailedListener(this)
                                                        .AddApi(LocationServices.API)
                                                        .EnableAutoManage(Activity, this)
                                                        .Build();
        }

        /// <summary>
        /// Ons the map ready.
        /// </summary>
        /// <param name="googleMap">Google map.</param>
        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            ApliClientBuild();
            if (map != null)
            {
                LatLng location;
                map.MyLocationEnabled = true;
                var horaActual = DateTime.Now.Hour;
                if (horaActual <= 6){
                    //GoogleMap noche
                    map.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this.Activity, Resource.Raw.googlemap_style_json_noche));
                }
                else if (horaActual >= 20){
                    //GoogleMap noche
                    map.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this.Activity, Resource.Raw.googlemap_style_json_noche));
                }
                else{
                    //GoogleMap dia
                    map.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this.Activity, Resource.Raw.googlemap_style_json));
                }

                if (DataManager.actualLatitud != null)
                {
                    var lat = DataManager.actualLatitud.Replace("\r", "").Replace(',', '.');
                    var lon = DataManager.actualLongitud.Replace("\r", "").Replace(',', '.');
                    var latitude = double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture);
                    var longitud = double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture);
                    location = new LatLng(latitude, longitud);

                    map.MyLocationButtonClick += delegate
                    {
                        map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(location, 14.0f));
                    };
                }
                else
                {
                    location = new LatLng(-33.53058, -70.674187);
                    map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(location, 14.0f));
                }
                map.MoveCamera(CameraUpdateFactory.NewLatLng(location));
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(location);
                builder.Zoom(15);
                builder.Bearing(0);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                map.MoveCamera(cameraUpdate);
                map.SetOnMarkerClickListener(this);
            }
            else
            {
                Toast.MakeText(Activity, "No se ha podido cargar el mapa", ToastLength.Short).Show();
            }
        }

        //EVENTO CLICK ITEMS RECYCLERVIEW
        /// <summary>
        /// Ons the item click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnItemClick(object sender, List<Object> e)
        {
            var descripcionAddress = (string)e[2];
            if (map != null)
            {
                if (DataManager.Sucursales != null)
                {
                    var sucursalTiendaWalmart = DataManager.RealmInstance.All<SucursalTienda>().ToList();
                    if (ValidationUtils.GetNetworkStatus())
                    {
                        if (sucursalTiendaWalmart.Any())
                        {
                            foreach (SucursalTienda sucursalSeleccionada in sucursalTiendaWalmart)
                            {
                                using (var trans = DataManager.RealmInstance.BeginWrite())
                                {
                                    DataManager.RealmInstance.Remove(sucursalSeleccionada);
                                    trans.Commit();
                                }
                            }
                        }
                    }

                    var sucursal = DataManager.Sucursales.Where(x => x.location.address == descripcionAddress).FirstOrDefault();
                    DataManager.sucursalSeleccionada = sucursal;

                    var existeSucursalSelecionada = DataManager.RealmInstance.All<SucursalTienda>();
                    if (!existeSucursalSelecionada.Any())
                    {   ///cada vez que escribamos en la bd debemos borrar el registro antiguo.
                        DataManager.RealmInstance.Write(() =>
                        {
                            SucursalTienda sucursalTiendaSeleccionada = new SucursalTienda
                            {
                                id = DataManager.sucursalSeleccionada.id,
                                name = DataManager.sucursalSeleccionada.name,
                                format = DataManager.sucursalSeleccionada.format,
                                address = DataManager.sucursalSeleccionada.location.address
                            };
                            DataManager.RealmInstance.Add(sucursalTiendaSeleccionada);

                        });
                    }
                    Intent intentAHomeAgregaProducto = new Intent(Activity, typeof(HomeAgregaProductoActivity));
                    StartActivity(intentAHomeAgregaProducto);
                    DataManager.estadoBusquedaProducto = string.Empty;
                }
            }
        }

        /// <summary>
        /// Ons the stop.
        /// </summary>
        public override void OnStop()
        {
            base.OnStop();
            if (client != null && client.IsConnected)
            {
                client.StopAutoManage(this.Activity);
                client.Disconnect();
            }
        }
    }
}