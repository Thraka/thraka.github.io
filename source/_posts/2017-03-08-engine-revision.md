title: Engine Revision
date: 2017-03-08 11:34:22
tags:
- SadConsole
category:
- SadConsole
---

Over the last two months I've spent a lot of time looking at how SadConsole is put together. Specifically, paying attention to namespaces and class names. There a few complexities in the object model that I wanted to address and just make things simpler. I've come to a point now where I would guess that I'm 99% complete on implementing these changes, and I'm extremely happy with how things have turned out.

Two major changes that I've figured out impact you as a coder.

1. All four libraries (Core, Ansi, Controls, GameHelpers) have been merged into a single library: `SadConsole.dll`.
2. Instead of two libraries, one for OpenGl and one for DirectX, a single library handles both.

Originally I thought I needed both types of libraries, however it turns out that you (creating the game) choose which target (OpenGL or DirectX) based on which MonoGame you choose. SadConsole will just play along without a problem.

The internal rendering and processing system has also changed.

<!-- more -->

Here are the notes from the readme.

The preview release NuGet package is at https://www.nuget.org/packages/SadConsole/

A lot of refactoring has happened. Many of the types in SadConsole have been moved around to a more logical position and a lot of redundancy has been removed. 

For example, the **Console** type used to be located at the `SadConsole.Consoles.Console` which felt very obtuse. **Console** is a core type that doesn't need its own namespace, much like **Cell**. This type is now located in the root namespace `SadConsole`.

Other things have simplified naming too, like `Console.CanUseKeyboard` is just `Console.UseKeyboard` now.

## Core engine

`SadConsole.Engine` has been removed. Its role was to coordinate all the parts of SadConsole. Instead this has been split into three parts, `SadConsole.Global` which represents state (time passed, keyboard/mouse, current thing to render), `SadConsole.Game` which is the `MonoGame.Game` instance, and `SadConsole.Settings` which provides full screen, toggle drawing on/off etc.

| Class |     |
| ----- | --- |
| SadConsole.Global   | Global state, like time elapsed, keyboard/mouse input state, the active thing to render. |
| SadConsole.Game     | `Microsoft.Xna.Framework.Game` game instance that you can run instead of providing your own. |
| SadConsole.Settings | Various settings like fullscreen, device clear color, enable/disable keyboard or mouse, other settings. | 


## Core types

The `SadConsole.Consoles` namespace does not exist anymore and is instead broken up into two different namespaces that better represents the types contained in it:

| Namespace |     |
| --------- | --- |
| SadConsole.Surfaces  | All types of surfaces that are attached to a Console. |
| SadConsole.Renderers | All renderers that render surfaces and are attached to a Console. |

The **TextSurface** naming convention has been simplified to **Surface**. And some of the interface and base class complexity of the **TextSurface** stuff has been simplified into fewer types. 

## Rendering

The rendering system in SadConsole has had some improvements. Instead of each ** Renderer**  having its own ** SpriteBatch** , there is a single `SadConosle.Global.SpriteBatch` which is reused by all renderers. This reduces memory and reduces CPU cycles that were wasted everytime a renderer was created.

Each **Surface** now provides a **RenderTarget2D** type which is a texture. Whenever a surface is rendered, it is drawn onto this texture. At the end of the global Draw call, all surfaces that are in the drawing pipeline are rendered to a single **RenderTarget2D** texture at `SadConsole.Global.RenderOutput`. This final texture (which contains all drawing from SadConsole) is then drawn to the screen. This simplifies fullscreen and stretch modes. This new system also allows anyone to bypass any part of SadConsole rendering and use the rendered textures to draw on any sort of 3D model or scene of their game. For example, you could build up a 3D scene of an old computer terminal and then use SadConsole on the screen of the computer.

The rendering system is now completely cached. Each **ISurface** type has a **IsDirty** flag which causes the backing **RenderTarget2D** to be updated.

## Notable types

Here is a list of types that have changed and what replaced them. The root `SadConsole` namespace is implied in all of these.

