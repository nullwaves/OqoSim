namespace ksim.Game.Generators
{
    public interface IWorldModifier
    {
        public static abstract string Name { get; }
        public static abstract World Modify(World world, Dictionary<string, object> parameters);
    }
}
