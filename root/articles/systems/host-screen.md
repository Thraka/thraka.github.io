---
title: Overview of GameHost.Screen
description: Learn about what the GameHost.Screen property is why it's so important for SadConsole.
ms.date: 09/12/2023
---

# GameHost.Screen overview (SadConsole Systems)

The <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> property is one of the most important properties in SadConsole. The `Screen` determines what's visible on the screen and what's processed by the keyboard and mouse.

> [!TIP]
> The host is accessed through the `SadConsole.GameHost.Instance` property.

## What does the Screen represent?

The `GameHost.Screen` object represents the consoles and surfaces that are displayed to the user. It's a single root object that all other objects are a child of. Because the `GameHost.Screen` represents what's on the screen, you can use it to transition from one part of your game to the other. For example, when your game starts you can show a title screen with all its logic, and then based on the user's selection, swap the `GameHost.Screen` to an instance of a screen that represents the settings menu or a screen that starts the game.

There's no limit to the children an object can have, nor is there a limit to how deep the parent-child relationship can be.

## Designating the startup screen

The startup screen is set by the game configuration object. To learn about the configuration object, see [Game startup](config.md).

Use the. For more information, see [Game startup - startup screen](config.md#startup-screen).

The <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> is set by the startup code. The easiest way to setup the start screen is to create a new object that inherits from `IScreenObject`, and configure it with all of your consoles, surfaces, and input handling code. Then, use the <xref:SadConsole.Configuration.Extensions.SetStartingScreen``1(SadConsole.Configuration.Builder)> configuration method, providing the type.

### Starting screen type

```csharp
class RootScreen : ScreenObject
{
    public RootScreen()
    {
        // Code here to create consoles, objects, and arrange them. 
        // Each object should be added to the Children collection.
    }
}
```

### Configuration setting

```csharp
using SadConsole.Configuration;

Game.Configuration gameStartup = new Game.Configuration()
    .SetScreenSize(120, 38)
    .SetStartingScreen<RootScreen>() // Startup object type is here
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();
```
