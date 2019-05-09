using System;
using System.Linq;
namespace FoodSafety.Common.Utils
{
    /// <summary>
    /// Validador barras.
    /// </summary>
    public class ValidadorBarras
    {
        /// <summary>
        /// Validadors the barras refresh.
        /// </summary>
        /// <returns>The barras refresh.</returns>
        public static string ValidadorBarrasRefresh(string code)
        {
            var sum = code.Reverse().Select((c, i) => (int)char.GetNumericValue(c) * (i % 2 == 0 ? 3 : 1)).Sum();
            var result = (10 - sum % 10) % 10;
            return code+result.ToString();
        }
    }
}
