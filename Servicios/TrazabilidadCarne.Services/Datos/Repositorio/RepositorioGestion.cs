using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrazabilidadCarne.Services.Datos.Repositorio
{
    public class RepositorioGestion
    {
        private TrazabilidadCarne.Services.Datos.GestionContenedor _Contenedor = new TrazabilidadCarne.Services.Datos.GestionContenedor();

        public List<vw_DM_LOCAL> ListaLocales()
        {
            string[] formatos = { "819", "826", "890", "815" };
            return _Contenedor.vw_DM_LOCAL.Where(loc => loc.LOCAL_TIPO_ID == 3 && formatos.Contains(loc.FORMATO_COD) && !loc.LOCAL_DESC.Contains("No Abierto")).ToList();
        }
    }
}