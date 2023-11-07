using System.Text;

namespace OqoSim.Gui
{
    public struct ConsoleScreen
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        public string[,] Pixels;

        public ConsoleScreen(int height, int width, string defaultPixel = " ")
        {
            Height = height;
            Width = width;
            Pixels = new string[height, width];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    Pixels[y, x] = defaultPixel;
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

}
