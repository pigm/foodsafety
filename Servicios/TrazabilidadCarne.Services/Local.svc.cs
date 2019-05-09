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
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Local" en el código, en svc y en el archivo de configuración a la vez.
    [ServiceErrorBehavior(typeof(ErrorHandler))]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Local : ILocal
    {
        public List<EntidadLocal> ObtieneLocales()
        {
            Datos.Repositorio.RepositorioGestion repositorioGestion = new Datos.Repositorio.RepositorioGestion();
            List<vw_DM_LOCAL> listaLocalFormato = repositorioGestion.ListaLocales();
            List<EntidadLocal> listaLocales = new List<EntidadLocal>();
            foreach (vw_DM_LOCAL localFormato in listaLocalFormato)
            {
                listaLocales.Add(new EntidadLocal { CodigoLocal = localFormato.LOCAL_COD, NombreLocal = localFormato.LOCAL_DESC, CodigoFormato = localFormato.FORMATO_COD, NombreFormato = localFormato.FORMATO_DESC });
            }
            //listaLocales.Add(new EntidadLocal { CodigoLocal = "001", NombreLocal = "001 PLAZA LYON", CodigoFormato = "826", NombreFormato = "826 EXPRESS" });
            //listaLocales.Add(new EntidadLocal { CodigoLocal = "002", NombreLocal = "002 PEDRO DE VALDIVIA", CodigoFormato = "826", NombreFormato = "826 EXPRESS" });
            //listaLocales.Add(new EntidadLocal { CodigoLocal = "004", NombreLocal = "004 VITACURA", CodigoFormato = "826", NombreFormato = "826 EXPRESS" });
            //listaLocales.Add(new EntidadLocal { CodigoLocal = "097", NombreLocal = "097 PUENTE NUEVO", CodigoFormato = "819", NombreFormato = "819 HIPER" });
            //listaLocales.Add(new EntidadLocal { CodigoLocal = "058", NombreLocal = "058 VIÑA DEL MAR", CodigoFormato = "819", NombreFormato = "819 HIPER" });
            //listaLocales.Add(new EntidadLocal { CodigoLocal = "075", NombreLocal = "075 MAIPU", CodigoFormato = "819", NombreFormato = "819 HIPER" });
            //listaLocales.Add(new EntidadLocal { CodigoLocal = "201", NombreLocal = "201 SAN DIEGO", CodigoFormato = "200", NombreFormato = "200 EKONO" });
            //listaLocales.Add(new EntidadLocal { CodigoLocal = "202", NombreLocal = "202 CURICO", CodigoFormato = "200", NombreFormato = "200 EKONO" });
            //listaLocales.Add(new EntidadLocal { CodigoLocal = "203", NombreLocal = "203 SAN PABLO", CodigoFormato = "200", NombreFormato = "200 EKONO" });
            return listaLocales;
        }
    }
}
