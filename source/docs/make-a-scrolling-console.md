title: How to make a scrolling Console
layout: docpage
---

Because a console already supports the idea of a [ViewPort](displaying-gameobjects-on-a-console-viewarea.md#use-a-render-area), you can combine it with a scroll bar to control what is shown on the screen. These two items would effectively create a user controlled scrolling console. You could have this become a map or even a more traditional "console" where the user scrolls up/down to see the history.

This article will show how to create a scrollable-history console.

## Three consoles

We're using three types of consoles to make a user-scrollable console: a normal `Console` which is the main display, a `ControlsConsole` to host the scroll bar, and a `ConsoleContainer` to group it all together.

Since the `ConsoleContainer` groups the main console and the scrolling part together, it will represent our new `ScrollingConsole` class. This class will define:

1. The **width**/**height** of the console. (Displayble portion of the console)
2. The **buffer** height of the console. (Max height of the console; scrollable area)
3. The controls console to host a scroll bar control.
4. How much of the buffer has been used.

```csharp
class ScrollingConsole: SadConsole.ConsoleContainer
{
    SadConsole.Console mainConsole;                 // The main console that is typed into
    SadConsole.ControlsConsole controlsHost;        // The scroll bar host
    SadConsole.Controls.ScrollBar scrollBar;        // The scroll bar

    int scrollingCounter;   // This is a counter to indicate how much buffer is used

    public ScrollingConsole(int width, int height, int bufferHeight)
    {
        UseKeyboard = true;
        UseMouse = true;
        controlsHost = new ControlsConsole(1, height);

        mainConsole = new Console(width - 1, bufferHeight);
        mainConsole.ViewPort = new Rectangle(0, 0, width - 1, height);
        mainConsole.Cursor.IsVisible = true;

        scrollBar = SadConsole.Controls.ScrollBar.Create(SadConsole.Orientation.Vertical, height);
        scrollBar.IsEnabled = false;
        scrollBar.ValueChanged += ScrollBar_ValueChanged;

        controlsHost.Add(scrollBar);
        controlsHost.Position = new Point(1 + mainConsole.Width, Position.Y);

        Children.Add(mainConsole);
        Children.Add(controlsHost);

        scrollingCounter = 0;
    }

    private void ScrollBar_ValueChanged(object sender, EventArgs e)
    {
        // Set the visible area of the console based on where the scroll bar is
        mainConsole.ViewPort = new Rectangle(0, scrollBar.Value, mainConsole.Width, mainConsole.ViewPort.Height);
    }

}
```

The code above does a few things. First it creates the main console that will be scrolled (`mainConsole`), setting the view area and height buffer. Then it creates the scroll bar (`scrollBar`) and a controls console (`controlsHost`) to hold it. These two consoles are added to the current base class, `ConsoleContainer` with the `Children.Add` method.

## Handling the input

Our class, `ScrollingConsole`, is the main console we add to `SadConsole.Global.CurrentScreen` (or to some other parent) so that it receives input. When it gets input, it will be forwarded to one of the child consoles. The `mainConsole` object will receive the keyboard input and the `controlsHost` will receive mouse input. Keyboard input is used to type in the `mainConsole`, and mouse input is used to move the scroll bar.

```csharp
class ScrollingConsole: SadConsole.ConsoleContainer
{
    ...

    public override bool ProcessKeyboard(Keyboard state)
    {
        // Send keyboard input to the main console
        return mainConsole.ProcessKeyboard(state);
    }

    public override bool ProcessMouse(MouseConsoleState state)
    {
        // Check the scroll bar for mouse info first. If mouse not handled by scroll bar, then..

        // Create a mouse state based on the controlsHost
        if (!controlsHost.ProcessMouse(new MouseConsoleState(controlsHost, state.Mouse)))
        {
            // Process this console normally.
            return mainConsole.ProcessMouse(state);
        }

        return false;
    }

}
```

Next we need to monitor how many times the console gets scrolled from input. This increases our `scrollingCounter` variable and modifies the scroll bar's maximum to allow scrolling to the correct areas.

```csharp
class ScrollingConsole: SadConsole.ConsoleContainer
{
    ...

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        // If we detect that this console has shifted the data up for any reason (like the virtual cursor reached the
        // bottom of the entire text surface, OR we reached the bottom of the render area, we need to adjust the 
        // scroll bar and follow the cursor
        if (mainConsole.TimesShiftedUp != 0 | mainConsole.Cursor.Position.Y >= mainConsole.ViewPort.Height + scrollingCounter)
        {
            // Once the buffer has finally been filled enough to need scrolling (a single screen's worth), turn on the scroll bar
            scrollBar.IsEnabled = true;

            // Make sure we've never scrolled the entire size of the buffer
            if (scrollingCounter < mainConsole.Height - mainConsole.ViewPort.Height)
                // Record how much we've scrolled to enable how far back the bar can see
                scrollingCounter += mainConsole.TimesShiftedUp != 0 ? mainConsole.TimesShiftedUp : 1;

            scrollBar.Maximum = (mainConsole.Height + scrollingCounter) - mainConsole.Height;

            // This will follow the cursor since we move the render area in the event.
            scrollBar.Value = scrollingCounter;

            // Reset the shift amount.
            mainConsole.TimesShiftedUp = 0;
        }
    }

}
```

And that's it. The console now has the ability to scroll the designated height buffer. Here is the code to the entire class.

## Finished class

```csharp
class ScrollingConsole : SadConsole.ConsoleContainer
{
    SadConsole.Console mainConsole;                 // The main console that is typed into
    SadConsole.ControlsConsole controlsHost;        // The scroll bar host
    SadConsole.Controls.ScrollBar scrollBar;        // The scroll bar

    int scrollingCounter;   // This is a counter to indicate how much buffer is used

    public ScrollingConsole(int width, int height, int bufferHeight)
    {
        UseKeyboard = true;
        UseMouse = true;
        controlsHost = new ControlsConsole(1, height);

        mainConsole = new Console(width - 1, bufferHeight);
        mainConsole.ViewPort = new Rectangle(0, 0, width - 1, height);
        mainConsole.Cursor.IsVisible = true;

        scrollBar = SadConsole.Controls.ScrollBar.Create(SadConsole.Orientation.Vertical, height);
        scrollBar.IsEnabled = false;
        scrollBar.ValueChanged += ScrollBar_ValueChanged;

        controlsHost.Add(scrollBar);
        controlsHost.Position = new Point(1 + mainConsole.Width, Position.Y);

        Children.Add(mainConsole);
        Children.Add(controlsHost);

        scrollingCounter = 0;
    }

    private void ScrollBar_ValueChanged(object sender, EventArgs e)
    {
        // Set the visible area of the console based on where the scroll bar is
        mainConsole.ViewPort = new Rectangle(0, scrollBar.Value, mainConsole.Width, mainConsole.ViewPort.Height);
    }

    public override bool ProcessKeyboard(Keyboard state)
    {
        // Send keyboard input to the main console
        return mainConsole.ProcessKeyboard(state);
    }

    public override bool ProcessMouse(MouseConsoleState state)
    {
        // Check the scroll bar for mouse info first. If mouse not handled by scroll bar, then..

        // Create a mouse state based on the controlsHost
        if (!controlsHost.ProcessMouse(new MouseConsoleState(controlsHost, state.Mouse)))
        {
            // Process this console normally.
            return mainConsole.ProcessMouse(state);
        }

        return false;
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        // If we detect that this console has shifted the data up for any reason (like the virtual cursor reached the
        // bottom of the entire text surface, OR we reached the bottom of the render area, we need to adjust the 
        // scroll bar and follow the cursor
        if (mainConsole.TimesShiftedUp != 0 | mainConsole.Cursor.Position.Y >= mainConsole.ViewPort.Height + scrollingCounter)
        {
            // Once the buffer has finally been filled enough to need scrolling (a single screen's worth), turn on the scroll bar
            scrollBar.IsEnabled = true;

            // Make sure we've never scrolled the entire size of the buffer
            if (scrollingCounter < mainConsole.Height - mainConsole.ViewPort.Height)
                // Record how much we've scrolled to enable how far back the bar can see
                scrollingCounter += mainConsole.TimesShiftedUp != 0 ? mainConsole.TimesShiftedUp : 1;

            scrollBar.Maximum = (mainConsole.Height + scrollingCounter) - mainConsole.Height;

            // This will follow the cursor since we move the render area in the event.
            scrollBar.Value = scrollingCounter;

            // Reset the shift amount.
            mainConsole.TimesShiftedUp = 0;
        }
    }
}
```