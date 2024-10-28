using ksim.Game;
using System.Drawing.Imaging;
using System.Drawing;

namespace ksim.IO
{
    public static class ImageFileManager
    {
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public static void RenderWorldToPNG(World world, string fileName)
        {
            double height = world.Layers.Count;
            double offset = world.Layers.Count / 2 + 1;
            using Bitmap bitmap = new(world.Size, world.Size);
            for (int y = 0; y < world.Size; y++)
                for (int x = 0; x < world.Size; x++)
                {
                    var grounded = world.Layers.Where(a => a.Value.Tiles[x, y].Type == TileType.Ground).ToList().OrderByDescending(a => a.Key);
                    double depth = (grounded.First().Key + offset) / height;
                    //Console.WriteLine($"X: {x} Y: {y} C: {(int)(depth * 255)} {depth}");
                    var c = grounded.First().Key + offset > offset - 1 ? Color.FromArgb((int)(depth * 255), (int)(depth * 255), (int)(depth * 255)) : Color.FromArgb(0, 0, (int)((1 - depth) * 255));
                    bitmap.SetPixel(x, y, c);
                }
            bitmap.Save(File.OpenWrite(fileName), ImageFormat.Png);
        }
    }
}
