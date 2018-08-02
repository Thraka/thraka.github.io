title: Major changes to be included in 7.0
date: 2018-08-01 18:23:16
tags:
---

A month and a half into the V7.0 rewrite, and I'm about 90% complete. There are some major changes to how a `console` and `surfaces` work. A friend of mine did a nice writeup about how SadConsole looks to a first-time user. The [current pull request](https://github.com/Thraka/SadConsole/pull/139) on GitHub shows all of the changes so far; 200+ files changed.

The major change is that the whole concept of changable backing surfaces is gone. Prior to 7.0, a console was the editor for a `TextSurface` property, a property represented the actual graphical text surface of a console. You could easily change the surface in/out of the console. There was also a `SurfaceEditor` class (which console inherited from) that could be directed at any `TextSurface` object, and edit it. This `SurfaceEditor` type was what had all of the editing methods such as `SetForeground` and `Print`.

This has been changed to where every `Surface` type itself (ex: layered, animated, basic) has all of the methods needed to manipulate the cells themselves directly. Surfaces are also positionable and drawable without the need a `Console`, if you don't want one. Previously every surface had to be wrapped by a console or by a bunch of code. A `Console` now inherits from a surface base class which means a console **is** a surface now.

I think this will greatly simplify the getting started aspects of the engine. I'll have more to talk about soon.