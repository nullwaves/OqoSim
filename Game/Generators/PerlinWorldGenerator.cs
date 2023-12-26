namespace OqoSim.Game.Generators
{
    class PerlinWorldGenerator : IWorldGenerator
    {
        public static string Name => "Perlin Noise";

        public static World Generate(int seed, int layersBelowZero, int layersAboveZero, int width)
        {
            World world = new()
            {
                Size = width
            };
            for (int z = -layersBelowZero; z <= layersAboveZero; z++)
            {
                if (z % 5 == 0) Console.WriteLine($"Populating Z: {z}");
                TileType mat = z > 0 ? TileType.Air : TileType.Ground; // 0 is "sea level"
                world.Layers.Add(z, new Layer(width, mat));
            }
            var noise = new FastNoiseLite(seed);
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            //var noisemap = new float[width, width];
            Console.WriteLine("Adding noise...");
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    var lnoise = noise.GetNoise(x, y);
                    //lnoise *= 1.2f;
                    int elevation = (int)(lnoise * layersAboveZero);
                    elevation = elevation >= -layersBelowZero ? elevation : -layersBelowZero;
                    elevation = elevation <= layersAboveZero ? elevation : layersAboveZero;
                    if (elevation < 0)
                    {
                        for (int z = 0; z > elevation; z--)
                            world.GetTileAtPos(x, y, z).Type = TileType.Water;
                    }
                    else if (elevation > 0)
                    {
                        // elevation /= 3;
                        for (int z = 0; z <= elevation; z++)
                            world.GetTileAtPos(x, y, z).Type = TileType.Ground;
                    }
                }
            }
            return world;
        }
    }
}
