# Post Processing in SadConsole

Sometimes you may want to apply a shader or graphical effect to your game when drawing, which this article will explain how to do. 

To apply a post processing effect to the screen, you can override SadConsole's default drawing functionality, and then apply your own custom draw commands.

To do this, you attach a new `DrawableGameComponent` to SadConsole's game instance components, which is set to draw *after* SadConsole has finished creating the frame for the default screen.

This `DrawableGameComponent` should then access SadConsole's final render surface, and apply whatever custom draw code is needed.

## Method

### Setting up the Class
To begin with, you'll need to create a new class that inherits from MonoGame's `DrawableGameComponent`, and override the constructor and "Draw" functions so that you can draw to screen.

```csharp
public class MyCustomPPFX : DrawableGameComponent
{
    public MyCustomPPFX() : base(SadConsole.Game.Instance) 
    {
        //...
	}

    //When we need to draw to the screen, it's done here.
    public override void Draw(GameTime gameTime)
    {

	}
}
```

After this, you'll need to have your constructor load in the shader you want to use. This can be done through two methods:

 1. The MonoGame Content Pipeline
 2. 2MGFX (Manual Inclusion)
 
 The MonoGame Content Pipeline is arguably easier, however requires you to have the pipeline available for changes.

### Loading a Shader
*Method 1: MonoGame Content Pipeline*

To load your content via. the MonoGame Content Pipeline, you can create a simple `.mgcb` and include the shader as a file. Once this is done, you can set up a content manager and load the shader like so:

```csharp
effect = Content.Load<Effect> ("myEffect");
```

*Method 2: 2MGFX and Manual Inclusion*

First, you'll need to compile your `.fx` shader using *2MGFX*, a free tool included in every MonoGame install. The default location for this is in the MonoGame MSBuild folder, located at `Program Files (x86)\MSBuild\MonoGame\Tools`.

> Note: Make sure you're compiling for the correct platform, either DesktopGL or DirectX_11, otherwise your shader will not compile properly, or fail to load.

Once this is done, you can include the compiled file as a resource in your assembly, as is done so [here,]([https://stackoverflow.com/questions/433171/how-to-embed-a-text-file-in-a-net-assembly](https://stackoverflow.com/questions/433171/how-to-embed-a-text-file-in-a-net-assembly)) and then load the effect from raw bytes, like so:

```csharp
Effect myEffect = new Effect(SadConsole.Global.GraphicsDevice, Resources.MyEffect);
```

### Configuring the Class
Once you've successfully loaded the shader in your constructor, it should look something like this:

```
//Load the shader into memory.
myShader = new Effect(SadConsole.Global.GraphicsDevice, Resources.MyShader);
```

Now you need to set the draw order of your `DrawableGameComponent` to be **6 or higher**, so it draws *after* SadConsole's finished doing its rendering.

```
//Set the draw order to 6, so we draw AFTER SadConsole.
DrawOrder = 6;

//Load the shader into memory.
myShader = new Effect(SadConsole.Global.GraphicsDevice, Resources.MyShader);
```

With the `DrawOrder` properly set, all that's left to do is configure the shader's parameters, and then include the `Draw` code. The first step is done on a shader-by-shader basis, however this is what the code looks like in the [SadConsole Shader Example]([https://github.com/SadConsole/SadConsole/blob/master/src/DemoProject/SharedCode/ShaderRendererTesting.cs](https://github.com/SadConsole/SadConsole/blob/master/src/DemoProject/SharedCode/ShaderRendererTesting.cs)):

```
public override void Draw(GameTime gameTime)
{
	// Respect the draw flag for sadconsole
	if (Settings.DoDraw)
	{
		spriteEffect.Parameters["textureSize"].SetValue(new Vector2(Global.RenderOutput.Width, Global.RenderOutput.Height));
		spriteEffect.Parameters["videoSize"].SetValue(new Vector2(Global.RenderOutput.Width, Global.RenderOutput.Height));
		spriteEffect.Parameters["outputSize"].SetValue(new Vector2(Global.RenderRect.Width, Global.RenderRect.Height));
	}
}
```

You'll need to configure any static values for your shader in your constructor after you've loaded it, as well. Finally, the `Draw` code. First you begin the rendering of the `SpriteBatch`, apply your shader, draw the spritebatch, and then end. Here's an example of that, again from the example:

```
Global.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone);

//Apply the shader before draw, but after begin.
spriteEffect.CurrentTechnique.Passes[0].Apply();

Global.SpriteBatch.Draw(Global.RenderOutput, Global.RenderRect, Color.White);
Global.SpriteBatch.End();
```

### Adding the Component, Removing Default Draw

Now that your custom renderer is completely set up, you can add it as a component to the game instance. Somewhere in your SadConsole initialization code, you can add an instance of your class to the `SadConsole.Game.Instance.Components` collection. You also need to turn `Settings.DoFinalDraw` off, so SadConsole isn't also drawing to the screen.

```
Settings.DoFinalDraw = false;
Game.Instance.Components.Add(new MyCustomPPFX());
```
Running the game, your shader should now be applied, permitting your parameters and configuration were correct.

## Troubleshooting
**I'm just seeing a black screen, what should I do?**

Has the shader been applied between the `Begin` and `Draw` methods? Is the shader broken? 

Another possible issue is the parameters of the shader. Make sure any size or output parameters are set in the `Draw` function, and based on SadConsole's actual dimensions. Also make sure that your static properties are configured properly.
