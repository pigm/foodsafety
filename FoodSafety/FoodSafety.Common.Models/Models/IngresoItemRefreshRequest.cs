using System.Xml.Serialization;
using Realms;

namespace FoodSafety.Common.Models.Models
{
    [XmlRoot(ElementName = "IngresoItemRefreshRequest")]
    public class IngresoItemRefreshRequest : RealmObject
    {                
        [XmlElement(ElementName = "itemPadre")]
        public string ItemPadre { get; set; }
        [XmlElement(ElementName = "fechaElaboracion")]
        public string FechaElaboracion { get; set; }
        [XmlElement(ElementName = "loteProduccion")]
        public string LoteProduccion { get; set; }
        [XmlElement(ElementName = "fechaDescongelado")]
        public string FechaDescongelado { get; set; }
        [XmlElement(ElementName = "idParametro")]
        public string IdParametro { get; set; }
        [XmlElement(ElementName = "temperatura")]
        public string Temperatura { get; set; }
        [XmlElement(ElementName = "etiquetaPropia")]
        public string EtiquetaPropia { get; set; }
        [XmlElement(ElementName = "usuarioCreacion")]
        public string UsuarioCreacion { get; set; }
        [XmlElement(ElementName = "fechaCreacion")]
        public string FechaCreacion { get; set; }
        [XmlElement(ElementName = "horaCreacion")]
        public string HoraCreacion { get; set; }
        [XmlElement(ElementName = "comentario")]
        public string Comentario { get; set; }
        [XmlElement(ElementName = "cantidadUnidades")]
        public string CantidadUnidades { get; set; }
        [XmlElement(ElementName = "imagen")]
        public byte[] Imagen { get; set; }
        [XmlElement(ElementName = "correlativo")]
        public int Correlativo { get; set; }
        [XmlElement(ElementName = "storeNbr")]
        public int StoreNbr { get; set; }
    }
}
