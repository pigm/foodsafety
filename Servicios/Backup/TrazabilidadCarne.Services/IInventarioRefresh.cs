using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TrazabilidadCarne.Services
{
    [ServiceContract]
    public interface IInventarioRefresh
    {
        [OperationContract]
        [XmlSerializerFormat]
        List<EntidadItemRefresh> ObtieneItemRefresh();

        [OperationContract]
        [XmlSerializerFormat]
        ResultadoProceso IngresoItemRefresh(decimal itemPadre, string fechaElaboracion, string loteProduccion, string fechaDescongelado,
                                     int idParametro, decimal temperatura, string etiquetaPropia, string usuarioCreacion, decimal fechaCreacion,
                                     decimal horaCreacion, string comentario, int cantidadUnidades);
    }

    [DataContract]
    public class EntidadItemRefresh
    {
        [DataMember]
        public decimal? ItemNumber { get; set; }

        [DataMember]
        public decimal? OldNumber { get; set; }

        [DataMember]
        public decimal? CodigoBalanza { get; set; }

        [DataMember]
        public decimal? Barra { get; set; }

        [DataMember]
        public decimal? NumeroDepartamento { get; set; }

        [DataMember]
        public string NombreDepartamento { get; set; }

        [DataMember]
        public string DescripcionItem1 { get; set; }

        [DataMember]
        public string DescripcionItem2 { get; set; }

        [DataMember]
        public Int32 DiasPericibilidad { get; set; }

        [DataMember]
        public Int32 Temperatura { get; set; }

        [DataMember]
        public string Comentario { get; set; }

        [DataMember]
        public Int32 CantidadUnidades { get; set; }
    }
}
