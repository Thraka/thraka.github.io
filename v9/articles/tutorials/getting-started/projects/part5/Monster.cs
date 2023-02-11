using SadConsole;
using SadRogue.Primitives;

namespace SadConsoleGame
{
    internal class Monster: GameObject
    {
        public Monster(Point position, IScreenSurface hostingSurface)
            : base(new ColoredGlyph(Color.Red, Color.Black, 'M'), position, hostingSurface)
        {

        }

        public override bool Touched(GameObject source, Map map)
        {
            return base.Touched(source, map);
        }
    }
}
