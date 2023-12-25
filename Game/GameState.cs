namespace OqoSim.Game
{
    internal interface IGameState
    {
        public string Name { get; }

        public void HandleInput(GameManager gameManager, ConsoleKeyInfo consoleKeyInfo);
        public void Update(GameManager game);
    }   
}
