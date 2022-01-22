---
description: Create a SadConsole project with Visual Studio.
ms.date: 02/05/2021
---

# Create a new SadConsole .NET project with Visual Studio

This page describes how to create a new project based on SadConsole Standard using .NET 6.0 and Visual Studio.

Before using Visual Studio, you'll need to install the SadConsole project templates. The first few sections of the [Create a new SadConsole .NET project with the SadConsole templates](getting-started-cli.md). article describes how to do this. Follow those instructions and then come back to this article.

## Prerequisites

01. [Download and install Visual Studio 2022](https://visualstudio.microsoft.com/vs/).
02. During install, make sure that you select the **.NET Core cross-platform development** workload.

    - If you have already installed Visual Studio, you can run the **Visual Studio Installer** that was added to your computer, and modify your installation to add the **.NET Core cross-platform development** workload.
03. Install the SadConsole templates with the dotnet command: `dotnet new --install SadConsole.Templates`. For more information, see [Create a new SadConsole .NET project with the SadConsole templates](getting-started-cli.md)

## Create a new project

Start Visual Studio.

01. In the *Create a new project* dialog, type **sadconsole** into the search box and select the **SadConsole Game (MonoGame)** project template.

    ![create a new sadconsole project in visual studio](images/getting-started-visualstudio/template.png)
    
    The `SadConsole Game (MonoGame)` template creates a SadConsole game that uses [MonoGame](https://www.monogame.net/) and the `SadConsole Game (SFML)` template creates a game that uses [SFML](https://www.sfml-dev.org/). MonoGame and SFML are the backend renderers for SadConsole. In general, the code you use for SadConsole doesn't care which rendering system you use. However, as your game progresses, which renderer you choose is very important. Currently, it's recommended that you use the MonoGame renderer as it has the following benefits:
    
    - Easier cross-platform targeting.
    - Supports 3D rendering: models, scenes, etc.
    - Built for .NET coding
    
    SFML is cross-platform, but it takes more work on your side to get that working.

02. Press _Next_ and then set the _Project name_ to **SadConsoleExample** and choose a _location_ on your computer to save the project.

Congratulations, you have a new project! Press <kbd>F5</kbd> to run the game:

![a new console in sadconsole with hello text](images/getting-started-visualstudio/hello-window.png)

## Creating a project without a template

If you want to create a project without using the SadConsole templates, it's also pretty easy to do.

01. Create a new C# or VB.NET project. Follow the wizard to set the name of the project and where you're going to save it. Choose .NET version 6.0.
01. Next, add the NuGet SadConsole MonoGame renderer package to the project.

    01. In the _Solution Explorer_, right-click on the project and select _Manage NuGet Packages_. This will display the NuGet package manager.
    01. Search for _SadConsole_ and install the `SadConsole.Host.MonoGame` package to your project.

Congratulations, you have all of the required libraries to start creating a SadConsole game!

### Create the game

You need to change the startup code that was automatically generated for your project.

In the _Solution Explorer_ pane, double-click the _Program.cs_ file to open it. You should see a very simple console application that we will change:

```csharp
using System;

namespace SadConsoleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

Change this code to:

```csharp
using System;
using SadConsole;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace SadConsoleGame
{
    public static class Program
    {
        static void Main()
        {
            // Setup the engine and create the main window.
            Game.Create(80, 25);

            // Hook the start event so we can add consoles to the system.
            Game.Instance.OnStart = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Init()
        {
            Game.Instance.StartingConsole.FillWithRandomGarbage(SadConsole.Game.Instance.StartingConsole.Font);
            Game.Instance.StartingConsole.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, Mirror.None);
            Game.Instance.StartingConsole.Print(4, 4, "Hello from SadConsole");
        }
    }
}
```

Press the <kbd>F5</kbd> key to run your SadConsole program. You should be presented with the following screen:

![a new console in sadconsole with hello text](images/getting-started-visualstudio/hello-window.png)

## Next steps

Now that you have the project created and working, check out the [Get Started 1 - Draw on a console](tutorials/getting-started/part-1-drawing.md) tutorial.
