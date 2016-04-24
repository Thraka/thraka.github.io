title: Thoughts on rendering a console
date: 2016-04-23 17:46:02
tags:
- SadConsole
category:
- SadConsole
---

The more and more that I use **SadConsole** on my own little games, the more I grow to dislike the way I render to the screen. Now, I don't mean specifically the rendering code itself. It's pretty good, though I have an improvement coming soon.

What I mean is the way you, say from a console, get your data rendered to the screen. A console generally has two roles, it either reads input from the user (providing visual feedback) and/or it presents characters to the user. However, the capabilities you want in drawing characters to the screen may vary. You may:

- Use a standard Width x Height console.
- Use a bigger console and allow scrolling.
- Use a single console the entire time.
- Use many different consoles positioned on the screen.
- A console that has multiple layers.
- A console with a bunch of extra metadata.
- A console for UI controls.
- A floating window console.
- An animated console (entity).

I think that is all the types provided by **SadConsole**. Here is a diagram of (out of all the libraries for SadConsole) the way the classes are structured.

{% asset_img cellsrenderer-class.png animated static ascii %}

The `CellsRenderer` type is what actually does the drawing to the screen. All the other classes that inherit it (or inherit from `Console`) are really specialty classes that are about how a console operates.

| Type | Behavior |
| ---- | -------- |
| CellsRenderer | Handles drawing data to the screen.<br>Manages viewport and scrolling.<br>Manages cell effects. |
| Console | Implements input keyboard handling.<br>Provides a cursor for input.<br> |
| LayeredConsole | Inherits from Console.<br>Provides layering. |
| ControlsConsole | Inherits from Console.<br>Supports rendering UI controls. |
| Window | Inherits from ControlsConsole, Console.<br>A bordered, title, movable console.<br>Modal or non-modal. |
| GameConsole | Inherits from LayeredConsole.<br>Adds a metadata system to attach data to cells. |
| Entity | Standalone console supporting animation. |

## Why this is bad

Now where things get messy is in cases like the **Entity** or a **Window** type. These console types shouldn't support scrolling. You don't make an entity that is bigger than the screen and only render a subset of it at a time. The same for a window. A window is meant to be totally visible on the screen. (the UI system supports hooking up scroll bars and things)

So what happens to an entity when you adjust the viewport to only render a subset of the cells? Nothing bad mind you, but you may not get what you expected. Especially when the entity is animating and flipping through multiple cell surfaces that have completely different sets of cells.

The problem is that the **CellsRenderer** type supports all these rendering systems out of the box, it's a jack-of-all-trades. Which is great for flexibility, but it puts a strain on the developer to deeply understand the behavior of the types and not mess something up.

### The entity

The way the entity works is it uses an animation type. The animation is a bunch of frames (CellData classes) and each frame is presented as the *current frame* during rendering. If the *current frame* hasn't changed, the **CellsRenderer** doesn't check for which cells to render, it already has them cached.

### Back to the problem

Now I ran into a situation where even myself, the one most familiar with the code, was confused as to why my entity wasn't rendering correctly. What I had done is resized the animation of an entity, which just resizes the CellData of all the animation's frames. The *current frame* class reference hadn't changed, but the size of the frame did. So when the **CellsRenderer** went to render the old 1x1 console, it only drew the top-left cell even though the CellsSurface was much bigger. Ouch. The fix was just to force a recalculation after I resized the animation.

Now with an entity, all this viewport rect rendering stuff doesn't matter. So the system that is in place for other consoles was stepping on the simpler entity rendering.

Well, I want this library to be easy to work with, so I think I need to separate out functionality out either into components, or specialized consoles. And then for when rendering systems must be different, specialized rendering. I'm not sure what it will look like, but I want to make sure it is easier for others to work with. 

There are some awesome .NET console game libraries (like [RLNET](https://bitbucket.org/clarktravism/rlnet/) and [SunShine Console](https://github.com/derrickcreamer/SunshineConsole)) out there much like mine that are quick and easy to use. That is something I want for SadConsole. I think I've gotten part of the way there though with NuGet. But more can be done...

.. Stay tuned ..
