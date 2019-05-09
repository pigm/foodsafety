using System.Xml.Serialization;
using Realms;
namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Entidad frigorifico.
    /// </summary>
    [XmlRoot(ElementName = "EntidadFrigorifico")]
    public class EntidadFrigorifico : RealmObject
    {
        [XmlElement(ElementName = "CodigoFrigorifico")]
        public string CodigoFrigorifico { get; set; }
        [XmlElement(ElementName = "NombreFrigorifico")]
        public string NombreFrigorifico { get; set; }
        [XmlElement(ElementName = "CodigoProcedencia")]
        public string CodigoProcedencia { get; set; }
        public int DayOfCharge { get; set; }
    }
}