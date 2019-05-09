using System.Xml.Serialization;
namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Ingreso item result.
    /// </summary>
    [XmlRoot(ElementName = "IngresoItemResult")]
    public class IngresoItemResult
    {
        [XmlElement(ElementName = "EstadoResutado")]
        public string EstadoResutado { get; set; }
        [XmlElement(ElementName = "Item")]
        public string Item { get; set; }
    }
}