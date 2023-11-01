namespace SadConsoleGame;

internal class Treasure : GameObject
{
    public Treasure(Point position, IScreenSurface hostingSurface)
    : base(new ColoredGlyph(Color.Yellow, Color.Black, 'v'), position, hostingSurface)
    {

    }

    public override bool Touched(GameObject source, Map map)
    {
        // Is the player the one that touched us?
        if (source == map.UserControlledObject)
        {
            map.RemoveMapObject(this);
            return true;
        }

        return false;
    }
}