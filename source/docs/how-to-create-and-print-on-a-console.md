title: How to create and print on a Console
layout: docpage
---

A console that you use in your program can be created in three ways:

1. Create a new instance of the default console class.
2. Loading a previously saved console.
3. Create a new derived console class.

Before a console can be displayed, it must be as the **SadConsole.Global.CurrentScreen** or added as a child of the current screen.

## Using the default console class
When you create a **SadConsole.Console** class instance, you are getting all of the basic functionality by default. However, the console will be blank, and you will need to configure it piece-by-piece. This means that you will have to print all of the things you want on the console every time you create it. This may work well for prototyping a design of some sort, but it doesn't really give you anything reusable.

The following example shows the basic steps to create a console that has some text on it.

```csharp
Console defaultConsole = new Console(80, 25);
defaultConsole.VirtualCursor.IsVisible = true;
defaultConsole.Print(0, 0, "WELCOME TO THE CONSOLE - PLEASE BEGIN TYPING");
defaultConsole.VirtualCursor.Position = new Point(0, 2);
```

## Loading a previously saved console
Saving and loading a console on the Windows platform is pretty simple. With only two lines of code, you can load and save your console. This lets you either design a console by hand, or through the SadConsole editor. The benefit of this is that you only have to design your console once, and then you can load it into multiple console instances, or just a single console instance.

The following code demonstrates how to load an entire console from a file.

```csharp
Console defaultConsole = SadConsole.Console.Load("mysavedconsole.console");
```

## Creating a new console type
If you want to create a console that is consistently created the same every time it is instantiated, you should create a newly derived type. You can hard code size, behavior settings, or even the look of the console.

This example creates a console that has a border applied to it.

#### Derived Class
```csharp
using Microsoft.Xna.Framework;

using SadConsole;
using Console = SadConsole.Console;

namespace StarterProject.CustomConsoles
{
    class BorderedConsole: Console
    {
        public BorderedConsole()
            : base(80, 25)
        {
            this.IsVisible = false;

            // Get the default box shape definition. Defines which characters to use for the box.
            SadConsole.Shapes.Box box = SadConsole.Shapes.Box.GetDefaultBox();

            // Customize the box
            box.Foreground = Color.Blue;
            box.BorderBackground = Color.White;
            box.FillColor = Color.White;
            box.Fill = true;
            box.Width = textSurface.Width;
            box.Height = textSurface.Height;
            
            // Draw the box shape onto the CellSurface that this console is displaying.
            box.Draw(this);

            this.Print(3, 1, "Shapes are easily created with only a few lines of code");

            // Get a circle
            SadConsole.Shapes.Circle circle = new SadConsole.Shapes.Circle();
            circle.BorderAppearance = new Cell(Color.YellowGreen, Color.White, 57);
            circle.Center = new Point(60, 13);
            circle.Radius = 10;

            circle.Draw(this);

            // Now time to make a line
            SadConsole.Shapes.Line line = new SadConsole.Shapes.Line();
            line.StartingLocation = new Point(10, 10);
            line.EndingLocation = new Point(45, 18);
            line.UseEndingCell = false;
            line.UseStartingCell = false;
            line.Cell = new Cell { Foreground = Color.Purple, Background = Color.White, Glyph = 88 };

            line.Draw(this);

        }

    }
}
```

#### Class Usage
```csharp
BorderedConsole console = new BorderedConsole(80, 25);
SadConsole.Global.CurrentScreen.Children.Add(console);
```