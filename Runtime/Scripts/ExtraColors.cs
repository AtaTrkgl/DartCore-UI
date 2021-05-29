using UnityEngine;

namespace DartCore.UI
{
    public class ExtraColors
    {
        public static Color orange = new Color(1f, .5f, 0f);
        public static Color teal = new Color(0f, .5f, .5f);
        public static Color purple = new Color(.5f, 0f, 1f);
        public static Color pink = new Color(244f / 255, 143f / 255, 177f / 255);
        public static Color pineGreen = new Color(0f, .35f, 0f);

        public static Color transparant = new Color(1f, 1f, 1f,0f);

        public static Color emerald = new Color(46f / 255, 204f / 255, 113f / 255);
        public static Color amethyst = new Color(155f / 255f, 89f / 255f, 182f / 255f);
        public static Color silver = new Color(189f / 255f, 195f / 255f, 199f / 255f);
        public static Color gold = new Color(255f / 255f, 215f / 255f, 0f);
        public static Color bronze = new Color(205f / 255f, 127f / 255f, 50f / 255f);

        /// <summary>
        /// returns a new color that's between white and the provided color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="t">a value between 0 and 1 which determines density of the color</param>
        /// <returns></returns>
        public static Color GetTransitionColor(Color color, float t = .5f)
        {
            return Color.Lerp(Color.white, color, Mathf.Clamp01(t));
        }
        
        /// <summary>
        /// Replaces the Alpha of the color with the given value
        /// </summary>
        public static Color GetColorWithAlpha(Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);
        
        /// <summary>
        /// Replaces the Red of the color with the given value
        /// </summary>
        public static Color GetColorWithRed(Color color, float red) => new Color(red, color.g, color.b, color.a);
        
        /// <summary>
        /// Replaces the Green of the color with the given value
        /// </summary>
        public static Color GetColorWithGreen(Color color, float green) => new Color(color.r, green, color.b, color.a);
        
        /// <summary>
        /// Replaces the Blue of the color with the given value
        /// </summary>
        public static Color GetColorWithBlue(Color color, float blue) => new Color(color.r, color.g, blue, color.a);
    }
}