using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TrazabilidadCarne.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IBarra" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IBarra
    {
        [OperationContract]
        [XmlSerializerFormat]
        ResultadoProceso ProcesaBarras(string barra1, string barra2, string codigoLocal);

        [OperationContract]
        [XmlSerializerFormat]
        List<EntidadItem> ObtieneItemBalanza();

        [OperationContract]
        [XmlSerializerFormat]
        List<EntidadProcedencia> ObtieneProcedencia();


        [OperationContract]
        [XmlSerializerFormat]
        List<EntidadFrigorifico> ObtieneFrigorificos();

        [OperationContract]
        [XmlSerializerFormat]
        List<EntidadParidad> ObtieneParidades();

        [OperationContract]
        [XmlSerializerFormat]
        ResultadoProceso ProcesaBarrasMSG(string Mensaje, string codigoLocal);

    }


    [DataContract]
    public class ResultadoProceso
    {
        [DataMember]
        public int EstadoResutado { get; set; }
        [DataMember]
        public decimal Item { get; set; }
    }



    [DataContract]
    public class EntidadItem
    {
        [DataMember]
        public string Item { get; set; }

        [DataMember]
        public string ItemDescripcion { get; set; }

        [DataMember]
        public string CodigoBalanza { get; set; }
    }

    [DataContract]
    public class EntidadProcedencia
    {
        [DataMember]
        public string CodigoProcedencia { get; set; }

        [DataMember]
        public string NombreProcedencia { get; set; }
    }


    [DataContract]
    public class EntidadFrigorifico
    {
        [DataMember]
        public string CodigoFrigorifico { get; set; }

        [DataMember]
        public string NombreFrigorifico { get; set; }

        [DataMember]
        public string CodigoProcedencia { get; set; }


    }

    [DataContract]
    public class EntidadParidad
    {
        [DataMember]
        public decimal ItemCompra { get; set; }

        [DataMember]
        public decimal ItemVenta { get; set; }
    }

}
