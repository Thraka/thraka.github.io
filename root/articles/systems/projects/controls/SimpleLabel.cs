using SadConsole.UI;
using SadConsole.UI.Controls;

namespace SadConsoleGame;

internal class SimpleLabel: ScreenSurface
{
    public SimpleLabel() : base(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY)
    {
        ControlHost controls = new();
        SadComponents.Add(controls);

        Label label1 = new("A text label")
        {
            Position = new Point(1, 1),
            TextColor = Color.Purple
        };

        Label label2 = new("A second label")
        {
            Position = new Point(1, 2),
            TextColor = Color.Green
        };

        controls.Add(label1);
        controls.Add(label2);
    }
}
