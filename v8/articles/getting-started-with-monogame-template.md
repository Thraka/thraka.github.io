# Create a new MonoGame project

This article describes how to use SadConsole from a Visual Studio MonoGame template. It is generally easier to create a game with [Create a new SadConsole .NET Core project with the SadConsole templates](getting-started-sadconsole-core-cli-template.md) and then open that project in Visual Studio.

- If you're using a MonoGame template to create a new project, which auto-generated your `game1` code file, continue to read this article. 
- If you do not have the MonoGame SDK installed, you should create a new blank .NET project by following the directions in the [Create a new SadConsole project](getting-started-with-sadconsole-framework.md) article.

>[!NOTE]
>SadConsole for MonoGame targets *.NET Standard 2.0*. The minimum version of the .NET Framework you must use is 4.6.1.

01. After creating a new MonoGame project, open the *game1.cs* or *game1.vb* source code file
01. Right-click on the project and click Manage NuGet Packages.
01. Search for [SadConsole](https://www.nuget.org/packages/SadConsole) and add it to your project.
01. Open up your `game1.cs` file.
01. Change the base class from the MonoGame Game class to `SadConsole.Game`

    ```csharp
    public class Game1 : SadConsole.Game
    ```

01. Change the constructor to initialize SadConsole. This constructor has a blank font for the first parameter which tells SadConsole to use the default font built in to SadConsole.

    ```csharp
    public Game1() : base("", 80, 25, null)
    ```

#### Initialize
Before anything can be done with SadConsole, the main engine must be initialized. The initialization routine takes a GraphicsDevice reference object. 

The following example overrides the `Initialize()` method on a Game class:

```csharp
protected override void Initialize()
{
    // Generally you don't want to hide the mouse from the user
    IsMouseVisible = true;

    // Finish the initialization of SadConsole
    base.Initialize();

    // Create your console
    var firstConsole = new SadConsole.Console(60, 30);

    firstConsole.FillWithRandomGarbage();
    firstConsole.Fill(new Rectangle(2, 2, 20, 3), Color.Aqua, Color.Black, 0);
    firstConsole.Print(3, 3, "Hello World!");

    SadConsole.Global.CurrentScreen.Children.Add(firstConsole);
}
```

### Example game1.cs class

This class shows using the SadConsole Engine Component to initialize MonoGame

```csharp
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadRogueSharp
{
    public class Game1 : SadConsole.Game
    {
        public Game1() : base("", 80, 25, null)
        {
            
        }

        protected override void Initialize()
        {
            // Generally you don't want to hide the mouse from the user
            IsMouseVisible = true;

            // Finish the initialization of SadConsole before you start your game init
            base.Initialize();

            // Create your console
            var firstConsole = new SadConsole.Console(60, 25);

            firstConsole.FillWithRandomGarbage();
            firstConsole.Fill(new Rectangle(2, 2, 20, 3), Color.Aqua, Color.Black, 0);
            firstConsole.Print(3, 3, "Hello World!");

            SadConsole.Global.CurrentScreen = firstConsole;
        }
    }
}
```

## Next steps

Now that you have the project created and working, check out the [existing tutorials](intro.md). These will walk you through the basics.