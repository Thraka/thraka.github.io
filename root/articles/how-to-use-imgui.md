---
title: How to use ImGui in SadConsole
description: Learn how to use ImGui. ImGui support for SadConsole is included in the the debugger package, which only works with MonoGame.
ms.date: 12/04/2024
---

# Enable ImGui in SadConsole

Dear ImGui is a bloat-free graphical user interface library for C++ that is designed to be fast, portable, and easy to integrate. It is particularly popular in the game development industry and real-time applications due to its immediate mode GUI paradigm, which simplifies the creation of dynamic user interfaces.

ImGui is supported with SadConsole through the [SadConsole.Debug.MonoGame NuGet package](https://www.nuget.org/packages/SadConsole.Debug.MonoGame/). While the package provides a debugger, it also provides a basic ImGui implementation that you can use to render ImGui controls on top of SadConsole.

Please note that ImGui operates in a unique way. It follows a very specific way of designing the UI. Because of the way it works, you can alter most of the ImGui commands without stopping your game. Simply alter the code and use your IDE to hot-reload the code. ImGui instantly responds to the changes.

Use the [ImGui GitHub repository](https://github.com/ocornut/imgui) to see examples and learn how to use ImGui. ImGui is implemented in C++ so the majority of the examples will be in C++ and you'll have to convert them to C#, which is usually straight forward.

## How it's implemented

SadConsole uses the [Hexa.NET.ImGui](https://github.com/HexaEngine/Hexa.NET.ImGui) library to provide ImGui functionality. ImGui is implemented as a MonoGame Game Component, which runs both before and after the SadConsole Game Component.

For ImGui to work, it must perform these following steps:

- Update IO state, like the mouse and keyboard.
- Create a new ImGui game frame.
- Create all widgets and UI.
- Generate rendering data.
- Render to MonoGame.

To render your ImGui widgets, you create instances of either `SadConsole.ImGuiSystem.ImGuiObjectBase` or `SadConsole.ImGuiSystem.ImGuiObjectCollection`, and add them to the `SadConsole.ImGuiSystem.ImGuiMonoGameComponent.UIComponents` collection. These types have a method, `BuildUI` which is where you write all of your ImGui code to draw UI. The instance of `ImGuiMonoGameComponent` added to MonoGame is available when SadConsole starts.

## Add ImGui to your project

To add ImGui to your project:

01. Add the [SadConsole.Debug.MonoGame NuGet package](https://www.nuget.org/packages/SadConsole.Debug.MonoGame/) to your project.
01. Add `EnableImGui` to the configuration builder.
01. Use a method callback to add the ImGui objects to the ImGui MonoGame Component.

[!code-csharp[](./snippets/how-to-use-imgui/csharp/Program.cs?highlight=10-12)]

The previous code uses the method callback to add an instance of `ImGuiWindow1` to the ImGui MonoGame Component. The implementation of `ImGuiWindow1` is described in the following section.

## Create an object that generates ImGui UI

Derive from `SadConsole.ImGuiSystem.ImGuiObjectBase` to create a class that's processed by ImGui and draws things to the screen. Override the `BuildUI` method and make calls to the ImGui API. For example, here's the `ImGuiWindow1` class that was used in the previous section:

[!code-csharp[](./snippets/how-to-use-imgui/csharp/ImGuiWindow1.cs?highlight=7,11)]

This ImGui code generates the following UI:

![A SadConsole screen with a ImGui UI over the top of it.](images/how-to-use-imgui/example.png)

## How to find the MonoGame component

After SadConsole starts and creates the ImGui Game Component, you can retrieve reference to it through the MonoGame Game:

```csharp
SadConsole.Game.Instance.MonoGameInstance.Components.OfType<SadConsole.ImGuiSystem.ImGuiMonoGameComponent>().First()
```
