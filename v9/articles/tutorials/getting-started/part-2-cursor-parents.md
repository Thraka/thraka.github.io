---
description: Learn how to get started with SadConsole by using the cursor. Also, learn a few basics about nesting and parenting consoles.
ms.date: 08/07/2021
---

# Get Started 2 - Cursors and parenting

This article is the 2nd in the series of Getting Started tutorials for SadConsole. In this article you'll learn how to use the `Cursor` object to write on a `Console`. SadConsole can also display more than one console, you'll also learn how to have multiple consoles on the screen.

Previous articles in this tutorial:

- [Get Started 1 - Draw on a console](part-1-drawing.md)

## Prerequisites

To start this tutorial you'll need to have created a SadConsole project, one that was created by following the previous tutorial in this series. However, you may have been experimenting with your code, trying new things out. To make sure that you start this tutorial with the same code, copy and paste the following code into your *program.cs*:

```csharp
using System;
using SadConsole;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace SadConsoleGame
{
    public static class Program
    {
        static void Main()
        {
            // Setup the engine and create the main window.
            Game.Create(80, 25);

            // Hook the start event so we can add consoles to the system.
            Game.Instance.OnStart = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Init()
        {

        }
    }
}
```

## Use the Cursor

The `SadConsole.Console` type is the basic type you use to get data on the screen. As you learned in the previous part of this tutorial series, you can use methods like `SetGlyph`, `SetForeground`, and even `Print`, to draw on the console. There is another way to write to the user, something the user and yourself may be more used to, and that's the console cursor.

When you run a terminal program, such as _cmd.exe_ or _bash_, you're presented with a blinking cursor, letting you know its ready for you to type something. When the programs you run communicate back to you, that cursor prints things to the screen:

![demonstrating a powershell and cmd terminal](images/part-2-cursor-parents/terminal.gif)

SadConsole provides a cursor system as way to show users where text is about to be written, where they should use the keyboard to type, or just as a convenient way to draw to a console. You can chain cursor commands into a series of actions:

01. Move the cursor to position 20,21
01. Print the text "Kato is my favorite dog"
01. Change the print color to Green
01. Move the cursor to a new line
01. Print the text "No, Birdie is my favorite dog"

Replace the `Init` code with the following and run your program:

```csharp
static void Init()
{
    Game.Instance.StartingConsole.Cursor.PrintAppearanceMatchesHost = false;
    Game.Instance.StartingConsole.Cursor
        .Move(0, 21)
        .Print("Kato is my favorite dog")
        .SetPrintAppearance(Color.Green)
        .NewLine()
        .Print("No, Birdie is my favorite dog");
}
```

The code causes the cursor to do things, but you don't _see_ a cursor on the screen. By default, the cursor is invisible and inactive. To show the cursor, change the `IsVisible` property to `true`.

```csharp
static void Init()
{
    Game.Instance.StartingConsole.Cursor.PrintAppearanceMatchesHost = false;
    Game.Instance.StartingConsole.Cursor
        .Move(0, 21)
        .Print("Kato is my favorite dog")
        .SetPrintAppearance(Color.Green)
        .NewLine()
        .Print("No, Birdie is my favorite dog");

    Game.Instance.StartingConsole.Cursor.IsVisible = true;
}
```

Run your program and you'll see the cursor blinking. When you type though, nothing happens. The cursor is currently visible, but it's not been enabled yet. To make your cursor respond to keyboard events, there are two conditions that must be satisfied:

01. The console must be focused (you can use `theConsole.IsFocused = true` to focus the console).
01. The cursor must be enabled.

To enable the cursor, set the `IsEnabled` property to `true`.

```csharp
static void Init()
{
    Game.Instance.StartingConsole.Cursor.PrintAppearanceMatchesHost = false;
    Game.Instance.StartingConsole.Cursor
        .Move(0, 21)
        .Print("Kato is my favorite dog")
        .SetPrintAppearance(Color.Green)
        .NewLine()
        .Print("No, Birdie is my favorite dog");

    Game.Instance.StartingConsole.Cursor.IsVisible = true;
    Game.Instance.StartingConsole.Cursor.IsEnabled = true;
}
```

Now when you run the program you'll see that the cursor is blinking, and when you type on the keyboard, the cursor prints the characters to the screen!

