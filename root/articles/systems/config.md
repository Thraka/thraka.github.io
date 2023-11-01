---
title: Overview of the SadConsole startup config
description: Learn about the startup configuration for SadConsole
ms.date: 10/31/2023
---

Game startup API is provided by the host library and not by SadConsole directly. The two hosts, MonoGame and SFML, have identical startup APIs. The MonoGame host is documented on this site, but should equally apply to SFML.

SadConsole games start when the <xref:SadConsole.Game.Create(SadConsole.Configuration.Builder)> method is called, providing an instance of the <xref:SadConsole.Configuration.Builder> object. The `Configuration` object is created fluently; as you call each method to configure the object, the object is returned for continued configuration. Consider the following example:

```csharp
using SadConsole.Configuration;

Settings.WindowTitle = "My SadConsole Game";

Builder configuration = new Builder()
    .SetScreenSize(120, 38)
    .SetStartingScreen<RootScreen>()
    .IsStartingScreenFocused(false)
    .ConfigureFonts(true)
    ;

Game.Create(configuration);
Game.Instance.Run();
Game.Instance.Dispose();
```

The configuration object, `configuration`, sequentially calls `SetScreenSize`, `SetStartingScreen`, `IsStartingScreenFocused`, and `ConfigureFonts`.

The following table lists the configuration options with an explanation of what it does:

