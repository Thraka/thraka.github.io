using SadConsole.Configuration;
using SadConsoleGame;

Settings.WindowTitle = "SadConsole Controls";

Builder configuration = new Builder()
    .SetScreenSize(120, 38)
    .SetStartingScreen<RootScreen>()
    .IsStartingScreenFocused(true)
    ;

Game.Create(configuration);
Game.Instance.Run();
Game.Instance.Dispose();