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

        public bool Move(Point newPosition, Map map)
        {
            // Check new position is valid
            if (!map.SurfaceObject.Surface.IsValidCell(newPosition.X, newPosition.Y)) return false;

            // Check if other object is there
            if (map.TryGetMapObject(newPosition, out GameObject foundObject))
            {
                // We touched the other object, but they won't allow us to move into the space
                if (!foundObject.Touched(this))
                    return false;
            }

            // Restore the old cell
            _mapAppearance.CopyAppearanceTo(map.SurfaceObject.Surface[Position]);

            // Store the map cell of the new position
            map.SurfaceObject.Surface[newPosition].CopyAppearanceTo(_mapAppearance);

            Position = newPosition;
            DrawGameObject(map.SurfaceObject);

            return true;
        }

        public virtual bool Touched(GameObject source)
        {
            return false;
        }
    }
}