---
title: Create a new project
description: Create a SadConsole project with the SadConsole templates.
ms.date: 02/05/2021
---

# Create a new SadConsole .NET project with the SadConsole templates

This page describes how to create a new project with the SadConsole templates hosted on [NuGet](https://www.nuget.org/packages/SadConsole.Templates/). This template creates a .NET project. The .NET SDK includes easy ways to create a project, add references, and build/compile, with or without an editor.

It's very simple to create a project this way, and then afterwords, use your favorite editor such as Visual Studio. If you prefer, you can create a [new SadConsole project with Visual Studio](getting-started-visualstudio.md).

## Prerequisites

- [Download and install the .NET 5.0 (or later) SDK](https://dotnet.microsoft.com/download/dotnet-core/5.0).

  To see what versions you have, run `dotnet --info`. If this command fails, your install may have failed, you don't have .NET Core, or it's not added to your terminal path variable.

  You can use .NET Core 3.1 if you would like.

## Create your working folder

After you have a .NET SDK installed, open up a terminal. If you're unsure how to do that, search the internet for a tutorial.

Once you have a terminal open, navigate to the folder you want to code in. This tutorial just assumes you're in an empty folder waiting to input commands. For example, I opened a terminal on Windows and ran the following commands:

```shell
mkdir sadconsolegame
cd sadconsolegame
```

It's highly likely that those commands work on every operating system, in the case that they don't, they'll be similar to this. Please refer to any documentation related to your operating system.

>[!WARNING]
>The folder name you use (in this example _sadconsolegame_) can't be **sadconsole**. The default behavior for .NET is to create a project file named the same as the folder, and then the *output* file will be the same name as the project. This will cause a clash with the _SadConsole.dll_ library.

## Install the templates

Next, install the SadConsole templates:

```shell
dotnet new --install SadConsole.Templates
```

When this command runs, it lists every template installed. You can run a command to list the SadConsole related templates:

```shell
dotnet new sadconsole --list
```

You should see output similar to the following:

```shell
Templates                                Short Name                 Language      Tags
--------------------------------------------------------------------------------------------------------------
SadConsole v9 Game (MonoGame)            sadconsole9mg              [C#]          Console/Roguelike/SadConsole
SadConsole v9 Game (SFML)                sadconsole9sfml            [C#]          Console/Roguelike/SadConsole
SadConsole v9 Console Class              sadconsole9console         [C#]          Console/SadConsole
SadConsole v9 Font Definition            sadconsole9font            [C#]          SadConsole
```

The `Game (MonoGame)` template creates a SadConsle game that uses [MonoGame](https://www.monogame.net/) and the `Game (SFML)` template creates a game that uses [SFML](https://www.sfml-dev.org/). MonoGame and SFML are the backend renderers for SadConsole. In general, the code you use for SadConsole doesn't care which rendering system you use. However, as your game progresses, which renderer you choose is very important. Currently, it's recommended that you use the MonoGame renderer as it has the following benefits:

- Easier cross-platform targeting.
- Supports 3D rendering: models, scenes, etc.
- Built for .NET coding

SFML is cross-platform, but it takes more work on your side to get that working. It may have a better framerate though, but I wouldn't worry about that until you run into some problem. Regarding framerates, both renderers perform pretty much the same, you may not even see a difference between the two.

## Create a project

Your terminal should be in the game directory you created at the start of this article. Create your project using the `dotnet new` command:

```shell
dotnet new sadconsole9mg
```

This command generates a small example game that you can then customize. Run the game with `dotnet run` from your terminal, and you'll see the following:

![a new console in sadconsole with hello text](images/getting-started-cli/hello-window.png)

Use your favorite IDE or editor to open your newly created project, something like Visual Studio or Visual Studio Code.

## Next steps

Now that you have the project created and working, check out the [other articles](index.md). These will walk you through the basics.
