using ksim.Gui;

namespace ksim.Game.States
{
    internal class DefaultState : IGameState
    {
        public string Name => "Default";

        public void Start(GameManager game)
        {
            // Maybe load hud or something.
        }

        public void Update(GameManager game)
        {
            //game.Tick();
            game.Camera.Draw();
        }

        public void HandleInput(GameManager game, ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                game.SetState(new PausedState());
            }
            else if (key.Key == ConsoleKey.W)
                game.Camera.Move(0, -10);
            else if (key.Key == ConsoleKey.A)
                game.Camera.Move(-10, 0);
            else if (key.Key == ConsoleKey.S)
                game.Camera.Move(0, 10);
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
                        new() { Title = "F1        - Help" },
                        new() { Title = "Esc       - Pause"},
                        new() { Title = "W/A/S/D   - Move Camera"},
                        new() { Title = "E/C       - Ascend/Descend"},
                        new() { Title = "G         - Place Actor" },
                        new() { Title = "Q/Z       - Increase/Decrease Sim Speed"},
                        new() { Title = "Ctrl+C    - Quit" } },
                    Top = 4,
                    Left = 8,
                } : null;
            }
        }
    }
}
