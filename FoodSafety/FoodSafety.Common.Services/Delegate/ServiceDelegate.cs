using System;
using System.Net;
using System.Threading.Tasks;
using FoodSafety.Common.Services.Properties;
using FoodSafety.Common.Services.Result;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using FoodSafety.Common.Models.Models;
using System.Text;
using FoodSafety.Common.Services.Helpers;
using Newtonsoft.Json.Linq;
using FoodSafety.Common.Utils;

namespace FoodSafety.Common.Services.Delegate
{
    /// <summary>
    /// Service delegate.
    /// </summary>
    public class ServiceDelegate
    {
        private const string MediaType = "text/xml";
        static readonly HttpClient client = new HttpClient();
        static ServiceDelegate instance = null;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ServiceDelegate Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServiceDelegate();
                return instance;
            }
        }

        #region WEB_API_MAPAS
        /// <summary>
        /// Gets the locales.
        /// </summary>
        /// <returns>The locales.</returns>
        public async Task<ServiceResult> GetLocales(string json)
        {
            string baseUrlPrimary = ConfigProperties.API_ENDPOINT_PRIMARY;
            string baseUrlSecond = ConfigProperties.API_ENDPOINT_SECOND;
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    var response = await client.GetAsync(baseUrlPrimary);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        List<Sucursal> sucursales = JsonConvert.DeserializeObject<List<Sucursal>>(responseString);
                        result.Success = true;
                        result.Response = sucursales;
                        result.Message = "sucursales desde service primary";
                    }
                    else
                    {
                        try
                        {
                            var responseSecond = await client.GetAsync(baseUrlSecond);
                            var responseStringSecond = await responseSecond.Content.ReadAsStringAsync();
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                List<Sucursal> sucursales = JsonConvert.DeserializeObject<List<Sucursal>>(responseStringSecond);
                                result.Success = true;
                                result.Response = sucursales;
                                result.Message = "sucursales desde service second";
                            }
                            else
                            {
                                List<Sucursal> sucursales = JsonConvert.DeserializeObject<List<Sucursal>>(json);
                                result.Success = true;
                                result.Response = sucursales;
                                result.Message = "sucursales desde JSON assets";
                            }
                        }
                        catch (Exception ex)
                        {
                            List<Sucursal> sucursales = JsonConvert.DeserializeObject<List<Sucursal>>(json);
                            result.Success = true;
                            result.Response = sucursales;
                            result.Message = "sucursales desde JSON assets";
                        }

                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }
        #endregion

        #region SOAP_CARNICERIA_REFRESH
        /// <summary>
        /// Obtienes the item caniceria.
        /// </summary>
        /// <returns>The item caniceria.</returns>
        public async Task<ServiceResult> ObtieneItemCaniceria()
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try{
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_ITEM_CARNICERIA);
                        client.Timeout = TimeSpan.FromMinutes(5);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_INVENTARIO;
                        var soapRequest = ServiceHelper.PrepareRequest(0);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseCarniceria(soapResponse);
                            result.Success = true;
                            result.Response = parseRespone;
                        }
                    }
                }catch(Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                    }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Ingresos the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="itemRequest">Item request.</param>
        public async Task<ServiceResult> IngresoItem(JObject itemRequest)
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromMinutes(2);
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_INGRESO_ITEM_CARNES);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_INVENTARIO;
                        var soapRequest = ServiceHelper.PrepareRequest(11,itemRequest);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {

                                var soapResponse = await response.Content.ReadAsStringAsync();
                                var parseRespone = ServiceHelper.ParseSoapResponseInsertItem(soapResponse);
                                if(parseRespone.EstadoResutado.Equals("2")){
                                    result.Success = false;
                                    result.Message = "El item padre no coincide";
                                    result.Response = 500;
                                }
                                else{
                                    result.Success = true;
                                    result.Response = parseRespone;
                                }
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Ingresos the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="itemRequest">Item request.</param>
        public async Task<ServiceResult> IngresoItemImage(JObject itemRequest)
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromMinutes(5);
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_INGRESO_ITEM_CARNES_IMG);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_INVENTARIO;
                        var soapRequest = ServiceHelper.PrepareRequest(12, itemRequest);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {

                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseInsertImgItem(soapResponse);
                            if (parseRespone.EstadoResutado.Equals("2"))
                            {
                                result.Success = true;
                                result.Message = "El item padre no coincide";
                                result.Response = 2;
                            }
                            else
                            {
                                result.Success = true;
                                result.Response = parseRespone;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Ingresos the item imagen.
        /// </summary>
        /// <returns>The item imagen.</returns>
        /// <param name="itemRequest">Item request.</param>
        public async Task<ServiceResult> IngresoItemImagen(JObject itemRequest, byte[] imageByte)
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromMinutes(2);
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_INGRESO_ITEM_CARNES_IMG);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_INVENTARIO;

                        var imageBinaryContent = new ByteArrayContent(imageByte);
                        var multipartContent = new MultipartFormDataContent();
                        multipartContent.Add(new StringContent(itemRequest.GetValue("Item").ToString()), "item");
                        multipartContent.Add(new StringContent(itemRequest.GetValue("Local").ToString()), "local"); 
                        multipartContent.Add(new StringContent(itemRequest.GetValue("Procedencia").ToString()), "procedencia");
                        multipartContent.Add(new StringContent(itemRequest.GetValue("OrigenFrigorifico").ToString()), "origenFrigorifico");
                        multipartContent.Add(new StringContent(itemRequest.GetValue("CertificadoEmbarque").ToString()), "certificadoEmbarque");
                        multipartContent.Add(new StringContent(itemRequest.GetValue("FechaFaena").ToString()), "fechaFaena");
                        multipartContent.Add(new StringContent(itemRequest.GetValue("PesaNeto").ToString()), "pesaNeto");
                        multipartContent.Add(new StringContent(itemRequest.GetValue("PesaBruto").ToString()), "pesaBruto");
                        multipartContent.Add(new StringContent(itemRequest.GetValue("Responsable").ToString()), "responsable");
                        multipartContent.Add(imageBinaryContent, "image");

                        using (var response = await client.PostAsync(uri, multipartContent))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseInsertItem(soapResponse);
                            if (parseRespone.EstadoResutado.Equals("2"))
                            {
                                result.Success = false;
                                result.Message = "El item padre no coincide";
                                result.Response = 500;
                            }
                            else
                            {
                                result.Success = true;
                                result.Response = parseRespone;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Ingresos the item refresh.
        /// </summary>
        /// <returns>The item refresh.</returns>
        /// <param name="itemRequest">Item request.</param>
        public async Task<ServiceResult> IngresoItemRefresh(JObject itemRequest)
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_INGRESO_ITEM_REFRESH);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_INVENTARIO_REFRESH;
                        var soapRequest = ServiceHelper.PrepareRequest(2, itemRequest);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseInsertItemRefresh(soapResponse);
                            if (parseRespone != null)
                            {
                                result.Success = true;
                                result.Response = parseRespone;
                            }
                            else
                            {
                                result.Success = false;
                                result.Response = 999;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                    result.Success = false;
                    result.Response = 1000;
                    result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Ingresos the item refresh image.
        /// </summary>
        /// <returns>The item refresh image.</returns>
        /// <param name="itemRequest">Item request.</param>
        public async Task<ServiceResult> IngresoItemRefreshImg(JObject itemRequest)
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_INGRESO_ITEM_REFRESH);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_INVENTARIO_REFRESH;
                        var soapRequest = ServiceHelper.PrepareRequest(13, itemRequest);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseInsertItemRefresh(soapResponse);
                            if (parseRespone != null)
                            {
                                result.Success = true;
                                result.Response = parseRespone;
                            }
                            else
                            {
                                result.Success = false;
                                result.Response = 999;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Obtienes the item refresh.
        /// </summary>
        /// <returns>The item refresh.</returns>
        public async Task<ServiceResult> ObtieneItemRefresh()
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_ITEM_REFRESH);
                        client.Timeout = TimeSpan.FromMinutes(20);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_INVENTARIO_REFRESH;
                        var soapRequest = ServiceHelper.PrepareRequest(10);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseRefresh(soapResponse);
                            result.Success = true;
                            result.Response = parseRespone;
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }
        #endregion

        #region SOAP_LOCALES
        /// <summary>
        /// Obtienes the locales SOAP.
        /// </summary>
        /// <returns>The locales SOAP.</returns>
        public async Task<ServiceResult> ObtieneLocalesSoap()
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_LOCALES);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_LOCAL;
                        var soapRequest = ServiceHelper.PrepareRequest(1);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseLocales(soapResponse);
                            result.Success = true;
                            result.Response = parseRespone;
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }
        #endregion

        #region SOAP_BARRA
        /// <summary>
        /// Obtienes the frigorificos.
        /// </summary>
        /// <returns>The frigorificos.</returns>
        public async Task<ServiceResult> ObtieneFrigorificos()
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_OBTIENE_FRIGORIFICOS);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_BARRA;
                        var soapRequest = ServiceHelper.PrepareRequest(4);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseFrigorificos(soapResponse);
                            result.Success = true;
                            result.Response = parseRespone;
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Obtienes the item balanza.
        /// </summary>
        /// <returns>The item balanza.</returns>
        public async Task<ServiceResult> ObtieneItemBalanza()
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_OBTIENE_ITEM_BALANZA);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_BARRA;
                        var soapRequest = ServiceHelper.PrepareRequest(5);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseItemBalanza(soapResponse);
                            result.Success = true;
                            result.Response = parseRespone;
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Obtienes the paridades.
        /// </summary>
        /// <returns>The paridades.</returns>
        public async Task<ServiceResult> ObtieneParidades()
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_OBTIENE_PARIDADES);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_BARRA;
                        var soapRequest = ServiceHelper.PrepareRequest(6);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseParidad(soapResponse);
                            result.Success = true;
                            result.Response = parseRespone;
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Obtienes the procedencias.
        /// </summary>
        /// <returns>The procedencias.</returns>
        public async Task<ServiceResult> ObtieneProcedencias()
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_OBTIENE_PROCEDENCIA);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_BARRA;
                        var soapRequest = ServiceHelper.PrepareRequest(7);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseProcedencia(soapResponse);
                            result.Success = true;
                            result.Response = parseRespone;
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Procesars the barras.
        /// </summary>
        /// <returns>The barras.</returns>
        /// <param name="procesaBarrasRequest">Procesa barras request.</param>
        public async Task<ServiceResult> ProcesarBarras(JObject procesaBarrasRequest)
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_PROCESA_BARRAS);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_BARRA;
                        var soapRequest = ServiceHelper.PrepareRequest(8,procesaBarrasRequest);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseProcesaBarra(soapResponse);
                            result.Success = true;
                            result.Response = parseRespone;
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }

        /// <summary>
        /// Procesars the barras message.
        /// </summary>
        /// <returns>The barras message.</returns>
        /// <param name="procesaBarrasRequest">Procesa barras request.</param>
        public async Task<ServiceResult> ProcesarBarrasMsg(JObject procesaBarrasRequest)
        {
            ServiceResult result = new ServiceResult();
            if (GetNetworkStatus())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(ConfigProperties.SOAP_ACTION, ConfigProperties.SOAP_ACTION_PROCESA_BARRAS_MSG);
                        var uri = ConfigProperties.SOAP_ENDPOINT + ConfigProperties.SOAP_PATH_BARRA;
                        var soapRequest = ServiceHelper.PrepareRequest(9, procesaBarrasRequest);
                        var content = new StringContent(soapRequest, Encoding.UTF8, MediaType);
                        using (var response = await client.PostAsync(uri, content))
                        {
                            var soapResponse = await response.Content.ReadAsStringAsync();
                            var parseRespone = ServiceHelper.ParseSoapResponseProcesaBarraMsg(soapResponse);
                            result.Success = true;
                            result.Response = parseRespone;
                        }
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = e.Message;
                    result.Response = 999;
                }
            }
            else
            {
                result.Success = false;
                result.Response = 1000;
                result.Message = ConfigProperties.NOTCONNECTION;
            }
            return result;
        }
        #endregion


        /// <summary>
        /// Gets the network status.
        /// </summary>
        /// <returns><c>true</c>, if network status was gotten, <c>false</c> otherwise.</returns>
        private bool GetNetworkStatus()
        {
            return ValidationUtils.GetNetworkStatus();
        }
    }
}