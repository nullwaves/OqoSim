namespace OqoSim.Game
{
    internal class World
    {
        public Dictionary<int, Layer> Layers { get; set; }

        /// <summary>
        /// Instantiate a world with specified size.
        /// </summary>
        /// <param name="height">Total number of layers in the world excluding layer 0.</param>
        public World(int height = 50, int width = 200)
        {
            var halfHeight = height / 2;
            Layers = new Dictionary<int, Layer>();
            for (int z = -halfHeight; z <= halfHeight; z++)
            {
                Console.WriteLine($"Populating Z: {z}");
                TileType mat = z > 0 ? TileType.Air : TileType.Ground; // 0 is "sea level"
                Layers.Add(z, new Layer(width, mat));
            }
            var noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            //var noisemap = new float[width, width];
            for (int x = 0; x < width; x++)
            {
                Console.WriteLine($"Adding noise along {x}");
                for (int y = 0; y < width; y++)
                {
                    var lnoise = noise.GetNoise(x, y);
                    int elevation = (int)(lnoise * halfHeight / 2);
                    if (elevation < 0)
                    {
                        for (int z = 0; z > elevation; z--)
                            GetTileAtPos(x, y, z).Type = TileType.Water;
                    }
                    else if (elevation > 0)
                    {
                        elevation /= 3;
                        for (int z = 0; z <= elevation; z++)
                            GetTileAtPos(x, y, z).Type = TileType.Ground;
                    }
                }
            }
        }

        public Tile GetTileAtPos(int x, int y, int z) => Layers[z].Tiles[x, y];
        public Tile GetTileAbove(int x, int y, int z) => GetTileAtPos(x, y, z + 1);
        public bool TileIsCovered(int x, int y, int z) => GetTileAbove(x,y,z).Type == TileType.Ground;
        public bool TileIsSubmerged(int x, int y, int z) => GetTileAbove(x,y,z).Type == TileType.Water;
    }
}
