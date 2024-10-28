using ksim.Game;
using System.Text.RegularExpressions;

namespace ksim.Gui
{
    public class Camera
    {
        private Layer? _currentLayer;

        private ConsoleScreen _lastScreen = new(0, 0);

        public Menu? Menu;

        public int Height { get; private set; }
        public int Width { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }


        public Camera(int height, int width)
        {
            Height = height;
            Width = width;
            X = 0;
            Y = 0;
        }

        public int GetMaxX()
        {
            return _currentLayer is not null ? _currentLayer.Size - Width - 1 : 0;
        }

        public int GetMaxY()
        {
            return _currentLayer is not null ? _currentLayer.Size - Height - 1 : 0;
        }

        public void SetLayer(Layer layer)
        {
            _currentLayer = layer;
        }

        public void SetPosition((int, int) position)
        {
            X = position.Item1;
            Y = position.Item2;
        }

        public void Move(int xDelta = 0, int yDelta = 0)
        {
            var newPos = (X + xDelta, Y + yDelta);
            SetPosition(newPos);
        }

        public void Draw(bool forceRedraw = false)
        {
            var screen = RenderScreen();
            if (_lastScreen.Height != Height || _lastScreen.Width != Width || GetScreenDiffs(_lastScreen, screen).Count > 0 || forceRedraw)
            {
                _lastScreen = screen;
                string cleaned = CleanInclines(screen.ToString());
                Console.SetCursorPosition(0, 0);
                Console.Write(cleaned);
            }
        }

        private static string CleanInclines(string screen)
        {
            var InclineGlyph = TileRenderer.InclineGlyph;
            var inclineRegex = "(" + Regex.Escape(InclineGlyph + Colors.NORMAL) + "){3,}";
            var inclines = Regex.Matches(screen, inclineRegex);
            if (inclines is not null)
            {
                foreach (Match incline in inclines.OrderByDescending(x => x.Length))
                {
                    var newPart = $"{InclineGlyph}{new String(' ', (incline.Length / (InclineGlyph.Length + Colors.NORMAL.Length)) - 2)}{InclineGlyph + Colors.NORMAL}";
                    screen = screen.Replace(incline.Value, newPart);
                }
            }
            return screen;
        }

        public void Resize(int height, int width)
        {
            Width = width;
            Height = height;
        }

        private ConsoleScreen RenderScreen()
        {
            var tiles = _currentLayer is not null ?
                TileRenderer.Render(X, Y, Height, Width) :
                throw new NullReferenceException("Camera has no active layer!");
            if (Menu is not null)
            {
                ConsoleScreen menu = Menu.Render(Height, Width);
                for (int y = 0; y < Math.Min(menu.Height, tiles.Height); y++)
                    for (int x = 0; x < Math.Min(menu.Width, tiles.Width); x++)
                        tiles.Pixels[y, x] = menu.Pixels[y, x].Length > 0 ? menu.Pixels[y, x] : tiles.Pixels[y, x];
            }
            return tiles;
        }

        private static List<(int, int, string)> GetScreenDiffs(ConsoleScreen oldScreen, ConsoleScreen newScreen)
        {
            var results = new List<(int, int, string)>();
            var h = Math.Min(oldScreen.Height, newScreen.Height);
            var w = Math.Min(oldScreen.Width, newScreen.Width);
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    if (oldScreen.Pixels[y, x] != newScreen.Pixels[y, x])
                        results.Add((y, x, newScreen.Pixels[y, x]));
            return results;
        }
    }
}
