using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;
using System.Web.Services;

namespace TrazabilidadCarne.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IInventarioCarnes" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IInventarioCarnes
    {
        [OperationContract]
        [XmlSerializerFormat]
        List<EntidadItemCarniceria> ObtieneItemCaniceria();

        [OperationContract]
        [XmlSerializerFormat]
        List<EntidadMotivoInventario> ObtieneMotivoInventario();

        [OperationContract]
        [XmlSerializerFormat]
        ResultadoProceso IngresoItem(decimal itemPadre, int store, decimal item, string fechaInventario, string fechaLectura,
                                     int origenFrigorifico, decimal certificadoEmbarque, int fechaFaena, decimal pesoNeto, decimal pesoBruto,
                                     int cajas, string responsable, int idMotivonventario);

        [OperationContract]
        [XmlSerializerFormat]
        ResultadoProceso ProcesaBarrasTablet(decimal item, decimal local, decimal procedencia, decimal origenFrigorifico,
                                decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string responsable);

        [OperationContract]
        [XmlSerializerFormat]
        ResultadoProceso ProcesaBarrasImgTablet(decimal item, decimal local, decimal procedencia, decimal origenFrigorifico,
                                decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string responsable,
            byte[] image);
    }


    [DataContract]
    public class EntidadItemCarniceria
    {
        [DataMember]
        public string Item { get; set; }

        [DataMember]
        public string ItemDescripcion { get; set; }

        [DataMember]
        public string FineLineNbr { get; set; }
    }

    [DataContract]
    public class EntidadMotivoInventario
    {
        [DataMember]
        public int IdMotivoInventario { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
    }

}
