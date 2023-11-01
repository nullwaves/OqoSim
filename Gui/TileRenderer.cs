using OqoSim.Game;

namespace OqoSim.Gui
{
    internal class TileRenderer
    {
        public static Dictionary<TileType, string> TileGlyphs = new Dictionary<TileType, string>()
        {
            { TileType.Air, " " },
            { TileType.Ground, "_" },
            { TileType.Water, "~" },
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
            return TileGlyphs[tile.Type];
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
                    slice[line] += _game.World.TileIsCovered(x,y,_game.CurrentLayer) ? "^" : GetGlyph(layer.Tiles[x, y]);
                }
                line++;
            }
            return slice;
        }
    }
}
