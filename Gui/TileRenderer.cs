using OqoSim.Game;
using System.Text;
using System.Text.RegularExpressions;

namespace OqoSim.Gui
{
    public struct ConsoleScreen
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        public string[,] Pixels;

        public ConsoleScreen(int height, int width)
        {
            Height = height;
            Width = width;
            Pixels = new string[height, width];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    Pixels[y, x] = " ";
        }

        public readonly string[] ToLines()
        {
            string[] lines = new string[Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    lines[y] += Pixels[y, x];
            return lines;
        }

        public override readonly string ToString()
        {
            var lines = ToLines();
            StringBuilder sb = new();
            foreach (string line in lines)
            {
                sb.Append(line + Environment.NewLine);
            }
            return sb.ToString();
        }
    }

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
            if (_game.World.Actors.Where(a => a.X == x && a.Y == y && a.Z == z).Any())
                return ActorGlyph + Colors.NORMAL;
            return GetGlyph(_game.World.GetTileAtPos(x, y, z));
        }

        public static ConsoleScreen Render(int x0 = 0, int y0 = 0, int height = 0, int width = 0)
        {
            if (_game is null) throw new NullReferenceException("GameManager not set in TileRenderer");
            var slice = new ConsoleScreen(height, width);
            //var inclineRegex = "(" + Regex.Escape(InclineGlyph + Colors.NORMAL) + "){3,}";
            //var actors = _game.World.GetActorsOnLayer(_game.CurrentLayer);
            int cY = 0;
            int cX = 0;
            for (int y = y0; y < y0 + height; y++)
            {
                for (int x = x0; x < x0 + width; x++)
                {
                    slice.Pixels[cY, cX] = RenderTileAtPos(x, y, _game.CurrentLayer);
                    cX++;
                }
                //var inclines = Regex.Matches(slice[line], inclineRegex);
                //if (inclines is not null)
                //{
                //    foreach (Match incline in inclines.OrderByDescending(x => x.Length))
                //    {
                //        var newPart = $"{InclineGlyph}{new String(' ', (incline.Length/(InclineGlyph.Length+Colors.NORMAL.Length))-2)}{InclineGlyph + Colors.NORMAL}";
                //        slice[line] = slice[line].Replace(incline.Value, newPart);
                //    }
                //}
                cX = 0;
                cY++;
            }
            return slice;
        }
    }
}
