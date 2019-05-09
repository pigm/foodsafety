using System.Collections.Generic;
using Android.Graphics;
using FoodSafety.Common.Models.Models;
using Realms;

namespace FoodSafety.Common.Utils
{
    /// <summary>
    /// Data manager.
    /// </summary>
    public class DataManager
    {
        public static ProductoRefreshPendiente ProductoPendienteSeleccionado { get; set; }
        public static List<ProductoRefreshPendiente> productosRefreshPendientes { get; set; }
        public static List<ProductoRefreshPendiente> productosRefreshPendientesArevisar { get; set; }
        public static Sucursal sucursalSeleccionada { get; set; }
        public static List<Sucursal> Sucursales { get; set; }
        public static string alertaPendientesArevisar { get; set; }
        public static string actualLatitud { get; set; }
        public static string actualLongitud { get; set; }
        public static string idSucursal { get; set; }
        public static string nombreSucursal { get; set; }
        public static string tipoSucursal { get; set; }
        public static bool appCenterActive { get; set; }
        public static bool isProductoNuevo = false;
        public static string fechaDescongelacionManager = string.Empty;
        public static string fechaElaboracionManager = string.Empty;
        public static int FechaHoy { get; set; }
        public static string nombreUsuario { get; set; }
        public static string barra { get; set; }
        public static bool estadoProductoAgregado { get; set; }
        public static bool RefreshDesdePendiente { get; set; }
        public static string nombreProductoAgregado { get; set; }
        public static string kgProductoAgregado { get; set; }
        public static string cantidadUnidadesProductoAgregado { get; set; }
        public static string estadoVolverAlMapa { get; set; }
        public static string estadoBusquedaProducto = string.Empty;
        public static string refreshItem = string.Empty;
        public static string refreshDepto = string.Empty;
        public static string refreshItemBalanza = string.Empty;
        public static string refreshNombreProducto = string.Empty;
        public static string[] dataProductosItem { get; set; }
        public static int correlativo { get; set; }
        public static int imageFrom { get; set; }
        public static string imagePath { get; set; }
        public static Android.Net.Uri imageData { get; set; }
        public static Bitmap thumbnail { get; set; }
        static Realm realm;

        /// <summary>
        /// Gets the realm instance.
        /// </summary>
        /// <value>The realm instance.</value>
        public static Realm RealmInstance
        {
            get
            {
                if (realm == null)
                {
                    try
                    {
                        RealmConfiguration config = new RealmConfiguration
                        {
                            SchemaVersion = 3,
                            ShouldDeleteIfMigrationNeeded = true
                        };
                        realm = Realm.GetInstance(config);
                    }
                    catch
                    {
                        realm = Realm.GetInstance();
                    }
                }
                return realm;
            }
        }
    }
}
