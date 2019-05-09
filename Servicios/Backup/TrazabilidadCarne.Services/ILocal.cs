using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TrazabilidadCarne.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "ILocal" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface ILocal
    {
        [OperationContract]
        [XmlSerializerFormat]
        List<EntidadLocal> ObtieneLocales();
    }

    [DataContract]
    public class EntidadLocal
    {
        [DataMember]
        public string CodigoLocal { get; set; }

        [DataMember]
        public string NombreLocal { get; set; }

        [DataMember]
        public string CodigoFormato { get; set; }

        [DataMember]
        public string NombreFormato { get; set; }
    }
}
