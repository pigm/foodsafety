using System.Linq;
using TrazabilidadCarne.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Collections.Generic;

namespace TrazabilidadCarne.Test
{
    
    
    /// <summary>
    ///Se trata de una clase de prueba para InventarioCarnesTest y se pretende que
    ///contenga todas las pruebas unitarias InventarioCarnesTest.
    ///</summary>
    [TestClass()]
    public class InventarioCarnesTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Obtiene o establece el contexto de la prueba que proporciona
        ///la información y funcionalidad para la ejecución de pruebas actual.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Atributos de prueba adicionales
        // 
        //Puede utilizar los siguientes atributos adicionales mientras escribe sus pruebas:
        //
        //Use ClassInitialize para ejecutar código antes de ejecutar la primera prueba en la clase 
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup para ejecutar código después de haber ejecutado todas las pruebas en una clase
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize para ejecutar código antes de ejecutar cada prueba
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup para ejecutar código después de que se hayan ejecutado todas las pruebas
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


       /* [TestMethod()]
        [HostType("ASP.NET")]
        [UrlToTest("http://dsac.dyndns.info:8084/TrazabilidadCarnesService/")]
        public void IngresoItemMotivoLecturaTest()
        {
            ServiceInventarioCarnes.InventarioCarnesClient servicio = new ServiceInventarioCarnes.InventarioCarnesClient();
            int itemPadre = 275801;
            int store = 93; // TODO: Inicializar en un valor adecuado
            int item = 414791; // TODO: Inicializar en un valor adecuado
            string fechaInventario = DateTime.Now.ToString("yyyyMMdd"); // TODO: Inicializar en un valor adecuado
            string fechaLectura = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"); // TODO: Inicializar en un valor adecuado
            int origenFrigorifico = 2; // TODO: Inicializar en un valor adecuado
            Decimal certificadoEmbarque = 456789987; // TODO: Inicializar en un valor adecuado
            int fechaFaena = 140812; // TODO: Inicializar en un valor adecuado
            Decimal pesoNeto = new decimal(23.67); // TODO: Inicializar en un valor adecuado
            Decimal pesoBruto = new decimal(25.01); // TODO: Inicializar en un valor adecuado
            int cajas = 1; // TODO: Inicializar en un valor adecuado
            string responsable = "js"; // TODO: Inicializar en un valor adecuado
            int idMotivonventario = 1; // TODO: Inicializar en un valor adecuado
            var actual = servicio.IngresoItem(itemPadre, store, item, fechaInventario, fechaLectura, origenFrigorifico, certificadoEmbarque, fechaFaena, pesoNeto, pesoBruto, cajas, responsable, idMotivonventario);
            Assert.IsTrue(actual.EstadoResutado == 0);
        }

        [TestMethod()]
        [HostType("ASP.NET")]
        [UrlToTest("http://dsac.dyndns.info:8084/TrazabilidadCarnesService/")]
        public void IngresoItemMotivoPalletTest()
        {
            ServiceInventarioCarnes.InventarioCarnesClient servicio = new ServiceInventarioCarnes.InventarioCarnesClient();
            int itemPadre = 275801;
            int store = 93; // TODO: Inicializar en un valor adecuado
            int item = 403766; // TODO: Inicializar en un valor adecuado
            string fechaInventario = DateTime.Now.ToString("yyyyMMdd"); // TODO: Inicializar en un valor adecuado
            string fechaLectura = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"); // TODO: Inicializar en un valor adecuado
            int origenFrigorifico = 2; // TODO: Inicializar en un valor adecuado
            Decimal certificadoEmbarque = 456789987; // TODO: Inicializar en un valor adecuado
            int fechaFaena = 140109; // TODO: Inicializar en un valor adecuado
            Decimal pesoNeto = new decimal(190.78); // TODO: Inicializar en un valor adecuado
            Decimal pesoBruto = new decimal(0); // TODO: Inicializar en un valor adecuado
            int cajas = 15; // TODO: Inicializar en un valor adecuado
            string responsable = "js"; // TODO: Inicializar en un valor adecuado
            int idMotivonventario = 2; // TODO: Inicializar en un valor adecuado
            var actual = servicio.IngresoItem(itemPadre, store, item, fechaInventario, fechaLectura, origenFrigorifico, certificadoEmbarque, fechaFaena, pesoNeto, pesoBruto, cajas, responsable, idMotivonventario);
            Assert.IsTrue(actual.EstadoResutado == 0);
        }

        /// <summary>
        ///Una prueba de ObtieneItemCaniceria
        ///</summary>
        // TODO: Asegúrese de que el atributo UrlToTest especifica una dirección URL de una página ASP.NET (por ejemplo, 
        // http://.../Default.aspx). Esto es necesario para ejecutar la prueba unitaria en el servidor web,
        // si va a probar una página, un servicio web o un servicio WCF.
        [TestMethod()]
        [HostType("ASP.NET")]
        [UrlToTest("http://dsac.dyndns.info:8084/TrazabilidadCarnesService/InventarioCarnes.svc")]
        public void ObtieneItemCaniceriaTest()
        {
            ServiceInventarioCarnes.InventarioCarnesClient servicio = new ServiceInventarioCarnes.InventarioCarnesClient();
            var actual = servicio.ObtieneItemCaniceria();
            Assert.IsTrue(actual.Any());
        }

        /// <summary>
        ///Una prueba de ObtieneMotivoInventario
        ///</summary>
        // TODO: Asegúrese de que el atributo UrlToTest especifica una dirección URL de una página ASP.NET (por ejemplo, 
        // http://.../Default.aspx). Esto es necesario para ejecutar la prueba unitaria en el servidor web,
        // si va a probar una página, un servicio web o un servicio WCF.
        [TestMethod()]
        [HostType("ASP.NET")]
        [UrlToTest("http://dsac.dyndns.info:8084/TrazabilidadCarnesService/InventarioCarnes.svc")]
        public void ObtieneMotivoInventarioTest()
        {
            ServiceInventarioCarnes.InventarioCarnesClient servicio = new ServiceInventarioCarnes.InventarioCarnesClient();
            var actual = servicio.ObtieneMotivoInventario();
            Assert.IsTrue(actual.Any());
        }*/
    }
}
