title: Cached console
date: 2016-06-01 17:45:58
tags:
- SadConsole
- Renderer
category:
- SadConsole
---

Oh hai! As part of my rewrite of how SadConsole renders to the screen ([my v3 branch](https://github.com/Thraka/SadConsole/tree/sadconsole-v3)) I came up with a few new console types. Well.. not really a new console, but it's actually a new renderer. The class is [SadConsole.Consoles.CachedTextSurfaceRenderer](https://github.com/Thraka/SadConsole/blob/sadconsole-v3/SadConsole.Core/Consoles/CachedTextSurfaceRenderer.cs) (name subject to change) and it works by rendering the text data to a texture instead of the screen. Then when the renderer is drawn, it draws the texture instead of console cell data.

<!-- more -->

The normal renderer ([TextSurfaceRenderer](https://github.com/Thraka/SadConsole/blob/sadconsole-v3/SadConsole.Core/Consoles/TextSurfaceRenderer.cs)) crawls through every cell that is supposed to be rendered and

1. Render a background fill for the whole rendered area
2. (on each cell) Render the background (if needed)
3. (on each cell) Render the character on top of that background (if needed)
4. Render a tint to the whole rendered area (if needed)

If you have a console that is displaying 80x25 (standard MS-DOS text screen) it is rendering: *2,000 background + 2,000 character + 1 fill + 1 tint = 4,002 sprite draw calls*. That is a lot of drawing. Of course, the renderer is smart enough to optimize out transparent colors and backgrounds that match the default background fill color. So you may not always be doing 4,002 draw calls. Here is a console rendering 4,001 draw calls (no tint)

{% asset_img normal-renderer.png normal console renderer %}


As you can see it is running at (it's in debug mode, so it's slower than normal) **~1000** frames per second. No problem really. However, if the console we're drawing isn't changing at all, we're really wasting time drawing each cell over and over. Here is that same console rendered to the `CachedTextSurfaceRenderer` renderer. (darker color here just so we know we're in cached mode, it's not normally darker)

{% asset_img cached-renderer.png cached console renderer %}

Much better performance. We're only doing a single draw call too. 

This renderer will be great for UI and static backgrounds, or anything that doesn't change often. You can easily update the cached renderer with the latest state of the console data.

I'll have some other renderers and text surface types coming soon too. 