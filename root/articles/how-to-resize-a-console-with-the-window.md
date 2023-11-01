---
description: Learn how to resize a console in response to the SadConsole window size changing.
ms.date: 10/31/2023
---

# Resize your main console with the window

A common scenario with SadConsole is resizing the window and then resizing the console with it. When you create a new SadConsole game, you specify the width and height in cells. The game window is sized in pixels to hold that many cells. However, once you resize the window, SadConsole generally centers or stretches the rendering surface to fill the window. It doesn't resize the internal rendering surface which would allow you to show a bigger console on the screen.

This article demonstrates how to resize the internal rendering surface (which is a simple configuration change) and how to resize a console to the size of the window.

## Example

If you create a game with a base console size of 80x25, you'll get a window that looks like the following image:

![Picture of sadconsole game window with numbers for each column](./images/how-to-resize-a-console-with-the-window/starting-console.png)

>[!NOTE]
>The code to generate the numbers and display the console size is provided at the end of the article.

When you resize the window, SadConsole fills the extra space with black (whatever the <xref:SadConsole.Settings.ClearColor>) property is set to> and centers the console.

![Picture of sadconsole game window with numbers for each column and more columns than before](./images/how-to-resize-a-console-with-the-window/normal-resize.png)

To resize a console (or `ScreenSurface`) to the size of the window, you must do the following:

- Configure SadConsole's resize mode to `None`, which allows hte internal rendering surface to fill the window.
- Handle the `WindowResized` event from the game host to detect the new window size.
- Use a surface object that implements `ICellSurfaceResize`, which the default objects in SadConsole use.
- Resize the surface object.

## Detect when the window is resized

You need to add an event handler to the <xref:SadConsole.Host.Game.WindowResized?displayProperty=fullName> event provided by the game host (MonoGame in this case) to detect when the window is resized.

Use the <xref:SadConsole.Configuration.Extensions.OnStart(SadConsole.Configuration.Builder,System.EventHandler{SadConsole.GameHost})> configuration object to add that event handler. This is also a good place to set the <xref:SadConsole.Settings.ResizeMode> property to <xref:SadConsole.Settings.WindowResizeOptions.None>.

The `WindowResized` event is called **after** the window finishes resizing.

```csharp
void Startup(object? sender, GameHost host)
{
    PrintHeader();
    Settings.ResizeMode = Settings.WindowResizeOptions.None;
    Game.Instance.MonoGameInstance.WindowResized += Game_WindowResized;
}

void Game_WindowResized(object? sender, EventArgs e)
{
    var rootConsole = (IScreenSurface)Game.Instance.Screen!;
    var resizableSurface = (ICellSurfaceResize)rootConsole.Surface;
    resizableSurface.Resize(Game.Instance.MonoGameInstance.WindowWidth / rootConsole.FontSize.X,
                            Game.Instance.MonoGameInstance.WindowHeight / rootConsole.FontSize.Y, false);
    PrintHeader();
}
```

> [!NOTE]
> The `PrintHeader` method is documented at the end of this article. It provides a way to visualize the size of the surface.

Now when you resize the window, the root console resizes to fit.

![picture of sadconsole with a dynamic resized console](./images/how-to-resize-a-console-with-the-window/dynamic-resize.png)

## Handling viewport consoles

If your console is larger than the screen, you resize the view port instead of the console itself. Since changing the size of the view port doesn't resize the console itself, the viewport will always cut off at the bounds of the console. The viewport will never get larger than the console, but it can be resized as small as needed.

```csharp
void Game_WindowResized(object? sender, EventArgs e)
{
    var rootConsole = (IScreenSurface)Game.Instance.Screen!;
    var resizableSurface = (ICellSurfaceResize)rootConsole.Surface;
    
    rootConsole.Surface.View = rootConsole.Surface.View.WithSize(
                                        Game.Instance.MonoGameInstance.WindowWidth / rootConsole.FontSize.X,
                                        Game.Instance.MonoGameInstance.WindowHeight / rootConsole.FontSize.Y);
    PrintHeader();
}
```

## PrintHeader

The following code prints a header along the top and size of a surface, which counts the cells.

```csharp
private static void PrintHeader()
{
    int counter = 0;
    var startingColor = Color.Black.GetRandomColor(SadConsole.Global.Random);
    var color = startingColor;
    for (int x = 0; x < RootDynamicConsole.Width; x++)
    {
        RootDynamicConsole[x].Glyph = counter.ToString()[0];
        RootDynamicConsole[x].Foreground = color;
                
        counter++;

        if (counter == 10)
        {
            counter = 0;
            color = color.GetRandomColor(SadConsole.Global.Random);
        }
    }

    counter = 0;
    color = startingColor;
    for (int y = 0; y < RootDynamicConsole.Height; y++)
    {
        RootDynamicConsole[0, y].Glyph = counter.ToString()[0];
        RootDynamicConsole[0, y].Foreground = color;

        counter++;

        if (counter == 10)
        {
            counter = 0;
            color = color.GetRandomColor(SadConsole.Global.Random);
        }
    }

    // Display console size
    RootDynamicConsole.Print(4, 2, "Console Size");
    RootDynamicConsole.Print(4, 3, "                         ");
    RootDynamicConsole.Print(4, 3, $"{RootDynamicConsole.Width} {RootDynamicConsole.Height}");
}
```

## Full code example

```csharp
using SadConsole.Configuration;

Settings.WindowTitle = "SadConsole Examples";

Builder startup = new Builder()
    .SetScreenSize(80, 25)
    .UseDefaultConsole()
    .OnStart(Startup)
    ;

Game.Create(startup);
Game.Instance.Run();
Game.Instance.Dispose();


void Startup(object? sender, GameHost host)
{
    PrintHeader();
    Settings.ResizeMode = Settings.WindowResizeOptions.None;
    Game.Instance.MonoGameInstance.WindowResized += Game_WindowResized;
}

void Game_WindowResized(object? sender, EventArgs e)
{
    var rootConsole = (IScreenSurface)Game.Instance.Screen!;
    var resizableSurface = (ICellSurfaceResize)rootConsole.Surface;
    resizableSurface.Resize(Game.Instance.MonoGameInstance.WindowWidth / rootConsole.FontSize.X, Game.Instance.MonoGameInstance.WindowHeight / rootConsole.FontSize.Y, false);
    PrintHeader();
}

void PrintHeader()
{
    int counter = 0;
    var startingColor = Color.Black.GetRandomColor(Game.Instance.Random);
    var color = startingColor;

    var rootConsole = (IScreenSurface)Game.Instance.Screen!;

    for (int x = 0; x < rootConsole.Surface.Width; x++)
    {
        rootConsole.Surface[x].Glyph = counter.ToString()[0];
        rootConsole.Surface[x].Foreground = color;

        counter++;

        if (counter == 10)
        {
            counter = 0;
            color = color.GetRandomColor(Game.Instance.Random);
        }
    }

    counter = 0;
    color = startingColor;
    for (int y = 0; y < rootConsole.Surface.Height; y++)
    {
        rootConsole.Surface[0, y].Glyph = counter.ToString()[0];
        rootConsole.Surface[0, y].Foreground = color;

        counter++;

        if (counter == 10)
        {
            counter = 0;
            color = color.GetRandomColor(Game.Instance.Random);
        }
    }

    // Display console size
    rootConsole.Surface.Print(4, 2, "Console Size");
    rootConsole.Surface.Print(4, 3, "                         ");
    rootConsole.Surface.Print(4, 3, $"{rootConsole.Surface.Width} {rootConsole.Surface.Height}");
}
```
