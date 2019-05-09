using System.Xml.Serialization;
namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Procesa barras result.
    /// </summary>
    [XmlRoot(ElementName = "ProcesaBarrasResult")]
    public class ProcesaBarrasResult
    {
        [XmlElement(ElementName = "EstadoResutado")]
        public string EstadoResutado { get; set; }
        [XmlElement(ElementName = "Item")]
        public string Item { get; set; }
    }
}