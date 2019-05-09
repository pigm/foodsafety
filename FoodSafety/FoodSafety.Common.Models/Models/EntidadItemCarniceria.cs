using System.Xml.Serialization;
using Realms;
namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Entidad item carniceria.
    /// </summary>
    [XmlRoot(ElementName = "EntidadItemCarniceria")]
    public class EntidadItemCarniceria : RealmObject
    {
        [XmlElement(ElementName = "Item")]
        public string Item { get; set; }
        [XmlElement(ElementName = "ItemDescripcion")]
        public string ItemDescripcion { get; set; }
        [XmlElement(ElementName = "FineLineNbr")]
        public string FineLineNbr { get; set; }
        public int DayOfCharge { get; set; }
    }
}