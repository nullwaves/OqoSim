using Newtonsoft.Json;
using OqoSim.Gui;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OqoSim.Game
{
    internal interface IGameState
    {
        public string Name { get; }

        public void HandleInput(GameManager gameManager, ConsoleKeyInfo consoleKeyInfo);
        public void Update(GameManager game);
    }

    internal class DefaultState : IGameState
    {
        public string Name => "Default";

        public void Update(GameManager game)
        {
            //game.Tick();
            game.Camera.Draw();
        }

        public void HandleInput(GameManager game, ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                game.SetPaused();
                game.Camera.Menu = new Menu()
                {
                    Title = "Paused",
                    Items = new List<MenuItem> { new MenuItem() { Title = "Esc - Resume" }, new MenuItem() { Title = "Ctrl+C - Quit" } },
                    Top = game.Camera.Height / 2,
                    Left = game.Camera.Width / 2,
                };
                game.SetState(new PausedState());
            }
            else if (key.Key == ConsoleKey.W)
                game.Camera.Move(0, -10);
            else if (key.Key == ConsoleKey.S)
            {
                if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    game.SetPaused();
                    Console.WriteLine("Beginning save...");
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    };
                    File.WriteAllText("world.oqo", JsonConvert.SerializeObject(game.World, typeof(World), settings));
                    Console.WriteLine("Saved.");
                    game.SetPaused(false);
                }
                else game.Camera.Move(0, 10);
            }
            else if (key.Key == ConsoleKey.A)
                game.Camera.Move(-10, 0);
            else if (key.Key == ConsoleKey.D)
                game.Camera.Move(10, 0);
            else if (key.Key == ConsoleKey.E)
                game.MoveZ(1);
            else if (key.Key == ConsoleKey.C)
                game.MoveZ(-1);
            else if (key.Key == ConsoleKey.G)
            {
                if (game.Camera.X < game.World.Size && game.Camera.X > -1 &&
                    game.Camera.Y < game.World.Size && game.Camera.Y > -1)
                {
                    Dwarf dwarf = new() { X = game.Camera.X, Y = game.Camera.Y, Z = game.CurrentLayer };
                    game.World.Actors.Add(dwarf);
                }
            }
            else if (key.Key == ConsoleKey.Q)
                game.SimulationSpeed++;
            else if (key.Key == ConsoleKey.Z)
                game.SimulationSpeed--;
            else if (key.Key == ConsoleKey.F1)
            {
                game.Camera.Menu = game.Camera.Menu is null ? new Menu()
                {
                    Border = ".",
                    Title = "Help",
                    Items = new List<MenuItem> {
                        new MenuItem() { Title = "F1        - Help" },
                        new MenuItem() { Title = "Esc       - Pause"},
                        new MenuItem() { Title = "W/A/S/D   - Move Camera"},
                        new MenuItem() { Title = "E/C       - Ascend/Descend"},
                        new MenuItem() { Title = "G         - Place Actor" },
                        new MenuItem() { Title = "Q/Z       - Increase/Decrease Sim Speed"},
                        new MenuItem() { Title = "Ctrl+C    - Quit" } },
                    Top = 4,
                    Left = 8,
                } : null ;
            }
            else if (key.Key == ConsoleKey.F2 && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var path = $"render/{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
                if (!Directory.Exists("render/")) Directory.CreateDirectory("render/");
                Console.WriteLine($"Rendering to \"{path}\"");
                //if (!File.Exists("render/")) File.Create("render/");
                RenderWorldToPNG(game.World, path);
                Console.Beep();
            }
        }

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        static void RenderWorldToPNG(World world, string fileName)
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
                    var c = grounded.First().Key + offset > offset-1 ? Color.FromArgb((int)(depth * 255), (int)(depth * 255), (int)(depth * 255)) : Color.FromArgb(0,0,(int)((1-depth)*255));
                    bitmap.SetPixel(x, y, c);
                }
            bitmap.Save(File.OpenWrite(fileName), ImageFormat.Png);
        }
    }

    internal class PausedState : IGameState
    {
        public string Name => "Paused";
        public void Update(GameManager game)
        {
            game.Camera.Draw();
            //Console.WriteLine("PAUSED");
        }

        public void HandleInput(GameManager game, ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                game.Camera.Menu = null;
                game.SetPaused(false);
                game.SetState(new DefaultState());
                game.Camera.Draw(true);
            }
        }
    }
}
