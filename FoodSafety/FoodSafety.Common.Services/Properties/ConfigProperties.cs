namespace FoodSafety.Common.Services.Properties
{
    /// <summary>
    /// Config properties.
    /// </summary>
    public static class ConfigProperties
    {
        //public static string API_ENDPOINT_PRIMARY = "https://api.production.walmartdigital.cl/foodsafety/stores";//produccion
        public static string API_ENDPOINT_SECOND = "https://api.production-v2.walmartdigital.cl/foodsafety/stores";//produccion
        public static string API_ENDPOINT_PRIMARY = "http://api.staging-v2.walmartdigital.cl/foodsafety/stores";//QA
        public static string PATH_STORES = "stores/discover"; //GET /stores/discover  -- GET /stores/discover/{country}
        public static string PATH_STORES_COUNTRY = "stores/"; //GET /stores/{country}   -- /stores/{country}/{store}

        //public static string SOAP_ENDPOINT = "http://192.168.1.40/WSTrazabilidadCarnes/";//dsac
        public static string SOAP_ENDPOINT = "http://10.10.55.150/FoodSafetyWebServices/";//qa
        //public static string SOAP_ENDPOINT = "http://10.10.26.214/WS_Pistola/";//Produccion

        public static string SOAP_PATH_BARRA = "Barra.svc?wsdl";
        public static string SOAP_PATH_LOCAL = "Local.svc?wsdl";
        public static string SOAP_PATH_INVENTARIO = "InventarioCarnes.svc?wsdl";
        public static string SOAP_PATH_INVENTARIO_REFRESH = "InventarioRefresh.svc?wsdl";
        public static string SOAP_NAMESPACE = "http://tempuri.org/";
        public static string SOAP_ACTION = "SOAPAction";
        public static string SOAP_ACTION_LOCALES = "http://tempuri.org/ILocal/ObtieneLocales";
        public static string SOAP_ACTION_ITEM_CARNICERIA = "http://tempuri.org/IInventarioCarnes/ObtieneItemCaniceria";
        public static string SOAP_ACTION_ITEM_REFRESH = "http://tempuri.org/IInventarioRefresh/ObtieneItemRefresh";
        public static string SOAP_ACTION_INGRESO_ITEM = "http://tempuri.org/IInventarioCarnes/IngresoItem";
        public static string SOAP_ACTION_INGRESO_ITEM_REFRESH = "http://tempuri.org/IInventarioRefresh/IngresoItemRefresh";
        public static string SOAP_ACTION_PROCESA_BARRAS = "http://tempuri.org/IBarra/ProcesaBarras";
        public static string SOAP_ACTION_PROCESA_BARRAS_MSG = "http://tempuri.org/IBarra/ProcesaBarrasMSG";
        public static string SOAP_ACTION_OBTIENE_ITEM_BALANZA = "http://tempuri.org/IBarra/ObtieneItemBalanza";
        public static string SOAP_ACTION_OBTIENE_PROCEDENCIA = "http://tempuri.org/IBarra/ObtieneProcedencia";
        public static string SOAP_ACTION_OBTIENE_FRIGORIFICOS = "http://tempuri.org/IBarra/ObtieneFrigorificos";
        public static string SOAP_ACTION_OBTIENE_PARIDADES = "http://tempuri.org/IBarra/ObtieneParidades";
        public static string SOAP_ACTION_INGRESO_ITEM_CARNES = "http://tempuri.org/IInventarioCarnes/ProcesaBarrasTablet";
        public static string SOAP_ACTION_INGRESO_ITEM_CARNES_IMG = "http://tempuri.org/IInventarioCarnes/ProcesaBarrasImgTablet";
        public static string LOCALES_FILTER = "CL?$select=id%2Cformat%2Cname%2Clocation%2Fregion%2Clocation%2Fcity%2Clocation%2Fcommune%2Clocation%2Faddress%2Clocation%2Faddress%2Clocation%2Fposition%2Fcoordinates&$top=2000";
        public static string NOTCONNECTION = "Dispositivo sin conexión a internet";
    }
}