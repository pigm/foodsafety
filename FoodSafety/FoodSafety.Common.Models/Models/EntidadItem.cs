using System.Xml.Serialization;
using Realms;
namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Entidad item.
    /// </summary>
    [XmlRoot(ElementName = "EntidadItem")]
    public class EntidadItem : RealmObject
    {
        [XmlElement(ElementName = "Item")]
        public string Item { get; set; }
        [XmlElement(ElementName = "ItemDescripcion")]
        public string ItemDescripcion { get; set; }
        [XmlElement(ElementName = "CodigoBalanza")]
        public string CodigoBalanza { get; set; }
        public int DayOfCharge { get; set; }
    }
}