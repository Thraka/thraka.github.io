using SadConsole.UI;
using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadConsoleGame;

internal class SimpleLabelExtended: ScreenSurface
{
    public SimpleLabelExtended() : base(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY)
    {
        ControlHost controls = new();
        SadComponents.Add(controls);

        Label label = new("A text label")
        {
            Position = new Point(1, 1),
            TextColor = Color.Purple
        };

        label.MouseEnter += Label_MouseEnter;
        label.MouseExit += Label_MouseExit;
        label.MouseButtonClicked += Label_MouseButtonClicked;

        controls.Add(label);
    }

    private void Label_MouseEnter(object? sender, ControlBase.ControlMouseState e)
    {
        ((Label)sender!).TextColor = Color.White;
    }

    private void Label_MouseExit(object? sender, ControlBase.ControlMouseState e)
    {
        ((Label)sender!).TextColor = Color.Purple;
    }

    private void Label_MouseButtonClicked(object? sender, ControlBase.ControlMouseState e)
    {
        ((Label)sender!).Position += (1, 1);
    }
}
