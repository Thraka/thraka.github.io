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
