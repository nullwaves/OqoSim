using OqoSim.Game;
using System.Text.RegularExpressions;

namespace OqoSim.Gui
{
    internal class TileRenderer
    {
        static string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";
        static string RED = Console.IsOutputRedirected ? "" : "\x1b[91m";
        static string GREEN = Console.IsOutputRedirected ? "" : "\x1b[92m";
        static string YELLOW = Console.IsOutputRedirected ? "" : "\x1b[93m";
        static string BLUE = Console.IsOutputRedirected ? "" : "\x1b[94m";
        static string MAGENTA = Console.IsOutputRedirected ? "" : "\x1b[95m";
        static string CYAN = Console.IsOutputRedirected ? "" : "\x1b[96m";
        static string GREY = Console.IsOutputRedirected ? "" : "\x1b[97m";

        public static Dictionary<TileType, string> TileGlyphs = new Dictionary<TileType, string>()
        {
            { TileType.Air, " " },
            { TileType.Ground, "_" },
            { TileType.Water, CYAN + "~" },
        };

        private static GameManager _game;

        public static TileRenderer Instance = new TileRenderer();

        private TileRenderer() {
            // Console.OutputEncoding = Encoding.UTF8;
        }

        public void AttachGM(GameManager game)
        {
            _game = game;
        }

        public string GetGlyph(Tile tile)
        {
            return TileGlyphs[tile.Type] + NORMAL;
        }

        public string GetGlyph(TileType type)
        {
            return TileGlyphs[type] + NORMAL;
        }

        public string[] Render(Layer layer, int x0 = 0, int y0 = 0, int height = 0, int width = 0)
        {
            height = height < 1 || y0 + height > layer.Size ? layer.Size-y0 : height;
            width = width < 1 || x0 + width > layer.Size ? layer.Size-x0 : width;
            var slice = new string[height];
            int line = 0;
            for (int y = y0; y < y0+height; y++)
            {
                for (int x = x0; x < x0+width; x++)
                {
                    slice[line] += _game.World.TileIsCovered(x,y,_game.CurrentLayer) ? "^" :
                        _game.World.TileIsSubmerged(x,y,_game.CurrentLayer) ? GetGlyph(TileType.Water) : GetGlyph(layer.Tiles[x, y]);
                }
                var inclines = Regex.Matches(slice[line], "\\^{3,}");
                if (inclines is not null)
                {
                    foreach (Match incline in inclines.OrderByDescending(x => x.Length))
                    {
                        var newPart = $"^{new String(' ', incline.Length-2)}^";
                        slice[line] = slice[line].Replace(incline.Value, newPart);
                    }
                }
                line++;
            }
            return slice;
        }
    }
}
