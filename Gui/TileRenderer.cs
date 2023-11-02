using OqoSim.Game;
using System.Text.RegularExpressions;

namespace OqoSim.Gui
{
    internal class TileRenderer
    {
        static readonly string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m\x1b[49m";
        static readonly string BLACK = Console.IsOutputRedirected ? "" : "\x1b[30m";
        static readonly string RED = Console.IsOutputRedirected ? "" : "\x1b[31m";
        static readonly string GREEN = Console.IsOutputRedirected ? "" : "\x1b[32m";
        static readonly string YELLOW = Console.IsOutputRedirected ? "" : "\x1b[33m";
        static readonly string BLUE = Console.IsOutputRedirected ? "" : "\x1b[34m";
        static readonly string MAGENTA = Console.IsOutputRedirected ? "" : "\x1b[35m";
        static readonly string CYAN= Console.IsOutputRedirected ? "" : "\x1b[36m";
        static readonly string WHITE = Console.IsOutputRedirected ? "" : "\x1b[37m";
        static readonly string GREY = Console.IsOutputRedirected ? "" : "\x1b[90m";
        static readonly string BRIGHT_RED = Console.IsOutputRedirected ? "" : "\x1b[91m";
        static readonly string BRIGHT_GREEN = Console.IsOutputRedirected ? "" : "\x1b[92m";
        static readonly string BRIGHT_YELLOW = Console.IsOutputRedirected ? "" : "\x1b[93m";
        static readonly string BRIGHT_BLUE = Console.IsOutputRedirected ? "" : "\x1b[94m";
        static readonly string BRIGHT_MAGENTA = Console.IsOutputRedirected ? "" : "\x1b[95m";
        static readonly string BRIGHT_CYAN = Console.IsOutputRedirected ? "" : "\x1b[96m";
        static readonly string BRIGHT_WHITE = Console.IsOutputRedirected ? "" : "\x1b[97m";
        static readonly string BG_BLACK = Console.IsOutputRedirected ? "" : "\x1b[40m";
        static readonly string BG_RED = Console.IsOutputRedirected ? "" : "\x1b[41m";
        static readonly string BG_GREEN = Console.IsOutputRedirected ? "" : "\x1b[42m";
        static readonly string BG_YELLOW = Console.IsOutputRedirected ? "" : "\x1b[43m";
        static readonly string BG_BLUE = Console.IsOutputRedirected ? "" : "\x1b[44m";
        static readonly string BG_MAGENTA = Console.IsOutputRedirected ? "" : "\x1b[45m";
        static readonly string BG_CYAN = Console.IsOutputRedirected ? "" : "\x1b[46m";
        static readonly string BG_WHITE = Console.IsOutputRedirected ? "" : "\x1b[47m";
        static readonly string BG_GREY = Console.IsOutputRedirected ? "" : "\x1b[100m";
        static readonly string BG_BRIGHT_RED = Console.IsOutputRedirected ? "" : "\x1b[101m";
        static readonly string BG_BRIGHT_GREEN = Console.IsOutputRedirected ? "" : "\x1b[102m";
        static readonly string BG_BRIGHT_YELLOW = Console.IsOutputRedirected ? "" : "\x1b[103m";
        static readonly string BG_BRIGHT_BLUE = Console.IsOutputRedirected ? "" : "\x1b[104m";
        static readonly string BG_BRIGHT_MAGENTA = Console.IsOutputRedirected ? "" : "\x1b[105m";
        static readonly string BG_BRIGHT_CYAN = Console.IsOutputRedirected ? "" : "\x1b[106m";
        static readonly string BG_BRIGHT_WHITE = Console.IsOutputRedirected ? "" : "\x1b[107m";

        public static readonly string InclineGlyph = GREEN + "^";

        public static Dictionary<TileType, string> TileGlyphs = new()
        {
            { TileType.Air, " " },
            { TileType.Ground, BG_GREY + WHITE + "_" },
            { TileType.Water, BG_CYAN + "~" },
        };

        private static GameManager? _game;

        public static TileRenderer Instance = new();

        private TileRenderer() {
            // Console.OutputEncoding = Encoding.UTF8;
        }

        public static void AttachGM(GameManager game)
        {
            _game = game;
        }

        public static string GetGlyph(Tile tile)
        {
            return TileGlyphs[tile.Type] + NORMAL;
        }

        public static string GetGlyph(TileType type)
        {
            return TileGlyphs[type] + NORMAL;
        }

        public static string[] Render(Layer layer, int x0 = 0, int y0 = 0, int height = 0, int width = 0)
        {
            if (_game is null)
                return new string[] { "TileRender not associated with a GameManager. Render Fail." };
            height = height < 1 || y0 + height > layer.Size ? layer.Size-y0 : height;
            width = width < 1 || x0 + width > layer.Size ? layer.Size-x0 : width;
            var slice = new string[height];
            int line = 0;
            for (int y = y0; y < y0+height; y++)
            {
                for (int x = x0; x < x0+width; x++)
                {
                    var glyph = _game.World.TileIsCovered(x, y, _game.CurrentLayer) ? InclineGlyph + NORMAL :
                        _game.World.TileIsSubmerged(x, y, _game.CurrentLayer) ? GetGlyph(TileType.Water) : GetGlyph(layer.Tiles[x, y]);
                    slice[line] += glyph;
                }
                var inclines = Regex.Matches(slice[line], "("+Regex.Escape(InclineGlyph+NORMAL)+"){3,}");
                if (inclines is not null)
                {
                    foreach (Match incline in inclines.OrderByDescending(x => x.Length))
                    {
                        var newPart = $"{InclineGlyph}{new String(' ', (incline.Length/(InclineGlyph.Length+NORMAL.Length))-2)}{InclineGlyph + NORMAL}";
                        slice[line] = slice[line].Replace(incline.Value, newPart);
                    }
                }
                line++;
            }
            return slice;
        }
    }
}
