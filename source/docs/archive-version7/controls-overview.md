title: Overview - Controls
layout: docpage
comments: false
searcharchive: true
---

>**NOTE**  
>This article was written for version 7 of SadConsole.

SadConsole provides a text-based UI system with the `SadConsole.Controls` namespace. You can use it to make it easy for your user to interact with your game. Or you can use it to display popup windows and dialogs. This basic functionality is demonstrated in the [DemoProject/CustomConsoles/ControlsTest][controls-test] code file.

## ControlConsole
A special console is provided that manages rendering and managing controls. Each control must used with the [SadConsole.ControlsConsole][controls-console] type.

The `ControlsConsole` not only hosts the controls, but it also manages input, focus, and state of the controls. For intents and purposes, the `ControlsConsole` acts exactly like a normal `Console`. When the controls are drawn, they are drawn on top of the controls console.

## Create and draw a control

First, create a new `SadConsole.ControlsConsole` object and get it drawing to the screen.

>**NOTE**  
>The following namespaces should be used:
>```csharp
>using SadConsole;
>using Microsoft.Xna.Framework;
>```

```csharp
var console = new SadConsole.ControlsConsole(80, 25);

SadConsole.Global.CurrentScreen.Children.Add(console);
```

Now the console has been created and added to the screen. Of course, you can't see it because the background is black. Let's add a simple [button][button] control that you can click on.

```csharp
var button1 = new SadConsole.Controls.Button(12, 1);
button1.Position = new Point(2, 2);
button1.Text = "Click Me";

console.Add(button1);
```

The `console.Add` code adds a control to the `Controls` collection of a `ControlsConsole`, which is a list of all of the controls on the console. You can remove a control with the `Remove` method, or use `RemoveAll` to clear all controls from the console.

>**NOTE**  
>A control can only be associated with one parent `ControlsConsole`. If you add the control to a second another `ControlsConsole`, it will be removed from the previous console.

## Working with events

Generally, each control has an event of some sort that lets you know when the user has interacted with the control, such as when a button is clicked or when a checkbox is unchecked.

To demonstrate this, let's create some code that sets the background of the console to a random color when the user clicks on a button.

```csharp
var colors = new Color[] { ColorAnsi.Black, ColorAnsi.Red, ColorAnsi.Green, ColorAnsi.Yellow, ColorAnsi.Blue, ColorAnsi.Magenta, ColorAnsi.Cyan, ColorAnsi.White };
            
button1.Click += (btn, args) => 
{
    console.DefaultBackground = colors[SadConsole.Global.Random.Next(0, colors.Length)];
    console.Clear();
};
```

## Using multiple controls

Working with multiple controls is just as easy as working with a single control. Create, position, and add the control. You can then hook up events to work between controls. 

Delete the code previously written and start fresh. In this example we'll create a label, a button, and a text box. The [text box][textbox] will be where you enter a name, and when you click on the button, a message is printed at the top of the console.

You'll notice the `label1` control is not of a type named something like `Label` but instead is a [SadConsole.Controls.DrawingSurface][drawingsurface]. The `DrawingSurface` is just a blank canvas for you to draw on and then position as if it was any other control.

```csharp
var console = new SadConsole.ControlsConsole(80, 25);

SadConsole.Global.CurrentScreen.Children.Add(console);

var label1 = new SadConsole.Controls.DrawingSurface(20, 1);
label1.Position = new Point(10, 7);
label1.Surface.Print(0, 0, "What is your name?");

var input = new SadConsole.Controls.TextBox(14);
input.Position = new Point(10, 8);

var button1 = new SadConsole.Controls.Button(9, 1);
button1.Position = new Point(10, 10);
button1.Text = "Print";

console.Add(input);
console.Add(label1);
console.Add(button1);

button1.Click += (btn, args) =>
{
    console.Print(1, 1, $"Hello {input.Text}, how are you today?");
};

SadConsole.Global.FocusedConsoles.Set(console);
```

The TextBox will not process our keyboard if the console is not focused. The code `SadConsole.Global.FocusedConsoles.Set(console);` sets the console as focused. Keyboard input is only ever processed on the *focused* console.


## Dialogs

The controls library provides a *windowing* system through the [SadConsole.Window][window] type. This class is a [ControlsConsole][controls-console] with a bunch of helper code to manage the window, like `Show` and `Hide`. It still functions the same as the ControlsConsole. When you call the `window.Show` method, the window is automatically added to the `SadConsole.Global.CurrentScreen.Children` collection in a position that makes it render on top of everything else, it also gives the window focus.

Replace the previous code with this new code that will use a window instead of a console.

```csharp
var window = new SadConsole.Window(30, 7);

var label1 = new SadConsole.Controls.DrawingSurface(20, 1);
label1.Position = new Point(2, 2);
label1.Surface.Print(0, 0, "What is your name?");

var input = new SadConsole.Controls.TextBox(14);
input.Position = new Point(2, 3);

var button1 = new SadConsole.Controls.Button(9, 1);
button1.Position = new Point(2, 5);
button1.Text = "Close";

window.Add(input);
window.Add(label1);
window.Add(button1);

button1.Click += (btn, args) =>
{
    window.Hide();
};
```

The window will be hidden when the `button1` control is clicked. However, if you run the program the window is not visible. It needs to be made visible. Additionally, we can add a title to the window. By clicking and dragging the title of the window, it can be moved around. The code below will complete the window.

```csharp
window.Title = "Question";
window.Center();
window.Show(true);
``` 

[controls-test]:     https://github.com/Thraka/SadConsole/blob/master/src/DemoProject/SharedCode/CustomConsoles/ControlsTest.cs
[drawingsurface]:    https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Controls/DrawingSurface.cs
[button]:            https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Controls/Button.cs
[textbox]:          https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Controls/TextBox.cs
[window]:            https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Window.cs
[controls-console]:  https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/ControlsConsole.cs