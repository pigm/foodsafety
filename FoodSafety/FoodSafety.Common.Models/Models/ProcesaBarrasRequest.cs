using System.Xml.Serialization;
namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Procesa barras request.
    /// </summary>
    [XmlRoot(ElementName = "ProcesaBarrasRequest")]
    public class ProcesaBarrasRequest
    {
        [XmlElement(ElementName = "barra1")]
        public string Barra1 { get; set; }
        [XmlElement(ElementName = "barra2")]
        public string Barra2 { get; set; }
        [XmlElement(ElementName = "codigoLocal")]
        public string CodigoLocal { get; set; }
    }
}