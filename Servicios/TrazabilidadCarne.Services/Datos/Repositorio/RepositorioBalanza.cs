using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrazabilidadCarne.Services.Datos.Repositorio
{
    /// <summary>
    /// Repositorio de BD de balanza
    /// </summary>
    public class RepositorioBalanza
    {
        private TrazabilidadCarne.Services.Datos.BalanzasContenedor _Contenedor = new TrazabilidadCarne.Services.Datos.BalanzasContenedor();

        #region Public

        /// <summary>
        /// Obtiene paridad
        /// </summary>
        /// <param name="itemCompra"></param>
        /// <returns></returns>
        public Datos.Paridad ObtieneParidad(decimal itemCompra)
        {
            return _Contenedor.Paridads.SingleOrDefault(par => par.Item_Compra == itemCompra);
        }

        /// <summary>
        /// Obtiene item padre
        /// </summary>
        /// <param name="itemVenta"></param>
        /// <returns></returns>
        public Datos.Item_Padre ObtieneItemPadre(decimal itemVenta)
        {
            return _Contenedor.Item_Padre.SingleOrDefault(pdr => pdr.OLD_NBR == itemVenta);
        }

        /// <summary>
        /// obtiene carga barra
        /// </summary>
        /// <param name="itemBarras"></param>
        /// <param name="idLocal"></param>
        /// <returns></returns>
        public Datos.WM_Barra_Carga ObtieneBarraCarga(string itemBarras, decimal idLocal)
        {
            return _Contenedor.WM_Barra_Carga.SingleOrDefault(bcar => bcar.Item_Barras == itemBarras && bcar.STORE_NBR == idLocal);
        }

        /// <summary>
        /// ObtieneInfArtLocalSesma
        /// </summary>
        /// <param name="itemBarras"></param>
        /// <param name="idLocal"></param>
        /// <returns></returns>
        public Datos.Informacion_Art_Local_Sesma ObtieneInfArtLocalSesma(decimal itemBarras, decimal idLocal)
        {
            return _Contenedor.Informacion_Art_Local_Sesma.FirstOrDefault(als => als.STORE_NBR == idLocal && als.OLD_NBR == itemBarras);
        }

        /// <summary>
        /// CargaBarras
        /// </summary>
        /// <param name="itemPadre"></param>
        /// <param name="storeNBR"></param>
        /// <param name="origen"></param>
        /// <param name="origenFrigorifico"></param>
        /// <param name="certificadoEmbarque"></param>
        /// <param name="fechaFaena"></param>
        /// <param name="pesaNeto"></param>
        /// <param name="pesaBruto"></param>
        /// <param name="barraLarga"></param>
        /// <param name="responsable"></param>
        public void CargaBarras(Datos.Item_Padre itemPadre, decimal storeNBR, decimal origen, decimal origenFrigorifico,
                                decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string barraLarga, string responsable)
        {
            Datos.WM_Barra_Carga barraCarga;
            Datos.WM_Barra_Carga barraCargaPadre = ObtieneBarraCarga(itemPadre.OLD_NBR.ToString(), storeNBR);
            if (barraCargaPadre == null)
            {
                barraCarga = crearBarraCarga(itemPadre.OLD_NBR.ToString(), storeNBR, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga);
                _Contenedor.WM_Barra_Carga.AddObject(barraCarga);

            }
            else
            {
                ActualizaBarraCarga(barraCargaPadre, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga);
                _Contenedor.WM_Barra_Carga.ApplyCurrentValues(barraCargaPadre);
            }
            Datos.WM_Barra_Carga_Bitacora barraCargaBitacora = crearBarraCargaBitacora(itemPadre.OLD_NBR.ToString(), storeNBR, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, responsable);
            _Contenedor.WM_Barra_Carga_Bitacora.AddObject(barraCargaBitacora);

            if (itemPadre.Item_Hijo != null && itemPadre.Item_Hijo.Count > 0)
            {
                foreach (Datos.Item_Hijo itemHijo in itemPadre.Item_Hijo)
                {
                    Datos.WM_Barra_Carga barraCargaHijo = ObtieneBarraCarga(itemHijo.OLD_NBR.ToString(), storeNBR);
                    if (barraCargaHijo == null)
                    {
                        barraCarga = crearBarraCarga(itemHijo.OLD_NBR.ToString(), storeNBR, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga);
                        _Contenedor.WM_Barra_Carga.AddObject(barraCarga);
                    }
                    else
                    {
                        ActualizaBarraCarga(barraCargaHijo, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga);
                        _Contenedor.WM_Barra_Carga.ApplyCurrentValues(barraCargaHijo);
                    }
                    barraCargaBitacora = crearBarraCargaBitacora(itemHijo.OLD_NBR.ToString(), storeNBR, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, responsable);
                    _Contenedor.WM_Barra_Carga_Bitacora.AddObject(barraCargaBitacora);
                }
            }
            _Contenedor.SaveChanges();

            string cadenaFechaFaena = DevuelveFechaFaena(fechaFaena);
            _Contenedor.Actualiza_ArtLocalSesma(storeNBR, itemPadre.OLD_NBR, origen.ToString(), cadenaFechaFaena, "N", "usr_rf", decimal.Parse(DateTime.Now.ToString("yyyyMMdd")), decimal.Parse(DateTime.Now.ToString("HHmmss")));
            _Contenedor.Actualiza_ArticuloProveedor(storeNBR, itemPadre.OLD_NBR, origen.ToString(), origenFrigorifico, cadenaFechaFaena, "usr_rf", decimal.Parse(DateTime.Now.ToString("yyyyMMdd")), decimal.Parse(DateTime.Now.ToString("HHmmss")));
        }

        /// <summary>
        /// Carga Barras con imagen de referencia
        /// </summary>
        /// <param name="itemPadre"></param>
        /// <param name="storeNBR"></param>
        /// <param name="origen"></param>
        /// <param name="origenFrigorifico"></param>
        /// <param name="certificadoEmbarque"></param>
        /// <param name="fechaFaena"></param>
        /// <param name="pesaNeto"></param>
        /// <param name="pesaBruto"></param>
        /// <param name="barraLarga"></param>
        /// <param name="responsable"></param>
        /// <param name="image"></param>
        public void CargaBarrasImg(Datos.Item_Padre itemPadre, decimal storeNBR, decimal origen, decimal origenFrigorifico,
                                decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string barraLarga, string responsable, byte[] image)
        {
            Datos.WM_Barra_Carga barraCarga;
            Datos.WM_Barra_Carga barraCargaPadre = ObtieneBarraCarga(itemPadre.OLD_NBR.ToString(), storeNBR);
            if (barraCargaPadre == null)
            {
                barraCarga = crearBarraCargaImg(itemPadre.OLD_NBR.ToString(), storeNBR, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, image, responsable);
                _Contenedor.WM_Barra_Carga.AddObject(barraCarga);

            }
            else
            {
                ActualizaBarraCargaImg(barraCargaPadre, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, image, responsable);
                _Contenedor.WM_Barra_Carga.ApplyCurrentValues(barraCargaPadre);
            }
            Datos.WM_Barra_Carga_Bitacora barraCargaBitacora = crearBarraCargaBitacoraImg(itemPadre.OLD_NBR.ToString(), storeNBR, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, responsable, image);
            _Contenedor.WM_Barra_Carga_Bitacora.AddObject(barraCargaBitacora);

            if (itemPadre.Item_Hijo != null && itemPadre.Item_Hijo.Count > 0)
            {
                foreach (Datos.Item_Hijo itemHijo in itemPadre.Item_Hijo)
                {
                    Datos.WM_Barra_Carga barraCargaHijo = ObtieneBarraCarga(itemHijo.OLD_NBR.ToString(), storeNBR);
                    if (barraCargaHijo == null)
                    {
                        barraCarga = crearBarraCargaImg(itemHijo.OLD_NBR.ToString(), storeNBR, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, image, responsable);
                        _Contenedor.WM_Barra_Carga.AddObject(barraCarga);
                    }
                    else
                    {
                        ActualizaBarraCargaImg(barraCargaHijo, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, image, responsable);
                        _Contenedor.WM_Barra_Carga.ApplyCurrentValues(barraCargaHijo);
                    }
                    barraCargaBitacora = crearBarraCargaBitacora(itemHijo.OLD_NBR.ToString(), storeNBR, origen, origenFrigorifico, certificadoEmbarque, fechaFaena, pesaNeto, pesaBruto, barraLarga, responsable);
                    _Contenedor.WM_Barra_Carga_Bitacora.AddObject(barraCargaBitacora);
                }
            }
            _Contenedor.SaveChanges();

            string cadenaFechaFaena = DevuelveFechaFaena(fechaFaena);
            _Contenedor.Actualiza_ArtLocalSesma(storeNBR, itemPadre.OLD_NBR, origen.ToString(), cadenaFechaFaena, "N", "usr_rf", decimal.Parse(DateTime.Now.ToString("yyyyMMdd")), decimal.Parse(DateTime.Now.ToString("HHmmss")));
            _Contenedor.Actualiza_ArticuloProveedor(storeNBR, itemPadre.OLD_NBR, origen.ToString(), origenFrigorifico, cadenaFechaFaena, "usr_rf", decimal.Parse(DateTime.Now.ToString("yyyyMMdd")), decimal.Parse(DateTime.Now.ToString("HHmmss")));
        }

        /// <summary>
        /// IngresaInventarioCarnes
        /// </summary>
        /// <param name="itemPadre"></param>
        /// <param name="store"></param>
        /// <param name="item"></param>
        /// <param name="fechaInventario"></param>
        /// <param name="fechaLectura"></param>
        /// <param name="origenFrigorifico"></param>
        /// <param name="certificadoEmbarque"></param>
        /// <param name="fechaFaena"></param>
        /// <param name="pesoNeto"></param>
        /// <param name="pesoBruto"></param>
        /// <param name="cajas"></param>
        /// <param name="responsable"></param>
        /// <param name="idMotivonventario"></param>
        /// <param name="usuario"></param>
        public void IngresaInventarioCarnes(decimal itemPadre, int store, decimal item, DateTime fechaInventario, DateTime fechaLectura,
                                            int? origenFrigorifico, decimal? certificadoEmbarque, int? fechaFaena, decimal pesoNeto, decimal pesoBruto,
                                            int cajas, string responsable, int idMotivonventario, string usuario)
        {
            var inventarioCarnes = new Inventario_Carnes();
            inventarioCarnes.Item_Padre = itemPadre;
            inventarioCarnes.STORE_NBR = store;
            inventarioCarnes.Item = item;
            inventarioCarnes.Fecha_Inventario = fechaInventario;
            inventarioCarnes.Fecha_Lectura = fechaLectura;
            inventarioCarnes.Origen_Frigorifico = origenFrigorifico;
            inventarioCarnes.Certificado_Embarque = certificadoEmbarque;
            inventarioCarnes.Fecha_Faena = fechaFaena;
            inventarioCarnes.Peso_Neto = pesoNeto;
            inventarioCarnes.Peso_Bruto = pesoBruto;
            inventarioCarnes.Cajas = cajas;
            inventarioCarnes.Responsable = responsable.Length > 10 ? responsable.Substring(0,10) : responsable;
            inventarioCarnes.IdMotivoInventario = idMotivonventario;
            inventarioCarnes.USUCRE = usuario;
            inventarioCarnes.FECCRE = decimal.Parse(DateTime.Now.ToString("yyyyMMdd"));
            inventarioCarnes.HORCRE = decimal.Parse(DateTime.Now.ToString("HHmmss"));
            var motivo = _Contenedor.Motivo_Inventario.Where(x => x.IdMotivoInventario == idMotivonventario).FirstOrDefault();
            inventarioCarnes.Motivo_Inventario = motivo;
            _Contenedor.Inventario_Carnes.AddObject(inventarioCarnes);
            _Contenedor.SaveChanges();
        }

        /// <summary>
        /// Lista items balanza
        /// </summary>
        /// <returns></returns>
        public List<ITEM_BALANZA> ListaItem()
        {
            return _Contenedor.ITEM_BALANZA.ToList();
        }

        /// <summary>
        /// Lista items carniceria
        /// </summary>
        /// <returns></returns>
        public List<ItemCarniceria_Result> ListaItemCarniceria()
        {
            return _Contenedor.Lista_ItemCarniceria().ToList();
        }

        /// <summary>
        /// Lista Items Refresh
        /// </summary>
        /// <returns></returns>
        public List<Lista_ItemRefresh_Result> ListaItemRefresh()
        {
            return _Contenedor.Lista_ItemRefresh().ToList();
        }

        /// <summary>
        /// Lista procedencias
        /// </summary>
        /// <returns></returns>
        public List<Procedencia> ListaProcedencias()
        {
            return _Contenedor.Procedencia.ToList();
        }

        /// <summary>
        /// Lista frigorificos
        /// </summary>
        /// <returns></returns>
        public List<Procedencia_Proveedor> ListaFrigorificos()
        {
            return _Contenedor.Procedencia_Proveedor.ToList();
        }

        /// <summary>
        /// Lista paridades
        /// </summary>
        /// <returns></returns>
        public List<Paridad> ListaParidades()
        {
            return _Contenedor.Paridads.ToList();
        }

        /// <summary>
        /// Lista motivo de inventario
        /// </summary>
        /// <returns></returns>
        public List<Motivo_Inventario> ListaMotivoInventario()
        {
            return _Contenedor.Motivo_Inventario.ToList();
        }

        /// <summary>
        /// ObtieneInfArtProveedor
        /// </summary>
        /// <param name="itemBarras"></param>
        /// <param name="idLocal"></param>
        /// <returns></returns>
        public Datos.Informacion_Articulo_Prov ObtieneInfArtProveedor(decimal itemBarras, decimal idLocal)
        {
            return _Contenedor.Informacion_Articulo_Prov.SingleOrDefault(apr => apr.STORE_NBR == idLocal && apr.OLD_NBR == itemBarras);
        }


        /// <summary>
        /// Ingresa item inventario refresh
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
        public void IngresoInventarioRefresh(decimal itemPadre, DateTime fechaElaboracion, string loteProduccion, DateTime fechaDescongelado,
            int idParametro, decimal temperatura, string etiquetaPropia, string usuarioCreacion, decimal fechaCreacion, decimal horaCreacion, string comentario, int cantidadUnidades, byte[] imagen, int correlativo)
        {
           /* var itemexistente = _Contenedor.Informacion_Refresh.Where(inf => inf.OLD_NBR == itemPadre).FirstOrDefault();

            if (itemexistente != null)
            {
                itemexistente.ETIQ_PROPIA = etiquetaPropia;
                itemexistente.FECACT = fechaCreacion;
                itemexistente.FECHA_ELABORACION = fechaElaboracion;
                itemexistente.FECHADESCONGELADO = fechaDescongelado;
                itemexistente.HORACT = horaCreacion;
                itemexistente.ID_PARAMETRO = idParametro;
                itemexistente.LOTEPRODUCCION = loteProduccion;
                itemexistente.OLD_NBR = itemPadre;
                itemexistente.TEMPERATURA = temperatura;
                itemexistente.USUACT = usuarioCreacion;
                itemexistente.COMENTARIO = comentario;
                itemexistente.CANTIDAD_UNIDADES = cantidadUnidades;
                itemexistente.FOTOETIQUETA = imagen;
                itemexistente.CORRELATIVO = correlativo;
                _Contenedor.Informacion_Refresh.ApplyCurrentValues(itemexistente);
            }
            else
            {*/
                var informacion_refresh = new Informacion_Refresh();
                informacion_refresh.ETIQ_PROPIA = etiquetaPropia;
                informacion_refresh.FECACT = fechaCreacion;
                informacion_refresh.FECCRE = fechaCreacion;
                informacion_refresh.FECHA_ELABORACION = fechaElaboracion;
                informacion_refresh.FECHADESCONGELADO = fechaDescongelado;
                informacion_refresh.HORACT = horaCreacion;
                informacion_refresh.HORCRE = horaCreacion;
                informacion_refresh.ID_PARAMETRO = idParametro;
                informacion_refresh.LOTEPRODUCCION = loteProduccion;
                informacion_refresh.OLD_NBR = itemPadre;
                informacion_refresh.TEMPERATURA = temperatura;
                informacion_refresh.USUACT = usuarioCreacion;
                informacion_refresh.USUCRE = usuarioCreacion;
                informacion_refresh.COMENTARIO = comentario;
                informacion_refresh.CANTIDAD_UNIDADES = cantidadUnidades;
                informacion_refresh.FOTOETIQUETA = imagen;
                informacion_refresh.CORRELATIVO = correlativo;
                _Contenedor.Informacion_Refresh.AddObject(informacion_refresh);
            //}
            _Contenedor.SaveChanges();
        }

        #endregion

        #region Private

        /// <summary>
        /// crearBarraCarga
        /// </summary>
        /// <param name="item"></param>
        /// <param name="storeNBR"></param>
        /// <param name="origen"></param>
        /// <param name="origenFrigorifico"></param>
        /// <param name="certificadoEmbarque"></param>
        /// <param name="fechaFaena"></param>
        /// <param name="pesaNeto"></param>
        /// <param name="pesaBruto"></param>
        /// <param name="barraLarga"></param>
        /// <returns></returns>
        private Datos.WM_Barra_Carga crearBarraCarga(string item, decimal storeNBR, decimal origen, decimal origenFrigorifico, decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string barraLarga)
        {
            Datos.WM_Barra_Carga barraCarga = new WM_Barra_Carga();
            barraCarga.Item_Barras = item;
            barraCarga.STORE_NBR = storeNBR;
            barraCarga.ORIGEN = origen.ToString();
            barraCarga.Origen_Frigorifico = origenFrigorifico;
            barraCarga.Certificado_Embarque = certificadoEmbarque;
            barraCarga.Fecha_Faena = decimal.Parse(fechaFaena);
            barraCarga.Peso_Neto = pesaNeto;
            barraCarga.Peso_Bruto = pesaBruto;
            barraCarga.Barra_Larga = barraLarga;
            barraCarga.INDPROC = "N";
            barraCarga.USUCRE = "usr_rf";
            barraCarga.FECCRE = decimal.Parse(DateTime.Now.ToString("yyyyMMdd"));
            barraCarga.HORCRE = decimal.Parse(DateTime.Now.ToString("HHmmss"));
            barraCarga.USUACT = string.Empty;
            barraCarga.FECACT = 0;
            barraCarga.HORACT = 0;
            return barraCarga;
        }

        /// <summary>
        /// ActualizaBarraCarga
        /// </summary>
        /// <param name="barraCarga"></param>
        /// <param name="origen"></param>
        /// <param name="origenFrigorifico"></param>
        /// <param name="certificadoEmbarque"></param>
        /// <param name="fechaFaena"></param>
        /// <param name="pesaNeto"></param>
        /// <param name="pesaBruto"></param>
        /// <param name="barraLarga"></param>
        /// <returns></returns>
        private Datos.WM_Barra_Carga ActualizaBarraCarga(Datos.WM_Barra_Carga barraCarga, decimal origen, decimal origenFrigorifico, decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string barraLarga)
        {
            barraCarga.ORIGEN = origen.ToString();
            barraCarga.Origen_Frigorifico = origenFrigorifico;
            barraCarga.Certificado_Embarque = certificadoEmbarque;
            barraCarga.Fecha_Faena = decimal.Parse(fechaFaena);
            barraCarga.Peso_Neto = pesaNeto;
            barraCarga.Peso_Bruto = pesaBruto;
            barraCarga.Barra_Larga = barraLarga;
            barraCarga.INDPROC = "N";
            barraCarga.USUACT = "usr_rf";
            barraCarga.FECACT = decimal.Parse(DateTime.Now.ToString("yyyyMMdd"));
            barraCarga.HORACT = decimal.Parse(DateTime.Now.ToString("HHmmss"));
            return barraCarga;
        }

        /// <summary>
        /// crearBarraCargaImg
        /// </summary>
        /// <param name="item"></param>
        /// <param name="storeNBR"></param>
        /// <param name="origen"></param>
        /// <param name="origenFrigorifico"></param>
        /// <param name="certificadoEmbarque"></param>
        /// <param name="fechaFaena"></param>
        /// <param name="pesaNeto"></param>
        /// <param name="pesaBruto"></param>
        /// <param name="barraLarga"></param>
        /// <returns></returns>
        private Datos.WM_Barra_Carga crearBarraCargaImg(string item, decimal storeNBR, decimal origen, decimal origenFrigorifico, decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string barraLarga, byte[] imagen, string responsable)
        {
            Datos.WM_Barra_Carga barraCarga = new WM_Barra_Carga();
            barraCarga.Item_Barras = item;
            barraCarga.STORE_NBR = storeNBR;
            barraCarga.ORIGEN = origen.ToString();
            barraCarga.Origen_Frigorifico = origenFrigorifico;
            barraCarga.Certificado_Embarque = certificadoEmbarque;
            barraCarga.Fecha_Faena = decimal.Parse(fechaFaena);
            barraCarga.Peso_Neto = pesaNeto;
            barraCarga.Peso_Bruto = pesaBruto;
            barraCarga.Barra_Larga = barraLarga;
            barraCarga.INDPROC = "N";
            barraCarga.USUCRE = responsable.Length > 10 ? responsable.Substring(0, 10) : responsable;//"usr_rf";
            barraCarga.FECCRE = decimal.Parse(DateTime.Now.ToString("yyyyMMdd"));
            barraCarga.HORCRE = decimal.Parse(DateTime.Now.ToString("HHmmss"));
            barraCarga.USUACT = string.Empty;
            barraCarga.FECACT = 0;
            barraCarga.HORACT = 0;
            barraCarga.FOTOETIQUETA = imagen;
            return barraCarga;
        }

        /// <summary>
        /// ActualizaBarraCargaImg
        /// </summary>
        /// <param name="barraCarga"></param>
        /// <param name="origen"></param>
        /// <param name="origenFrigorifico"></param>
        /// <param name="certificadoEmbarque"></param>
        /// <param name="fechaFaena"></param>
        /// <param name="pesaNeto"></param>
        /// <param name="pesaBruto"></param>
        /// <param name="barraLarga"></param>
        /// <returns></returns>
        private Datos.WM_Barra_Carga ActualizaBarraCargaImg(Datos.WM_Barra_Carga barraCarga, decimal origen, decimal origenFrigorifico, decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string barraLarga, byte[] imagen, string responsable)
        {
            barraCarga.ORIGEN = origen.ToString();
            barraCarga.Origen_Frigorifico = origenFrigorifico;
            barraCarga.Certificado_Embarque = certificadoEmbarque;
            barraCarga.Fecha_Faena = decimal.Parse(fechaFaena);
            barraCarga.Peso_Neto = pesaNeto;
            barraCarga.Peso_Bruto = pesaBruto;
            barraCarga.Barra_Larga = barraLarga;
            barraCarga.INDPROC = "N";
            barraCarga.USUACT = responsable.Length > 10 ? responsable.Substring(0, 10) : responsable;//"usr_rf";
            barraCarga.FECACT = decimal.Parse(DateTime.Now.ToString("yyyyMMdd"));
            barraCarga.HORACT = decimal.Parse(DateTime.Now.ToString("HHmmss"));
            barraCarga.FOTOETIQUETA = imagen;
            return barraCarga;
        }

        /***********/



        /// <summary>
        /// crearBarraCargaBitacora
        /// </summary>
        /// <param name="item"></param>
        /// <param name="storeNBR"></param>
        /// <param name="origen"></param>
        /// <param name="origenFrigorifico"></param>
        /// <param name="certificadoEmbarque"></param>
        /// <param name="fechaFaena"></param>
        /// <param name="pesaNeto"></param>
        /// <param name="pesaBruto"></param>
        /// <param name="barraLarga"></param>
        /// <param name="responsable"></param>
        /// <returns></returns>
        private Datos.WM_Barra_Carga_Bitacora crearBarraCargaBitacora(string item, decimal storeNBR, decimal origen, decimal origenFrigorifico, decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string barraLarga, string responsable)
        {
            Datos.WM_Barra_Carga_Bitacora barraCargaBitacora = new WM_Barra_Carga_Bitacora();
            barraCargaBitacora.Item_Barras = item;
            barraCargaBitacora.STORE_NBR = storeNBR;
            barraCargaBitacora.ORIGEN = origen.ToString();
            barraCargaBitacora.Origen_Frigorifico = origenFrigorifico;
            barraCargaBitacora.Certificado_Embarque = certificadoEmbarque;
            barraCargaBitacora.Fecha_Faena = decimal.Parse(fechaFaena);
            barraCargaBitacora.Peso_Neto = pesaNeto;
            barraCargaBitacora.Peso_Bruto = pesaBruto;
            barraCargaBitacora.Barra_Larga = barraLarga;
            barraCargaBitacora.INDPROC = "N";
            barraCargaBitacora.USUCRE = "usr_rf";
            barraCargaBitacora.FECCRE = decimal.Parse(DateTime.Now.ToString("yyyyMMdd"));
            barraCargaBitacora.HORCRE = decimal.Parse(DateTime.Now.ToString("HHmmss"));
            barraCargaBitacora.Responsable = responsable.Length > 10 ? responsable.Substring(0,10) : responsable;
            return barraCargaBitacora;
        }

        private Datos.WM_Barra_Carga_Bitacora crearBarraCargaBitacoraImg(string item, decimal storeNBR, decimal origen, decimal origenFrigorifico, decimal certificadoEmbarque, string fechaFaena, decimal pesaNeto, decimal pesaBruto, string barraLarga, string responsable, byte[] fotoEtiqueta)
        {
            Datos.WM_Barra_Carga_Bitacora barraCargaBitacora = new WM_Barra_Carga_Bitacora();
            barraCargaBitacora.Item_Barras = item;
            barraCargaBitacora.STORE_NBR = storeNBR;
            barraCargaBitacora.ORIGEN = origen.ToString();
            barraCargaBitacora.Origen_Frigorifico = origenFrigorifico;
            barraCargaBitacora.Certificado_Embarque = certificadoEmbarque;
            barraCargaBitacora.Fecha_Faena = decimal.Parse(fechaFaena);
            barraCargaBitacora.Peso_Neto = pesaNeto;
            barraCargaBitacora.Peso_Bruto = pesaBruto;
            barraCargaBitacora.Barra_Larga = barraLarga;
            barraCargaBitacora.INDPROC = "N";
            barraCargaBitacora.USUCRE = "usr_rf";
            barraCargaBitacora.FECCRE = decimal.Parse(DateTime.Now.ToString("yyyyMMdd"));
            barraCargaBitacora.HORCRE = decimal.Parse(DateTime.Now.ToString("HHmmss"));
            barraCargaBitacora.Responsable = responsable.Length > 10 ? responsable.Substring(0,10) : responsable;
            barraCargaBitacora.FOTOETIQUETA = fotoEtiqueta;
            return barraCargaBitacora;
        }

        /// <summary>
        /// ActualizaArtLocalSesma
        /// </summary>
        /// <param name="Art_Local_Sesma"></param>
        /// <param name="origen"></param>
        /// <param name="fechaFaena"></param>
        /// <returns></returns>
        private Datos.Informacion_Art_Local_Sesma ActualizaArtLocalSesma(Datos.Informacion_Art_Local_Sesma Art_Local_Sesma, decimal origen, string fechaFaena)
        {

            Art_Local_Sesma.ORIGEN = origen.ToString();
            Art_Local_Sesma.RESSNS = fechaFaena;
            Art_Local_Sesma.INDPROC = "N";
            Art_Local_Sesma.USUACT = "usr_rf";
            Art_Local_Sesma.FECACT = decimal.Parse(DateTime.Now.ToString("yyyyMMdd"));
            Art_Local_Sesma.HORACT = decimal.Parse(DateTime.Now.ToString("HHmmss"));
            return Art_Local_Sesma;
        }

        /// <summary>
        /// ActualizaArtProveedor
        /// </summary>
        /// <param name="Art_Local_Sesma"></param>
        /// <param name="fechaFaena"></param>
        /// <returns></returns>
        private Datos.Informacion_Articulo_Prov ActualizaArtProveedor(Datos.Informacion_Articulo_Prov Art_Local_Sesma, string fechaFaena)
        {
            Art_Local_Sesma.RESSNS = fechaFaena;
            Art_Local_Sesma.USUACT = "usr_rf";
            Art_Local_Sesma.FECACT = decimal.Parse(DateTime.Now.ToString("yyyyMMdd"));
            Art_Local_Sesma.HORACT = decimal.Parse(DateTime.Now.ToString("HHmmss"));
            return Art_Local_Sesma;
        }

        /// <summary>
        /// DevuelveFechaFaena
        /// </summary>
        /// <param name="fechaFaena"></param>
        /// <returns></returns>
        private string DevuelveFechaFaena(string fechaFaena)
        {
            string fecFaena = fechaFaena;
            string yy = fecFaena.Substring(0, 2);
            string mm = fecFaena.Substring(2, 2);
            string dd = fecFaena.Substring(4, 2);
            string yyPaso = string.Format("{0:yyyy}", DateTime.Now);
            string yyAnio = yyPaso.Substring(0, 2);
            string AnioFinal = yyAnio + yy;
            DateTime fechaFaenaD = new DateTime(int.Parse(AnioFinal), int.Parse(mm), int.Parse(dd));
            string CadenaFechaFaena = "Fecha Faena:" + string.Format("{0:dd-MM-yyyy}", fechaFaenaD);
            return CadenaFechaFaena;
        }

        #endregion
    }
}