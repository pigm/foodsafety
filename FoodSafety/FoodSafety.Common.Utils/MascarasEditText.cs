using System;
namespace FoodSafety.Common.Utils
{
    /// <summary>
    /// Mascaras edit text.
    /// </summary>
    public class MascarasEditText
    {
        /// <summary>
        /// Formatears the monto.
        /// </summary>
        /// <returns>The monto.</returns>
        /// <param name="montoKG">Monto.</param>
        public static string formatearMonto(string montoKG)
        {
            string montoPesoKG;
            if (montoKG == null || montoKG.Equals(""))
            {
                montoPesoKG = string.Empty;
            }
            else
            {
                montoKG = limpiarMonto(montoKG);

                long num = Convert.ToInt64(montoKG);
                montoKG = String.Format("{0:N0}", num);

                montoPesoKG = montoKG.Replace(".", ".");
                montoPesoKG = montoKG;
            }
            return montoPesoKG;
        }

        /// <summary>
        /// Limpiars the monto pesos.
        /// </summary>
        /// <returns>The monto pesos.</returns>
        /// <param name="monto">Monto.</param>
        public static string limpiarMonto(string monto)
        {
            monto = monto.Replace(".", "");
            return monto;
        }
    }
}