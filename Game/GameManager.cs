using OqoSim.Game.States;
using OqoSim.Gui;

namespace OqoSim.Game
{
    public class GameManager
    {
        private IGameState gameState;

        private readonly Thread inputThread;
        private readonly Thread tickThread;
        private bool Paused;

        public int SimulationSpeed = 1;

        public string State => gameState.Name;

        public World World { get; private set; }

        public Camera Camera { get; private set; }

        public int CurrentLayer { get; private set; }

        public Random Random { get; private set; } = new Random();

        public GameManager(World? world)
        {
            World = world ?? new World();
            TileRenderer.AttachGM(this);
            Camera = new Camera(Console.BufferHeight - 1, Console.BufferWidth);
            CurrentLayer = 0;
            UpdateCameraLayer();
            gameState = new DefaultState();
            Paused = false;
            inputThread = new Thread(new ThreadStart(ListenForInput));
            tickThread = new Thread(new ThreadStart(Tick));
        }

        public void Start()
        {
            inputThread.Start();
            tickThread.Start();
        }

        public void Update()
        {
            gameState.Update(this);
        }

        public void Tick()
        {
            while (true)
            {
                if (!Paused)
                {
                    if (SimulationSpeed > 0)
                    {
                        foreach (var actor in World.Actors)
                        {
                            actor.Tick(this);
                        }
                        Thread.Sleep(1000 / SimulationSpeed);
                    }
                    else Thread.Sleep(100);
                }
            }
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
                if (Console.KeyAvailable)
                {
                    gameState.HandleInput(this, Console.ReadKey(true));
                }
            }
        }

        internal void SetState(IGameState state)
        {
            state.Start(this);
            gameState = state;
        }

        internal void SetPaused(bool val = true)
        {
            Paused = val;
        }
    }
}
