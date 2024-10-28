namespace ksim.Gui
{
    public class Menu
    {
        public string Border = "#";
        public int BorderWidth = 1;
        public int Top = 2;
        public int Left = 4;
        public string Title = "Menu";
        public List<MenuItem> Items = new();
        public ConsoleScreen Render(int height, int width)
        {
            var screen = new ConsoleScreen(height, width, string.Empty);
            var listWidth = Items.Count > 0 ? Items.OrderByDescending(x => x.Title.Length).First().Title.Length : Title.Length;
            listWidth = Math.Max(listWidth, Title.Length);
            var borderPad = BorderWidth * 2;
            for (int y = Top; y < Items.Count + borderPad + Top; y++)
            {
                if (y > 0 && y < height)
                {
                    if (y < Top + BorderWidth || y > Top + BorderWidth + Items.Count - 1)
                        for (int x = Left; x < Left + borderPad + listWidth; x++)
                        {
                            if (x > 0 && x < width) screen.Pixels[y, x] = Border;
                        }
                    else
                    {
                        var item = Items[y - (Top + BorderWidth)];
                        for (int x = Left; x < Left + borderPad + listWidth; x++)
                        {
                            if (x < Left + BorderWidth || x > Left + BorderWidth + listWidth - 1)
                            {
                                if (x > 0 && x < width) screen.Pixels[y, x] += Border;
                            }
                            else if (x > 0 && x < width)
                            {
                                var cpos = x - (Left + BorderWidth);
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

        public MenuItem(string? title = null)
        {
            Title = title is not null ? title : string.Empty;
        }
    }

}
