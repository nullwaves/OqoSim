using Newtonsoft.Json;
using ksim.Game;
using System.IO.Compression;

namespace ksim.IO
{
    public static class WorldFileManager
    {
        public static JsonSerializerSettings JsonSerializerSettings => new() { TypeNameHandling = TypeNameHandling.Objects };
        public static JsonSerializer Serializer => JsonSerializer.Create(JsonSerializerSettings);

        public static World? LoadWorldFromFile(string filename)
        {
            World? world = null;
            if (!File.Exists(filename)) { return null; }
            try
            {
                Console.WriteLine($"Loading world from {filename}...");
                using FileStream fs = new(filename, FileMode.Open);
                using GZipStream gs = new(fs, CompressionMode.Decompress);
                using StreamReader sr = new(gs);
                var savestring = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<World>(savestring);
                // world = JsonConvert.DeserializeObject<World>(File.ReadAllText(filename), JsonSerializerSettings);
            }
            catch(Exception ex)
            {
                Console.Write(Gui.Colors.RED + Gui.Colors.BG_WHITE + ex.Message + Gui.Colors.NORMAL + Environment.NewLine);
            }
            return world;
        }

        public static bool SaveWorldToFile(World world, string filename)
        {
            Console.WriteLine("Beginning save...");
            try
            {
                Console.WriteLine("Serializing...");
                string jstring = JsonConvert.SerializeObject(world, JsonSerializerSettings);
                Console.WriteLine("Opening Streams...");
                using FileStream fs = new(filename, FileMode.OpenOrCreate);
                using GZipStream gs = new(fs, CompressionMode.Compress);
                using StreamWriter sw = new(gs);
                Console.WriteLine("Writing Save...");
                sw.Write(jstring);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(Gui.Colors.RED + Gui.Colors.BG_WHITE + ex.Message + Gui.Colors.NORMAL);
            }
            return false;
        }
    }
}
