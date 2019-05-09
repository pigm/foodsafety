using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Utils;
using FoodSafety.Services;
using Android.App;

namespace FoodSafety
{
    /// <summary>
    /// Home agrega producto activity.
    /// </summary>
    [Activity(Label = "Trazabilidad de Carnes", Theme = "@style/ThemeNoActionBarAzul", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenLayout)]
    public class HomeAgregaProductoActivity : AppCompatActivity
    {
        Animation animationVisible;
        Animation animationInvisible;
        List<SucursalTienda> dataSucursal = DataManager.RealmInstance.All<SucursalTienda>().ToList();
        Button btnCerrar;
        Fuente fuente;
        ImageButton btnHomeAgregarProducto;
        ImageButton btnLogout;
        ImageView btnVolver;
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
        Typeface fontRobotoRegular;
        Typeface fontRobotoMedium;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation0 || WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation180)
            {
                SetContentView(Resource.Layout.home_agrega_producto_activity);
            } else {
                SetContentView(Resource.Layout.home_agrega_producto_horizontal_activity);
            }
            animationVisible = AnimationUtils.LoadAnimation(this, Resource.Animation.box_amination_visible);
            animationInvisible = AnimationUtils.LoadAnimation(this, Resource.Animation.box_amination_invisible);
            btnCerrar = FindViewById<Button>(Resource.Id.btnCerrar);
            btnVolver = FindViewById<ImageView>(Resource.Id.btnVolver);
            btnLogout = FindViewById<ImageButton>(Resource.Id.btnLogout);
            linearLayoutBtnCambiarUser = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnCambiarUser);
            linearLayoutBtnCambiarUser.Click += LinearLayoutBtnCambiarUser_Click;
            btnHomeAgregarProducto = FindViewById<ImageButton>(Resource.Id.btnHomeAgregarProducto);
            linearLayoutProductoAgregadoOK = FindViewById<LinearLayout>(Resource.Id.linearLayoutProductoAgregadoOK);
            linearLayoutBtnIrAMapasDeTienda = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnIrAMapasDeTienda);
            linearLayoutEtiquetaProductosNoEnviados = FindViewById<LinearLayout>(Resource.Id.linearLayoutEtiquetaProductosNoEnviados);
            lblNombreProducto = FindViewById<TextView>(Resource.Id.lblNombreProducto);
            lblCantidadProductosKg = FindViewById<TextView>(Resource.Id.lblCantidadProductoKg);
            lblMensajeCargaYContadorDeProductosPendientesACargar = FindViewById<TextView>(Resource.Id.lblMensajeCargaYContadorDeProductosPendientesACargar);
            lblNombreActivity = FindViewById<TextView>(Resource.Id.lblNombreActivity);
            lblNombreTienda = FindViewById<TextView>(Resource.Id.lblNombreTienda);
            lblTipoTienda = FindViewById<TextView>(Resource.Id.lblTipoTienda);
            lblNombreUsuarioToolbar = FindViewById<TextView>(Resource.Id.lblNombreUsuarioToolbar);
            lblNombreActivity.Text = "Trazabilidad de Carnes";
            btnVolver.Visibility = ViewStates.Invisible;
            btnHomeAgregarProducto.Click += BtnHomeAgregarProducto_Click;
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
            UpdateBox(0, null);
            UpdateModalProductoAgregado();
            nombreUsuarioToolbar();
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
                lblNombreProducto.Text = DataManager.nombreProductoAgregado;
                lblCantidadProductosKg.Text = "ha sido agregado con " + DataManager.kgProductoAgregado + " kg";
                linearLayoutProductoAgregadoOK.Visibility = ViewStates.Visible;
            }
            else
            {
                linearLayoutProductoAgregadoOK.Visibility = ViewStates.Invisible;
            }
        }         

        /// <summary>
        /// Updates the box.
        /// </summary>
        public void UpdateBox(int cod, string productoItem)
        {
            var dataInventarioNoEnviado = DataManager.RealmInstance.All<IngresaProcesaBarrasTabletRequest>().Count();
           
            if (dataInventarioNoEnviado > 0)
            {
                if(linearLayoutEtiquetaProductosNoEnviados.Visibility == ViewStates.Invisible){
                    animationVisible.Duration = 800;
                    linearLayoutEtiquetaProductosNoEnviados.StartAnimation(animationVisible);
                    lblMensajeCargaYContadorDeProductosPendientesACargar.Text = dataInventarioNoEnviado + " productos pendientes por cargar al servidor";
                    linearLayoutEtiquetaProductosNoEnviados.Visibility = ViewStates.Visible;
                }
                else
                {
                    animationVisible.Duration = 800;
                    linearLayoutEtiquetaProductosNoEnviados.StartAnimation(animationVisible);
                    lblMensajeCargaYContadorDeProductosPendientesACargar.Text = dataInventarioNoEnviado + " productos pendientes por cargar al servidor";
                    linearLayoutEtiquetaProductosNoEnviados.Visibility = ViewStates.Visible;
                }

                if (cod == 2)
                {
                    Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                    builder.SetTitle("Trazabilidad de carnes");
                    builder.SetIcon(Resource.Mipmap.ic_trazabilidad);
                    builder.SetCancelable(false);
                    builder.SetMessage("No existe item padre, para " + productoItem);
                    builder.SetPositiveButton("Aceptar", delegate {});
                    builder.Show();
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

                if (cod == 2)
                {
                    Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                    builder.SetTitle("Trazabilidad de carnes");
                    builder.SetIcon(Resource.Mipmap.ic_trazabilidad);
                    builder.SetCancelable(false);
                    builder.SetMessage("No existe item padre, para " + productoItem);
                    builder.SetPositiveButton("Aceptar", delegate { });
                    builder.Show();
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
        /// Buttons the home agregar producto click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BtnHomeAgregarProducto_Click(object sender, EventArgs e)
        {
            /* Intent intentAgregarProductoActivity = new Intent(this, typeof(AgregarProductoActivity));
             StartActivity(intentAgregarProductoActivity);*/;
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
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
            builder.SetTitle("Trazabilidad de Carnes");
            builder.SetMessage("¿Está seguro que desea salir de la app?");
            builder.SetPositiveButton("Aceptar", delegate {
                Cerrar.closeApplication(this);
            });
            builder.SetNegativeButton("Cancelar", delegate { });
            builder.Show();
        }

    }
}
