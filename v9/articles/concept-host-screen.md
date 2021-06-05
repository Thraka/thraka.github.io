---
title: Overview of GameHost.Screen
description: Learn about what the GameHost.Screen property is why it's so important for SadConsole.
ms.date: 02/05/2021
---

# SadConsole **GameHost.Screen** Property

The <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> property is one of the most important properties in SadConsole. The `Screen` determines what is visible on the screen and what's processed by the keyboard and mouse.

When SadConsole starts, the host automatically create a <xref:SadConsole.Console?displayProperty=fullName> and assigns it to the `Screen` property. You can use this console or replace it with your own. For more information about hosts, see [SadConsole Game Host Overview](concept-host.md).

## What does the Screen represent?

The `GameHost.Screen` object represents the consoles and surfaces that are displayed to the user. There is a single root object, the `GameHost.Screen`, and all other objects are a child of this object.

There is no limit to the children an object can have, nor is there a limit to how deep the parent-child relationship can be.

## Input processing

Each **Update** frame, SadConsole performs many operations. One of which is to process input. The input processor handles keyboard and mouse state. Mouse related input events are continually processed on the object under the mouse cursor. Keyboard processing is different, only the object currently _focused_ is provided keyboard input.

### Mouse

Mouse input scans the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> object hierarchy and determines which object the mouse is over. That object's <xref:SadConsole.IScreenObject.ProcessMouse(SadConsole.Input.MouseScreenObjectState)> method is called. If that object is going to handle the mouse, which it will unless it's been disabled. More mouse related events are raised on the object when it's processing the mouse, such as <xref:SadConsole.IScreenSurface.MouseEnter> and <xref:SadConsole.IScreenSurface.MouseButtonClicked>.

### Keyboard

Keyboard input is always sent to the object that is "focused." An object is considered focused when its <xref:SadConsole.IScreenObject.IsFocused> property is set to `true` and it's the top-most object of the <xref:SadConsole.GameHost.FocusedScreenObjects?displayProperty=nameWithType> collection.

## Default screen

The <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> starts as a <xref:SadConsole.Console> instance that's sized to the game window. This is the dimensions you passed to the <xref:SadConsole.GameHost.Create%2A> method. Reference to this object is stored, and can be used, in the <xref:SadConsole.Game.StartingConsole?displayProperty=nameWithType>.

## Use the StartingConsole object

The default console screen is accessible by the <xref:SadConsole.Game.StartingConsole?displayProperty=nameWithType> property. You can use this to get started with SadConsole and not worry about creating a console that fills the screen:

```csharp
private static void Init()
{
    SadConsole.Game.Instance.StartingConsole.FillWithRandomGarbage(SadConsole.Game.Instance.StartingConsole.Font);
    SadConsole.Game.Instance.StartingConsole.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0);
    SadConsole.Game.Instance.StartingConsole.Print(4, 4, "Hello from SadConsole");
}
```

## How to replace the Screen

If you're going to replace the starting screen with your own object, make sure to tell SadConsole that you're not going to be using it anymore. After you assign your own object to the the <xref:SadConsole.GameHost.Screen> property, call the <xref:SadConsole.GameHost.DestroyDefaultStartingConsole%2A> method.

```csharp
private static void Init()
{
    var myNewObject = new ScreenObject();

    Game.Instance.Screen = myNewObject;
    Game.Instance.DestroyDefaultStartingConsole();
}
```
