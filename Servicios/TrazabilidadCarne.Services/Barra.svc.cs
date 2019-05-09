using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TrazabilidadCarne.Services.Datos;
using System.ServiceModel.Activation;
using TrazabilidadCarne.Services;

namespace TrazabilidadCarne.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Barra" en el código, en svc y en el archivo de configuración a la vez.
    [ServiceErrorBehavior(typeof(ErrorHandler))]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Barra : IBarra
    {
        public ResultadoProceso ProcesaBarras(string barra1, string barra2, string codigoLocal)
        {
            decimal stroreNBR = decimal.Parse(codigoLocal);
            decimal itemNumber = decimal.Parse(barra1.Substring(3, 6));
            string origen = barra1.Substring(12, 3);
            decimal origenFrigorifico = decimal.Parse(barra1.Substring(18, 3));
            decimal certificadoEmbarque = 0;
            if (barra1.Length == 40)
                certificadoEmbarque = decimal.Parse(barra1.Substring(23, 17));
            if (barra1.Length == 41)
                certificadoEmbarque = decimal.Parse(barra1.Substring(23, 18));

            string fechaFaena = barra2.Substring(2, 6);
            decimal pesaNeto = decimal.Parse(barra2.Substring(12, 6));
            decimal pesaBruto = decimal.Parse(barra2.Substring(22, 6));
            string barraLarga = string.Concat(barra1, barra2);
            barraLarga = string.Empty;
            string responsable = string.Empty;


            Datos.Repositorio.RepositorioBalanza repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();

            Datos.Paridad paridad = repositorioBalanza.ObtieneParidad(itemNumber);
            if (paridad == null)
                return new ResultadoProceso { EstadoResutado = 1 };

            decimal itemVenta = paridad.Item_Venta;
            Datos.Item_Padre itemPadre = repositorioBalanza.ObtieneItemPadre(itemVenta);
            if (itemPadre == null)
                return new ResultadoProceso { EstadoResutado = 2 };

            repositorioBalanza.CargaBarras(itemPadre, stroreNBR, decimal.Parse(origen), origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, responsable);

            return new ResultadoProceso { EstadoResutado = 0 };
        }


        public ResultadoProceso ProcesaBarrasMSG(string msg, string codigoLocal)
        {
            //msg = "156551|1|004|002|00000001234123412|130805|001850|001945|24115655142200425100200000000012341234121113080531020018503302001945";
            //msg = "285197|2|002|2|0|131125|123456|0|";

            //codigoLocal = "201";

            decimal stroreNBR = decimal.Parse(codigoLocal);
            string[] mensaje = msg.Split('|');
            //if ((mensaje.Length != 9) || (mensaje.Length != 10))
            /*if (mensaje.Length != 10)
                throw new InvalidOperationException("Valide el formato y largo del msg.");*/
            decimal itemNumber = Convert.ToDecimal(mensaje[0]);
            int tipoItem = int.Parse(mensaje[1].ToString()); // 1: Item Compra     2: Item Venta
            string origen = mensaje[2].ToString();
            decimal origenFrigorifico = decimal.Parse(mensaje[3].ToString());
            string certificadoEmbarqueTexto = mensaje[4].ToString();
            decimal certificadoEmbarque = 0;
            if (!string.IsNullOrEmpty(certificadoEmbarqueTexto.Trim()))
                certificadoEmbarque = decimal.Parse(certificadoEmbarqueTexto.Trim());
            string fechaFaena = mensaje[5].ToString();
            decimal pesaNeto = decimal.Parse(mensaje[6]);
            decimal pesaBruto = decimal.Parse(mensaje[7]);
            string barraLarga = mensaje[8];
            string responsable = string.Empty;
            if (mensaje.Length == 10)
                responsable = mensaje[9];

            Datos.Repositorio.RepositorioBalanza repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            decimal itemVenta = itemNumber;
            if (tipoItem == 1)
            {
                Datos.Paridad paridad = repositorioBalanza.ObtieneParidad(itemNumber);
                if (paridad == null)
                    return new ResultadoProceso { EstadoResutado = 1, Item = itemNumber };
                itemVenta = paridad.Item_Venta;
            }
            Datos.Item_Padre itemPadre = repositorioBalanza.ObtieneItemPadre(itemVenta);
            if (itemPadre == null)
                return new ResultadoProceso { EstadoResutado = 2, Item = itemVenta };

            repositorioBalanza.CargaBarras(itemPadre, stroreNBR, decimal.Parse(origen), origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, responsable);
            return new ResultadoProceso { EstadoResutado = 0, Item = itemPadre.OLD_NBR };
        }

        public List<EntidadItem> ObtieneItemBalanza()
        {
            Datos.Repositorio.RepositorioBalanza repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            List<ITEM_BALANZA> listaItemBalanza = repositorioBalanza.ListaItem();
            List<EntidadItem> listaItem = new List<EntidadItem>();
            foreach (ITEM_BALANZA ItemBalanza in listaItemBalanza)
            {
                listaItem.Add(new EntidadItem { Item = ItemBalanza.OLD_NBR.ToString(), ItemDescripcion = ItemBalanza.ITEM1_DESC, CodigoBalanza = ItemBalanza.PLU_NBR.ToString() });
            }
            return listaItem;
        }

        public List<EntidadProcedencia> ObtieneProcedencia()
        {
            Datos.Repositorio.RepositorioBalanza repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            List<Procedencia> listaFrigorificoProcedencia = repositorioBalanza.ListaProcedencias();
            List<EntidadProcedencia> listadProcedencia = new List<EntidadProcedencia>();
            string formatCodigoProcedencia = "0";
            foreach (Procedencia Procedencia in listaFrigorificoProcedencia)
            {
                formatCodigoProcedencia = Procedencia.ORIGEN.Trim();
                formatCodigoProcedencia = string.Format("{0:000}", Convert.ToInt32(formatCodigoProcedencia));

                listadProcedencia.Add(new EntidadProcedencia { CodigoProcedencia = formatCodigoProcedencia, NombreProcedencia = Procedencia.DESORI });
            }
            return listadProcedencia;
        }

        public List<EntidadFrigorifico> ObtieneFrigorificos()
        {
            Datos.Repositorio.RepositorioBalanza repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            List<Procedencia_Proveedor> listaFrigorificoProcedencia = repositorioBalanza.ListaFrigorificos();
            List<EntidadFrigorifico> listadProcedenciaProveedor = new List<EntidadFrigorifico>();

            string formatCodigoProcedencia = "0";
            foreach (Procedencia_Proveedor FrigorificoProcedencia in listaFrigorificoProcedencia)
            {
                formatCodigoProcedencia = FrigorificoProcedencia.ORIGEN.Trim();
                formatCodigoProcedencia = string.Format("{0:000}", Convert.ToInt32(formatCodigoProcedencia));

                listadProcedenciaProveedor.Add(new EntidadFrigorifico { CodigoFrigorifico = FrigorificoProcedencia.Origen_Frigorifico.ToString(), NombreFrigorifico = FrigorificoProcedencia.RAZSOC30.Trim(), CodigoProcedencia = formatCodigoProcedencia });

            }
            return listadProcedenciaProveedor;
        }



        public List<EntidadParidad> ObtieneParidades()
        {
            Datos.Repositorio.RepositorioBalanza repositorioBalanza = new Datos.Repositorio.RepositorioBalanza();
            List<Paridad> listaParidad = repositorioBalanza.ListaParidades();
            List<EntidadParidad> listaEntidadParidad = new List<EntidadParidad>();
            foreach (Paridad paridad in listaParidad)
            {
                listaEntidadParidad.Add(new EntidadParidad { ItemCompra = paridad.Item_Compra, ItemVenta = paridad.Item_Venta });
            }
            return listaEntidadParidad;
        }
    }
}
