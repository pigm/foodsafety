using System.Xml.Serialization;
using Realms;
namespace FoodSafety.Common.Models.Models
{
    /// <summary>
    /// Entidad procedencia.
    /// </summary>
    [XmlRoot(ElementName = "EntidadProcedencia")]
    public class EntidadProcedencia : RealmObject
    {
        [XmlElement(ElementName = "CodigoProcedencia")]
        public string CodigoProcedencia { get; set; }
        [XmlElement(ElementName = "NombreProcedencia")]
        public string NombreProcedencia { get; set; }
        public int DayOfCharge { get; set; }
    }
}