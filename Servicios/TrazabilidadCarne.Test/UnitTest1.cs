using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrazabilidadCarne.Test
{
    /// <summary>
    /// Descripción resumida de UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtiene o establece el contexto de las pruebas que proporciona
        ///información y funcionalidad para la ejecución de pruebas actual.
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
        // Puede usar los siguientes atributos adicionales conforme escribe las pruebas:
        //
        // Use ClassInitialize para ejecutar el código antes de ejecutar la primera prueba en la clase
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup para ejecutar el código una vez ejecutadas todas las pruebas en una clase
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Usar TestInitialize para ejecutar el código antes de ejecutar cada prueba 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup para ejecutar el código una vez ejecutadas todas las pruebas
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /*[TestMethod]
        public void TestMethod1()
        {
            TrazabilidadCarnesService.BarraClient servicio = new TrazabilidadCarnesService.BarraClient();
            servicio.ProcesaBarras("24166666642233325133300000000028282828280", "1121057931020024883302002588", "201");
        }


        [TestMethod]
        public void TestObtieneLocales()
        {
            TrazabilidadCarnesSericeLocal.LocalClient servicio = new TrazabilidadCarnesSericeLocal.LocalClient();
            Services.EntidadLocal[] listaLocales = servicio.ObtieneLocales();
            foreach (Services.EntidadLocal local in listaLocales)
            {
                string nombreLocal = local.NombreLocal;
            }
            Assert.IsTrue(listaLocales.Any());
        }

        [TestMethod]
        public void TestObtieneItemBalanza()
        {
            TrazabilidadCarnesService.BarraClient servicio = new TrazabilidadCarnesService.BarraClient();
            Services.EntidadItem[] listaItem = servicio.ObtieneItemBalanza();
            foreach (Services.EntidadItem Item in listaItem)
            {
                string ItemDescripcion = Item.ItemDescripcion;
            }
            Assert.IsTrue(listaItem.Any());
        }

        [TestMethod]
        public void TestObtieneFrigorificos()
        {
            TrazabilidadCarnesService.BarraClient servicio = new TrazabilidadCarnesService.BarraClient();
            Services.EntidadFrigorifico[] listaFrigorifico = servicio.ObtieneFrigorificos();
            foreach (Services.EntidadFrigorifico frigorifico in listaFrigorifico)
            {
                string NombreFrigorifico = frigorifico.NombreFrigorifico;
            }
            Assert.IsTrue(listaFrigorifico.Any());
        }

        [TestMethod]
        public void TestObtieneParidades()
        {
            TrazabilidadCarnesService.BarraClient servicio = new TrazabilidadCarnesService.BarraClient();
            Services.EntidadParidad[] listaParidad = servicio.ObtieneParidades();
            foreach (Services.EntidadParidad paridad in listaParidad)
            {
                decimal itemCompra = paridad.ItemCompra;
                decimal itemVenta = paridad.ItemVenta;
            }
            Assert.IsTrue(listaParidad.Any());
        }


        [TestMethod]
        public void TestMethod2()
        {
            TrazabilidadCarnesService.BarraClient servicio = new TrazabilidadCarnesService.BarraClient();
            servicio.ProcesaBarrasMSG("156551|1|004|002|00000001234123412|130805|001850|001945|24115655142200425100200000000012341234121113080531020018503302001945", "201");
        }

        [TestMethod]
        public void TestBaraMensaje()
        {
            TrazabilidadCarnesService.BarraClient servicio = new TrazabilidadCarnesService.BarraClient();
            servicio.ProcesaBarrasMSG("450339|2|005|25||130525|0|0|", "87");
        }*/
    }
}
