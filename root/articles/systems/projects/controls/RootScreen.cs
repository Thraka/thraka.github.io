using SadConsole.UI;
using SadConsole.UI.Controls;

namespace SadConsoleGame;

internal class RootScreen : ControlsConsole
{
    public RootScreen() : base(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY)
    {
        Point buttonPosition = (1, 0);
        AddButtonType<SimpleLabel>("Simple Label");
        AddButtonType<SimpleLabelExtended>("Moving Label");
        AddButtonTarget("Simple List", SimpleList);
        AddButtonTarget("Simple List Console", SimpleListConsole);
        AddButtonTarget("Change Def Colors", ChangeDefaultColors);
        AddButtonTarget("Change Host Colors", ChangeHostColors);
        AddButtonTarget("Change Control Colors", ChangeControlColors);

        void AddButtonType<T>(string text) where T: IScreenObject, new()
        {
            buttonPosition += (0, 1);
            Button button = new(text) { Position = buttonPosition };
            button.Click += (s, e) => GameHost.Instance.Screen = new T();
            Controls.Add(button);
        }

        void AddButtonTarget(string text, Action target)
        {
            buttonPosition += (0, 1);
            Button button = new(text) { Position = buttonPosition };
            button.Click += (s, e) => target();
            Controls.Add(button);
        }
    }

    static void SimpleList()
    {
        // Create a new surface sized to the window
        ScreenSurface surfaceObject = new(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY);
        ControlHost controls = new();
        
        surfaceObject.SadComponents.Add(controls);

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
        GameHost.Instance.Screen = surfaceObject;
    }

    static void SimpleListConsole()
    {
        ControlsConsole console = new(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY);

        ListBox list = new(18, 7);
        list.DrawBorder = true;
        list.Items.Add("John");
        list.Items.Add("Steve");
        list.Items.Add("Nancy");
        list.Items.Add("Lewis");
        list.Items.Add("Karl");
        list.Items.Add("Carl with a C");
        list.Position = new Point(1, 1);

        console.Controls.Add(list);
        GameHost.Instance.Screen = console;
    }

    static void ShowSomeButtons()
    {
        ControlsConsole console = new(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY);

        Button button1 = new("Button 1");
        button1.Position = new Point(1, 1);

        console.Controls.Add(button1);
        GameHost.Instance.Screen = console;
    }

    static void ChangeDefaultColors()
    {
        Colors.Default.ControlForegroundNormal.SetColor(Color.Purple);
        Colors.Default.RebuildAppearances();

        ShowSomeButtons();
    }

    static void ChangeHostColors()
    {
        // Create the new colors
        Colors newColors = Colors.Default.Clone();

        newColors.Red = Color.Red.GetBrightest();
        newColors.ControlForegroundNormal.SetColor(Color.Purple);
        newColors.RebuildAppearances();

        // Create the controls console, which has a controls host
        ControlsConsole console = new(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY);

        // Update the controls host
        console.Controls.ThemeColors = newColors;

        // Add a single control
        Button button1 = new("Button 1");
        button1.Position = new Point(1, 1);

        console.Controls.Add(button1);
        
        // Show the console
        GameHost.Instance.Screen = console;
    }

    static void ChangeControlColors()
    {
        // Create the new colors
        Colors newColors = Colors.Default.Clone();

        newColors.Red = Color.Red.GetBrightest();
        newColors.ControlForegroundNormal.SetColor(Color.Purple);
        newColors.RebuildAppearances();

        // Create the controls console, which has a controls host
        ControlsConsole console = new(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY);

        // Add first button with custom colors
        Button button1 = new("Button 1");
        button1.SetThemeColors(newColors);
        button1.Position = new Point(1, 1);
        console.Controls.Add(button1);

        // Add second button without custom colors
        Button button2 = new("Button 2");
        button2.Position = new Point(1, 2);
        console.Controls.Add(button2);

        // Show the console
        GameHost.Instance.Screen = console;
    }

    static void ResetDefaultColors()
    {
        Colors.Default = Colors.CreateAnsi();
        Colors.Default = Colors.CreateSadConsoleBlue();
    }

    static void GetColors()
    {
        ControlBase someControl = new Button("Test");
        Colors assignedColors = someControl.FindThemeColors();

        ControlsConsole console = new ControlsConsole(10, 10);
        // Using a ControlsConsole which provides a ControlHost
        Colors hostColors = console.Controls.GetThemeColors();
    }
}