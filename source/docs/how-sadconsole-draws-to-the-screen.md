title: How does drawing work?
layout: docpage
---

When SadConsole draws a dirty surface, it does not draw it directly to the screen. Each surface (a Console is a surface) has a [`LastRenderResult`][] property (which is a texture) that represents the last drawn state of the surface. When SadConsole is ready to draw the screen, all of the visible surfaces are gathered together (using their `LastRenderResult` value) and drawn on a final texture, [`Global.RenderOutput`][]. This texture is then drawn to the screen.

## Cached textures

As stated above, each surface creates a texture that represents the final look of the console-surface. A [`SadConsole.Renderers.IRenderer`][] object is responsible for this. When the renderer processes a surface, it first checks if the [`IsDirty`][] property is set to `true`. If so, it updates the cached texture with the latest state of the surface.

This is very efficient because there is less to render to the screen each frame. A 100x100 console has about 20,000 sprite-draws each frame. If a surface doesn't change very often, this is a lot of wasted time for your GPU. By caching each draw to a texture, a single draw call is required each frame instead of all 20,000 individual draws. Once the 100x100 surface becomes dirty, then you would have the 20,000 individual draw calls that single dirty frame.

## Final draw

Now that we know how surfaces are predrawn, let's break down the code used to draw SadConsole. SadConsole is implemented as a MonoGame component, [`SadConsole.Game.SadConsoleGameComponent`][], here is the `Draw` method.

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

The above code resets all of the previous frame's draw calls. The current screen has `Draw` called on it which should build a new list of draw calls. A callback is then invoked.

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

Finally, if the `Settings.DoFinalDraw` is `true`, the final render texture is then drawn to the screen. Why does this `DoFinalDraw` gate exist? This allows you to integrate SadConsole into an existing game. For example, if you had a 3D FPS where the player can walk up to and use a computer in the game, you can use SadConsole to process and draw. Then that final texture created by SadConsole could be mapped to the 3D computer object.

[`Global.RenderOutput`]: https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Global.cs
[`LastRenderResult`]: https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Surfaces/Basic.cs
[`SadConsole.Renderers.IRenderer`]: https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Renderers/IRenderer.cs
[`SadConsole.Console`]: https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Console.cs
[`IsDirty`]: https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Surfaces/Basic.cs
[`SadConsole.Game.SadConsoleGameComponent`]: https://github.com/Thraka/SadConsole/blob/master/src/SadConsole.Shared/Graphics.MonoGame/SadConsoleGameComponent.cs