| Old Class        | New Class     |
| ---------------- | --- |
| Engine           | Replaced by **Global**, **Game**, and **Settings**. | 
| ICellAppearance  | Removed - Use **Cell**. |
| CellAppearance   | Removed - Use **Cell**. |
| Consoles.IConsole     | IConsole. Still exists, implements **IScreen** now. |
| Consoles.IConsoleList | Removed. |
| Consoles.Console      | Console |
| Consoles.ConsoleList  | Removed. All **IScreen** types have both **Parent** and **Children** properties. |
| Consoles.ITextSurface        | Surfaces.ISurface |
| Consoles.TextSurfaceBasic    | Removed. Merged into **Surfaces.Surface** |
| Consoles.TextSurface         | Surfaces.BasicSurface |
| Consoles.TextSurfaceView     | Surfaces.SurfaceView  |
| Consoles.AnimatedTextSurface | Surfaces.AnimatedSurface  |
| Consoles.LayeredTextSurface  | Surfaces.LayeredSurface  |
| Consoles.Cursor              | Cursor  |
| Consoles.SurfaceEditor       | Surfaces.SurfaceEditor  |
| Consoles.ITextSurfaceRenderer       | Renderers.ISurfaceRenderer |
| Consoles.TextSurfaceRenderer        | Renderers.SurfaceRenderer |
| Consoles.LayeredTextRenderer        | Renderers.LayeredSurfaceRenderer |
| Consoles.ITextSurfaceRendererUpdate | Removed - All surfaces support cached rendering. |
| Consoles.CachedTextSurfaceRenderer  | Removed - All surfaces support cached rendering. |
| Input.MouseInfo       | Renamed to Input.Mouse |
| Input.KeyboardInfo    | Renamed to Input.Keyboard | 

Besides the **Consoles** namespace, startup, and **Engine** -> **Global** changes, not much else has changed.

Some methods and/or properties have been renamed. Here are some of them.

| Old name              | New name |
| --------------------- | -------- |
| Input.Keyboard.ProcessKeys | Input.Keyboard.Process |
| Input.Mouse.ProcessMouse   | Input.Mouse.Process |
| Engine.ActiveConsole       | Global.InputTargets -- This is a new type that allows a Push/Pop/Set system for who gets keyboard/exclusive mouse input |

## Input

Input has been overhauled a bit. Keyboard is mostly the same except for some minor method refactoring. Mouse has change a lot. Previously each console evaulated mouse state for itself. This is no longer how mouse input works. Instead mouse input is driven by the `SadConsole.Input.Mouse.Update` method which cycles through the `SadConsole.Global.Screen` gathering all console types. Then, each console has the `ProcessMouse` method called. If `true` is returned, mouse processing stops. This happens unless the `Global.InputTargets.Console` has the `IsExclusiveMouse` property set to `true`. If `true`, mouse is always sent to this console and never to anything else.

## Startup code

The code to start SadConsole from a dedicated SadConsole project is pretty much the same. But now that `Engine` is gone, `Global` is used and the names of the draw/update events are simplier. They also are direct delegates instead of event.

```csharp
static void Main(string[] args)
{
    // Setup the engine and creat the main window.
    SadConsole.Game.Create("IBM.font", 80, 25);

    // Hook the start event so we can add consoles to the system.
    SadConsole.Game.OnInitialize = Init;

    // Hook the update event that happens each frame so we can trap keys and respond.
    SadConsole.Game.OnUpdate = Update;

    // Hook the "after render" even though we're not using it.
    SadConsole.Game.OnDraw = DrawFrame;
            
    // Start the game.
    SadConsole.Game.Instance.Run();

    //
    // Code here will not run until the game has shut down.
    //
}

private static void DrawFrame(GameTime time)
{
    // Custom drawing. You don't usually have to do this.
}

private static void Update(GameTime time)
{
    // Called each logic update.
}

private static void Init()
{
    // Any setup
}
```