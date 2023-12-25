using OqoSim.IO;

namespace OqoSim.Game.States
{
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
            else if (key.Key == ConsoleKey.S)
            {
                if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    var success = WorldFileManager.SaveWorldToFile(game.World, "world.oqo");
                    Console.WriteLine($"World saved: {success}");
                }
            }
        }
    }
}
