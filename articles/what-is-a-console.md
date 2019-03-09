# What is a Console?

The @SadConsole.Console is the main type of object in SadConsole. It's sized by width and height which is made up a grid of cells. Each @SadConsole.Cell in the console is made up of a glyph character, a foreground color, and a background color. Console's let you print to them and even provide a @SadConsole.Console.Cursor property to allow to use the console like a terminal.

## Types

The following types are the objects that are related to console.

| Type | Function |
| ---- | -------- |
| @SadConsole.CellSurface | A `CellSurface` is the base class for `Console`. It represents a collection of cells that can be printed to. |
| @SadConsole.Console | Provides keyboard, mouse, virtual cursor support. Renders the cell data to the screen. |
| @SadConsole.ContainerConsole | A special console that does not but contain other consoles. This disables all rendering and is a **1x1** in size. It shouldn't be used like a console. |
| @SadConsole.ScrollingConsole | The same as a normal `Console` but provides a `ViewPort` property to allow displaying a subsection of the console. |
| @SadConsole.LayeredConsole | Inherits from `ScrollingConsole`, but provides multiple layers. |
| @SadConsole.ControlsConsole | Inherits from `ScrollingConsole`, but adds a GUI-like control system. |
| @SadConsole.AnimatedConsole | Inherits from `Console` and allows you to store multiple frames of surfaces. Provides a play system to play back the animation to the screen. |
| @SadConsole.Entities.Entity | Inherits from `Console` and allows you to store multiple `SadConsole.AnimatedConsole` animations. |

## Using a console

When a console is created, the width and height are specified. The console starts out blank and then you can [change individual cells](how-to-draw-on-a-console.md) or [print text](how-to-draw-on-a-console.md#printing) on the console.

The following code creates a console and sets it as the @SadConsole.Global.CurrentScreen.

```csharp
var defaultConsole = new SadConsole.Console(80, 25);
defaultConsole.Print(0, 0, "WELCOME TO THE CONSOLE - PLEASE BEGIN TYPING");
defaultConsole.Cursor.IsVisible = true;
defaultConsole.Cursor.Position = new Point(0, 2);

SadConsole.Global.CurrentScreen = defaultConsole;
SadConsole.Global.FocusedConsoles.Set(defaultConsole);
```

For more information, see [How to create a console](how-to-create-a-console.md).