| Configuration option                                                                                                 | Description                                                                                                                                                                                                                                      |
|----------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| <xref:SadConsole.Configuration.Extensions.SetStartingScreen``1(SadConsole.Configuration.Builder)>                    | Configures the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> to an object type that contains a parameterless constructor. For more information, see [Startup screen](#startup-screen). Don't use this with <xref:SadConsole.Configuration.Extensions.UseDefaultConsole(SadConsole.Configuration.Builder)>. |
| <xref:SadConsole.Configuration.Extensions.SetStartingScreen(SadConsole.Configuration.Builder,System.Func{SadConsole.Game,SadConsole.IScreenObject})> | Uses the specified delegate to get a `IScreenObject` that's assigned to <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType>. For more information, see [Startup screen](#startup-screen). Don't use this with <xref:SadConsole.Configuration.Extensions.UseDefaultConsole(SadConsole.Configuration.Builder)>. |
| <xref:SadConsole.Configuration.Extensions.UseDefaultConsole(SadConsole.Configuration.Builder)>                       | Sets <xref:SadConsole.GameHost.StartingConsole?displayProperty=nameWithType> and <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> to a new console. Don't use this with <xref:SadConsole.Configuration.Extensions.SetStartingScreen*>. |
| <xref:SadConsole.Configuration.Extensions.IsStartingScreenFocused(SadConsole.Configuration.Builder,System.Boolean)>  | Defaults to `true`. Use this method to pass `false` and disable focusing the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> object. |
| <xref:SadConsole.Configuration.Extensions.SetScreenSize(SadConsole.Configuration.Builder,System.Int32,System.Int32)> | Sets the size of the starting console and sets the <xref:SadConsole.GameHost.ScreenCellsX> and <xref:SadConsole.GameHost.ScreenCellsY> properties to the **Width, Height** values provided, respectively. If not called, defaults to **80, 25**. |
| <xref:SadConsole.Configuration.Extensions.ConfigureFonts(SadConsole.Configuration.Builder,System.Boolean)>           | When passing `true`, uses the extended SadConsole font. |
| <xref:SadConsole.Configuration.Extensions.ConfigureFonts(SadConsole.Configuration.Builder,System.String,System.String[])>     | The first string parameter specifies a different default font. The string array that follows is an optional set of extra fonts to load. |
| <xref:SadConsole.Configuration.Extensions.ConfigureFonts(SadConsole.Configuration.Builder,System.Action{SadConsole.Configuration.FontConfig,SadConsole.Game})> | Use a delegate to configure the provided font settings object. |
| <xref:SadConsole.Configuration.Extensions.OnStart(SadConsole.Configuration.Builder,System.EventHandler{SadConsole.GameHost})> | The delegate provided to this method is invoked when the game starts running. All startup objects are already created and ready to go by the time it's invoked. |
| <xref:SadConsole.Configuration.Extensions.OnEnd(SadConsole.Configuration.Builder,System.EventHandler{SadConsole.GameHost})>   | The delegate provided to this method is invoked when the game is shutting down. |

## Default configuration

As soon as the <xref:SadConsole.Configuration.Builder> is created, the following defaults are applied:

- The screen size is set to **80, 25**.
- Built-in font is enabled.

The defaults can be overridden.

## Starting console

The starting console is a helper that creates a default console sized to the size of the screen. This is a good place to play with SadConsole.

The <xref:SadConsole.GameHost.StartingConsole?displayProperty=nameWithType> provides access to the console. It's also assigned to the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> property, as this property controls what is on the screen. You can use either property to access the same object. `StartingConsole` is a strongly typed property while `Screen` isn't.

If the `StartingConsole` is going to be used, you'll want to configure the object. For more information, see [Startup delegate](#startup-delegate).

```csharp
using SadConsole.Configuration;

Builder configuration = new Builder()
    .SetScreenSize(90, 30)
    .UseDefaultConsole()
    .OnStart(Startup)
    ;

Game.Create(configuration);
Game.Instance.Run();
Game.Instance.Dispose();

static void Startup(object? sender, GameHost host)
{
    if (Game.Instance.StartingConsole is null)
        throw new NullReferenceException("You should never have this error if you used the UseDefaultConsole startup code.");

    Console startingConsole = Game.Instance.StartingConsole;

    startingConsole.Cursor.PrintAppearanceMatchesHost = false;

    startingConsole.Cursor
        .SetPrintAppearanceToHost()
        .Move(0, 21)
        .Print("Kato is my favorite dog")
        .SetPrintAppearance(Color.Green)
        .NewLine()
        .Print("No, Birdie is my favorite dog");
}
```

## Startup screen

The <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> is set by the startup code. There are two ways to choose what the `Screen` is set to:

- Designate a type with <xref:SadConsole.Configuration.Extensions.SetStartingScreen``1(SadConsole.Configuration.Builder)>.

  With the configuration object, call <xref:SadConsole.Configuration.Extensions.SetStartingScreen``1(SadConsole.Configuration.Builder)> and pass a type that implements the <xref:SadConsole.IScreenObject> interface. The type provided must have a parameterless constructor.

  ```csharp
  using SadConsole.Configuration;

  Builder configuration = new Builder()
      .SetScreenSize(120, 38)
      .SetStartingScreen<RootScreen>()
      ;

  Game.Create(configuration);
  Game.Instance.Run();
  Game.Instance.Dispose();
  ```

  In the preceding example, `RootScreen` is a type that implements `IScreenObject` or a type that implements that interface such as `ScreenSurface`. It must have a parameterless constructor.

- Use a delegate to generate an object.

  Like the previous item, you can use a delegate that returns a game object, which is set to the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> property.

  ```csharp
  using SadConsole.Configuration;

  Builder configuration = new Builder()
      .SetScreenSize(120, 38)
      .SetStartingScreen(CreateStartupObject)
      ;
  
  Game.Create(configuration);
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

  The delegate is invoked by the startup code right before the game starts, so all of SadConsole is configured and ready to be accessed. The object can be created any way you want.

As an alternative to this method of creating the starting object, you can use the [Startup delegate](#startup-delegate) to assign a value to <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> or [create the starting console](#starting-console).

## Startup delegate

If you have further initialization you want to perform, especially when you don't designate a different startup object, you can use the <xref:SadConsole.Configuration.Extensions.OnStart(SadConsole.Configuration.Builder,System.EventHandler{SadConsole.GameHost})> configuration option to specify an event handler that's invoked after the game starts. Regardless of how the startup object was created, this event handler can further configure that object.

If you use the [starting console](#starting-console), you must use this configuration option to set up the console. The following code example demonstrates printing some text on the `StartingConsole`.

```csharp
using SadConsole.Configuration;

Builder configuration = new Builder()
    .SetScreenSize(90, 30)
    .UseDefaultConsole()
    .OnStart(Startup)
    ;

Game.Create(configuration);
Game.Instance.Run();
Game.Instance.Dispose();

static void Startup(object? sender, GameHost host)
{
    if (Game.Instance.StartingConsole is null)
        throw new NullReferenceException("You should never have this error if you used the UseDefaultConsole startup code.");

    Console startingConsole = Game.Instance.StartingConsole;

    startingConsole.Cursor.PrintAppearanceMatchesHost = false;

    startingConsole.Cursor
        .SetPrintAppearanceToHost()
        .Move(0, 21)
        .Print("Kato is my favorite dog")
        .SetPrintAppearance(Color.Green)
        .NewLine()
        .Print("No, Birdie is my favorite dog");
}
```

## Configure fonts

SadConsole uses the IBM 8x16 Code Page 437 font by default. To change which font is the default font, use the various font loader extension methods. There are a few helper methods you can use:

- <xref:SadConsole.Configuration.Extensions.ConfigureFonts(SadConsole.Configuration.Builder,System.Boolean)>

  When passing `true`, uses the extended SadConsole font.

- <xref:SadConsole.Configuration.Extensions.ConfigureFonts(SadConsole.Configuration.Builder,System.String,System.String[])>

  The first string parameter specifies a different default font. The string array that follows is an optional set of extra fonts to load.

To have full control over the font loading system, use <xref:SadConsole.Configuration.Extensions.ConfigureFonts(SadConsole.Configuration.Builder,System.Action{SadConsole.Configuration.FontConfig,SadConsole.Game})> which takes a delegate parameter that configures the fonts.

### Use the SadConsole Extended font

SadConsole includes an extended version of the IBM 8x16 Code Page 437 font with other graphical characters that help with building text-ish interfaces and characters. Pass `true` to <xref:SadConsole.Configuration.Extensions.ConfigureFonts(SadConsole.Configuration.Builder,System.Boolean)> to set the extended font as the default font.

```csharp
using SadConsole.Configuration;

Builder configuration = new Builder()
    // ...config options...
    .ConfigureFonts(true)
    ;

Game.Create(configuration);
Game.Instance.Run();
Game.Instance.Dispose();
```

### Use your own font as the default font

Pass in the path of a font file to the <xref:SadConsole.Configuration.Extensions.ConfigureFonts(SadConsole.Configuration.Builder,System.String,System.String[])> method. The string array at the end of the method is optional and is used to load extra fonts.

```csharp
using SadConsole.Configuration;

Builder configuration = new Builder()
    // ...config options...
    .ConfigureFonts("fonts\\new.font") // Load a new default font
    // - or -
    .ConfigureFonts("fonts\\new.font", new[] { "fonts\\c64.font", "fonts\\zx.font" }) // Load a new default font and add some others
    ;

Game.Create(configuration);
Game.Instance.Run();
Game.Instance.Dispose();
```

### Use a delegate to configure the font options

The startup config can also use a delegate to configure the default font and to load other fonts. Use the <xref:SadConsole.Configuration.Extensions.ConfigureFonts(SadConsole.Configuration.Builder,System.Action{SadConsole.Configuration.FontConfig,SadConsole.Game})> option to set the delegate. Then create a method that configures the font.

```csharp
Builder configuration = new Builder()
    // ...config options...
    .ConfigureFonts(SetupFont)
    ;

Game.Create(configuration);
Game.Instance.Run();
Game.Instance.Dispose();

static void SetupFont(BuilderFontLoader loader)
{
    loader.UseBuiltinFontExtended();
    loader.AddExtraFonts("fonts\\new.font", "fonts\\second.font");
}
```
