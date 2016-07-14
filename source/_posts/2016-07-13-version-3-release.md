title: Version 3 Release
date: 2016-07-13 15:42:42
tags:
- SadConsole
category:
- SadConsole
---

It's finally done! Version 3 of SadConsole brings a lot of structural changes and increases flexibility.  [Previously](/2016/04/23/thoughts-on-rendering-a-console) it was hard to implement new types of objects that hooked into the rendering system. And whenever I did create something extra, I always felt like I was trying to fit it around the system instead of the system being flexible enough to handle it. Not any more!

Version 3.0 has taken me 2 1/2 months, 113 commits to the code repository, and 192 files were edited.

Here is a class diagram of how the system is put together now. Click below after the picture for more about the changes.

{% asset_img v3-rendering-classes.png version 3 class structure %}

<!-- more -->

The system is made up of three concepts (that work out to different types)

1. *TextSurface*  
This represents the text data of a surface.

2. *TextSurfaceRenderer*  
Takes in TextSurface data and draws it to the screen.

3. *SurfaceEditor*  
Provides a way to change a TextSurface, like the background or foreground of cells.


What about the classic **SadConsole.Consoles.Console** type? It's there. This class is a `SurfaceEditor` that has both a `TextSurface` and `TextSurfaceRenderer`. The concept of a `Console` comes together when you combine those three elements. The `Console` still has all the other things: cursor, position, and input handling.

You can easily use a `TextSurfaceRenderer` to render a `TextSurface` directly, and you can use the `SurfaceEditor` to change the data on the `TextSurface`, all without the actual `Console` type.

```csharp
var surface = new SadConsole.Consoles.TextSurface(16, 5, SadConsole.Engine.DefaultFont);
var editor = new SadConsole.Consoles.SurfaceEditor(surface);
var renderer = new SadConsole.Consoles.TextSurfaceRenderer();

editor.Fill(Color.Aqua, Color.Gray, 0, null);
editor.Print(2, 2, "Hello World!");

renderer.Render(surface, new Point(1, 1));
```

This flexability allows us to create new types of data and rendering systems. For example, there is now a `CachedTextSurfaceRenderer` that instead of rendering the `TextSurface` on every render call, it renders once and keeps that output for every subsequent render call. You have to explicitly tell it to update itself. This is handy for UI elements, frames and borders, and text areas that never change once rendered. If a text surface is 10x10, that is about 200 renders calls every frame. With the `CachedTextSurfaceRenderer` you take those 200 render calls once, and then every time after that a single render call is done. Very optimized.

## Sample code

Here are some common examples of using the new types.

### Create a console

By default, SadConsole handles all rendering and updates of a `Console` type for you. Just add it to the `SadConsole.Engine.ConsoleRenderStack` collection.

```csharp
var console = new SadConsole.Consoles.Console(16, 5);
console.Fill(Color.Aqua, Color.Gray, 0, null);
console.Print(2, 2, "Hello World!");
console.Position = new Point(1, 1);
SadConsole.Engine.ConsoleRenderStack.Add(console);
```

{% asset_img hello-world-1.png Hello world rendered in SadConsole %}

### Manually render a TextSurface

You can take any existing TextSurface and render it multiple times. A console draws it self once per frame, wherever `Console.Position` points to on the screen, but we can also draw manually if we would like to duplicate the console. 

>**NOTE**  
>Remember that the console is still in the `SadConsole.Engine.ConsoleRenderStack` and will draw at position **1, 1**.

```csharp
console.Renderer.Render(console.TextSurface, new Point(19, 1), false);
console.Renderer.Render(console.TextSurface, new Point(37, 1), false);
```

{% asset_img hello-world-2.png Hello world rendered three times in SadConsole %}

Instead of using the console itself to render, you could also do it all by hand.

```csharp
var renderer = new SadConsole.Consoles.TextSurfaceRenderer();
var textSurface = console.TextSurface;

renderer.Render(textSurface, new Point(19, 1), false);
renderer.Render(textSurface, new Point(37, 1), false);
```

## Next

In the next blog post, I'll show how to use the render multiple layers on a console using built in types. 
