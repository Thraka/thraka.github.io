using Hexa.NET.ImGui;
using SadConsole.ImGuiSystem;
using System.Numerics;

namespace SCTesting;

internal class ImGuiWindow1 : SadConsole.ImGuiSystem.ImGuiObjectBase
{
    private Vector3 _clearColor = Color.White.ToVector3();

    public override void BuildUI(ImGuiRenderer renderer)
    {
        if (ImGui.Begin("Operations"))
        {
            // Run random garbage fill
            if (ImGui.Button("Randomize"))
                ((IScreenSurface)GameHost.Instance.Screen!.Children[0]).Surface.FillWithRandomGarbage(255);

            // Add a random box
            if (ImGui.Button("Add box"))
            {
                IScreenSurface surface = (IScreenSurface)GameHost.Instance.Screen!.Children[0];

                int x = GameHost.Instance.Random.Next(0, surface.Surface.Width - 10);
                int y = GameHost.Instance.Random.Next(0, surface.Surface.Height - 10);

                Rectangle area = new(x, y,
                                     GameHost.Instance.Random.Next(5, surface.Surface.Width - x),
                                     GameHost.Instance.Random.Next(4, surface.Surface.Height - y));

                ShapeParameters shape = ShapeParameters.CreateStyledBoxFilled(ICellSurface.ConnectedLineThin,
                                                                              new ColoredGlyph(Color.White.GetRandomColor(GameHost.Instance.Random),
                                                                                               Color.White.GetRandomColor(GameHost.Instance.Random)),
                                                                              new ColoredGlyph(Color.White.GetRandomColor(GameHost.Instance.Random),
                                                                                               Color.White.GetRandomColor(GameHost.Instance.Random))
                                                                              );

                SadConsole.Instructions.AnimatedBoxGrow box = new(area, TimeSpan.FromSeconds(0.5d), shape);
                box.RemoveOnFinished = true;
                surface.SadComponents.Add(box);
            }

            // Color picking control. When the color changes, fill the surface
            ImGui.Separator();
            ImGui.Text("Set all cell backgrounds");
            ImGui.SetNextItemWidth(150);
            if (ImGui.ColorPicker3("##clearcolor", ref _clearColor, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.NoSidePreview))
                ((IScreenSurface)GameHost.Instance.Screen!.Children[0]).Surface.Fill(background: _clearColor.ToColor());
        }
        ImGui.End();
    }
}
