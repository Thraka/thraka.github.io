title: Version 7.0 Completed
date: 2018-08-30 18:39:02
tags:
- v7
- SadConsole
category:
- SadConsole
---

Version 7.0 has been released to [NuGet](https://www.nuget.org/packages/SadConsole/)!!

This release was driven by valuable feedback from you! I think this will make it a lot easier to work with surfaces, animations, and objects in general.

I did remove the world generation code. It felt very specific to a game scenario and I have plans to release a new library that adds more game functions (such as random world/dungeon generation) and plays very nicely with [GoRogue](https://github.com/Chris3606/GoRogue).

I've compiled (but not released) a  .NET Standard 2.0 version of SadConsole that will replace the existing release. This will make it easy to use with .NET Core.

The [documentation](http://sadconsole.com/docs/) is just about finished. I'll have that done tonight.

##### Next goals

1. Update the Sample Games in the GitHub repo.
1. Release the .NET Standard 2.0 build.
1. Start work on the GoRogue-SadConsole game helper.
1. Start work on redoing the Roguelike tutorial.

##### Notes about the changes

The NuGet release notes details some of these changes but the most notible are:

-   SurfaceEditor has been removed and is now implemented on SurfaceBase directly.

    You do not need to use a `SurfaceEditor` type to edit surfaces. You won't really notice this change on the `Console` as it had inherited from `SurfaceEditor`. `Console` is now a surface, and all surfaces can be edited directly.

-   Console no longer combines Renderer and TextSurface for drawing.

    `Console` had a `TextSurface` property that allowed you to swap out different surfaces that represented the visual of the console. This has gone away. A `Console` is now a surface.

-   SadConsole.Serialization uses Newtonsoft.Json instead of the default .NET classes.

    The `Newtonsoft.Json` library is much more capable of de/serializing objects. Compression is also supported now.

-   Removed GameHelpers namespace and GameObject renamed to Entity.

    GameHelpers had the GameObject and world generation code. The GameObject is now `SadConsole.Entities.Entity` and the world generation code has been removed. The generation code will be added to a different library in the future. 
    
    If you were previously using the world generation code, you can use a temporary library [here](https://github.com/Thraka/SadConsole/issues/149).

-   Control themes completely rewritten. Themes control all drawing for a control now.

    Previously the control itself would draw itself using some color settings from a theme object. Now the theme object actually [draws the entire control](https://github.com/Thraka/SadConsole/blob/c2103c218f829433d147669fc3a4fc59957fecff/src/SadConsole.Shared/Themes/ButtonTheme.cs#L44) using various settings from the control instance itself.