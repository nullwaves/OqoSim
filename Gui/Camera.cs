using OqoSim.Game;

namespace OqoSim.Gui
{
    public class Camera
    {
        private Layer? _currentLayer;

        private ConsoleScreen _lastScreen = new(0, 0);

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
                var lines = screen.ToLines();
                Console.SetCursorPosition(0, 0);
                Console.Write(screen);
            }
            //else
            //{
            //    var diffs = GetScreenDiffs(_lastScreen, screen);
            //    if (diffs.Count > Height * Width / 8)
            //        Draw(true);
            //    _lastScreen = screen;
            //    foreach(var d in diffs)
            //    {
            //        Console.SetCursorPosition(d.Item2, d.Item1);
            //        Console.Write(d.Item3);
            //    }
            //}
        }

        public void Resize(int height, int width)
        {
            Width = width;
            Height = height;
        }

        private ConsoleScreen RenderScreen()
        {
            return _currentLayer is not null ?
                TileRenderer.Render(X, Y, Height, Width) :
                throw new NullReferenceException("Camera has no active layer!");
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
