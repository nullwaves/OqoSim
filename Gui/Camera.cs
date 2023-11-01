using OqoSim.Game;

namespace OqoSim.Gui
{
    internal class Camera
    {
        private Layer _currentLayer;

        private string[] _lastScreen = new string[0];

        public int Height { get; private set; }
        public int Width { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        private int _maxX => _currentLayer.Size - Width - 1;
        private int _maxY => _currentLayer.Size - Height - 1;

        public Camera(int height, int width)
        {
            Height = height;
            Width = width;
            X = 0;
            Y = 0;
        }

        public void SetLayer(Layer layer)
        {
            _currentLayer = layer;
        }

        public void SetPosition((int,int) position)
        {
            var newX = position.Item1;
            var newY = position.Item2;
            if(newX >= 0 &&
                newY >= 0 &&
                newX <= _maxX &&
                newY <= _maxY)
            {
                X = newX;
                Y = newY;
                return;
            }
            // throw new IndexOutOfRangeException();
        }

        public void Move(int xDelta = 0, int yDelta = 0)
        {
            var newPos = (X+xDelta, Y+yDelta);
            SetPosition(newPos);
        }

        public void Draw()
        {
            var screen = RenderScreen();
            if (!_lastScreen.SequenceEqual(screen))
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
            X = X > _maxX ? _maxX : X;
            Y = Y > _maxY ? _maxY : Y;
        }

        private string[] RenderScreen()
        {
            return TileRenderer.Instance.Render(_currentLayer, X, Y, Height, Width);
        }
    }
}
