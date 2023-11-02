using Newtonsoft.Json;
using OqoSim.Gui;

namespace OqoSim.Game
{
    internal class GameManager
    {
        private IGameState gameState;

        private readonly Thread inputThread;

        public World World { get; private set; }

        public Camera Camera { get; private set; }

        public int CurrentLayer { get; private set; }

        public GameManager(World? world) 
        {
            World = world ?? new World();
            TileRenderer.AttachGM(this);
            Camera = new Camera(Console.BufferHeight-1, Console.BufferWidth);
            CurrentLayer = 0;
            UpdateCameraLayer();
            gameState = new DefaultState();
            inputThread = new Thread(new ThreadStart(ListenForInput));
        }

        public void Start()
        {
            inputThread.Start();
        }

        public void Update()
        {
            gameState.Update(this);
        }

        public void Tick()
        {
            // Do updates
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
                    gameState.HandleInput(this, Console.ReadKey(true));
                }
            }
        }

        internal void SetState(IGameState state)
        {
            gameState = state;
        }
    }
}
