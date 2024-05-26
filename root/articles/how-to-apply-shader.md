---
title: How to apply a shader
description: Learn how to apply a shader to the SadConsole output or a surface.
ms.date: 05/26/2024
---

# How to apply a shader

Shaders can be applied both to individual surfaces and the final output texture that SadConsole draws to the screen.

SadConsole works through a few steps to compose and draw surfaces to the screen. Each surface composes itself to a texture object, drawing the individual cells, any layers, other attached objects such as entities and cursors. If a surface becomes "dirty" and ends up redrawing itself, the process starts over. The final visual of the surface, stored in the texture object, is used any time that surface needs to be drawn.

When SadConsole draws what you see on the screen, it actually draws all of those cached object textures to a single texture known as the **final draw texture**. This final draw texture is what is actually drawn on the screen.

## Prerequisites

To follow this article, you must:

- Use the MonoGame host.
- Target the DesktopGL version of MonoGame. (You could use a different one but the article was written for DesktopGL)
- A MonoGame Content Builder (MGCB) Editor project, named _Assets.mgcb_.

  > [!TIP]
  > The project file is named Assets.mgcb for the sake of this article. It can be named whatever you want.

You can use the [MGCB Editor](https://docs.monogame.net/articles/tools/mgcb_editor.html) to create a project file. With this editor you can also compile the assets for your project instead of auto-generating your assets at compile-time.

## 1. Install the tools and NuGet packages

Shaders are loaded in MonoGame as "compiled" assets. The MonoGame pipeline can take raw assets and compile them to make the final asset used in the game. Some assets can be used in raw form, such as images. Shaders, however, must be compiled for the target graphics system, such as OpenGL or DirectX.

The following steps install the required NuGet package and tool to support automatically generating your assets at compile-time.

1. First, install the MonoGame Content Builder .NET tool.

   Open a terminal and navigate to your project's code. Run the following commands:

   ```shell
   dotnet new tool-manifest
   dotnet tool install dotnet-mgcb
   ```

   The first command generates the .NET tool manifest file. If you already have one, ignore the failure messages.

1. Next, add the `MonoGame.Content.Builder.Task` NuGet package to your project.

   You can use Visual Studio (or your editor) to do this, but since you have the terminal opened, use the .NET CLI to add the package. Runn the following command:

   ```shell
   dotnet add package MonoGame.Content.Builder.Task
   ```

## 2. Auto-generate assets at compile-time

1. In Visual Studio, find the **Solution Explorer** window and right-click on the project > **Add** > **New Folder**. Set the name to `Content`.
1. Using your file manager, navigate to that newly created folder and copy in the _Assets.mgcb_ project file and any assets referenced by it.
1. Back in Visual Studio, double-click on the project file in **Solution Explorer** to open the XML editor.
1. Between the starting `<Project>` and ending `</Project>` nodes, add the following XML:

   ```xml
   <ItemGroup>
       <MonoGameContentReference Include="Content\Assets.mgcb" />
       <None Remove="Content\bin\**\*" />
       <None Remove="Content\obj\**\*" />
   </ItemGroup>
   ```

1. Save the file and close the file editor.

## 3. Configure SadConsole to use a shader

This step configures SadConsole to use a shader for the final draw that happens on the screen. To do this, you need to disable SadConsole's drawing function and supply your own that uses the shader.

01. In your startup code, disable SadConsole's final draw:

    ```csharp
    SadConsole.Settings.DoFinalDraw = false;
    ```

    If you ran your game now, you would just see a blank screen, though the game is actually running in the background.

01. Add a new class to your project named `FinalDrawShader`.
01. Replace the code with the following:

    ```csharp
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;
    using Color = Microsoft.Xna.Framework.Color;

    namespace SadConsole;

    public class FinalDrawShader : DrawableGameComponent
    {
        public Effect Effect;
        private static int width;
        private static int height;

        private const string SHADER_FILE = "crt-lottes-mg";

        public FinalDrawShader() : base(SadConsole.Game.Instance.MonoGameInstance)
        {
            DrawOrder = 6;

            Effect = SadConsole.Game.Instance.MonoGameInstance.Content.Load<Effect>(SHADER_FILE);

        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                // Respect the draw flag for sadconsole 
                width = Host.Global.RenderOutput.Width;
                height = Host.Global.RenderOutput.Height;

                // Configure the shader
                Effect.Parameters["hardScan"]?.SetValue(-8.0f);
                Effect.Parameters["hardPix"]?.SetValue(-3.0f);
                //Effect.Parameters["warpX"]?.SetValue(0.031f);
                //Effect.Parameters["warpY"]?.SetValue(0.041f);
                Effect.Parameters["maskDark"]?.SetValue(0.5f);
                Effect.Parameters["maskLight"]?.SetValue(1.5f);
                Effect.Parameters["scaleInLinearGamma"]?.SetValue(0.3f);
                Effect.Parameters["brightboost"]?.SetValue(1f);
                Effect.Parameters["hardBloomScan"]?.SetValue(-1.5f);
                Effect.Parameters["hardBloomPix"]?.SetValue(-2.0f);
                Effect.Parameters["bloomAmount"]?.SetValue(0.15f);
                //Effect.Parameters["shape"]?.SetValue(2.0f);
            
                Effect.Parameters["textureSize"].SetValue(new Vector2(width, height));
                Effect.Parameters["outputSize"].SetValue(new Vector2(SadConsole.Settings.Rendering.RenderRect.Width, SadConsole.Settings.Rendering.RenderRect.Height));

                // Start drawing
                Host.Global.SharedSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone);

                //Apply the shader before draw, but after begin.
                Effect.CurrentTechnique.Passes[0].Apply();

                Host.Global.SharedSpriteBatch.Draw(Host.Global.RenderOutput, SadConsole.Settings.Rendering.RenderRect.ToMonoRectangle(), Color.White);
                Host.Global.SharedSpriteBatch.End();
            }
        }
    }

    ```

1. In your game startup code, after the SadConsole game is configured and running, add the `FinalDrawShader` to the MonoGame host:

   ```csharp
   SadConsole.Game.Instance.MonoGameInstance.Components.Add(new FinalDrawShader());
   ```

   SadConsole does its drawing with a `DrawOrder` of 5. Because the shader component was created with a `DrawOrder` of 6, it runs **after** SadConsole prepares the final texture. That way, SadConsole always prepares the final draw texture before the shader is applied.

1. Run the game.

You can download a complete sample project from [here](./snippets/how-to-apply-shader/csharp.zip).
