using System.Collections.Concurrent;

namespace ksim.Game
{
    public class World
    {
        public ConcurrentBag<IActor> Actors { get; set; }
        public Dictionary<int, Layer> Layers { get; set; }
        public int Seed { get; set; }
        public int Size { get; set; }

        public World(int seed = 1337)
        {
            Actors = new();
            Layers = new();
            Seed = seed;
        }

        public Tile GetTileAtPos(int x, int y, int z) => Layers[z].Tiles[x, y];
        public Tile GetTileAbove(int x, int y, int z) => GetTileAtPos(x, y, z + 1);
        public bool TileIsCovered(int x, int y, int z) => ValidZ(z+1) && GetTileAbove(x, y, z).Type == TileType.Ground;
        public bool TileIsSubmerged(int x, int y, int z) => ValidZ(z+1) && GetTileAbove(x, y, z).Type == TileType.Water;
        public bool ValidZ(int z) => Layers.ContainsKey(z);

        public List<IActor> GetActorsOnLayer(int z) => Actors.Where(a => a.Z == z).ToList();
    }
}
