using System.Xml.Serialization;
using Realms;

namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Ingreso item request.
    /// </summary>
    [XmlRoot(ElementName = "IngresoItemRequest")]
    public class IngresoItemRequest : RealmObject
    {
        [XmlElement(ElementName = "itemPadre")]
        public string ItemPadre { get; set; }
        [XmlElement(ElementName = "store")]
        public string Store { get; set; }
        [XmlElement(ElementName = "item")]
        public string Item { get; set; }
        [XmlElement(ElementName = "fechaInventario")]
        public string FechaInventario { get; set; }
        [XmlElement(ElementName = "fechaLectura")]
        public string FechaLectura { get; set; }
        [XmlElement(ElementName = "origenFrigorifico")]
        public string OrigenFrigorifico { get; set; }
        [XmlElement(ElementName = "certificadoEmbarque")]
        public string CertificadoEmbarque { get; set; }
        [XmlElement(ElementName = "fechaFaena")]
        public string FechaFaena { get; set; }
        [XmlElement(ElementName = "pesoNeto")]
        public string PesoNeto { get; set; }
        [XmlElement(ElementName = "pesoBruto")]
        public string PesoBruto { get; set; }
        [XmlElement(ElementName = "cajas")]
        public string Cajas { get; set; }
        [XmlElement(ElementName = "responsable")]
        public string Responsable { get; set; }
        [XmlElement(ElementName = "idMotivonventario")]
        public string IdMotivonventario { get; set; }
        [XmlElement(ElementName = "procedencia")]
        public string Procedencia { get; set; }
        [XmlElement(ElementName = "image")]
        public string Image { get; set; }

    }
}