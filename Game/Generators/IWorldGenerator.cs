namespace OqoSim.Game.Generators
{
    public interface IWorldGenerator
    {
        public static abstract string Name { get; }
        public static abstract World Generate(int seed, int layersBelowZero, int layersAboveZero, int width);
    }
}
