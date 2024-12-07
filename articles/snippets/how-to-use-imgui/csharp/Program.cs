using SadConsole.Configuration;

Settings.WindowTitle = "My SadConsole Game";

Builder gameStartup = new Builder()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<SCTesting.Scenes.RootScreen>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts(true)
    .EnableImGui(startupAction: (imguiComponent) => {
        imguiComponent.UIComponents.Add(new SCTesting.ImGuiWindow1());
    })
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();