---
title: Overview of SadConsole Hosts
description: Learn about what SadConsole Hosts are. Your game has to target a specific host.
ms.date: 09/12/2023
---

# SadConsole Game Host Overview

A SadConsole Host is the graphics engine that SadConsole uses to draw to the screen, process input, and handle the game logic-loop. While the SadConsole library can be added to a project, it doesn't actually draw or handle any sort of input natively. SadConsole is designed to be generic library that isn't written to a specific game engine, which is what the "host" provides.

In short, the host is the rendering game engine that manages a SadConsole game. SadConsole supports two game hosts: [MonoGame](https://www.monogame.net/) and [SFML](https://www.sfml-dev.org/).

## MonoGame

The MonoGame host is [NuGet package `SadConsole.Host.MonoGame`](https://www.nuget.org/packages/SadConsole.Host.MonoGame/).

MonoGame is a community-driven game engine that supports 2D and 3D. The main SadConsole library only describes objects that are rendered in 2D; however, you can still use the 3D capabilities of MonoGame if you wanted to. MonoGame has the following features:

- As stated, 3D and 2D capabilities.
- Easy to get your app working on other operating systems.
- Built for .NET
- Large community of users.

## SFML

The SFML host is [NuGet package `SadConsole.Host.SFML`](https://www.nuget.org/packages/SadConsole.Host.SFML/).

SFML is a C engine built for 2D games. SFML has bindings for multiple languages, including .NET languages. SFML does work cross-platform, on other operating systems, but it requires a lot of work to get going. SadConsole and SFML is currently built for Windows.

## The host interface

SadConsole is designed to not know about the host rendering engine. It provides a base class that defines common functionality between all host renderers, and is defined in the <xref:SadConsole.GameHost?displayProperty=fullName> class. A host may provide specific capabilities and types, but you generally don't to use them unless you want to do something specific with the host renderer, like using 3D capabilities provided by MonoGame.

After a game is started, the host is accessed through the <xref:SadConsole.GameHost.Instance?displayProperty=nameWithType> property.

## Input processing

SadConsole performs many operations during the **update** frame, one of which, is processing input. The input processor handles keyboard and mouse state. Mouse related input events are continually processed on the object under the mouse cursor, but the keyboard is different. Only the object currently _focused_ is provided keyboard input. The focused object is designated by the <xref:SadConsole.GameHost.FocusedScreenObjects?displayProperty=nameWithType> property.

> [!TIP]
> You can easily have an object set itself as focused by setting <xref:SadConsole.IScreenObject.IsFocused> to `true`.

### Mouse

Mouse input scans the <xref:SadConsole.GameHost.Screen?displayProperty=nameWithType> object hierarchy and determines which object is under the mouse. That object's <xref:SadConsole.IScreenObject.ProcessMouse(SadConsole.Input.MouseScreenObjectState)> method is called. Usually, this method checks the <xref:SadConsole.IScreenObject.UseMouse> property, if it's `true`, the object handles the mouse. When the object processes the mouse, it raises the following mouse input events:

- <xref:SadConsole.IScreenSurface.MouseEnter>
- <xref:SadConsole.IScreenSurface.MouseMove>
- <xref:SadConsole.IScreenSurface.MouseButtonClicked>
- <xref:SadConsole.IScreenSurface.MouseExit>

### Keyboard

Keyboard input is always sent to the object that is "focused." An object is considered focused when its <xref:SadConsole.IScreenObject.IsFocused> property is set to `true` and it's the top-most object of the <xref:SadConsole.GameHost.FocusedScreenObjects?displayProperty=nameWithType> collection. When focused, the object's <xref:SadConsole.IScreenObject.ProcessKeyboard(SadConsole.Input.Keyboard)> method is called every **update** frame, and if the object's <xref:SadConsole.IScreenObject.UseKeyboard> property is `true`, the keyboard is processed.
