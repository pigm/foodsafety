using System.Xml.Serialization;
using Realms;
namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Entidad paridad.
    /// </summary>
    [XmlRoot(ElementName = "EntidadParidad")]
    public class EntidadParidad : RealmObject
    {
        [XmlElement(ElementName = "ItemCompra")]
        public string ItemCompra { get; set; }
        [XmlElement(ElementName = "ItemVenta")]
        public string ItemVenta { get; set; }

        public int DayOfCharge { get; set; }
    }
}