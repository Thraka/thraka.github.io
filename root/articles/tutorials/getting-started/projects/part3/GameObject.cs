using SadConsole;
using SadRogue.Primitives;

namespace SadConsoleGame
{
    public class GameObject
    {
        public Point Position { get; private set; }

        public ColoredGlyph Appearance { get; set; }

        private ColoredGlyph _mapAppearance = new ColoredGlyph();

        public GameObject(ColoredGlyph appearance, Point position, IScreenSurface hostingSurface)
        {
            Appearance = appearance;
            Position = position;

            // Store the map cell
            hostingSurface.Surface[position].CopyAppearanceTo(_mapAppearance);

            // Draw the object
            DrawGameObject(hostingSurface);
        }

        private void DrawGameObject(IScreenSurface screenSurface)
        {
            Appearance.CopyAppearanceTo(screenSurface.Surface[Position]);
            screenSurface.IsDirty = true;
        }

        public void Move(Point newPosition, IScreenSurface screenSurface)
        { 
            // Restore the old cell
            _mapAppearance.CopyAppearanceTo(screenSurface.Surface[Position]);

            // Store the map cell of the new position
            screenSurface.Surface[newPosition].CopyAppearanceTo(_mapAppearance);

            Position = newPosition;
            DrawGameObject(screenSurface);
        }
    }
}