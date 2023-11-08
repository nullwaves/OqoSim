using OqoSim.Game;

namespace OqoSim.Gui
{
    internal class TileRenderer
    {

        public static readonly string InclineGlyph = Colors.GREEN + "^";
        public static readonly string ActorGlyph = Colors.RED + "@";

        public static Dictionary<TileType, string> TileGlyphs = new()
        {
            { TileType.Air, " " },
            { TileType.Ground, Colors.BG_GREY + Colors.WHITE + "_" },
            { TileType.Water, Colors.BG_CYAN + "~" },
        };

        private static GameManager? _game;

        public static TileRenderer Instance = new();

        private TileRenderer()
        {
            // Console.OutputEncoding = Encoding.UTF8;
        }

        public static void AttachGM(GameManager game)
        {
            _game = game;
        }

        public static string GetGlyph(Tile tile)
        {
            return TileGlyphs[tile.Type] + Colors.NORMAL;
        }

        public static string GetGlyph(TileType type)
        {
            return TileGlyphs[type] + Colors.NORMAL;
        }

        public static string RenderTileAtPos(int x, int y, int z)
        {
            if (_game is null) throw new NullReferenceException("GameManager not set in TileRenderer");
            if (y < 0 || x < 0 || y >= _game.World.Size || x >= _game.World.Size)
            {
                return " ";
            }
            if (_game.World.TileIsCovered(x, y, z))
                return InclineGlyph + Colors.NORMAL;
            if (_game.World.TileIsSubmerged(x, y, z))
                return GetGlyph(TileType.Water);
            if (_game.World.Actors.Where(a => a is not null).Where(a => a.X == x && a.Y == y && a.Z == z).Any())
                return ActorGlyph + Colors.NORMAL;
            return GetGlyph(_game.World.GetTileAtPos(x, y, z));
        }

        public static ConsoleScreen Render(int x0 = 0, int y0 = 0, int height = 0, int width = 0)
        {
            if (_game is null) throw new NullReferenceException("GameManager not set in TileRenderer");
            var slice = new ConsoleScreen(height, width);
            int cY = 0;
            int cX = 0;
            for (int y = y0; y < y0 + height; y++)
            {
                for (int x = x0; x < x0 + width; x++)
                {
                    slice.Pixels[cY, cX] = RenderTileAtPos(x, y, _game.CurrentLayer);
                    cX++;
                }
                cX = 0;
                cY++;
            }
            return slice;
        }
    }
}
