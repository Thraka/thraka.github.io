# What is a Console?

The `SadConsole.Console` type is the main type of object you'll be interacting with in SadConsole. It represents a grid of cells that you can manipulate. It also has a bunch of functionality that makes it so that you can use it like a terminal. It is not exactly meant to emulate a complete terminal, but it has some similar capabilities. The console also provides keyboard and mouse input processing. A `Cursor` object is provided that emulates a terminal cursor.

Each cell in the console is a `SadConsole.Cell` object. A `Cell` is a glyph with a foreground and background color. The console itself provides methods for printing text. Other methods such as shifting the entire surface in a direction, setting the background color of a single cell, or drawing a box shape, are provided. SadConsole has two main functions that are performed on consoles as the game loop runs:

01. Update
01. Draw

Intermixed in that process are all sorts of other things, input management, texture caching for performance, screen position calculations, etc.

The following types are the objects that are related to console.

| Type | Function |
| ---- | -------- |
| SadConsole.CellSurface | A `CellSurface` is the base class for `Console`. It represents a collection of cells that can be printed to. |
| SadConsole.Console | Provides keyboard, mouse, virtual cursor support. Renders the cell data to the screen. |
| SadConsole.ContainerConsole | A special console that does not but contain other consoles. This disables all rendering and is a **1x1** in size. It shouldn't be used like a console. |
| SadConsole.ScrollingConsole | The same as a normal `Console` but provides a `ViewPort` property to allow displaying a subsection of the console. |
| SadConsole.LayeredConsole | Inherits from `ScrollingConsole`, but provides multiple layers. |
| SadConsole.ControlsConsole | Inherits from `ScrollingConsole`, but adds a GUI-like control system. |
| SadConsole.AnimatedConsole | Inherits from `Console` and allows you to store multiple frames of surfaces. Provides a play system to play back the animation to the screen. |
| SadConsole.Entities.Entity | Inherits from `Console` and allows you to store multiple `SadConsole.AnimatedConsole` animations. |

For more information about printing on a console, see [How to create a console](how-to-create-a-console.md).