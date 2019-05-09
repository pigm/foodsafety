using Realms;

namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Sucursal tienda.
    /// </summary>
    public class SucursalTienda : RealmObject
    {
        public int id { get; set; }
        public string format { get; set; }
        public string name { get; set; }
        public string address { get; set; }
    }
}
