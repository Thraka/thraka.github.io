using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;

namespace SadConsoleGame
{
    public class Map
    {
        private List<GameObject> _mapObjects;
        private ScreenSurface _mapSurface;

        public IReadOnlyList<GameObject> GameObjects => _mapObjects.AsReadOnly();
        public ScreenSurface SurfaceObject => _mapSurface;
        public GameObject UserControlledObject { get; set; }

        public Map(int mapWidth, int mapHeight)
        {
            _mapObjects = new List<GameObject>();
            _mapSurface = new ScreenSurface(mapWidth, mapHeight);
            _mapSurface.UseMouse = false;

            FillBackground();

            UserControlledObject = new GameObject(new ColoredGlyph(Color.White, Color.Black, 2), _mapSurface.Surface.Area.Center, _mapSurface);

            for (int i = 0; i < 5; i++)
            {
                CreateTreasure();
                CreateMonster();
            }
        }

        private void FillBackground()
        {
            Color[] colors = new[] { Color.LightGreen, Color.Coral, Color.CornflowerBlue, Color.DarkGreen };
            float[] colorStops = new[] { 0f, 0.35f, 0.75f, 1f };

            Algorithms.GradientFill(_mapSurface.FontSize,
                                    _mapSurface.Surface.Area.Center,
                                    _mapSurface.Surface.Width / 3,
                                    45,
                                    _mapSurface.Surface.Area,
                                    new Gradient(colors, colorStops),
                                    (x, y, color) => _mapSurface.Surface[x, y].Background = color);
        }

        public void CreateTreasure()
        {
            // Try 1000 times to get an empty map position
            for (int i = 0; i < 1000; i++)
            {
                // Get a random position
                Point randomPosition = new Point(Game.Instance.Random.Next(0, _mapSurface.Surface.Width),
                                                 Game.Instance.Random.Next(0, _mapSurface.Surface.Height));

                // Check if any object is already positioned there.
                foreach (var obj in _mapObjects)
                {
                    if (obj.Position == randomPosition)
                        continue;
                }

                // If the code reaches here, we've got a good position, create the game object.
                Treasure treasure = new Treasure(randomPosition, _mapSurface);
                _mapObjects.Add(treasure);
                break;
            }
        }

        public void CreateMonster()
        {
            // Try 1000 times to get an empty map position
            for (int i = 0; i < 1000; i++)
            {
                // Get a random position
                Point randomPosition = new Point(Game.Instance.Random.Next(0, _mapSurface.Surface.Width),
                                                 Game.Instance.Random.Next(0, _mapSurface.Surface.Height));

                // Check if any object is already positioned there.
                foreach (var obj in _mapObjects)
                {
                    if (obj.Position == randomPosition)
                        continue;
                }

                // If the code reaches here, we've got a good position, create the game object.
                Monster monster = new Monster(randomPosition, _mapSurface);
                _mapObjects.Add(monster);
                break;
            }
        }

        public bool TryGetMapObject(Point position, out GameObject gameObject)
        {
            // Try to find a map object at that position
            foreach (var otherGameObject in _mapObjects)
            {
                if (otherGameObject.Position == position)
                {
                    gameObject = otherGameObject;
                    return true;
                }
            }

            gameObject = null;
            return false;
        }

        public void RemoveMapObject(GameObject mapObject)
        {
            if (_mapObjects.Contains(mapObject))
            {
                _mapObjects.Remove(mapObject);
                mapObject.RestoreMap(this);
            }
        }
    }
}
