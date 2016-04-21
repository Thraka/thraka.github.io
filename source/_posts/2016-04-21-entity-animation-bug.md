title: Entity animation bug
date: 2016-04-21 08:10:44
tags:
- SadConsole
- Bug
category:
- SadConsole
---

Yesterday I was writing some [code to generate](https://github.com/Thraka/SadConsole/commit/066e68bc4ce8ae8f1a3e525e347f89e0e9f14f66#diff-2a6004b6fa04ffddd85619ce217d01a5R20) an entity that animated white static, like you have on an old TV. 

{% asset_img static.gif animated static ascii %}

The bug has to do with when you create an entity and then add a new animation to it that replaces the **default** animation. The entity has the new animation but the `Entity.CurrentAnimation` property still points to the old one. Not good.

Fixed now in version 2.7+.

