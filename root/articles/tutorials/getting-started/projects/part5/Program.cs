using System;
using SadConsole;
using SadConsole.UI.Controls;
using SadConsole.UI;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace SadConsoleGame
{
    public static class Program
    {
        static void Main()
        {
            // Setup the engine and create the main window.
            Game.Create(120, 38);

            // Hook the start event so we can add consoles to the system.
            Game.Instance.OnStart = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Init()
        {
            Game.Instance.Screen = new RootScreen();
            Game.Instance.Screen.IsFocused = true;

            // This is needed because we replaced the initial screen object with our own.
            Game.Instance.DestroyDefaultStartingConsole();
        }
    }
}