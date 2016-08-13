title: Easier Startup
date: 2016-08-12 22:16:17
tags:
- SadConsole
category:
- SadConsole
---

First, NuGet has been updated with a bunch more fixes. I'm trying my best now to stick with the [semantic versioning](http://semver.org/) format. It's hard to transition from the old versioning style. It's also getting harder and harder to not break interface on updates. So I'm really going to try hard to no longer do that. 

Down to business.....

I've created new init sequences for SadConsole that removes all the crap that comes with MonoGame. That means if you're not making a full MonoGame-game where you need all the extra 3D management, you can omit the whole giant `game1.cs` class and instead use this nice, compact, easy, startup file.

<!-- more -->

```csharp
using System;
using Console = SadConsole.Consoles.Console;
using SadConsole.Consoles;
using Microsoft.Xna.Framework;

namespace MyNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup the engine and creat the main window.
            SadConsole.Engine.Initialize("IBM.font", 80, 25);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Engine.EngineStart += Engine_EngineStart;

            // Hook the update event that happens each frame so we can do logic.
            SadConsole.Engine.EngineUpdated += Engine_EngineUpdated;

            // Start the game.
            SadConsole.Engine.Run();
        }

        private static void Engine_EngineStart(object sender, EventArgs e)
        {
            var defaultConsole = (Console)SadConsole.Engine.ActiveConsole;

            defaultConsole.Print(1, 1, "Welcome to SadConsole", Color.Aqua, Color.Black);
        }

        private static void Engine_EngineUpdated(object sender, EventArgs e)
        {
            // Game logic if doing it in a console directly.
        }
    }
}
``` 

Of course that is how to startup with MonoGame. I'm also introducing support for SFML.

### SFML

>[SFML](http://www.sfml-dev.org/) provides a simple interface to the various components of your PC, to ease the development of games and multimedia applications. It is composed of five modules: system, window, graphics, audio and network.

That is the summary of what SFML website says. It's a C++ library that someone has lovingly provided a .NET wrapper for on NuGet. I'm using the wrapper. 

Using the SFML version of SadConsole doesn't really change much from your perspective. While the MonoGame framework itself is cleaner from a game developer perspective, there is a bit of overhead. SFML renders slightly faster.

The NuGet packages for SFML are

* https://www.nuget.org/packages/SadConsole.Core.SFML/
* https://www.nuget.org/packages/SadConsole.Controls.SFML/
* https://www.nuget.org/packages/SadConsole.GameHelpers.SFML/
* https://www.nuget.org/packages/SadConsole.Ansi.SFML/

The starter tutorial for SFML is available [here](https://github.com/Thraka/SadConsole/wiki/NuGet%20Starter%20SFML)