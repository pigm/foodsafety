using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Services.Delegate;
using FoodSafety.Common.Utils;
using FoodSafety.ProductosRefresh.Adapters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FoodSafety.ProductosRefresh
{
    /// <summary>
    /// Productos pendientes activity.
    /// </summary>
    [Activity(Label = "Productos Pendientes", Theme = "@style/ThemeNoActionBarAzul", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenLayout)]
    public class ProductosPendientesActivity : AppCompatActivity
    {
        AdapterProductoPendiente adapter;
        ImageView btnVolver;
        LinearLayoutManager layoutManager;
        List<SucursalTienda> dataSucursal = DataManager.RealmInstance.All<SucursalTienda>().ToList();
        List<ProductoRefreshPendiente> dataProductosPendientes = DataManager.RealmInstance.All<ProductoRefreshPendiente>().ToList();
        LinearLayout linearLayoutBtnIrAMapasDeTienda;
        LinearLayout linearLayoutBtnCambiarUser;
        RecyclerView recyclerView;
        String nombreSucursal;
        String tipoSucursal;
        String idSucursal;
        TextView lblNombreActivity;
        TextView lblNombreTienda;
        TextView lblTipoTienda;
        TextView lblSinProductosPendientes;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.productos_pendientes_activity);

            lblNombreActivity = FindViewById<TextView>(Resource.Id.lblNombreActivity);
            lblNombreActivity.Text = "Productos Pendientes";
            linearLayoutBtnCambiarUser = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnCambiarUser);
            linearLayoutBtnCambiarUser.Visibility = ViewStates.Gone;
            lblNombreTienda = FindViewById<TextView>(Resource.Id.lblNombreTienda);
            lblTipoTienda = FindViewById<TextView>(Resource.Id.lblTipoTienda);
            nombreSucursal = dataSucursal.FirstOrDefault().name.ToString();
            tipoSucursal = dataSucursal.FirstOrDefault().format.ToString();
            lblNombreTienda.Text = nombreSucursal;
            lblTipoTienda.Text = tipoSucursal;
            linearLayoutBtnIrAMapasDeTienda = FindViewById<LinearLayout>(Resource.Id.linearLayoutBtnIrAMapasDeTienda);
            linearLayoutBtnIrAMapasDeTienda.Click += LinearLayoutBtnIrAMapasDeTienda_Click;
            btnVolver = FindViewById<ImageView>(Resource.Id.btnVolver);
            btnVolver.Click += BtnVolver_Click;
            layoutManager = new LinearLayoutManager(this);
            recyclerView = FindViewById<RecyclerView>(Resource.Id.rvProductosPendientes);
            lblSinProductosPendientes = FindViewById<TextView>(Resource.Id.lblSinProductosPendientes);
            PrepareRecycler();
        }



        /// <summary>
        /// Linears the layout button ir AM apas de tienda click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void LinearLayoutBtnIrAMapasDeTienda_Click(object sender, EventArgs e)
        {
            Intent intentAUbicacionTiendaActivity = new Intent(this, typeof(UbicacionTiendaActivity));
            StartActivity(intentAUbicacionTiendaActivity);
            DataManager.estadoVolverAlMapa = "1";
        }

        /// <summary>
        /// Buttons the volver click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BtnVolver_Click(object sender, EventArgs e)
        {
            Intent intentAlHome = new Intent(this, typeof(HomeAgregaProductoActivity));
            StartActivity(intentAlHome);
        }

        /// <summary>
        /// Prepares the recycler.
        /// </summary>
        void PrepareRecycler()
        {
            if (dataProductosPendientes.Count >= 1)
            {
                recyclerView.Visibility = ViewStates.Visible;
                lblSinProductosPendientes.Visibility = ViewStates.Gone;
                adapter = new AdapterProductoPendiente(dataProductosPendientes, this);
                adapter.itemClick += OnItemClick;
                recyclerView.SetAdapter(adapter);
                recyclerView.SetLayoutManager(layoutManager);
            }
            else
            {
                recyclerView.Visibility = ViewStates.Gone;
                lblSinProductosPendientes.Visibility = ViewStates.Visible;
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
            Intent intentAAgregaProducto = new Intent(this, typeof(AgregarProductoActivity));
            StartActivity(intentAAgregaProducto);
            DataManager.estadoBusquedaProducto = "pendiente";
        }
    }
}