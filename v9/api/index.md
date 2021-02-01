---
description: SadConsole v9 API documentation
ms.date: 01/30/2021
---

## Welcome to SadConsole v9

SadConsole is a game framework that lets you build ASCII-styled games. See the [main documentation](../articles/index.md) for more information. A quick *program.cs* file that will get you started is below.

```csharp
using System;
using SadConsole;
using SadConsole.Input;
using Console = SadConsole.Console;

namespace MyGame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Settings.WindowTitle = "My Game!";

            SadConsole.Game.Create(80, 25); //If you want to use another font, pass in the .font file as a parameter
            SadConsole.Game.Instance.OnStart = Init;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            // Any setup you want.

            // By default SadConsole adds a blank ready-to-go console to the rendering system. 
            // cast it to Console to use it
            var rootConsole = (Console)Game.Instance.Screen;
            rootConsole.FillWithRandomGarbage(255);
            rootConsole.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0);
            rootConsole.Print(4, 4, "Hello from SadConsole");
        }
    }
}

```
