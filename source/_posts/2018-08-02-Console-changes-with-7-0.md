title: Console changes with 7.0
date: 2018-08-02 12:59:41
tags:
- v7
- SadConsole
category:
- SadConsole
---

As the previous entry stated, the `Console` type is changing. As an example take creating a border on the outside of a console. This involved creating a derived class that handled the drawing of the console itself and the border. There was a lot to do just to create the border.

### V6 style

```csharp
public class BorderedConsole: Console
{
    private SadConsole.Surfaces.BasicSurface border;
    private DrawCallSurface drawCall;

    public BorderedConsole(int width, int height):base(width, height)
    {
        border = new BasicSurface(width + 2, height + 2);
        border.DefaultBackground = Color.Black;

        var editor = new SurfaceEditor(border);
        editor.Clear();

        var borderBox = Box.Thick();
        borderBox.Position = new Point(0, 0);
        borderBox.Width = border.Width;
        borderBox.Height = border.Height;
        borderBox.Draw(editor);

        Renderer.Render(border);

        drawCall = new DrawCallSurface(border, CalculatedPosition, false);

        Print(0, 0, "Hello inside the border");
    }

    public override void Draw(TimeSpan delta)
    {
        Global.DrawCalls.Clear();
        drawCall.Position = border.Font.GetWorldPosition((CalculatedPosition - new Point(1))).ToVector2();
        Global.DrawCalls.Add(drawCall);

        base.Draw(delta);
    }
}
```

### V7 style

In 7.0 this is much easier, and it doesn't require creating a derived class.

```csharp
var console = new SadConsole.Console(10, 10);
var border = new SadConsole.Surfaces.Basic(console.Width + 2, console.Height + 2);

console.Position = new Point(10, 10);
console.Print("Hello inside the border");

border.DrawBox(new Rectangle(0, 0, border.Width, border.Height), 
               foreground: Color.White, background: Color.Black, 
               connectedLineStyle: SurfaceBase.ConnectedLineThick);

border.Draw(TimeSpan.Zero);
border.Position = new Point(-1, -1);

console.Children.Add(border);
```

Sure, some code is missing because it's easier to draw shapes now, but this also requires less overhead in types you create just to do simple things. And since any surface is now a positionable and drawable object, it can be added as a child to a console. Now the position of the border is automatically kept in place.
