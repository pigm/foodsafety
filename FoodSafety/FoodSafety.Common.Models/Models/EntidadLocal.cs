using System.Xml.Serialization;
namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Entidad local.
    /// </summary>
    [XmlRoot(ElementName = "EntidadLocal")]
    public class EntidadLocal
    {
        [XmlElement(ElementName = "CodigoLocal")]
        public string CodigoLocal { get; set; }
        [XmlElement(ElementName = "NombreLocal")]
        public string NombreLocal { get; set; }
        [XmlElement(ElementName = "CodigoFormato")]
        public string CodigoFormato { get; set; }
        [XmlElement(ElementName = "NombreFormato")]
        public string NombreFormato { get; set; }
    }
}