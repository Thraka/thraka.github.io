using SadConsole.Configuration;
using SadConsoleGame;

Settings.WindowTitle = "My SadConsole Game";

Builder
    .GetBuilder()
    .SetWindowSizeInCells(120, 38)
    .ConfigureFonts(true)
    .SetStartingScreen<RootScreen>()
    .IsStartingScreenFocused(true)
    .Run();
