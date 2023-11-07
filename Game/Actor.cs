namespace OqoSim.Game
{
    public interface IActor
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public string ActorType { get; }
        public void Tick(GameManager game);
    }

    public abstract partial class BaseActor : IActor
    {
        public Vector3i Position = new(0, 0, 0);

        public int X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public int Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        public int Z
        {
            get { return Position.Z; }
            set { Position.Z = value; }
        }

        public abstract string ActorType { get; }

        public abstract void Tick(GameManager game);

        public List<(int, int, int)> GetAcceptableMovePositions(GameManager game)
        {
            List<(int, int, int)> results = new();
            for (int z = -1; z <= 1; z++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int nX = X + x;
                        int nY = Y + y;
                        int nZ = Z + z;
                        Tile t = game.World.GetTileAtPos(nX, nY, nZ);
                        if (t.Type == TileType.Ground &&
                            !game.World.TileIsCovered(nX, nY, nZ) &&
                            !game.World.TileIsSubmerged(nX, nY, nZ))
                            results.Add((nX, nY, nZ));
                    }
                }
            }
            return results;
        }

        public void MoveTo(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Dwarf : BaseActor
    {
        public override string ActorType => "Person";

        public override void Tick(GameManager game)
        {
            var list = GetAcceptableMovePositions(game);
            if (list.Count > 0)
            {
                var newPos = list[game.Random.Next(0, list.Count)];
                MoveTo(newPos.Item1, newPos.Item2, newPos.Item3);
            }
        }
    }
}
