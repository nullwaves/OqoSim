using Newtonsoft.Json;
using OqoSim.Gui;

namespace OqoSim.Game
{
    internal class GameManager
    {
        private Thread inputThread;

        public World World { get; private set; }

        public Camera Camera { get; private set; }

        public int CurrentLayer { get; private set; }

        public GameManager(World? world) 
        {
            World = world ?? new World();
            TileRenderer.Instance.AttachGM(this);
            Camera = new Camera(Console.BufferHeight-1, Console.BufferWidth);
            CurrentLayer = 0;
            UpdateCameraLayer();
            inputThread = new Thread(new ThreadStart(ListenForInput));
        }

        public void Start()
        {
            inputThread.Start();
        }

        public void Tick()
        {
            // Do updates then draw.
            Camera.Draw();
        }

        public void MoveZ(int zDelta)
        {
            int newZ = CurrentLayer + zDelta;
            if (!(newZ > World.Layers.Keys.Max() || newZ < World.Layers.Keys.Min()))
            {
                CurrentLayer = newZ;
                UpdateCameraLayer();
            }
        }

        private void UpdateCameraLayer()
        {
            Camera.SetLayer(World.Layers[CurrentLayer]);
        }

        public void ListenForInput()
        {
            while (true)
            {
                if(Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                        break;
                    else if (key.Key == ConsoleKey.W)
                        Camera.Move(0, -1);
                    else if (key.Key == ConsoleKey.S)
                    {
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
                        {
                            Console.WriteLine("Beginning save...");
                            File.WriteAllText("world.oqo", JsonConvert.SerializeObject(World));
                            Console.WriteLine("Saved.");
                        }
                        else Camera.Move(0, 1);
                    }
                    else if (key.Key == ConsoleKey.A)
                        Camera.Move(-2, 0);
                    else if (key.Key == ConsoleKey.D)
                        Camera.Move(2, 0);
                    else if (key.Key == ConsoleKey.Q)
                        MoveZ(1);
                    else if (key.Key == ConsoleKey.E)
                        MoveZ(-1);
                }
            }
        }
    }
}
