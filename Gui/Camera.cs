using OqoSim.Game;

namespace OqoSim.Gui
{
    public class Camera
    {
        private Layer? _currentLayer;

        private string[] _lastScreen = Array.Empty<string>();

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

        public void SetPosition((int,int) position)
        {
            X = position.Item1;
            Y = position.Item2;
        }

        public void Move(int xDelta = 0, int yDelta = 0)
        {
            var newPos = (X+xDelta, Y+yDelta);
            SetPosition(newPos);
        }

        public void Draw(bool forceRedraw = false)
        {
            var screen = RenderScreen();
            if (!_lastScreen.SequenceEqual(screen) || forceRedraw)
            {
                _lastScreen = screen;
                Console.Clear();
                foreach (var line in screen)
                    Console.WriteLine(line);
            }
        }

        public void Resize(int height, int width)
        {
            Width = width;
            Height = height;
            X = X > GetMaxX() ? GetMaxX() : X;
            Y = Y > GetMaxY() ? GetMaxY() : Y;
        }

        private string[] RenderScreen()
        {
            return _currentLayer is not null ?
                TileRenderer.Render(_currentLayer, X, Y, Height, Width) : 
                new string[] { "Camera not set to a layer. Render Fail."};
        }
    }
}
