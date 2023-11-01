// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using OqoSim.Game;
using OqoSim.Gui;
using Terminal.Gui;

Application.Run<MainMenu>();
Application.Shutdown();

Console.WriteLine("Creating World...");

var world = File.Exists("world.oqo") ? JsonConvert.DeserializeObject<World>(File.ReadAllText("world.oqo")) : new World(200, 500);

Console.WriteLine("Done.");
Console.Beep();

GameManager gm = new GameManager(world);
var halfPoint = gm.World.Layers[0].Size / 2;
gm.Camera.SetPosition((halfPoint, halfPoint));
while(true)
{
    gm.Tick();
    Console.Title = $"Oqo - X:{gm.Camera.X} Y:{gm.Camera.Y} Z:{gm.CurrentLayer}";
    var key = Console.ReadKey();
    if (key.Key == ConsoleKey.Escape)
        break;
    else if (key.Key == ConsoleKey.W)
        gm.Camera.Move(0, -1);
    else if (key.Key == ConsoleKey.S)
    {
        if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
        {
            Console.WriteLine("Beginning save...");
            File.WriteAllText("world.oqo", JsonConvert.SerializeObject(gm.World));
            Console.WriteLine("Saved.");
        }
        else gm.Camera.Move(0, 1);
    }
    else if (key.Key == ConsoleKey.A)
        gm.Camera.Move(-1, 0);
    else if (key.Key == ConsoleKey.D)
        gm.Camera.Move(1, 0);
    else if (key.Key == ConsoleKey.Q)
        gm.MoveZ(1);
    else if (key.Key == ConsoleKey.E)
        gm.MoveZ(-1);
    if (Console.BufferHeight-1 != gm.Camera.Height || Console.BufferWidth != gm.Camera.Width)
        gm.Camera.Resize(Console.BufferHeight-1, Console.BufferWidth);
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