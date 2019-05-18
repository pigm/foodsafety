using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using FoodSafety.Common.Utils;

namespace FoodSafety.ProductosRefresh
{
    /// <summary>
    /// Ubicacion tienda activity.
    /// </summary>
    [Activity(Label = "¿En qué tienda estás?", Theme = "@style/ThemeNoActionBarAzul")]
    public class UbicacionTiendaActivity : AppCompatActivity
    {
        public const string TAG = "UbicacionSucursales";
        Android.Support.V4.App.FragmentTransaction ft;
        bool posicionPantalla = true;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ubicacion_tienda_activity);
        }

        /// <summary>
        /// Ons the back pressed.
        /// </summary>
        public override void OnBackPressed()
        {
            if (DataManager.estadoVolverAlMapa.Equals("1"))
            {
                Finish();
            }
            else
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

        /// <summary>
        /// Ons the resume.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            if (WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation0 || WindowManager.DefaultDisplay.Rotation == SurfaceOrientation.Rotation180)
            {
                posicionPantalla = true;
            }
            else
            {
                posicionPantalla = false;
            }
            ft = SupportFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.linear_wrapper_mapa, new FragmentSucursales(SupportFragmentManager, posicionPantalla));
            ft.Commit();
        }
    }
}