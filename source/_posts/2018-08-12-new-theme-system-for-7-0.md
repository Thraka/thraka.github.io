title: New Theme System for 7.0
date: 2018-08-12 22:14:59
tags:
- v7
- SadConsole
category:
- SadConsole
---

The last two weeks have been pretty busy, but I've been slowly banging around on the controls redesign for SadConsole. One thing that I didn't like with how the controls worked was that each control does its own drawing. This means that if you have a button control, and you wanted it to draw a border around it all of the time, you had to create a new button class; adding whole new types of controls just to tweak the drawing of an existing control. 

The new system for 7.0 decouples the drawing from the control, putting all drawing in the theme assigned to the control. To create a new button look-and-feel, create a new theme type and customize the draw method. This new "the theme draws the control" system is also implemented on the `ControlsConsole` and `Window` types.

To test this out I create 3 types of themes for the button type: 

1.  The normal button from v6.
2.  A 3d button.
3.  Bordered 3d button (which uses the SadConsole extended character sheet). 

{% asset_img new-button-themes.gif 3d themes for buttons in sadconsole %}