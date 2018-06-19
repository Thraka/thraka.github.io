title: Overview - Controls
layout: docpage
---

SadConsole provides a GUI system with the `SadConsole.Controls` namespace. You can display control surfaces, use popup windows and dialogs, even create new controls. This basic functionality is demonstrated in the [DemoProject/CustomConsoles/ControlsTest][controls-test] code file.

## ControlConsole
A special console is provided that manages rendering and managing controls. Each control must used with the [SadConsole.ControlsConsole][controls-console] type.

The ControlsConsole not only hosts the controls, but it also manages input, focus, and state of the controls. Additionally it makes sure that the renderer of the console knows about the controls. This however requires that a special renderer be used, [SadConsole.Renderers.ControlsConsoleRenderer][controls-renderer]. The reason we do not use the normal [ConsoleRenderer][console-renderer] is that each control is its own [Surface][textsurface]. The controls are not rendered to the backing Surface but instead are rendered after that Surface has already been drawn.

>**WARNING**  
>If you do not use a renderer that is or derives from `ControlsConsoleRenderer`, an exception will be thrown.

## Create and draw a control

The first thing to do to get controls drawn on the screen is to create the console. And like all consoles, you can do this by creating your own custom console that encapsulates all the controls and logic, or use the basic provided console. The code here will get you started using the built in ControlsConsole type.

```csharp
var console = new SadConsole.ControlsConsole(80, 25);

SadConsole.Global.CurrentScreen.Children.Add(console);
```

Now the console has been created and added to the engine. Of course, you can't see it because the background is black. Let's add a simple [button][button] control that you can click on.

```csharp
var button1 = new SadConsole.Controls.Button(12);
button1.Position = new Point(2, 2);
button1.Text = "Click me";

console.Add(button1);
```

The `Add` method adds a control to the `Controls` collection of a ControlsConsole. You can remove a control with the `Remove` method, or use `RemoveAll` to clear all controls from the console.

>**NOTE**  
>A control can only be associated with one parent ControlsConsole. If you add the control to another ControlsConsole, it will be removed from the previous console.

## Working with events

Generally each control has an event of some sort that lets you respond to when something happens between the user and the control, such as when a button is clicked or when a checkbox is unchecked.

To show this, we'll create some code that takes an array of colors, and when the button is clicked, sets the background of the console to a random color.

```csharp
var colors = new Color[] { ColorAnsi.Black, ColorAnsi.Red, ColorAnsi.Green, ColorAnsi.Yellow, ColorAnsi.Blue, ColorAnsi.Magenta, ColorAnsi.Cyan, ColorAnsi.White };
            
button1.Click += (btn, args) => 
{
    console.TextSurface.DefaultBackground = colors[SadConsole.Global.Random.Next(0, colors.Length)];
    console.Clear();
};
```

## Using multiple controls

Working with multiple controls is just as easy as working with a single control. Create, position, and add the control. Simple as that. You can then hook up events to work between controls. 

Delete the previous code that when you clicked a button a random background color was set. Also, delete the `button1` code and we'll recreate it with some new code. In this example we'll create a label, a button, and an input box. The [input box][inputbox] will be where you enter a name, and when you click on the button, a message is printed at the top of the console.

You'll notice the `label1` control is not of a type named something like `Label` but instead is a [SadConsole.Controls.DrawingSurface][drawingsurface]. The DrawingSurface is just a BasicSurface that is a blank canvas for you to draw on and then position as if it was any other control.

```csharp
var label1 = new SadConsole.Controls.DrawingSurface(20, 1);
label1.Position = new Point(10, 7);
label1.Print(0, 0, "What is your name?");

var input = new SadConsole.Controls.InputBox(14);
input.Position = new Point(10, 8);

var button1 = new SadConsole.Controls.Button(9);
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

The InputBox will not process our keyboard if the console is not focused. The code `SadConsole.Global.FocusedConsoles.Set(console);` sets the console as focused. Keyboard input is only ever processed on the focused console.


## Dialogs

The controls library provides a *windowing* system through the [SadConsole.Window][window] type. This class is a [ControlsConsole][controls-console] with a bunch of helper code to manage the window, like `Show` and `Hide`. It still functions the same as the ControlsConsole. You do not need to add a window to the `SadConsole.Global.CurrentScreen.Children` like you do any other console. When you call the `window.Show` method, the window is automatically added to the `CurrentScreen.Children` collection in a position that makes it render on top of everything else.

Replace the previous code with this new code that will use a window instead of a console.

```csharp
var window = new SadConsole.Window(30, 7);

var label1 = new SadConsole.Controls.DrawingSurface(20, 1);
label1.Position = new Point(2, 2);
label1.Print(0, 0, "What is your name?");

var input = new SadConsole.Controls.InputBox(14);
input.Position = new Point(2, 3);

var button1 = new SadConsole.Controls.Button(9);
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

As you can see in the `button1.Click` event, the window will be hidden when the button is clicked. However, if you run the program the window is not visible, we need to show it. Additionally, we can add a title to the window, and also position the window. The code below will complete the window. You can also hold the mouse button down on the window title and move the window around.

```csharp
window.Title = "Question";
window.Center();
window.Show(true);
``` 
https://github.com/Thraka/SadConsole/tree/master/src/

[controls-test]:     https://github.com/Thraka/SadConsole/blob/master/src/DemoProject/SharedCode/CustomConsoles/ControlsTest.cs
[controls-renderer]: https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Renderers/ControlsConsoleRenderer.cs
[console-renderer]:  https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Renderers/SurfaceRenderer.cs
[textsurface]:       https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Surfaces/BasicSurface.cs
[drawingsurface]:    https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Controls/DrawingSurface.cs
[button]:            https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Controls/Button.cs
[inputbox]:          https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Controls/InputBox.cs
[window]:            https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Consoles/Window.cs
[window-renderer]:   https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Renderers/WindowRenderer.cs
[controls-console]:  https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/ConsoleTypes/ControlsConsole.cs