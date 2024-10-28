using ksim.Gui;
using ksim.IO;
using System.Runtime.InteropServices;

namespace ksim.Game.States
{
    internal class PausedState : IGameState
    {
        public string Name => "Paused";

        public void Start(GameManager game)
        {
            game.SetPaused();
            game.Camera.Menu = new Menu()
            {
                Border = Colors.BLUE + "#" + Colors.NORMAL,
                Title = "Paused",
                Items = new List<MenuItem> {
                        new("Esc    - Resume"),
                        new("Ctrl+S - Save"),
                        new("F2     - Render"),
                        new("---------------"),
                        new("Ctrl+C - Quit"),
                        },
                Top = game.Camera.Height / 2,
                Left = game.Camera.Width / 2,
            };
        }

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
            else if (key.Key == ConsoleKey.S && key.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                var success = WorldFileManager.SaveWorldToFile(game.World, "world.ksim");
                Console.WriteLine($"World saved: {success}");
            }
            else if (key.Key == ConsoleKey.F2 && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var path = $"render/{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
                if (!Directory.Exists("render/")) Directory.CreateDirectory("render/");
                Console.WriteLine($"Rendering to \"{path}\"");
                //if (!File.Exists("render/")) File.Create("render/");
                ImageFileManager.RenderWorldToPNG(game.World, path);
                Console.Beep();
            }
        }
    }
}
