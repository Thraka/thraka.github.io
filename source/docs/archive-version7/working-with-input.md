title: Working with input
layout: docpage
comments: false
searcharchive: true
---

>**NOTE**  
>This article was written for version 7 of SadConsole.

The *SadConsole.Engine* static class processes input for the entire SadConsole framework. At this point, it only includes the **Keyboard** and **Mouse** inputs as **Controller** support has not been implemented yet. **Controller** support is provided by MonoGame. Keyboard and Mouse states have been updated by the time the `Global.Update` method has been called.

When the mouse and keyboard information is sent to a console, the *SadConsole.Console.ProcessMouse(SadConsole.Input.MouseConsoleState)* and the *SadConsole.Console.ProcessKeyboard(SadConsole.Input.Keyboard)* methods will be called. Each console can indicate if it wants to process the mouse and keyboard input. These two properties, `SadConsole.Console.UseKeyboard` and `SadConsole.Console.UseMouse`, default to `True`. Regardless of the **Console** settings though, if the Engine's mouse or keyboard processing is turned off, the console will never have these methods called.

#### Engine input
Input processing by the engine can be toggled on or off with the `Settings.Input.DoKeyboard` and `Settings.Input.DoMouse` properties. Both input types are on by default.

The `Global.FocusedConsoles` property specifies which consoles should receive keyboard events. It is always the first in the stack. This console is considered the focused console. No other console will ever receive keyboard input, however, other consoles may receive mouse input. When this property changes, the `Console.Focused()` and `Console.FocusLost()` methods are called on the appropriate consoles.

When the `Engine.Update` method is called, the input system flows like this: 

1. If `Settings.Input.DoKeyboard` is true
    1. Update `Global.KeyboardState` with the latest state.
    2. Look for a `Global.FocusedConsoles.Console` to process the keyboard.
2. If `Settings.Input.DoMouse` is true
    1. Update `Global.MouseState` with the latest state.
    2. Check if `Global.FocusedConsoles.Console` has `ExclusiveFocus` set.
    3. If `ExclusiveFocus` is not set, search through `Global.CurrentScreen` for all `IConsole` types and `ProcessMouse` until one returns true.

5. Cycle through the `Global.CurrentScreen` and call `Update` on everything.
6. Invoke the `SadConsole.Game.OnUpdate` callback.

##### Mouse details
The left and right button states are tracked, as well as any information related to the console under the mouse. The `Global.MouseState` property is an instance of the `SadConsole.Input.Mouse` type and is always available for you to peek at.

| Mouse Properties | Description
| ------------------ | ----------------------------------------------------------------
| LeftButtonDown     | The left button is currently down.
| LeftClicked        | The left button was down at the last check but is currently up.
| LeftDoubleClicked  | The left button was just double clicked in under a second.
| RightButtonDown    | The right button is currently down.
| RightClicked       | The right button was down at the last check but is currently up.
| RightDoubleClicked | The right button was just double clicked in under a second.

Whenever the `ProcessMouse` method is called on an `IConsole`, the mouse state is wrapped in a `SadConsole.Input.MouseConsoleState` type which represents how the console sees the mouse.

##### Keyboard details
The keyboard works similar to the mouse. The `Global.KeyboardState` property is an instance of the `SadConsole.Input.Keyboard` type and is always available for you to peek at. This instance tracks the state of the keyboard throughout the application's lifetime. 

| Keyboard Properties | Description
| ------------ | -----------
| KeysDown     | A collection of all keys that are currently down.
| KeysPressed  | A collection of all keys that were down on the last check, but are now up, indicating a key press.
| KeysReleased | A collection of all keys are currently up. This may include keys in the KeysPressed collection.


#### Focusing consoles with the mouse
By default, a console can become the active console by clicking it with the mouse. When the mouse clicks on a console, and the `Console.FocusOnMouseClick` property is set to `True`, the `Global.FocusedConsoles` property will be set to the console. 