using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public static class EntityPalette
    {
        private const float GoldenRatioConjugate = 0.61803398875f;

        public static Color ColorFor(int pairId)
        {
            float hue = (pairId * GoldenRatioConjugate) % 1f;
            return Color.HSVToRGB(hue, 0.65f, 0.95f);
        }
    }
}
