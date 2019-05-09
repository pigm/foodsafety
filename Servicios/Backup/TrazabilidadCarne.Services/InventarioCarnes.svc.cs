using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TrazabilidadCarne.Services.Datos;
using TrazabilidadCarne.Services;

namespace TrazabilidadCarne.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "InventarioCarnes" en el código, en svc y en el archivo de configuración a la vez.
    public class InventarioCarnes : IInventarioCarnes
    {

        public List<EntidadItemCarniceria> ObtieneItemCaniceria()
        {
            var repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            List<ItemCarniceria_Result> listaItemCarniceria = repositorioBalanza.ListaItemCarniceria();
            List<EntidadItemCarniceria> listaItem = new List<EntidadItemCarniceria>();
            foreach (ItemCarniceria_Result itemCarniceria in listaItemCarniceria)
            {
                listaItem.Add(new EntidadItemCarniceria { Item = itemCarniceria.OLD_NBR.ToString(), ItemDescripcion = itemCarniceria.ITEM1_DESC, FineLineNbr = itemCarniceria.FINELINE_NBR.ToString() });
            }
            return listaItem;
        }

        public List<EntidadMotivoInventario> ObtieneMotivoInventario()
        {
            var repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            List<Motivo_Inventario> listaMotivoInventario = repositorioBalanza.ListaMotivoInventario();
            List<EntidadMotivoInventario> motivoInventarios = new List<EntidadMotivoInventario>();
            foreach (Motivo_Inventario motivoInventario in listaMotivoInventario)
            {
                motivoInventarios.Add(new EntidadMotivoInventario { IdMotivoInventario = motivoInventario.IdMotivoInventario, Descripcion = motivoInventario.Descripcion });
            }
            return motivoInventarios;
        }

        public ResultadoProceso ProcesaBarrasTablet(decimal item, decimal local, decimal procedencia, decimal origenFrigorifico,
                                decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string responsable)
        {

            Datos.Repositorio.RepositorioBalanza repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            decimal itemVenta = item;
            Datos.Item_Padre itemPadre = repositorioBalanza.ObtieneItemPadre(itemVenta);
            if (itemPadre == null)
                return new ResultadoProceso { EstadoResutado = 2, Item = itemVenta };

            repositorioBalanza.CargaBarras(itemPadre, local, procedencia, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, "0", responsable);
            return new ResultadoProceso { EstadoResutado = 0, Item = itemPadre.OLD_NBR };
        }

        public ResultadoProceso IngresoItem(decimal itemPadre, int store, decimal item, string fechaInventario, string fechaLectura,
                                            int origenFrigorifico, decimal certificadoEmbarque, int fechaFaena, decimal pesoNeto, decimal pesoBruto,
                                            int cajas, string responsable, int idMotivonventario)
        {
            var repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            DateTime fechaInventarioDateTime = DateTime.ParseExact(fechaInventario, "yyyyMMdd", CultureInfo.InvariantCulture);
            DateTime fechaLecturaDateTime = DateTime.ParseExact(fechaLectura, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);

            int? origenFrigorificoCast = null;
            decimal? certificadoEmbarqueCast = null;
            int? fechaFaenaCast = null;
            if (idMotivonventario == 1) /* Motivo Lectura */
            {
                origenFrigorificoCast = origenFrigorifico;
                certificadoEmbarqueCast = certificadoEmbarque;
                fechaFaenaCast = fechaFaena;
            }
            repositorioBalanza.IngresaInventarioCarnes(itemPadre, store, item, fechaInventarioDateTime, fechaLecturaDateTime,
                                                       origenFrigorificoCast, certificadoEmbarqueCast, fechaFaenaCast, pesoNeto, pesoBruto,
                                                       cajas, responsable, idMotivonventario, "usr_rf");
            return new ResultadoProceso { EstadoResutado = 0, Item = itemPadre };
        }
    }
}
