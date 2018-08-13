title: New Theme System for 7.0
date: 2018-08-12 22:14:59
tags:
- v7
- SadConsole
category:
- SadConsole
---

The last two weeks have been pretty busy, but I've been slowly banging around on the controls redesign for SadConsole. One thing that I didn't with how the controls worked was that each control controls its drawing. This means that if you have a button control, and you wanted it to draw a border around it all of the time, you had to create a new button class. This meant adding whole new types of controls just to tweak the drawing. 

The new system for 7.0 decouples the drawing from the control. The drawing is now moved to the theme object. Now all you need to do is create a new button theme type and implement the draw method, and set it to the `ButtonVariable.Theme` property. This new system also controls the drawing for the `ControlsConsole` and the `Window` types.

To test this out I create 3 types of themes for the button type. The normal button from v7, a 3d button, and a new bordered button which uses the SadConsole extended character sheet. 

{% asset_img new-button-themes.gif 3d themes for buttons in sadconsole %}