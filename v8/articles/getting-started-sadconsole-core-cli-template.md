---
description: Create a .NET Core SadConsole project with the SadConsole templates.
ms.date: 04/22/2020
---

# Create a new SadConsole .NET Core project with the SadConsole templates

This page describes how to create a new project with the SadConsole templates hosted on [NuGet](https://www.nuget.org/packages/SadConsole.Templates/). This template creates a .NET Core project. The .NET Core SDK includes easy ways to create a project, add references, and build/compile, with or without an editor.

You can also [create a new project in Visual Studio](getting-started-sadconsole-core-visualstudio.md).

## Prerequisites

[Download and install the .NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1). You can use .NET Core 2.1 or .NET Core 3.1. To see what versions you have, run `dotnet --info`. If this command fails, your install may have failed, you don't have .NET Core, or it's not added to your terminal path variable.

## Create your working folder

After you have a .NET Core SDK installed, open up a terminal. If you're unsure how to do that, search the internet for a tutorial.

Once you have a terminal open, navigate to the folder you want to code in. This tutorial just assumes you're in a clean folder waiting to input commands. For example, I opened a terminal on Windows and ran the following commands:

```shell
mkdir sadconsolegame
cd sadconsolegame
```

It's highly likely that those commands work on every operating system, in the case that they don't, they'll be similar to this. Please refer to any documentation related to your operating system.

>[!WARNING]
>The folder name you use (in this example sadconsolegame) cannot be **sadconsole**. The default behavior for .NET Core is to create a project file named the same as the folder, and then the *output* file will be the same name as the project. This will cause a clash.

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
SadConsole v8 Game                       sadconsole8                [C#]          Console/Roguelike/SadConsole
SadConsole v8 Console Class              sadconsole8console         [C#]          Console/SadConsole
SadConsole v8 Font Definition            sadconsole8font            [C#]          SadConsole
```

## Create a project

Next, create your project.

```shell
dotnet new sadconsole8
```

Now, run the game again with `dotnet run` from your terminal, and you'll see the following:

![a new console in sadconsole with hello text](images/new-core-hello-window.png)

## Next steps

Now that you have the project created and working, check out the [existing tutorials](intro.md). These will walk you through the basics.
