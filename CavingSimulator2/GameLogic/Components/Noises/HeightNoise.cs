using DotnetNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Components.Noises
{
    public class HeightNoise
    {
        private static HeightNoise instance = new HeightNoise();
        public static HeightNoise GetInstance() { return instance; }
        public const float maxHeight = 20f;
        public const float minHeight = 4f;

        public FastNoise noise;
        private HeightNoise()
        {
            noise = new FastNoise();
            noise.UsedNoiseType = FastNoise.NoiseType.PerlinFractal;
            noise.Frequency = 0.03f;

            noise.FractalTypeMethod = FastNoise.FractalType.Fbm;

            noise.Octaves = 3;
            noise.Lacunarity = 2f;
            noise.Gain = 0.5f;
            //noise.UsedCellularDistanceFunction = FastNoise.CellularDistanceFunction.Euclidean;
            //noise.UsedCellularReturnType = FastNoise.CellularReturnType.Distance2Add;
            //noise.CellularJitter = 1f;


        }
        public static int GetHeight(int x,int y)
        {
            return (int)((instance.noise.GetPerlinFractal(x, y) + 0.5f) *  (maxHeight - minHeight)) + (int)minHeight;
            return (int)(MathF.Abs(MathF.Pow(instance.noise.GetPerlin(x, y),3)) * 3f * (maxHeight-minHeight)) + (int)minHeight;
        }

        
    }
}
