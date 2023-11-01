// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using OqoSim.Game;
using OqoSim.Gui;
using Terminal.Gui;

//Application.Run<MainMenu>();
//Application.Shutdown();

World world;

if (!File.Exists("world.oqo"))
{

    Console.WriteLine("Creating World...");
    world = new World(200, 500);
}
else 
{
    Console.WriteLine("Loading existing world...");
    world = JsonConvert.DeserializeObject<World>(File.ReadAllText("world.oqo"));
}

Console.WriteLine("Done.");
Console.Beep();

GameManager gm = new GameManager(world);
var halfPoint = gm.World.Layers[0].Size / 2;
gm.Camera.SetPosition((halfPoint, halfPoint));
gm.Start();
while(true)
{
    gm.Tick();
    Console.Title = $"Oqo - X:{gm.Camera.X} Y:{gm.Camera.Y} Z:{gm.CurrentLayer}";
    if (Console.BufferHeight-1 != gm.Camera.Height || Console.BufferWidth != gm.Camera.Width)
        gm.Camera.Resize(Console.BufferHeight-1, Console.BufferWidth);
    Thread.Sleep(100);
}

//for(int i = world.Layers.Keys.Min(); i <=  world.Layers.Keys.Max(); i++)
//{
//    var slice = TileRenderer.Instance.Render(world.Layers[i], 0, 0, Console.BufferHeight-2, Console.BufferWidth);
//    Console.WriteLine($"########## {i.ToString("D3")} ##########");
//    foreach(var line in slice)
//    {
//        Console.WriteLine(line);
//    }
//    _ = Console.ReadKey();
//    Console.WriteLine("############################");
//}