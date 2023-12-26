using System.Collections.Concurrent;

namespace OqoSim.Game
{
    public class World
    {
        public ConcurrentBag<IActor> Actors { get; set; }
        public Dictionary<int, Layer> Layers { get; set; }
        public int Size { get; set; }

        public World() 
        {
            Actors = new();
            Layers = new();
        }

        public Tile GetTileAtPos(int x, int y, int z) => Layers[z].Tiles[x, y];
        public Tile GetTileAbove(int x, int y, int z) => GetTileAtPos(x, y, z + 1);
        public bool TileIsCovered(int x, int y, int z) => GetTileAbove(x, y, z).Type == TileType.Ground;
        public bool TileIsSubmerged(int x, int y, int z) => GetTileAbove(x, y, z).Type == TileType.Water;

        public List<IActor> GetActorsOnLayer(int z) => Actors.Where(a => a.Z == z).ToList();
    }
}
