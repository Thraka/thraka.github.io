using SadConsole.Configuration;
using SadConsole.Input;

Settings.WindowTitle = "My SadConsole Game";

Builder gameStartup = new Builder()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<SCTesting.Scenes.RootScreen>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts(true)
    .EnableImGuiDebugger(Keys.F12)
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();