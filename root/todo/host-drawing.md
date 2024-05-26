---
title: How does drawing work
description: Learn about how SadConsole draws surfaces and objects to the screen. Objects are drawn and cached for future use before being drawn to screen.
ms.date: 05/26/2024
---

# How does drawing work

When SadConsole draws a dirty surface, it doesn't draw it directly to the screen. Each <xref:SadConsole.IScreenSurface> object, which a console is, has a renderer. The renderer caches the last drawn state of the surface to a texture. When SadConsole is ready to draw the screen, all of the visible surfaces are gathered together (using their cached textures) and drawn on a final texture, <xref:SadConsole.Host.Global.RenderOutput>. This texture is then drawn to the screen.

## Cached textures

As stated above, each surface has a texture that represents the final look of the console-surface. A <xref:SadConsole.Renderers.IRenderer> object is responsible for this and is exposed on the surface through the <xref:SadConsole.IScreenSurface.Renderer> property. A renderer is made up of many different <xref:SadConsole.Renderers.IRenderStep> instances that compose the surface and draw it on the cached texture. For example, the default renderer attached to a surface has the <xref:SadConsole.Renderers.SurfaceRenderStep> which draws the surface data.


When the renderer processes a surface, it first checks if the <xref:SadConsole.CellSurface.IsDirty> property is set to `true`. If so, it updates the cached texture with the latest state of the surface.

This is very efficient because there is less to render to the screen each frame. A 100x100 console has about 20,000 sprite-draws each frame. If a surface doesn't change very often, this is a lot of wasted time for your GPU. By caching each draw to a texture, a single draw call is required each frame instead of all 20,000 individual draws. Once the 100x100 surface becomes dirty, then you would have the 20,000 individual draw calls that single dirty frame.

## Final draw

Now that we know how surfaces are predrawn, let's break down the code used to draw SadConsole. SadConsole is implemented as a MonoGame component, <xref:SadConsole.Game.SadConsoleGameComponent> here is the <xref:SadConsole.Game.SadConsoleGameComponent.Draw(Microsoft.Xna.Framework.GameTime)> method.

```csharp
public override void Draw(GameTime gameTime)
{
    if (Settings.DoDraw)
    {
        var oldViewPort = GraphicsDevice.Viewport;
        Global.GameTimeRender = gameTime;
        Global.GameTimeElapsedRender = gameTime.ElapsedGameTime.TotalSeconds;

        // Clear draw calls for next run
        Global.DrawCalls.Clear();

        // Make sure all items in the screen are drawn. (Build a list of draw calls)
        Global.CurrentScreen?.Draw(gameTime.ElapsedGameTime);

        SadConsole.Game.OnDraw?.Invoke(gameTime);
```

The above code resets all of the previous frame's draw calls. The <xref:SadConsole.Global.CurrentScreen> has <xref:SadConsole.Console.Draw(System.TimeSpan)> called on it which should build a new list of draw calls. A callback is then invoked.

```csharp
        // Render to the global output texture
        GraphicsDevice.SetRenderTarget(Global.RenderOutput);
        GraphicsDevice.Clear(Settings.ClearColor);

        // Render each draw call
        Global.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone);
        foreach (var call in Global.DrawCalls)
        {
            call.Draw();
        }

        Global.SpriteBatch.End();
        GraphicsDevice.SetRenderTarget(null);
```

The above code tells the graphics device to draw to the final rendered texture. Each draw call is then then run. Afterwards, the graphics device is reset to draw to the screen.

```csharp
        GraphicsDevice.Viewport = oldViewPort;

        // If we're going to draw to the screen, do it.
        if (Settings.DoFinalDraw)
        {
            Global.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone);
            Global.SpriteBatch.Draw(Global.RenderOutput, Global.RenderRect, Color.White);
            Global.SpriteBatch.End();
        }
    }
}
```

Finally, if the <xref:SadConsole.Settings.DoFinalDraw> is `true`, the final render texture is then drawn to the screen. Why does this `DoFinalDraw` gate exist? This allows you to integrate SadConsole into an existing game. For example, if you had a 1st-person game where the player can walk up to and use a computer in the game, you can use SadConsole to process and draw the terminal. Then, that final texture created by SadConsole could be mapped to the 3D computer object.