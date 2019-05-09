using System;
using System.Xml.Serialization;
using Realms;

namespace FoodSafety.Common.Models.Models
{
    [XmlRoot(ElementName = "IngresaProcesaBarrasTabletRequest")]
    public class IngresaProcesaBarrasTabletRequest : RealmObject
    {
        [XmlElement(ElementName = "item")]
        public string Item { get; set; }
        [XmlElement(ElementName = "local")]
        public string Local { get; set; }
        [XmlElement(ElementName = "procedencia")]
        public string Procedencia { get; set; }
        [XmlElement(ElementName = "origenFrigorifico")]
        public string OrigenFrigorifico { get; set; }
        [XmlElement(ElementName = "certificadoEmbarque")]
        public string CertificadoEmbarque { get; set; }
        [XmlElement(ElementName = "fechaFaena")]
        public string FechaFaena { get; set; }
        [XmlElement(ElementName = "pesaNeto")]
        public string PesaNeto { get; set; }
        [XmlElement(ElementName = "pesaBruto")]
        public string PesaBruto { get; set; }
        [XmlElement(ElementName = "responsable")]
        public string Responsable { get; set; }
        [XmlElement(ElementName = "image")]
        //public string Image { get; set; }
        public byte[] Image { get; set; }
    }
}
