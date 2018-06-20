title: How to create a GameObject
layout: docpage
---

The `SadConsole.GameHelpers.GameObject` type provides a simple way to independently track text objects. 

Game objects are very similar to consoles, but they have a different focus. Where a console provides features related to typing, mouse handling, and a virtual cursor, the `GameObject` type provides simple animation management. By default, the game object type doesn't do anything with the keyboard or mouse.

# Animation
The game object presents a single animation to the screen when rendered. The `GameObject.Animation` property is the `SadConsole.Surfaces.AnimatedSurface` type. The animation defines the size of the game object.

## Frames
The animation is used to create individual frames. When you call the `AnimatedSurface.CreateFrame` method, you're returned a `SadConsole.Surfaces.BasicSurface` type which represents the surface array of cells. You can modify the cells individually or you can provide that frame to a `SadConsole.Surfaces.SurfaceEditor` type for helpful editing methods (SetBackground, Print, SetGlyph, etc).

The total amount of time it takes to play all frames is in seconds and is set on the `AnimatedSurface.AnimationDuration` property. 

# Example
The following example demonstrates how to create a basic animation and add it to an game object. The animation is text that moves down the object.

First, declare a variable to hold our game object.
```csharp
private SadConsole.GameHelpers.GameObject fallingText;
```

Next, initialize the game object, create an animation, attach the animation to the game object, and position the game object.

```csharp
fallingText = new SadConsole.GameHelpers.GameObject();

// Height of 5, one line for each frame
var textAnimation = new SadConsole.Surfaces.AnimatedSurface("default", 14, 5);

// We create a "BasicSurface" just to satisfy the creation of the SurfaceEditor
var editor = new SadConsole.Surfaces.SurfaceEditor(new SadConsole.Surfaces.BasicSurface(1, 1));

for (int line = 0; line < 5; line++)
{
    var frame = textAnimation.CreateFrame();
    editor.TextSurface = frame;
    editor.Fill(Color.Purple, Color.DarkGray, 0, null);
    editor.Print(1, line, "Hello World!");
}

textAnimation.AnimationDuration = 2;
textAnimation.Repeat = true;

textAnimation.Start();

fallingText.Animation = textAnimation;
fallingText.Position = new Point(1, 1);
```

During each update and render pass, you need to call `GameObject.Update` and `GameObject.Draw`. You could do this in a derived console that overrides the render and update methods.

```csharp
fallingText.Update();
```

and

```csharp
fallingText.Draw(time.ElapsedGameTime);
```

Next, read through [this](displaying-gameobjects-on-a-console-viewarea.md) tutorial to understand how to draw an entity with a console.