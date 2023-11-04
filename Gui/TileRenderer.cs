using OqoSim.Game;
using System.Text.RegularExpressions;

namespace OqoSim.Gui
{
    internal class TileRenderer
    {

        public static readonly string InclineGlyph = Colors.GREEN + "^";

        public static Dictionary<TileType, string> TileGlyphs = new()
        {
            { TileType.Air, " " },
            { TileType.Ground, Colors.BG_GREY + Colors.WHITE + "_" },
            { TileType.Water, Colors.BG_CYAN + "~" },
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
            return TileGlyphs[tile.Type] + Colors.NORMAL;
        }

        public static string GetGlyph(TileType type)
        {
            return TileGlyphs[type] + Colors.NORMAL;
        }

        public static string[] Render(Layer layer, int x0 = 0, int y0 = 0, int height = 0, int width = 0)
        {
            if (_game is null)
                return new string[] { "TileRender not associated with a GameManager. Render Fail." };
            height = height < 1 || y0 + height > layer.Size ? layer.Size-y0 : height;
            width = width < 1 || x0 + width > layer.Size ? layer.Size-x0 : width;
            var slice = new string[height];
            int line = 0;
            var inclineRegex = "(" + Regex.Escape(InclineGlyph + Colors.NORMAL) + "){3,}";
            var actors = _game.World.GetActorsOnLayer(_game.CurrentLayer);
            for (int y = y0; y < y0+height; y++)
            {
                for (int x = x0; x < x0+width; x++)
                {
                    if (y < 0 || x < 0 || y >= layer.Size || x >= layer.Size)
                    {
                        slice[line] += " ";
                    }
                    else
                    {
                        var glyph = _game.World.TileIsCovered(x, y, _game.CurrentLayer) ? InclineGlyph + Colors.NORMAL :
                            _game.World.TileIsSubmerged(x, y, _game.CurrentLayer) ? GetGlyph(TileType.Water) : actors.Where(a => a.X == x && a.Y == y).Any() ? Colors.RED + "@" + Colors.NORMAL: GetGlyph(layer.Tiles[x, y]);
                        slice[line] += glyph;
                    }
                }
                var inclines = Regex.Matches(slice[line], inclineRegex);
                if (inclines is not null)
                {
                    foreach (Match incline in inclines.OrderByDescending(x => x.Length))
                    {
                        var newPart = $"{InclineGlyph}{new String(' ', (incline.Length/(InclineGlyph.Length+Colors.NORMAL.Length))-2)}{InclineGlyph + Colors.NORMAL}";
                        slice[line] = slice[line].Replace(incline.Value, newPart);
                    }
                }
                line++;
            }
            return slice;
        }
    }
}
