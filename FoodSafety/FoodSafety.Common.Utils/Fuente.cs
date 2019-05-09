using Android.App;
using Android.Graphics;

namespace FoodSafety.Common.Utils
{
    /// <summary>
    /// Fuente.
    /// </summary>
    public class Fuente
    {
        Activity activity;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FoodSafety.Common.Utils.Fuente"/> class.
        /// </summary>
        public Fuente()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FoodSafety.Common.Utils.Fuente"/> class.
        /// </summary>
        /// <param name="activity">Activity.</param>
        public Fuente(Activity activity)
        {
            this.activity = activity;
        }

        /// <summary>
        /// Fuentes the roboto regular.
        /// </summary>
        /// <returns>The roboto regular.</returns>
        public Typeface fuenteRobotoRegular()
        {
            return Typeface.CreateFromAsset(activity.Assets, "fonts/Roboto/Roboto-Regular.ttf");
        }

        /// <summary>
        /// Fuentes the roboto medium.
        /// </summary>
        /// <returns>The roboto medium.</returns>
        public Typeface fuenteRobotoMedium()
        {
            return Typeface.CreateFromAsset(activity.Assets, "fonts/Roboto/Roboto-Medium.ttf");
        }

        /// <summary>
        /// Fuentes the roboto bold.
        /// </summary>
        /// <returns>The roboto bold.</returns>
        public Typeface fuenteRobotoBold()
        {
            return Typeface.CreateFromAsset(activity.Assets, "fonts/Roboto/Roboto-Bold.ttf");
        }
    }
}