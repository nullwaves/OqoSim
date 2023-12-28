using OqoSim.Game;
using OqoSim.Game.Generators;
using OqoSim.IO;

World? world = WorldFileManager.LoadWorldFromFile("world.oqo");

if (world is null)
{
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
    Console.Write("World Seed: ");
    string? input = Console.ReadLine();
    Console.WriteLine("Creating World...");
    world = PerlinWorldGenerator.Generate(input is not null ? input.GetHashCode() : 1337, 40, 40, 500);
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