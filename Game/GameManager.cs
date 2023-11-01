using OqoSim.Gui;

namespace OqoSim.Game
{
    internal class GameManager
    {
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
    }
}
