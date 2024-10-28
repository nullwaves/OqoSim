namespace ksim.Game
{
    public enum TileType
    {
        Air = 0,
        Water = 5,
        Ground = 10,
    }

    public class Tile
    {
        public TileType Type { get; set; }

        public Tile(TileType type = TileType.Air)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
