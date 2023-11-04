using Newtonsoft.Json;

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
            game.Tick();
            game.Camera.Draw();
        }

        public void HandleInput(GameManager game, ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
                game.SetState(new PausedState());
            else if (key.Key == ConsoleKey.W)
                game.Camera.Move(0, -1);
            else if (key.Key == ConsoleKey.S)
            {
                if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    Console.WriteLine("Beginning save...");
                    File.WriteAllText("world.oqo", JsonConvert.SerializeObject(game.World));
                    Console.WriteLine("Saved.");
                }
                else game.Camera.Move(0, 1);
            }
            else if (key.Key == ConsoleKey.A)
                game.Camera.Move(-2, 0);
            else if (key.Key == ConsoleKey.D)
                game.Camera.Move(2, 0);
            else if (key.Key == ConsoleKey.E)
                game.MoveZ(1);
            else if (key.Key == ConsoleKey.C)
                game.MoveZ(-1);
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

        public void HandleInput(GameManager game,ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                game.SetState(new DefaultState());
                game.Camera.Draw(true);
            }
        }
    }
}
