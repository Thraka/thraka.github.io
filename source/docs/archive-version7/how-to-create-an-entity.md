title: How to create an Entity
layout: docpage
comments: false
searcharchive: true
---

>**NOTE**  
>This article was written for version 7 of SadConsole.

The `SadConsole.Entities.Entity` type provides a simple way to independently track text objects. 

Entities are very similar to consoles, but they have a different purpose. Where a console provides features related to typing, mouse handling, and a virtual cursor, the `Entity` type provides simple animation management. By default, the entity doesn't do anything with the keyboard or mouse.

# Animation
Entities are positioned objects that do not have a width or height. However, when an entity is drawn, it draws a `SadConsole.Surfaces.Animated` object, which does have a width and height. This is defined by the `Entity.Animation` property.

## Frames
The animation is made up of individual frames. When you call the `SadConsole.Surfaces.Animated.CreateFrame` method, you're returned a `SadConsole.Surfaces.BasicNoDraw` type which represents the surface array of cells. You can edit that animation frame as you would any other surface or console in SadConsole. Every time you call `CreateFrame`, you create a new frame in the animation. When you're finished, you have multiple frames that make up the animation.

The total amount of time it takes to play all frames is in seconds, and is set with the `Animated.AnimationDuration` property. 

# Example
The following example demonstrates how to create a basic animation and add it to an entity.

First, declare a variable to hold our entity.

```csharp
private SadConsole.Entities.Entity fallingText;
```

Next, initialize the entity, create an animation, attach the animation to the entity, and position the entity.

```csharp
// Height of 5, one line for each frame
var animation = new SadConsole.Surfaces.Animated("default", 24, 5);

for (var line = 0; line < 5; line++)
{
    // use the default animation built into the object
    var frame = animation.CreateFrame();
    frame.Fill(Color.Purple, Color.DarkGray, 0, null);
    frame.Print(1, line, "Hello World!");
}

animation.AnimationDuration = 2;
animation.Repeat = true;
animation.Start();

// Create the new entity
var fallingTextEntity = new SadConsole.Entities.Entity(animation);
fallingTextEntity.Position = new Point(1, 1);

// Add the entity to the current screen so we can preview it
Global.CurrentScreen.Children.Add(fallingTextEntity);
```

You should see the following animation:

![animated entity](images/falling-text.gif)


Next, read through [this](display-entity-on-console-viewport.md) tutorial to understand how to draw an entity with a console.