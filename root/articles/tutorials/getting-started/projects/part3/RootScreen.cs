using SadConsole.Input;

namespace SadConsoleGame;

internal class RootScreen : ScreenObject
{
    private ScreenSurface _map;
    private GameObject _controlledObject;

    public RootScreen()
    {
        _map = new ScreenSurface(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY - 5);
        _map.UseMouse = false;

        FillBackground();

        Children.Add(_map);

        _controlledObject = new GameObject(new ColoredGlyph(Color.White, Color.Black, 2), _map.Surface.Area.Center, _map);
    }

    private void FillBackground()
    {
        Color[] colors = new[] { Color.LightGreen, Color.Coral, Color.CornflowerBlue, Color.DarkGreen };
        float[] colorStops = new[] { 0f, 0.35f, 0.75f, 1f };

        Algorithms.GradientFill(_map.FontSize,
                                _map.Surface.Area.Center,
                                _map.Surface.Width / 3,
                                45,
                                _map.Surface.Area,
                                new Gradient(colors, colorStops),
                                (x, y, color) => _map.Surface[x, y].Background = color);
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.Up))
        {
            _controlledObject.Move(_controlledObject.Position + Direction.Up, _map);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Down))
        {
            _controlledObject.Move(_controlledObject.Position + Direction.Down, _map);
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Left))
        {
            _controlledObject.Move(_controlledObject.Position + Direction.Left, _map);
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.Right))
        {
            _controlledObject.Move(_controlledObject.Position + Direction.Right, _map);
            handled = true;
        }

        return handled;
    }
}