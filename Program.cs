using Newtonsoft.Json;
using OqoSim.Game;

World? world;

if (!File.Exists("world.oqo"))
{

    Console.WriteLine("Creating World...");
    world = new World(40, 500);
}
else 
{
    Console.WriteLine("Loading existing world...");
    world = JsonConvert.DeserializeObject<World>(File.ReadAllText("world.oqo"), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
}

Console.WriteLine("Done.");
Console.Beep();

GameManager gm = new(world);
var halfPoint = gm.World.Layers[0].Size / 2;
gm.Camera.SetPosition((halfPoint, halfPoint));
gm.Start();
while(true)
{
    gm.Update();
    Console.Title = $"Oqo - X:{gm.Camera.X} Y:{gm.Camera.Y} Z:{gm.CurrentLayer} - {gm.State}";
    if (Console.BufferHeight-1 != gm.Camera.Height || Console.BufferWidth != gm.Camera.Width)
        gm.Camera.Resize(Console.BufferHeight-1, Console.BufferWidth);
    Thread.Sleep(50);
}