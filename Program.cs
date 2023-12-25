using OqoSim.Game;
using OqoSim.IO;

World? world = WorldFileManager.LoadWorldFromFile("world.oqo");

if (world is null)
{
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
    Console.WriteLine("Creating World...");
    world = new World(40, 500);
}

Console.WriteLine("Done.");
Console.Beep();

GameManager gm = new(world);
var halfPoint = gm.World.Layers[0].Size / 2;
gm.Camera.SetPosition((halfPoint, halfPoint));
gm.Start();
while (true)
{
    gm.Update();
    Console.Title = $"Oqo - X:{gm.Camera.X} Y:{gm.Camera.Y} Z:{gm.CurrentLayer} - {gm.State}";
    if (Console.BufferHeight - 1 != gm.Camera.Height || Console.BufferWidth != gm.Camera.Width)
        gm.Camera.Resize(Console.BufferHeight - 1, Console.BufferWidth);
    Thread.Sleep(50);
}