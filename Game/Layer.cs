namespace OqoSim.Game
{
    public class Layer
    {
        public int Size { get; private set; }
        public Tile[,] Tiles { get; set; }

        public Layer(int size, TileType fillMaterial = TileType.Air)
        {
            Size = size;
            Tiles = new Tile[Size, Size];
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Tiles[x, y] = new Tile(fillMaterial);
                }
            }
        }
    }
}
