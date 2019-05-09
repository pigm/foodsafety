using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TrazabilidadCarne.Services;
using TrazabilidadCarne.Services.Datos;
using System.Globalization;

namespace TrazabilidadCarne.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "InventarioRefresh" en el código, en svc y en el archivo de configuración a la vez.
    public class InventarioRefresh : IInventarioRefresh
    {
        /// <summary>
        /// Ingresar item refresh
        /// </summary>
        /// <param name="itemPadre"></param>
        /// <param name="fechaElaboracion"></param>
        /// <param name="loteProduccion"></param>
        /// <param name="fechaDescongelado"></param>
        /// <param name="idParametro"></param>
        /// <param name="temperatura"></param>
        /// <param name="etiquetaPropia"></param>
        /// <param name="usuarioCreacion"></param>
        /// <param name="fechaCreacion"></param>
        /// <param name="horaCreacion"></param>
        /// <returns></returns>
        public ResultadoProceso IngresoItemRefresh(decimal itemPadre, string fechaElaboracion, string loteProduccion, 
            string fechaDescongelado, int idParametro, decimal temperatura, string etiquetaPropia, string usuarioCreacion,
            decimal fechaCreacion, decimal horaCreacion, string comentario, int cantidadUnidades)
        {
            var repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            DateTime fechaElaboracionDateTime = DateTime.ParseExact(fechaElaboracion, "yyyyMMdd", CultureInfo.InvariantCulture);
            DateTime fechaDescongeladoDateTime = DateTime.ParseExact(fechaDescongelado, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);

            repositorioBalanza.IngresoInventarioRefresh(itemPadre, fechaElaboracionDateTime, loteProduccion,
                fechaDescongeladoDateTime, idParametro, temperatura, etiquetaPropia, usuarioCreacion, fechaCreacion, horaCreacion, comentario, cantidadUnidades);
            return new ResultadoProceso { EstadoResutado = 0, Item = itemPadre };
        }

        /// <summary>
        /// Obtiene lista de item de refresh
        /// </summary>
        /// <returns></returns>
        public List<EntidadItemRefresh> ObtieneItemRefresh()
        {
            var repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            List<Lista_ItemRefresh_Result> listaItemRefreshRepositorio = repositorioBalanza.ListaItemRefresh();
            List<EntidadItemRefresh> listaItemRefresh = new List<EntidadItemRefresh>();
            foreach (Lista_ItemRefresh_Result itemRefresh in listaItemRefreshRepositorio)
            {
                listaItemRefresh.Add(new EntidadItemRefresh { 
                    ItemNumber = itemRefresh.ItemNumber,
                    OldNumber = itemRefresh.OldNumber,
                    CodigoBalanza = itemRefresh.CodigoBalanza,
                    Barra = itemRefresh.Barra,
                    NumeroDepartamento = itemRefresh.NumeroDepartamento,
                    NombreDepartamento = itemRefresh.NombreDepartamento,
                    DescripcionItem1 = itemRefresh.DescripcionItem1,
                    DescripcionItem2 = itemRefresh.DescripcionItem2,
                    DiasPericibilidad = itemRefresh.DiasPericibilidad,
                    Temperatura = itemRefresh.Temperatura
                });
            }
            return listaItemRefresh;
        }
    }
}
