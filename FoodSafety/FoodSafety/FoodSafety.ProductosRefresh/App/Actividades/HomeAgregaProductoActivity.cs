using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Utils;
using FoodSafety.ProductosRefresh.Services;
using Microsoft.AppCenter.Analytics;
using System.Threading;
using Newtonsoft.Json;
using FoodSafety.Common.Services.Delegate;
using Newtonsoft.Json.Linq;

namespace FoodSafety.ProductosRefresh
{
    /// <summary>
    /// Home agrega producto activity.
    /// </summary>
    [Activity(Label = "Productos Refresh", Theme = "@style/ThemeNoActionBarAzul", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenLayout)]
    public class HomeAgregaProductoActivity : AppCompatActivity
    {
        Animation animationVisible;
        Animation animationInvisible;
        List<SucursalTienda> dataSucursal = DataManager.RealmInstance.All<SucursalTienda>().ToList();
        List<ProductoRefreshPendiente> dataProductosPendientes;
        Button btnCerrar;
        Fuente fuente;
        ImageButton btnHomeAgregarProducto;
        ImageButton btnHomeProductoPendientes;
        ImageButton btnLogout;
        ImageView btnVolver;
        ImageView imgIcProductoAgregado;
        LinearLayout linearLayoutProductoAgregadoOK;
        LinearLayout linearLayoutEtiquetaProductosNoEnviados;
        LinearLayout linearLayoutBtnIrAMapasDeTienda;
        LinearLayout linearLayoutBtnCambiarUser;
        TextView lblNombreProducto;
        TextView lblCantidadProductosKg;
        TextView lblMensajeCargaYContadorDeProductosPendientesACargar;
        TextView lblNombreActivity;
        TextView lblNombreTienda;
        TextView lblTipoTienda;
        TextView lblNombreUsuarioToolbar;
        Timer timer;
        TimerCallback timerDelegate;
        Typeface fontRobotoRegular;
        Typeface fontRobotoMedium;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation0
                || WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation180) {
                SetContentView(Resource.Layout.home_agrega_producto_activity);
            } else {
                SetContentView(Resource.Layout.home_agrega_producto_horizontal_activity);
            }
            imgIcProductoAgregado = FindViewById<ImageView>(Resource.Id.imgIcProductoAgregado);
            animationVisible = AnimationUtils.LoadAnimation(this, Resource.Animation.box_amination_visible);
            animationInvisible = AnimationUtils.LoadAnimation(this, Resource.Animation.box_amination_invisible);
            btnCerrar = FindViewById<Button>(Resource.Id.btnCerrar);
            btnVolver = FindViewById<ImageView>(Resource.Id.btnVolver);
            btnLogout = FindViewById<ImageButton>(Resource.Id.btnLogout);
            btnHomeAgregarProducto = FindViewById<ImageButton>(Resource.Id.btnHomeAgregarProducto);
            btnHomeProductoPendientes = FindViewById<ImageButton>(Resource.Id.btnHomeProductoPendientes);
            linearLayoutProductoAgregadoOK = FindViewById<LinearLayout>(Resource.Id.linearLayoutProductoAgregadoOK);
            linearLayoutBtnIrAMapasDeTienda = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnIrAMapasDeTienda);
            linearLayoutEtiquetaProductosNoEnviados = FindViewById<LinearLayout>(Resource.Id.linearLayoutEtiquetaProductosNoEnviados);
            lblNombreProducto = FindViewById<TextView>(Resource.Id.lblNombreProducto);
            lblCantidadProductosKg = FindViewById<TextView>(Resource.Id.lblCantidadProductoKg);
            lblNombreUsuarioToolbar = FindViewById<TextView>(Resource.Id.lblNombreUsuarioToolbar);
            lblMensajeCargaYContadorDeProductosPendientesACargar = FindViewById<TextView>(Resource.Id.lblMensajeCargaYContadorDeProductosPendientesACargar);
            lblNombreActivity = FindViewById<TextView>(Resource.Id.lblNombreActivity);
            lblNombreTienda = FindViewById<TextView>(Resource.Id.lblNombreTienda);
            lblTipoTienda = FindViewById<TextView>(Resource.Id.lblTipoTienda);
            linearLayoutBtnCambiarUser = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnCambiarUser);
            linearLayoutBtnCambiarUser.Click += LinearLayoutBtnCambiarUser_Click;
            lblNombreActivity.Text = "Productos Refresh";
            btnVolver.Visibility = ViewStates.Invisible;
            btnHomeAgregarProducto.Click += BtnHomeAgregarProducto_Click;
            btnHomeProductoPendientes.Click += BtnHomeProductoPendientes_Click;
            btnCerrar.Click += BtnCerrar_Click;
            linearLayoutBtnIrAMapasDeTienda.Click += LinearLayoutBtnIrAMapasDeTienda_Click;
            string nombreSucursal = dataSucursal.FirstOrDefault().name.ToString();
            string tipoSucursal = dataSucursal.FirstOrDefault().format.ToString();
            lblNombreTienda.Text = nombreSucursal;
            lblTipoTienda.Text = tipoSucursal;
            fuente = new Fuente(this);
            fontRobotoRegular = fuente.fuenteRobotoRegular();
            fontRobotoMedium = fuente.fuenteRobotoMedium();
            tipografiasEnBotones(btnCerrar);
            tipografiasEnTextView(lblNombreProducto, lblCantidadProductosKg, lblMensajeCargaYContadorDeProductosPendientesACargar);
            ActivityContexts.homeAddActivity = this;
            linearLayoutEtiquetaProductosNoEnviados.Visibility = ViewStates.Invisible;
            if (string.IsNullOrEmpty(DataManager.nombreUsuario))
            {
                IngresoUsuarioResponsableActivity.viewFormularioUser(this);
            }
            nombreUsuarioToolbar();
            AnalyticService.TrackAnalytics("Home app", new Dictionary<string, string> {
                { "Category", "sucursal asignada "+nombreSucursal+", "+tipoSucursal },
                { "Action", "OnCreate"}
            });
        }

        /// <summary>
        /// Ons the resume.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            ChangeColorButtonProductosPendientes();
            UpdateBox();
            UpdateModalProductoAgregado();
            nombreUsuarioToolbar();

        }

        /// <summary>
        /// Changes the color button productos pendientes.
        /// </summary>
        public void ChangeColorButtonProductosPendientes()
        {
            dataProductosPendientes = DataManager.RealmInstance.All<ProductoRefreshPendiente>().ToList();
            var dataProductoPendiente = DataManager.RealmInstance.All<ProductoRefreshPendiente>();
            if (dataProductoPendiente.Any())
            {
                estadoBtnProductosPendientes(true, Resource.Drawable.botonproductopendiente);
            }
            else
            {
                estadoBtnProductosPendientes(false, Resource.Drawable.botonproductopendienteblock);
            }
        }

        /// <summary>
        /// Eliminars the pendientes atrasados.
        /// </summary>
        public async void EliminarPendientesAtrasados()
        {
            List<ProductoRefreshPendiente> dataProductoPendientes = DataManager.RealmInstance.All<ProductoRefreshPendiente>().ToList();
            foreach (var productoPendiente in dataProductoPendientes)
            {
                var existenProductosPendientesAtrasadosLote = DataManager.RealmInstance.All<ProductoRefreshPendiente>().Where(w => w.ItemPadre == productoPendiente.ItemPadre && w.DayOfCharge != DateTime.Now.DayOfYear && w.Correlativo == productoPendiente.Correlativo);
                var existenProductosPendientesAtrasadosElaboracion = DataManager.RealmInstance.All<ProductoRefreshPendiente>().Where(w => w.ItemPadre == productoPendiente.ItemPadre && w.DayOfCharge != DateTime.Now.DayOfYear);
                if (!string.IsNullOrEmpty(productoPendiente.LoteProduccion))
                {
                    foreach (var pendienteAtrasado in existenProductosPendientesAtrasadosLote)
                    {
                        DataManager.RealmInstance.Write(() => {
                            pendienteAtrasado.Temperatura = "-999";
                            pendienteAtrasado.Comentario = "No Finalizado";
                        });
                        string postData = JsonConvert.SerializeObject(pendienteAtrasado);
                        var itemInventario = await ServiceDelegate.Instance.IngresoItemRefreshImg(JObject.Parse(postData));
                        if (itemInventario.Success)
                        {
                            using (var transitemref = DataManager.RealmInstance.BeginWrite())
                            {
                                DataManager.RealmInstance.RemoveRange<ProductoRefreshPendiente>(existenProductosPendientesAtrasadosLote);
                                transitemref.Commit();
                            }
                        }
                        else
                        {
                            IngresoItemRefreshRequest iirr = JsonConvert.DeserializeObject<IngresoItemRefreshRequest>(postData);
                            DataManager.RealmInstance.Write(() =>
                            {
                                DataManager.RealmInstance.Add(iirr);
                            });
                        }
                        continue;
                    }
                }
                else
                {
                    foreach (var pendienteAtrasado in existenProductosPendientesAtrasadosElaboracion)
                    {
                        DataManager.RealmInstance.Write(() => {
                            pendienteAtrasado.Temperatura = "-999";
                            pendienteAtrasado.Comentario = "No Finalizado";
                        });
                        string postData = JsonConvert.SerializeObject(pendienteAtrasado);
                        var itemInventario = await ServiceDelegate.Instance.IngresoItemRefreshImg(JObject.Parse(postData));
                        if (itemInventario.Success)
                        {
                            using (var transitemref = DataManager.RealmInstance.BeginWrite())
                            {
                                DataManager.RealmInstance.RemoveRange<ProductoRefreshPendiente>(existenProductosPendientesAtrasadosElaboracion);
                                transitemref.Commit();
                            }
                        }
                        else
                        {
                            IngresoItemRefreshRequest iirr = JsonConvert.DeserializeObject<IngresoItemRefreshRequest>(postData);
                            DataManager.RealmInstance.Write(() =>
                            {
                                DataManager.RealmInstance.Add(iirr);
                            });
                        }
                        continue;
                    }
                }
                continue;
            }
        }

        /// <summary>
        /// Checks the status async.
        /// </summary>
        public void CheckStatusAsync()
        {
            EliminarPendientesAtrasados();
            ChangeColorButtonProductosPendientes();
        }

        /// <summary>
        /// Estados the button productos pendientes.
        /// </summary>
        /// <param name="estado">If set to <c>true</c> estado.</param>
        /// <param name="boton">Boton.</param>
        void estadoBtnProductosPendientes(bool estado, int boton)
        {
            btnHomeProductoPendientes.Enabled = estado;
            btnHomeProductoPendientes.Clickable = estado;
            btnHomeProductoPendientes.SetBackgroundResource(boton);
        }

        /// <summary>
        /// Nombres the usuario toolbar.
        /// </summary>
        public void nombreUsuarioToolbar()
        {
            if (!string.IsNullOrEmpty(DataManager.nombreUsuario))
            {
                lblNombreUsuarioToolbar.Visibility = ViewStates.Visible;
                lblNombreUsuarioToolbar.Text = DataManager.nombreUsuario;
                linearLayoutBtnCambiarUser.Visibility = ViewStates.Visible;
            }
        }

        /// <summary>
        /// Updates the modal producto agregado.
        /// </summary>
        public void UpdateModalProductoAgregado()
        {
            if (DataManager.estadoProductoAgregado)
            {
                 if (DataManager.cantidadUnidadesProductoAgregado.Contains("quedó"))
                 {
                    imgIcProductoAgregado.SetImageResource(Resource.Drawable.thermometer_red);
                }else{
                    imgIcProductoAgregado.SetImageResource(Resource.Drawable.IconoOk);
                }
                lblNombreProducto.Text = DataManager.nombreProductoAgregado;
                lblCantidadProductosKg.Text = DataManager.cantidadUnidadesProductoAgregado;
                linearLayoutProductoAgregadoOK.Visibility = ViewStates.Visible;
            }
            else
            {
                linearLayoutProductoAgregadoOK.Visibility = ViewStates.Invisible;
            }
        }

        /// <summary>
        /// Alerts the notificacion revisar producto.
        /// </summary>
        public void alertNotificacionRevisarProducto(){
            NotificacionAlertDialogActivity.viewAlertProductosPendientes(this);
        }

        /// <summary>
        /// Updates the box.
        /// </summary>
        public void UpdateBox()
        {
            var dataInventarioNoEnviado = DataManager.RealmInstance.All<IngresoItemRefreshRequest>().Count();

            if (dataInventarioNoEnviado > 0)
            {
                if (linearLayoutEtiquetaProductosNoEnviados.Visibility == ViewStates.Invisible)
                {
                    animationVisible.Duration = 800;
                    linearLayoutEtiquetaProductosNoEnviados.StartAnimation(animationVisible);
                    lblMensajeCargaYContadorDeProductosPendientesACargar.Text = dataInventarioNoEnviado + " productos pendientes por cargar al servidor";
                    linearLayoutEtiquetaProductosNoEnviados.Visibility = ViewStates.Visible;
                }

            }
            else
            {
                if (linearLayoutEtiquetaProductosNoEnviados.Visibility == ViewStates.Visible)
                {
                    animationInvisible.Duration = 800;
                    linearLayoutEtiquetaProductosNoEnviados.StartAnimation(animationInvisible);
                    linearLayoutEtiquetaProductosNoEnviados.Visibility = ViewStates.Invisible;
                }
            }
        }

        /// <summary>
        /// Tipografiases the en text view.
        /// </summary>
        /// <param name="tLblNombreProducto">T lbl nombre producto.</param>
        /// <param name="tLblCantidadProductosKg">T lbl cantidad productos kg.</param>
        /// <param name="tLblMensajeCargaYContadorDeProductosPendientesACargar">T lbl mensaje carga YC ontador de productos pendientes AC argar.</param>
        private void tipografiasEnTextView(TextView tLblNombreProducto, TextView tLblCantidadProductosKg, TextView tLblMensajeCargaYContadorDeProductosPendientesACargar)
        {
            tLblNombreProducto.Typeface = fontRobotoRegular;
            tLblCantidadProductosKg.Typeface = fontRobotoRegular;
            tLblMensajeCargaYContadorDeProductosPendientesACargar.Typeface = fontRobotoRegular;
        }

        /// <summary>
        /// Tipografiases the en botones.
        /// </summary>
        /// <param name="tBtnCerrar">T button cerrar.</param>
        private void tipografiasEnBotones(Button tBtnCerrar)
        {
            tBtnCerrar.Typeface = fontRobotoMedium;
        }

        /// <summary>
        /// Linears the layout button cambiar user click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void LinearLayoutBtnCambiarUser_Click(object sender, EventArgs e)
        {
            IngresoUsuarioResponsableActivity.viewFormularioUser(this);
        }

        /// <summary>
        /// Buttons the home producto pendientes click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BtnHomeProductoPendientes_Click(object sender, EventArgs e)
        {
            var dataProductoPendiente = DataManager.RealmInstance.All<ProductoRefreshPendiente>().Where(x => x.DayOfCharge == DateTime.Now.DayOfYear);
            
            if (dataProductoPendiente.Any())
            {
                Intent intentAProductosPendientes = new Intent(this, typeof(ProductosPendientesActivity));
                StartActivity(intentAProductosPendientes);
            }
            else
            {
                Toast.MakeText(ApplicationContext, "Sin productos pendientes a revisar", ToastLength.Short).Show();
            }
        }

        /// <summary>
        /// Buttons the home agregar producto click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BtnHomeAgregarProducto_Click(object sender, EventArgs e)
        {
            /*Intent intentAgregarProductoActivity = new Intent(this, typeof(AgregarProductoActivity));
            StartActivity(intentAgregarProductoActivity);*/
            GaleriaFragment galeriaFragment = new GaleriaFragment();
            var transcation = SupportFragmentManager.BeginTransaction();
            galeriaFragment.Show(transcation, galeriaFragment.Tag);
            galeriaFragment.Cancelable = false;
            Toast.MakeText(ApplicationContext, "Saca una foto a la etiqueta", ToastLength.Long).Show();
            DataManager.estadoBusquedaProducto = string.Empty;
            DataManager.barra = string.Empty;
            DataManager.refreshItem = string.Empty;
            DataManager.refreshDepto = string.Empty;
            DataManager.refreshItemBalanza = string.Empty;
            DataManager.refreshNombreProducto = string.Empty;
            AnalyticService.TrackAnalytics("Home app", new Dictionary<string, string> {
                { "Category", "Boton agregar podructo ir a formulario de inngreso producto" },
                { "Action", "click BtnHomeAgregarProducto_Click"}
            });
        }

        /// <summary>
        /// Buttons the cerrar click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BtnCerrar_Click(object sender, EventArgs e)
        {
            DataManager.estadoProductoAgregado = false;
            DataManager.nombreProductoAgregado = string.Empty;
            DataManager.kgProductoAgregado = string.Empty;
            linearLayoutProductoAgregadoOK.Visibility = ViewStates.Invisible;
        }

        /// <summary>
        /// Linears the layout button ir AM apas de tienda click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void LinearLayoutBtnIrAMapasDeTienda_Click(object sender, EventArgs e)
        {
            Intent intentUbicacionTiendaActivity = new Intent(this, typeof(UbicacionTiendaActivity));
            StartActivity(intentUbicacionTiendaActivity);
            DataManager.estadoVolverAlMapa = "1";
            AnalyticService.TrackAnalytics("Home app", new Dictionary<string, string> {
                { "Category", "Boton ir a mapas" },
                { "Action", "click BtnHomeAgregarProducto_Click"}
            });
        }

        /// <summary>
        /// Ons the back pressed.
        /// </summary>
        public override void OnBackPressed()
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Productos Refresh");
            builder.SetMessage("¿Está seguro que desea salir de la app?");
            builder.SetPositiveButton("Aceptar", delegate {
                Cerrar.closeApplication(this);
            });
            builder.SetNegativeButton("Cancelar", delegate { });
            builder.Show();
        }
    }
}