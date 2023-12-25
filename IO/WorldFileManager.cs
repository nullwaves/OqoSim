using Newtonsoft.Json;
using OqoSim.Game;

namespace OqoSim.IO
{
    public static class WorldFileManager
    {
        public static JsonSerializerSettings JsonSerializerSettings => new() { TypeNameHandling = TypeNameHandling.Objects };

        public static World? LoadWorldFromFile(string filename)
        {
            World? world = null;
            if (!File.Exists(filename)) { return null; }
            try
            {
                Console.WriteLine($"Loading world from {filename}...");
                world = JsonConvert.DeserializeObject<World>(File.ReadAllText(filename), JsonSerializerSettings);
            }
            catch(Exception ex)
            {
                Console.Write(Gui.Colors.RED + Gui.Colors.BG_WHITE + ex.Message + Gui.Colors.NORMAL + Environment.NewLine);
            }
            return world;
        }

        public static bool SaveWorldToFile(World world, string filename)
        {
            bool success = false;
            Console.WriteLine("Beginning save...");
            try
            {
                File.WriteAllText(filename, JsonConvert.SerializeObject(world, typeof(World), JsonSerializerSettings));
                success = true;
            }
            catch (Exception ex)
            {
                Console.Write(Gui.Colors.RED + Gui.Colors.BG_WHITE + ex.Message + Gui.Colors.NORMAL);
            }
            return success;
        }
    }
}
