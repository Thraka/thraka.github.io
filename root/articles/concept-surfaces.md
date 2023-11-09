---
title: Overview of surfaces
description: Learn about how surfaces group cells together to form virtual consoles.
ms.date: 02/05/2021
---

# Surface Overview

A surface in SadConsole is collection of the cells, with each cell representing a single spot on the surface. Sometimes people use surface and console interchangeably, because they work exactly the same. In SadConsole, the <xref:SadConsole.Console> type is what people usually create to get things on the screen, but more than likely, <xref:SadConsole.ScreenSurface> is a better choice.

## Quick notes about Console

SadConsole contains a default console for you to use, as a beginner. It's sized to the screen and easily accessed through the `SadConsole.Game.Instance.StartingConsole` property. Here are some facts about <xref:SadConsole.Console>.

- Inherits from <xref:SadConsole.ScreenSurface>.
- Has <xref:SadConsole.Components.Cursor?displayProperty=fullName> component exposed through the <xref:SadConsole.Console.Cursor?displayProperty=nameWithType> property.
- The keyboard is enabled on it.
- Implements the <xref:SadConsole.ICellSurface> interface directly.

## 