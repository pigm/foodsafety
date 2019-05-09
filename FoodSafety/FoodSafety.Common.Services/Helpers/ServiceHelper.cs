using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using FoodSafety.Common.Models.Models;
using FoodSafety.Common.Services.Properties;
using Newtonsoft.Json.Linq;

namespace FoodSafety.Common.Services.Helpers
{
    public static class ServiceHelper
    {
        /// <summary>
        /// Prepares the request.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="typeRequest">Type request.</param>
        public static string PrepareRequest(int typeRequest,JObject paramsObject=null)
        {
            var request = string.Empty;
            switch (typeRequest)
            {
                case 0: //ObtieneItemCaniceria
                    request = @"<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">" +
                               "<soapenv:Header/>" +
                                "<soapenv:Body>" +
                                "<tem:ObtieneItemCaniceria/>" +
                                "</soapenv:Body>" +
                                "</soapenv:Envelope>";
                    break;
                case 1: //ObtieneLocales
                    request = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">" +
                                "<soapenv:Header/>" +
                                "<soapenv:Body>" +
                                "<tem:ObtieneLocales/>" +
                                "</soapenv:Body>"+
                                "</soapenv:Envelope>";
                    break;
                case 2: //InsertaItemRefresh
                    request = String.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                                           <soapenv:Header/>
                                           <soapenv:Body>
                                              <tem:IngresoItemRefresh>
                                                 <tem:itemPadre>{0}</tem:itemPadre>
                                                 <!--Optional:-->
                                                 <tem:fechaElaboracion>{1}</tem:fechaElaboracion>
                                                 <!--Optional:-->
                                                 <tem:loteProduccion>{2}</tem:loteProduccion>
                                                 <!--Optional:-->
                                                 <tem:fechaDescongelado>{3}</tem:fechaDescongelado>
                                                 <tem:idParametro>{4}</tem:idParametro>
                                                 <tem:temperatura>{5}</tem:temperatura>
                                                 <!--Optional:-->
                                                 <tem:etiquetaPropia>{6}</tem:etiquetaPropia>
                                                 <!--Optional:-->
                                                 <tem:usuarioCreacion>{7}</tem:usuarioCreacion>
                                                 <tem:fechaCreacion>{8}</tem:fechaCreacion>
                                                 <tem:horaCreacion>{9}</tem:horaCreacion>
                                                 <tem:comentario>{10}</tem:comentario>
                                                 <tem:cantidadUnidades>{11}</tem:cantidadUnidades>
                                              </tem:IngresoItemRefresh>
                                           </soapenv:Body>
                                        </soapenv:Envelope>
                                        ", paramsObject.GetValue("ItemPadre").ToString(),
                                            paramsObject.GetValue("FechaElaboracion").ToString(),
                                            paramsObject.GetValue("LoteProduccion").ToString(),
                                            paramsObject.GetValue("FechaDescongelado").ToString(),
                                            paramsObject.GetValue("IdParametro").ToString(),
                                            paramsObject.GetValue("Temperatura").ToString(),
                                            paramsObject.GetValue("EtiquetaPropia").ToString(),
                                            paramsObject.GetValue("UsuarioCreacion").ToString(),
                                            paramsObject.GetValue("FechaCreacion").ToString(),
                                            paramsObject.GetValue("HoraCreacion").ToString(),
                                            paramsObject.GetValue("Comentario").ToString(),
                                            paramsObject.GetValue("CantidadUnidades").ToString()
                                           );
                    break;
                case 3: //InsertaItem
                    request = String.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                               <soapenv:Header/>
                                   <soapenv:Body>
                                       <tem:IngresoItem>
                                             <tem:itemPadre>{0}</tem:itemPadre>
                                             <tem:store>{1}</tem:store>
                                             <tem:item>{2}</tem:item>
                                             <!--Optional:-->
                                             <tem:fechaInventario>{3}</tem:fechaInventario>
                                             <!--Optional:-->
                                             <tem:fechaLectura>{4}</tem:fechaLectura>
                                             <tem:origenFrigorifico>{5}</tem:origenFrigorifico>
                                             <tem:certificadoEmbarque>{6}</tem:certificadoEmbarque>
                                             <tem:fechaFaena>{7}</tem:fechaFaena>
                                             <tem:pesoNeto>{8}</tem:pesoNeto>
                                             <tem:pesoBruto>{9}</tem:pesoBruto>
                                             <tem:cajas>{10}</tem:cajas>
                                             <!--Optional:-->
                                             <tem:responsable>{11}</tem:responsable>
                                             <tem:idMotivonventario>{12}</tem:idMotivonventario>
                                             <tem:procedencia>{13}</tem:procedencia>
                                             //<tem:image>{14}</tem:image>
                                          </tem:IngresoItem>
                                       </soapenv:Body>
                               </soapenv:Envelope>", paramsObject.GetValue("ItemPadre").ToString(),
                                            paramsObject.GetValue("Store").ToString(),
                                            paramsObject.GetValue("Item").ToString(),
                                            paramsObject.GetValue("FechaInventario").ToString(),
                                            paramsObject.GetValue("FechaLectura").ToString(),
                                            paramsObject.GetValue("OrigenFrigorifico").ToString(),
                                            paramsObject.GetValue("CertificadoEmbarque").ToString(),
                                            paramsObject.GetValue("FechaFaena").ToString(),
                                            paramsObject.GetValue("PesoNeto").ToString(),
                                            paramsObject.GetValue("PesoBruto").ToString(),
                                            paramsObject.GetValue("Cajas").ToString(),
                                            paramsObject.GetValue("Responsable").ToString(),
                                            paramsObject.GetValue("IdMotivonventario").ToString(),
                                            paramsObject.GetValue("Procedencia").ToString()//,
                                            //paramsObject.GetValue("Image").ToString()
                                           );
                    break;
                case 4: //ObtieneFrigorificos
                    request = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">" +
                                "<soapenv:Header/>" +
                                "<soapenv:Body>" +
                                "<tem:ObtieneFrigorificos/>" +
                                "</soapenv:Body>" +
                                "</soapenv:Envelope>";
                    break;
                case 5: //ObtieneItemBalanza
                    request = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">" +
                                "<soapenv:Header/>" +
                                "<soapenv:Body>" +
                                "<tem:ObtieneItemBalanza/>" +
                                "</soapenv:Body>" +
                                "</soapenv:Envelope>";
                    break;
                case 6: //ObtieneParidades
                    request = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">" +
                                "<soapenv:Header/>" +
                                "<soapenv:Body>" +
                                "<tem:ObtieneParidades/>" +
                                "</soapenv:Body>" +
                                "</soapenv:Envelope>";
                    break;
                case 7: //ObtieneProcedencia
                    request = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">" +
                                "<soapenv:Header/>" +
                                "<soapenv:Body>" +
                                "<tem:ObtieneProcedencia/>" +
                                "</soapenv:Body>" +
                                "</soapenv:Envelope>";
                    break;
                case 8: //ProcesaBarras
                    request = String.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                                           <soapenv:Header/>
                                           <soapenv:Body>
                                              <tem:ProcesaBarras>
                                                 <tem:barra1>{0}</tem:barra1>
                                                 <tem:barra2>{1}</tem:barra2>
                                                 <tem:codigoLocal>{2}</tem:codigoLocal>
                                              </tem:ProcesaBarras>
                                           </soapenv:Body>
                                        </soapenv:Envelope>", paramsObject.GetValue("barra1"),
                                            paramsObject.GetValue("barra2"),
                                            paramsObject.GetValue("codigoLocal"));
                    break;
                case 9: //ProcesaBarrasMSG
                    request = String.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                                           <soapenv:Header/>
                                           <soapenv:Body>
                                              <tem:ProcesaBarrasMSG>
                                                 <tem:mensaje>{0}</tem:mensaje>
                                                 <tem:codigoLocal>{1}</tem:codigoLocal>
                                              </tem:ProcesaBarras>
                                           </soapenv:Body>
                                        </soapenv:Envelope>", paramsObject.GetValue("mensaje"),
                                            paramsObject.GetValue("codigoLocal"));
                    break;
                case 10: //ListaProductosRefresh
                    request = @"<?xml version=""1.0"" encoding=""utf-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">" +
                               "<soapenv:Header/>" +
                                "<soapenv:Body>" +
                                "<tem:ObtieneItemRefresh/>" +
                                "</soapenv:Body>" +
                                "</soapenv:Envelope>";
                    break;
                case 11: //InsertaItem
                    request = String.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                               <soapenv:Header/>
                                   <soapenv:Body>
                                       <tem:ProcesaBarrasTablet>
                                             <tem:item>{0}</tem:item>
                                             <tem:local>{1}</tem:local>
                                             <tem:procedencia>{2}</tem:procedencia>                                             
                                             <tem:origenFrigorifico>{3}</tem:origenFrigorifico>                                 
                                             <tem:certificadoEmbarque>{4}</tem:certificadoEmbarque>
                                             <tem:fechaFaena>{5}</tem:fechaFaena>
                                             <tem:pesaNeto>{6}</tem:pesaNeto>
                                             <tem:pesaBruto>{7}</tem:pesaBruto>
                                             <tem:responsable>{8}</tem:responsable>                                            
                                          </tem:ProcesaBarrasTablet>
                                       </soapenv:Body>
                               </soapenv:Envelope>", paramsObject.GetValue("Item").ToString(),
                                            paramsObject.GetValue("Local").ToString(),
                                            paramsObject.GetValue("Procedencia").ToString(),
                                            paramsObject.GetValue("OrigenFrigorifico").ToString(),
                                            paramsObject.GetValue("CertificadoEmbarque").ToString(),
                                            paramsObject.GetValue("FechaFaena").ToString(),
                                            paramsObject.GetValue("PesaNeto").ToString(),
                                            paramsObject.GetValue("PesaBruto").ToString(),
                                            paramsObject.GetValue("Responsable").ToString()
                                           );
                    break;
                case 12: //InsertaItemImage
                    request = String.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                               <soapenv:Header/>
                                   <soapenv:Body>
                                       <tem:ProcesaBarrasImgTablet>
                                             <tem:item>{0}</tem:item>
                                             <tem:local>{1}</tem:local>
                                             <tem:procedencia>{2}</tem:procedencia>                                             
                                             <tem:origenFrigorifico>{3}</tem:origenFrigorifico>                                 
                                             <tem:certificadoEmbarque>{4}</tem:certificadoEmbarque>
                                             <tem:fechaFaena>{5}</tem:fechaFaena>
                                             <tem:pesaNeto>{6}</tem:pesaNeto>
                                             <tem:pesaBruto>{7}</tem:pesaBruto>
                                             <tem:responsable>{8}</tem:responsable>
                                             <tem:image>{9}</tem:image>                                                                                        
                                          </tem:ProcesaBarrasImgTablet>
                                       </soapenv:Body>
                               </soapenv:Envelope>", paramsObject.GetValue("Item").ToString(),
                                            paramsObject.GetValue("Local").ToString(),
                                            paramsObject.GetValue("Procedencia").ToString(),
                                            paramsObject.GetValue("OrigenFrigorifico").ToString(),
                                            paramsObject.GetValue("CertificadoEmbarque").ToString(),
                                            paramsObject.GetValue("FechaFaena").ToString(),
                                            paramsObject.GetValue("PesaNeto").ToString(),
                                            paramsObject.GetValue("PesaBruto").ToString(),
                                            paramsObject.GetValue("Responsable").ToString(),
                                            paramsObject.GetValue("Image")
                                           );
                    break;
                case 13: //InsertaItemRefresh
                    request = String.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                                           <soapenv:Header/>
                                           <soapenv:Body>
                                              <tem:IngresoItemRefresh>
                                                 <tem:itemPadre>{0}</tem:itemPadre>
                                                 <!--Optional:-->
                                                 <tem:fechaElaboracion>{1}</tem:fechaElaboracion>
                                                 <!--Optional:-->
                                                 <tem:loteProduccion>{2}</tem:loteProduccion>
                                                 <!--Optional:-->
                                                 <tem:fechaDescongelado>{3}</tem:fechaDescongelado>
                                                 <tem:idParametro>{4}</tem:idParametro>
                                                 <tem:temperatura>{5}</tem:temperatura>
                                                 <!--Optional:-->
                                                 <tem:etiquetaPropia>{6}</tem:etiquetaPropia>
                                                 <!--Optional:-->
                                                 <tem:usuarioCreacion>{7}</tem:usuarioCreacion>
                                                 <tem:fechaCreacion>{8}</tem:fechaCreacion>
                                                 <tem:horaCreacion>{9}</tem:horaCreacion>
                                                 <tem:comentario>{10}</tem:comentario>
                                                 <tem:cantidadUnidades>{11}</tem:cantidadUnidades>
                                                 <tem:imagen>{12}</tem:imagen>
                                                 <tem:correlativo>{13}</tem:correlativo>
                                                 <tem:storeNbr>{14}</tem:storeNbr>
                                              </tem:IngresoItemRefresh>
                                           </soapenv:Body>
                                        </soapenv:Envelope>
                                        ", paramsObject.GetValue("ItemPadre").ToString(),
                                            paramsObject.GetValue("FechaElaboracion").ToString(),
                                            paramsObject.GetValue("LoteProduccion").ToString(),
                                            paramsObject.GetValue("FechaDescongelado").ToString(),
                                            paramsObject.GetValue("IdParametro").ToString(),
                                            paramsObject.GetValue("Temperatura").ToString(),
                                            paramsObject.GetValue("EtiquetaPropia").ToString(),
                                            paramsObject.GetValue("UsuarioCreacion").ToString(),
                                            paramsObject.GetValue("FechaCreacion").ToString(),
                                            paramsObject.GetValue("HoraCreacion").ToString(),
                                            paramsObject.GetValue("Comentario").ToString(),
                                            paramsObject.GetValue("CantidadUnidades").ToString(),
                                            paramsObject.GetValue("Imagen"),
                                            paramsObject.GetValue("Correlativo").ToString(),
                                            paramsObject.GetValue("StoreNbr")
                                           );
                    break;

            }
            return request;
        }

        /// <summary>
        /// Parses the SOAP response.
        /// </summary>
        /// <returns>The SOAP response.</returns>
        /// <param name="response">Response.</param>
        public static List<EntidadItemCarniceria> ParseSoapResponseCarniceria(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ObtieneItemCaniceriaResult", nsmgr);
            List<EntidadItemCarniceria> listCarniceria = new List<EntidadItemCarniceria>();
            foreach (XmlNode xn in xNodelst)
            {
                var itemsCarniceria = xn.ChildNodes;
                foreach (XmlElement item in itemsCarniceria)
                {
                    try
                    {
                        listCarniceria.Add(new EntidadItemCarniceria
                        {
                            Item = item.ChildNodes[0].InnerText,
                            ItemDescripcion = item.ChildNodes[1].InnerText,
                            FineLineNbr = item.ChildNodes[2].InnerText,
                            DayOfCharge = DateTime.Now.DayOfYear
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.ToString());
                        continue;
                    }

                }
            }
            return listCarniceria;
        }

        /// <summary>
        /// Parses the SOAP response refresh.
        /// </summary>
        /// <returns>The SOAP response refresh.</returns>
        /// <param name="response">Response.</param>
        public static List<EntidadItemRefresh> ParseSoapResponseRefresh(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ObtieneItemRefreshResult", nsmgr);
            List<EntidadItemRefresh> listCarniceria = new List<EntidadItemRefresh>();
            foreach (XmlNode xn in xNodelst)
            {
                var itemsRefresh = xn.ChildNodes;
                foreach (XmlElement item in itemsRefresh)
                {
                    try
                    {
                        listCarniceria.Add(new EntidadItemRefresh
                        {
                            ItemNumber = item.ChildNodes[0].InnerText,
                            OldNumber = item.ChildNodes[1].InnerText,
                            CodigoBalanza = item.ChildNodes[2].InnerText,
                            Barra = item.ChildNodes[3].InnerText,
                            NumeroDepartamento = item.ChildNodes[4].InnerText,
                            NombreDepartamento = item.ChildNodes[5].InnerText,
                            DescripcionItem1 = item.ChildNodes[6].InnerText,
                            DescripcionItem2 = item.ChildNodes[7].InnerText,
                            DiasPericibilidad = item.ChildNodes[8].InnerText,
                            Temperatura = item.ChildNodes[9].InnerText,
                            TemperaturaIdeal = item.ChildNodes[11].InnerText,
                            HoraTemperaturaIdeal = item.ChildNodes[12].InnerText,
                            DayOfCharge = DateTime.Now.DayOfYear
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.ToString());
                        continue;
                    }
                }
            }
            return listCarniceria;
        }


        /// <summary>
        /// Parses the SOAP response item balanza.
        /// </summary>
        /// <returns>The SOAP response item balanza.</returns>
        /// <param name="response">Response.</param>
        public static List<EntidadItem> ParseSoapResponseItemBalanza(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ObtieneItemBalanzaResult", nsmgr);
            List<EntidadItem> listCarniceria = new List<EntidadItem>();
            foreach (XmlNode xn in xNodelst)
            {
                var items = xn.ChildNodes;
                foreach (XmlElement item in items)
                {
                    try
                    {
                        listCarniceria.Add(new EntidadItem
                        {
                            Item = item.ChildNodes[0].InnerText,
                            ItemDescripcion = item.ChildNodes[1].InnerText,
                            CodigoBalanza = item.ChildNodes[2].InnerText,
                            DayOfCharge = DateTime.Now.DayOfYear
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.ToString());
                        continue;
                    }
                }
            }
            return listCarniceria;
        }


        /// <summary>
        /// Parses the SOAP response locales.
        /// </summary>
        /// <returns>The SOAP response locales.</returns>
        /// <param name="response">Response.</param>
        public static List<EntidadLocal> ParseSoapResponseLocales(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ObtieneLocalesResult", nsmgr);
            List<EntidadLocal> listLocales = new List<EntidadLocal>();
            foreach (XmlNode xn in xNodelst)
            {
                var result = xn.ChildNodes;
                foreach (XmlElement item in result)
                {
                    try
                    {
                        listLocales.Add(new EntidadLocal
                        {
                            CodigoLocal = item.ChildNodes[0].InnerText,
                            NombreLocal = item.ChildNodes[1].InnerText,
                            CodigoFormato = item.ChildNodes[2].InnerText,
                            NombreFormato = item.ChildNodes[3].InnerText
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.ToString());
                        continue;
                    }
                }
            }
            return listLocales;
        }


        /// <summary>
        /// Parses the SOAP response frigorificos.
        /// </summary>
        /// <returns>The SOAP response frigorificos.</returns>
        /// <param name="response">Response.</param>
        public static List<EntidadFrigorifico> ParseSoapResponseFrigorificos(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ObtieneFrigorificosResult", nsmgr);
            List<EntidadFrigorifico> listaFrigorificos = new List<EntidadFrigorifico>();
            foreach (XmlNode xn in xNodelst)
            {
                var result = xn.ChildNodes;
                foreach (XmlElement item in result)
                {
                    try
                    {
                        listaFrigorificos.Add(new EntidadFrigorifico
                        {
                            CodigoFrigorifico = item.ChildNodes[0].InnerText,
                            NombreFrigorifico = item.ChildNodes[1].InnerText,
                            CodigoProcedencia = item.ChildNodes[2].InnerText,
                            DayOfCharge = DateTime.Now.DayOfYear
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.ToString());
                        continue;
                    }
                }
            }
            return listaFrigorificos;
        }

        /// <summary>
        /// Parses the SOAP response paridad.
        /// </summary>
        /// <returns>The SOAP response paridad.</returns>
        /// <param name="response">Response.</param>
        public static List<EntidadParidad> ParseSoapResponseParidad(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ObtieneParidadesResult", nsmgr);
            List<EntidadParidad> listaFrigorificos = new List<EntidadParidad>();
            foreach (XmlNode xn in xNodelst)
            {
                var result = xn.ChildNodes;
                foreach (XmlElement item in result)
                {
                    try
                    {
                        listaFrigorificos.Add(new EntidadParidad
                        {
                            ItemCompra = item.ChildNodes[0].InnerText,
                            ItemVenta = item.ChildNodes[1].InnerText,
                            DayOfCharge = DateTime.Now.DayOfYear
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.ToString());
                        continue;
                    }
                }
            }
            return listaFrigorificos;
        }


        /// <summary>
        /// Parses the SOAP response procedencia.
        /// </summary>
        /// <returns>The SOAP response procedencia.</returns>
        /// <param name="response">Response.</param>
        public static List<EntidadProcedencia> ParseSoapResponseProcedencia(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ObtieneProcedenciaResult", nsmgr);
            List<EntidadProcedencia> listaProcedencias = new List<EntidadProcedencia>();
            foreach (XmlNode xn in xNodelst)
            {
                var result = xn.ChildNodes;
                foreach (XmlElement item in result)
                {
                    try
                    {
                        listaProcedencias.Add(new EntidadProcedencia
                        {
                            CodigoProcedencia = item.ChildNodes[0].InnerText,
                            NombreProcedencia = item.ChildNodes[1].InnerText,
                            DayOfCharge = DateTime.Now.DayOfYear
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(item.ToString());
                        continue;
                    }
                }
            }
            return listaProcedencias;
        }

        /// <summary>
        /// Parses the SOAP response procedencia.
        /// </summary>
        /// <returns>The SOAP response procedencia.</returns>
        /// <param name="response">Response.</param>
        public static IngresoItemResult ParseSoapResponseInsertItem(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ProcesaBarrasTabletResult", nsmgr);
            IngresoItemResult ingresoItem = new IngresoItemResult();
            foreach (XmlNode xn in xNodelst)
            {
                var result = xn.ChildNodes;
                ingresoItem.EstadoResutado = result[0].InnerText;
                ingresoItem.Item = result[1].InnerText;
            }
            return ingresoItem;
        }

        /// <summary>
        /// Parses the SOAP response insert image item.
        /// </summary>
        /// <returns>The SOAP response insert image item.</returns>
        /// <param name="response">Response.</param>
        public static IngresoItemResult ParseSoapResponseInsertImgItem(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ProcesaBarrasImgTabletResult", nsmgr);
            IngresoItemResult ingresoItem = new IngresoItemResult();
            foreach (XmlNode xn in xNodelst)
            {
                var result = xn.ChildNodes;
                ingresoItem.EstadoResutado = result[0].InnerText;
                ingresoItem.Item = result[1].InnerText;
            }
            return ingresoItem;
        }


        /// <summary>
        /// Parses the SOAP response procedencia.
        /// </summary>
        /// <returns>The SOAP response procedencia.</returns>
        /// <param name="response">Response.</param>
        public static IngresoItemResult ParseSoapResponseInsertItemRefresh(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:IngresoItemRefreshResult", nsmgr);
            IngresoItemResult ingresoItem = new IngresoItemResult();
            foreach (XmlNode xn in xNodelst)
            {
                var result = xn.ChildNodes;
                ingresoItem.EstadoResutado = result[0].InnerText;
                ingresoItem.Item = result[1].InnerText;
            }
            return ingresoItem;
        } 

        /// <summary>
        /// Parses the SOAP response procesa barra.
        /// </summary>
        /// <returns>The SOAP response procesa barra.</returns>
        /// <param name="response">Response.</param>
        public static ProcesaBarrasResult ParseSoapResponseProcesaBarra(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ProcesaBarrasResult", nsmgr);
            ProcesaBarrasResult procesaBarras = new ProcesaBarrasResult();
            foreach (XmlNode xn in xNodelst)
            {
                var result = xn.ChildNodes;
                procesaBarras.EstadoResutado = result[0].InnerText;
                procesaBarras.Item = result[1].InnerText;
            }
            return procesaBarras;
        }

        /// <summary>
        /// Parses the SOAP response procesa barra message.
        /// </summary>
        /// <returns>The SOAP response procesa barra message.</returns>
        /// <param name="response">Response.</param>
        public static ProcesaBarrasResult ParseSoapResponseProcesaBarraMsg(string response)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("bhr", ConfigProperties.SOAP_NAMESPACE);
            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("//bhr:ProcesaBarrasMSGResult", nsmgr);
            ProcesaBarrasResult procesaBarras = new ProcesaBarrasResult();
            foreach (XmlNode xn in xNodelst)
            {
                var result = xn.ChildNodes;
                procesaBarras.EstadoResutado = result[0].InnerText;
                procesaBarras.Item = result[1].InnerText;
            }
            return procesaBarras;
        }
    }
}
