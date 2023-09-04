---
title: Overview of SadConsole Hosts
description: Learn about what SadConsole Hosts are. Your game has to target a specific host.
ms.date: 02/05/2021
---

# SadConsole Game Host Overview

A SadConsole Host is the graphics engine that SadConsole uses to draw to the screen, process input, and handle the game logic-loop. While the SadConsole library can be added to a project, it doesn't actually draw or handle any sort of input natively. SadConsole is designed to be generic library that isn't written to a specific game engine.

In short, the host is the rendering game engine that manages a SadConsole game. SadConsole supports two game hosts: [MonoGame](https://www.monogame.net/) and [SFML](https://www.sfml-dev.org/).

## MonoGame

The MonoGame host is [NuGet package `SadConsole.Host.MonoGame`](https://www.nuget.org/packages/SadConsole.Host.MonoGame/).

MonoGame is a community-driven game engine that supports 2D and 3D. The main SadConsole library only describes objects that are rendered in 2D, however, you can still use the 3D capabilities of MonoGame if you wanted to. MonoGame has the following features:

- As stated, 3D and 2D capabilities.
- Easy to get your app working on other operating systems.
- Built for .NET
- Large community of users.

## SFML

The SFML host is [NuGet package `SadConsole.Host.SFML`](https://www.nuget.org/packages/SadConsole.Host.SFML/).

SFML is a C engine built for 2D games. SFML has bindings for multiple languages, including .NET languages. SFML does work cross-platform, on other operating systems, but it requires a lot of work to get going. SadConsole and SFML is currently built for Windows.

## The host interface

SadConsole is designed to not know about the host rendering engine. It provides a base class that defines common functionality between all host renderers, and is defined in the <xref:SadConsole.GameHost?displayProperty=fullName> class. A host may provide specific capabilities and types, but you generally don't to use then unless you want to do something specific with the host renderer, like using 3D capabilities provided by MonoGame.

<!--

Input related:

The `FocusedScreenObjects` collection usually contains a single object, the one focused. The default behavior of an object, as it becomes focused, is to remove the previously focused object from the `FocusedScreenObjects` collection. This behavior can be changed though, by setting the <xref:SadConsole.IScreenObject.FocusedMode> to <xref:SadConsole.FocusBehavior.Push>. When an object uses `Push` behavior for focusing, such as a <xref:SadConsole.UI.Window>, it keeps the existing stack of focused objects. When the top of the stack becomes "unfocused" it's removed and the previously focused object becomes focused again.

-->


## See also

- [SadConsole `GameHost.Screen` Property](concept-host-screen.md)