## Containers

SadConsole has a framework in place that lets you create more than one console and display them at the same time. SadConsole provides a generic object that allows parenting but doesn't display anything itself: `ScreenObject`.

The starting screen, though, is a `Console`, so we'll need to get rid of that and start over. Let's create a new `ScreenObject` that will host a few consoles.

First, erase all the code in the `Init` method:

```csharp
static void Init()
{

}
```

Now, do the following:

01. Create a new `ScreenObject` and assign it to a variable named `container`.
01. To make this object the main object processed by SadConsole, assign it to the `Game.Instance.Screen` property.
01. Finally, destroy the original `Game.Instance.StartingConsole` with the `DestroyDefaultStartingConsole` method.

```csharp
static void Init()
{
    ScreenObject container = new ScreenObject();
    Game.Instance.Screen = container;
    Game.Instance.DestroyDefaultStartingConsole();
}
```

If you run the game now, nothing will be displayed. A `ScreenObject` is just a container that lets you add multiple child objects to it, but it itself doesn't draw anything. You can do this same thing with a `Console`, but the console would also want to draw something and use resources. So when you have a container that doesn't need to draw anything directly, `ScreenObject` is the object you want to use.

> [!IMPORTANT]
> Because you've replaced the `Game.Instance.Screen` property and you've destroyed the starting console, `Game.Instance.StartingConsole`can no longer be used.

## First child console

The first console we'll create is on the top-left of the screen. It won't take up the whole screen, and it'll use a different background color so that it can be distinguished from the second console we'll soon create.

```csharp
static void Init()
{
    ScreenObject container = new ScreenObject();
    Game.Instance.Screen = container;
    Game.Instance.DestroyDefaultStartingConsole();

    // First console
    Console console1 = new Console(60, 14);
    console1.Position = new Point(3, 2);
    console1.DefaultBackground = Color.AnsiCyan;
    console1.Clear();
    console1.Print(1, 1, "Type on me!");
    console1.Cursor.Position = new Point(1, 2);
    console1.Cursor.IsEnabled = true;
    console1.Cursor.IsVisible = true;
    console1.Cursor.MouseClickReposition = true;
    console1.IsFocused = true;

    container.Children.Add(console1);
}
```

The code above introduces a few new concepts you may be unfamiliar with:

01. `console1.DefaultBackground` and `console1.Clear`

    Each console and surface in SadConsole has a `DefaultBackground` and `DefaultForeground` property. The background property is the most important of the two. This controls the "fill" color used on each cell. SadConsole has some optimization built into it based on this property. You'll always want to set the `DefaultBackground` property to match your most used background color of a console.

    The `console1.Clear` method is called to reset every cell to the new default background color. In our case, this makes sure every cell is colored with a **Cyan** background.

01. `console1.IsFocused = true`

    Previously when you were using the starting console, it was automatically focused, so you didn't have to worry about that. Only the focused console receives keyboard input. If the `container` was focused, `console1` still wouldn't receive keyboard input, even though it's a child object. This is a common mistake developers make with SadConsole, they forget to **Focus** the console or object to receive keyboard input. 

01. `console1.Cursor.MouseClickReposition = true`

    This allows the mouse input to move the cursor for you. When you click on the console, the cursor will reposition itself to wherever the mouse is.

01. `console1.Position = new Point(3, 2);`

    `container` is the root object, which doesn't draw anything because it's a `ScreenObject`, however, it contains a single child: `console1`. This object will draw something on the screen because it's a console. Children are positioned relative to their parent. In this case, `console1` is positioned at _(x3,y2)_ of its parent, `container` which is at _(x0,y0)_. The final drawing position of `console1` is calculated using the formula `self.Position + parent.Position`. Because `container` is the root object, _(x0,y0)_ represents the top-left of the game window and `console` is drawn at _(x3, y2)_ on the window. If `container` was moved to _(x1, y1)_, `console` would be drawn at _(x4, y3)_.

When you run the code, you'll see a screen similar to the following, try typing with the keyboard and clicking the mouse:

![child console with cursor displayed](images/part-2-cursor-parents/cyan-cursor-console.png)

## Add a child to the first console

When children are added to a parent, they draw on top of those parents. Right now the object hierarchy of `Game.Instance.Screen` (which is what is processed and drawn to the game window by SadConsole):

