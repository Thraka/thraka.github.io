title: SadConsoleEditor and GameHelper changes
date: 2016-10-16 02:00:00
tags:
- SadConsoleEditor
- SadConsole
category:
- SadConsole
---

For the last month or so, I've worked really hard on updating [SadConsoleEditor][]. If you're unfamiliar with what that is, it's the most comprehensive SadConsole project to date. It's a screen and animation editor that outputs JSON files that are compatibile with SadConsole. Here is a screenshot, with more information after it.

{% asset_img editor.png sad console preview %}

<!-- more -->

After the rewrite of SadConsole (dubbed V3), the editor tool that has been available for quite some time has been unusable. I started upgrading the tool right after V3 was released. However, the tool was a bit hard to just easily transplant one architecutre to another. I don't suspect that this architecture change would have been hard on actual games made with SadConsole. The editor however tries to disect the SadConsole types and transform them into smaller, easier to manage, types. 

Easier to manage? Yes. Since the tool lets you visually change surface data across all sorts of types (animated, multi-layered, etc), the tool must break things down into the most basic bits of data. This is something you normally don't need to do in a game.

## GameHelper enhancements

The [GameHelper][] library provides has added some more game creation help. There are three new types to help with this:

| SadConsole.Game.Zone | A rectangle region within a surface. Has a set dictionary of settings. |
| SadConsole.Game.Hotspot | One or more spots within a surface. Has a set of dictionary of settings. |
| SadConsole.Game.Scene | Combines a `LayeredTextSurface`, Zones, Hotspots, and `SadConsole.Game.GameObjects` into a single type. | 

>**NOTE** GameObject types have been available since V3. They are the replacement for the old Entity types. A GameObject combines position and animations into a single object.

The tool has been updated to handle all these types within the **Scene editor** part of the tool.

### Scenes
While your game scenes would usually have the objects placed dynamically (such as monsters or items) when you load map data, this is helpful to compose non-dynamic objects such as health bars, regional panes (like splitting the screen up into sections for consoles to be loaded into) and individual hotspots. 

Zones are useful for marking large regions of your console for something. For example, if you wanted to mark where a roof was on a roof layer.

Hotspots are helpful for marking individual spaces on the console that you can react to layter. For example you could mark where the name of the character is placed, where the health or power bar is located. Also, when you combine this with a zone, you can create an effect of some sort. As an example, you could mark the entrance to a building, combine that with a zone that represents the roof of the building, and when the player enters the building, the roof disappears. 

As time goes on, my work is going to be more focused on this library than on SadConsole. I feel like SadConsole is in a good place feature wise. There isn't a whole lot to do to enhance it. You can do pretty much anything with it, especially combined with the **Controls** library. What I want to do with the **GameHelper** library is continue to add things that make it easier to compose a game. One thing I started to add pre-V3 was a trigger/reaction system so you could compose events and reactions based on SadConsole types and data.

## Editor

The editor has three types, a standered multilayered console, animations, and a scene. 
