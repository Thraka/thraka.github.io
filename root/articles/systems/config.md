---
title: Overview of the startup config
description: Learn about the startup configuration for SadConsole
ms.date: 09/12/2023
---

# Game startup

Game startup API is provided by the host library and not by SadConsole directly. The two hosts, MonoGame and SFML, have identical startup APIs. The MonoGame host is documented on this site, but should equally apply to SFML.

SadConsole games start when the <xref:SadConsole.Game.Create(SadConsole.Game.Configuration)> method is called, providing an instance of the <xref:SadConsole.Game.Configuration> object. The `Configuration` object is created fluently, meaning, as you call each method to configure the object, the object is returned for continued configuration. Consider the following example:

```csharp
Game.Configuration gameStartup = new Game.Configuration()
    .SetScreenSize(120, 32)
    .SetStartingScreen<RootScreen>()
    .IsStartingScreenFocused(false)
    .ConfigureFonts((f) => f.UseBuiltinFontExtended())
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();
```

The configuration object, `gameStartup`, sequentially calls `SetScreenSize`, `SetStartingScreen`, `IsStartingScreenFocused`, and `ConfigureFonts`.

The following table lists the configuration options with an explanation of what it does:

| Configuration option                                                                                        | Description                                                                                                                                                                                                                                      |
|-------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| <xref:SadConsole.Game.Configuration.SetStartingScreen``1>                                                                                         | Configures the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> to an object. For more information, see [Startup screen](#startup-screen).                                                                                         |
| <xref:SadConsole.Game.Configuration.SetScreenSize(System.Int32,System.Int32)>                               | Sets the size of the starting console and sets the <xref:SadConsole.GameHost.ScreenCellsX> and <xref:SadConsole.GameHost.ScreenCellsY> properties to the **Width, Height** values provided, respectively. If not called, defaults to **80, 25**. |
| <xref:SadConsole.Game.Configuration.ConfigureFonts(System.Action{SadConsole.Game.ConfigurationFontLoader})> | Use a delegate to configure the provided font settings object.                                                                                                                                                                                   |
| <xref:SadConsole.Game.Configuration.IsStartingScreenFocused(System.Boolean)>                                | Defaults to `true`. Use this method to pass `false` and disable focusing the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> object.                                                                                              |
| <xref:SadConsole.Game.Configuration.OnStart(System.Action)>                                                 | The delegate provided to this method is invoked when the game starts running. All startup objects are already created and ready to go by the time it's invoked.                                                                                  |
| <xref:SadConsole.Game.Configuration.OnEnd(System.Action)>                                                   | The delegate provided to this method is invoked when the game is shutting down.                                                                                                                                                                  |

## Default configuration

As soon as the <xref:SadConsole.Game.Configuration> is created, the following defaults are applied:

- The screen size is set to **80, 25**.
- Builtin font is enabled.
- The <xref:SadConsole.GameHost.StartingConsole?displayProperty=nameWithType> property is set to a <xref:SadConsole.Console> object sized to **80, 25**.
- The `StartingConsole` is assigned to the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> property.

The defaults can be overridden.

## Starting console

When no other screen is designated as a startup screen, the <xref:SadConsole.GameHost.StartingConsole?displayProperty=nameWithType> is created and assigned to the to the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> property. You can use either property to access the same object. `StartingConsole` is a strongly typed property while `Screen` isn't.

If the `StartingConsole` is going to be used, you'll want to configure the object. For more information, see [Startup delegate](#startup-delegate).

## Startup screen

The <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> is set by the startup code. There are three ways to choose what the `Screen` is set to:

- Designate a type with <xref:SadConsole.Game.Configuration.SetStartingScreen``1>.

  With the configuration object, call <xref:SadConsole.Game.Configuration.SetStartingScreen``1> and pass a type that implements the <xref:SadConsole.IScreenObject> interface. The type provided must have a parameterless constructor.

  ```csharp
  Game.Configuration gameStartup = new Game.Configuration()
      .SetScreenSize(120, 38)
      .SetStartingScreen<RootScreen>()
      ;

  Game.Create(gameStartup);
  Game.Instance.Run();
  Game.Instance.Dispose();
  ```

  In the preceding example, `RootScreen` is a type that implements `IScreenObject` and has a parameterless constructor.

  When the starting screen is specified in this way, <xref:SadConsole.GameHost.StartingConsole?displayProperty=nameWithType> is destroyed and can't be used.

- Use a delegate to generate an object.

  Like the previous item, you can use a delegate that returns a game object, which is set to the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> property.

  ```csharp
  Game.Configuration gameStartup = new Game.Configuration()
      .SetScreenSize(120, 38)
      .SetStartingScreen(CreateStartupObject)
      ;
  
  Game.Create(gameStartup);
  Game.Instance.Run();
  Game.Instance.Dispose();
  
  static IScreenObject CreateStartupObject(Game gameInstance)
  {
      SadConsole.Components.Cursor cursor = new();
      ScreenSurface screen = new(gameInstance.ScreenCellsX, gameInstance.ScreenCellsY);
  
      screen.UseKeyboard = true;
      screen.SadComponents.Add(cursor);
      cursor.IsEnabled = true;
  
      cursor.Move((2, 2))
            .Print("Welcome to SadConsole! You can start typing now!")
            .NewLine()
            .Right(2);
  
      return screen;
  }
  ```

  The delegate is invoked by the startup code right before the game starts, so all configuration is completed and SadConsole ready to be used. The object can be created any way you want.

  When the starting screen is specified in this way, <xref:SadConsole.GameHost.StartingConsole?displayProperty=nameWithType> is destroyed and can't be used.

- Do nothing.

  If the startup config doesn't designate a startup object, a <xref:SadConsole.Console> is created and assigned to both the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> and <xref:SadConsole.GameHost.StartingConsole?displayProperty=nameWithType> properties.

  When you use the default startup, you must use the <xref:SadConsole.Game.Configuration.OnStart(System.Action)?displayProperty=nameWithType> configuration option and provide a delegate that customizes the `StartingConsole`. For more information, see [Startup delegate](#startup-delegate).
  
  The `StartingConsole` is convenient when you want to quickly play around with different ideas or concepts when you're learning SadConsole.

## Startup delegate

If you have further initialization you want to perform, especially when you don't designate a different startup object, you can use the <xref:SadConsole.Game.Configuration.OnStart(System.Action)?displayProperty=nameWithType> configuration option to specify a delegate that's invoked after the game starts.

When you leave the default configuration that creates the <xref:SadConsole.GameHost.StartingConsole?displayProperty=nameWithType> instance, you must use this configuration option to setup the console. The following code example demonstrates printing some text on the `StartingConsole`.

```csharp
Game.Configuration gameStartup = new Game.Configuration()
    .SetScreenSize(90, 30)
    .OnStart(Startup)
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();

static void Startup()
{
    Game.Instance.StartingConsole.FillWithRandomGarbage(SadConsole.Game.Instance.StartingConsole.Font);
    Game.Instance.StartingConsole.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, Mirror.None);
    Game.Instance.StartingConsole.Print(4, 4, "Hello from SadConsole");
}
```
