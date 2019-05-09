using Realms;

namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Lote parcelado producto.
    /// </summary>
    public class LoteParceladoProducto : RealmObject
    {
        public string ItemPadre { get; set; }
        public string FechaElaboracion { get; set; }
        public string LoteProduccion { get; set; }
        public string FechaDescongelado { get; set; }
        public string IdParametro { get; set; }
        public string Temperatura { get; set; }
        public string EtiquetaPropia { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string HoraCreacion { get; set; }
        public string Comentario { get; set; }
        public string CantidadUnidades { get; set; }
        public byte[] Imagen { get; set; }
        public string NombreProducto { get; set; }
        public string TiempoRestante { get; set; }
        public string NombreDpto { get; set; }
        public string FechaVencimiento { get; set; }
        public bool EstadoAlertaNotificacion { get; set; }
        public int DayOfCharge { get; set; }
        public string Correlativo { get; set; }
    }
}