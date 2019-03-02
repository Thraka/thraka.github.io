title: How to create and print on a Console
layout: docpage
comments: false
searcharchive: true
---

>**NOTE**  
>This article was written for version 7 of SadConsole.

A console that you use in your program can be created in two ways:

1.  Create a new instance of the default console class.
1.  Create a new console class.

Before a console can be displayed, it must be set as the `SadConsole.Global.CurrentScreen` or added as a child of the `CurrentScreen`.

## Using the default console class
When you create a `SadConsole.Console` class instance, you are getting all of the basic functionality by default. However, the console will be blank, and you will need to configure it piece-by-piece. This means that you will have to print all of the things you want on the console every time you create it. This may work well for prototyping a design of some sort, but it doesn't really give you anything reusable.

The following example shows the basic steps to create a console that has some text on it.

```csharp
var defaultConsole = new SadConsole.Console(80, 25);
defaultConsole.Cursor.IsVisible = true;
defaultConsole.Print(0, 0, "WELCOME TO THE CONSOLE - PLEASE BEGIN TYPING");
defaultConsole.Cursor.Position = new Point(0, 2);

SadConsole.Global.CurrentScreen = defaultConsole;
SadConsole.Global.FocusedConsoles.Set(defaultConsole);
```

The last two lines set the console we just created to the `CurrentScreen` which displays it. Then, the `FocusedConsoles.Set` method is called and we pass in the console we created. This lets the keyboard input work on the console.

## Creating a new console type
If you want to create a console that is consistently created the same every time it is instantiated, you should create a new class type. You can hard code size, behavior settings, or even the look of the console.

This example creates a console that takes a string parameter as a title, and creates a small panel.

#### Derived Class
```csharp
using Microsoft.Xna.Framework;

using SadConsole;
using Console = SadConsole.Console;

namespace MyProject
{
    class TitleConsole: Console
    {
        public TitleConsole(string title)
            : base(25, 6)
        {
            Fill(Color.White, Color.Black, 176);
            Print(0, 0, title.Align(HorizontalAlignment.Center, Width), Color.Black, Color.Yellow);
        }
    }
}
```

#### Class Usage
```csharp
SadConsole.Global.CurrentScreen = new SadConsole.ScreenObject();
SadConsole.Global.CurrentScreen.Children.Add(new TitleConsole("Player Inventory") { Position = new Point(1, 1) });
SadConsole.Global.CurrentScreen.Children.Add(new TitleConsole("Game World") { Position = new Point(1, 8) });
SadConsole.Global.CurrentScreen.Children.Add(new TitleConsole("Status Panel") { Position = new Point(1, 15) });
```