```text
- container
  - console1
```

Let's add another object to the hierarchy. Instead of a `Console` though, add a `ScreenSurface`. A `ScreenSurface` is pretty similar to a `Console`, with only with a few minor differences. First, a console automatically has a `Cursor` object. Second, you interact with the visual surface of a console directly, with commands like `console.Print` or `console.SetGlyph`. A `ScreenSurface` doesn't contain a cursor (though one could be added), and the surface is accessed through the `screenSurface.Surface` property. To interact with a screen surface's surface, you use `screenSurface.Surface.Print` and `screenSurface.Surface.SetGlyph`.

This surface is going to be a child of the console. It will be drawn on top of the console, however, we'll not allow it to gain focus or process any input:

```csharp
// Add a child surface
ScreenSurface surfaceObject = new ScreenSurface(5, 3);
surfaceObject.Surface.FillWithRandomGarbage(surfaceObject.Font);
surfaceObject.Position = console1.Area.Center - (surfaceObject.Surface.Area.Size / 2);
surfaceObject.UseMouse = false;

console1.Children.Add(surfaceObject);
```

When you run the program and you can see the other surface displayed on top of the console.

![console with child surface](images/part-2-cursor-parents/single-console-child-surface.png)

Right now the object hierarchy of `Game.Instance.Screen` is:

```text
- container
  - console1
    - surfaceObject
```

Try moving the cursor and typing behind `surfaceObject`.

## Second child console

The final thing we'll do in this tutorial article is add a second console. This console will be a duplicate of the first, but with a different background color. This demonstrates changing focus between consoles and objects.

```csharp
// Second console
Console console2 = new Console(58, 12);
console2.Position = new Point(19, 11);
console2.DefaultBackground = Color.AnsiRed;
console2.Clear();
console2.Print(1, 1, "Type on me!");
console2.Cursor.Position = new Point(1, 2);
console2.Cursor.IsEnabled = true;
console2.Cursor.IsVisible = true;
console2.FocusOnMouseClick = true;
console2.MoveToFrontOnMouseClick = true;

container.Children.Add(console2);
container.Children.MoveToBottom(console2);
```

There are two new properties and a new method being called here:

01. `console2.FocusOnMouseClick = true;`

    Remember, only the **focused** object receives keyboard input. If we want the second console to receive that keyboard input, we need to remove focus from the first console and give it to the second. Normally you do this with `target.IsFocused = true`. SadConsole can detect when the mouse is clicked on a surface and automatically give it focus, which is what this code does. Set that property on `console1` also. You'll be able to click back and forth between both consoles and type on them.

01. `console2.MoveToFrontOnMouseClick = true;`

    The draw order of the objects is based on where they are in the children collection of the parent, specified by the `Children` property. Whichever object is added last becomes the top-most drawn object. Whichever object is first in this collection is the back-most drawn object. Changing focus to the objects, though, doesn't change their draw order. This property responds to the mouse click and moves the object to the top of the collection (last in the collection) to make it appear on top of everything else.

01. `container.Children.MoveToBottom(console2);`

    Because we created `console2` after `console1` and added it to the `container` after `console`, it's the top-most object drawn. However, we want the experience to be that `console1` is the first console you interact with. There are two ways of doing this, either move `console2` to the bottom of the draw order (which this code does), or simply add both consoles after creating them, in the order you want them drawn. For example, remove the existing `container.Children.Add` method calls for both consoles, and add this to the end of the code routine:

    ```csharp
    container.Children.Add(console2);
    container.Children.Add(console1);
    ```

    Now the consoles are inserted in the order you want them displayed, with `console1` on top.

Right now the object hierarchy of `Game.Instance.Screen` is:

```text
- container
  - console2   <-- Moved to the 'bottom'
  - console1   <-- Top-most child of container
    - surfaceObject
```

When you run the program and you can both consoles. You can click and interact with both.

![console with child surface](images/part-2-cursor-parents/two-consoles.png)

## Conclusion

Now you have both a working console and a non-console surface. You've explored how SadConsole uses the Cursor object to let you type, emulating a terminal. You also learned how to parent one object to another. The next part of this series will explore more about the keyboard and mouse input.

- Part 3: Input -- Not written, coming soon!
