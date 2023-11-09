using SadConsole.UI;
using SadConsole.UI.Controls;

namespace SadConsoleGame;

internal class ListDisplay: ScreenSurface
{
    public ListDisplay() : base(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY)
    {
        ControlHost controls = new();
        SadComponents.Add(controls);

        ListBox list = new(18, 7);
        list.DrawBorder = true;
        list.Items.Add("John");
        list.Items.Add("Steve");
        list.Items.Add("Nancy");
        list.Items.Add("Lewis");
        list.Items.Add("Karl");
        list.Items.Add("Carl with a C");
        list.Position = new Point(1, 1);

        controls.Add(list);
    }
}
