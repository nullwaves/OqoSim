namespace OqoSim.Gui
{
    public class Menu
    {
        public string Border = "#";
        public int BorderWidth = 1;
        public int Offset = 2;
        public string Title = "Menu";
        public List<MenuItem> Items = new();
        public ConsoleScreen Render(int height, int width)
        {
            var screen = new ConsoleScreen(height, width, string.Empty);
            var listWidth = Items.Count > 0 ? Items.OrderByDescending(x => x.Title.Length).First().Title.Length : Title.Length;
            listWidth = Math.Max(listWidth, Title.Length);
            var borderPad = BorderWidth * 2;
            for (int y = Offset; y < Items.Count + borderPad + Offset; y++)
            {
                if (y > 0 && y < height)
                {
                    if (y < Offset + BorderWidth || y > Offset + BorderWidth + Items.Count - 1)
                        for (int x = Offset; x < Offset + borderPad + listWidth; x++)
                        {
                            if (x > 0 && x < width) screen.Pixels[y, x] = Border;
                        }
                    else
                    {
                        var item = Items[y - (Offset + BorderWidth)];
                        for (int x = Offset; x < Offset + borderPad + listWidth; x++)
                        {
                            if (x < Offset + BorderWidth || x > Offset + BorderWidth + listWidth - 1)
                            {
                                if (x > 0 && x < width) screen.Pixels[y, x] += Border;
                            }
                            else if (x > 0 && x < width)
                            {
                                var cpos = x - (Offset + BorderWidth);
                                screen.Pixels[y, x] += cpos >= 0 && cpos < item.Title.Length ? item.Title.Substring(cpos,1) : " ";
                            }
                        }
                    }
                }
            }
            return screen;
        }
    }

    public class MenuItem
    {
        public string Title { get; set; } = "MenuItem";
    }

}
