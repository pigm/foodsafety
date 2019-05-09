using System.Xml.Serialization;
using Realms;

namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Entidad item refresh.
    /// </summary>
    [XmlRoot(ElementName = "EntidadItemRefresh")]
    public class EntidadItemRefresh : RealmObject
    {
        [XmlElement(ElementName = "ItemNumber")]
        public string ItemNumber { get; set; }
        [XmlElement(ElementName = "OldNumber")]
        public string OldNumber { get; set; }
        [XmlElement(ElementName = "CodigoBalanza")]
        public string CodigoBalanza { get; set; }
        [XmlElement(ElementName = "Barra")]
        public string Barra { get; set; }
        [XmlElement(ElementName = "NumeroDepartamento")]
        public string NumeroDepartamento { get; set; }
        [XmlElement(ElementName = "NombreDepartamento")]
        public string NombreDepartamento { get; set; }
        [XmlElement(ElementName = "DescripcionItem1")]
        public string DescripcionItem1 { get; set; }
        [XmlElement(ElementName = "DescripcionItem2")]
        public string DescripcionItem2 { get; set; }
        [XmlElement(ElementName = "DiasPericibilidad")]
        public string DiasPericibilidad { get; set; }
        [XmlElement(ElementName = "Temperatura")]
        public string Temperatura { get; set; }
        [XmlElement(ElementName = "TemperaturaIdeal")]
        public string TemperaturaIdeal { get; set; }
        [XmlElement(ElementName = "HoraTemperaturaIdeal")]
        public string HoraTemperaturaIdeal { get; set; }

        public int DayOfCharge { get; set; }
    }
}