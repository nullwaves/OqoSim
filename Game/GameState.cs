namespace ksim.Game
{
    internal interface IGameState
    {
        public string Name { get; }

        public void HandleInput(GameManager gameManager, ConsoleKeyInfo consoleKeyInfo);
        public void Start(GameManager game);
        public void Update(GameManager game);
    }   
}
