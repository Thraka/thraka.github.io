title: Roguelike Tutorial - Part 1 - Character and Map
layout: docpage
---

In this series of tutorials we'll look at creating a roguelike while using SadConsole. SadConsole is a .NET library that works with MonoGame to create a tile based game. The focus of SadConsole though is to treat the tile system like a classic terminal or command prompt system.

In this article we'll accomplish the following:

- [x] Create a new project in Visual Studio.
- [x] Add SadConsole to the project.
- [x] Display a character on the screen.

##### Tutorial index

- **Part 1 - Create the project**
- [Part 2 - Character and map](part-2---character-and-map.md)
- [Part 3 - Map generation](part-3---map-generation.md)
- More coming soon...

[Download the source code for the entire series](sadconsole-rogueliketutorial.zip).


## Create the SadConsole project

You need to create a new blank .NET project in [Visual Studio](https://www.visualstudio.com/products/visual-studio-community-vs). You can use C# or VB.NET and probably any other .NET compatible language. 

1. Create a new Empty Project (of your language choice, the tutorial uses C#).

   This creates a project that has little-to-no references on external libraries and doesn't create a default code file.

2. Right-click on the project that was created and click **Manage NuGet Packages...**

3. Click the **Browse** tab and then type in `sadconsole` into the search box.

4. Select and install the `SadConsole.Starter` NuGet package.

   ![nuget preview](images/part1-nuget.png)

   >**NOTE**  
   >You can close the NuGet SadConsole Starter page that pops up, we're going to do all of the steps described by the starter page here.

   This package will add a **program-example.cs** to your project, delete it.

5. Search for `monogame.framework` and install (most likely) either `MonoGame.Framework.WindowsDX` for DirectX or `MonoGame.Framework.DesktopGL` for OpenGL.

   >**IMPORTANT**  
   >If you chose the **DesktopGL** library, you need to add reference to **SDL2.dll**. Currently the MonoGame NuGet library is not providing this dll. There is a copy [here](https://github.com/Thraka/SadConsole/raw/master/src/DemoProject/DesktopGL/SDL2.dll). 
   >
   >1. Download this file and copy it to the root of your project.
   >2. Right-click the **Project** > **Add** > **Reference...**.
   >3. Click **Browse**
   >4. Navigate to the dll file and select it.

6. Close the NuGet tab.

7. Right-click on the project in the **Solution Explorer** and choose **Properties**.

8. Change your assembly name and default namespace to something more friendly. For example, I'm going to choose **SadRogue** for both.

9. Change the Output type from **Console Application** to **Windows Application**.

10. Save the project.

11. Right-click on the project and click **Add** > **Class...**

12. Name the class *Program.cs* and click **Add**.

13. Replace the generated `Program` class (not the generated namespace though) with the following:

    ```csharp
    using System;
    using Console = SadConsole.Consoles.Console;

    public const int ScreenWidth = 80;
    public const int ScreenHeight = 25;

    namespace SadRogue
    {
        class Program
        {
            static void Main(string[] args)
            {
                // Setup the engine and create the main window.
                SadConsole.Game.Create("IBM.font", ScreenWidth, ScreenHeight);

                // Hook the start event so we can add consoles to the system.
                SadConsole.Game.OnInitialize = Init;

                // Hook the update event that happens each frame so we can trap keys and respond.
                SadConsole.Game.OnUpdate = Update;

                // Start the game.
                SadConsole.Game.Instance.Run();

                // Clean up
                SadConsole.Game.Instance.Dispose();
            }

            private static void Update(GameTime delta)
            {

            }

            private static void Init()
            {

            }
        }

    }
    ```

    >**NOTE**  
    >The namespace for the class should be what you entered in step 8.

14. Save *Program.cs* and go back to the project properties window (step 7).

15. Click the **Startup object** drop down and choose your program class. (You may need to compile first)

    ![project program](images/part1-project-program.png) 

16. Save the project.

17. Compile and run.

When the game runs you'll see a blank black window show up. The SadConsole window clears the screen black by default (since a terminal is generally black). Our screen was sized to what our font was using. The IBM font we have uses characters that are 8px x 16px in size. We told the game window to be 80 x 24 of those cells.

## Handle some global keyboard hooks

The `Update` method is called once per game-frame. This is a good place to check the keyboard for some global states, like whenever you want to close the game or toggle full screen. 

```csharp
private static void Update(GameTime time)
{
    // Called each logic update.

    // As an example, we'll use the F5 key to make the game full screen
    if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
    {
        SadConsole.Settings.ToggleFullScreen();
    }
    else if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Escape))
    {
        SadConsole.Game.Instance.Exit();
    }
}
```

## Printing a game character

In the `Init` method we need to create a new console for SadConsole to use. In the center of it we'll print a `'@'` character to simulate the player and then assign the console as the "current screen."

```csharp
private static void Init()
{
    SadConsole.Console startingConsole = new Console(ScreenWidth, ScreenHeight);

    startingConsole.Print(ScreenWidth / 2, ScreenHeight / 2, "@", ColorAnsi.CyanBright);

    // Set our new console as the "thing" to render and process
    SadConsole.Global.CurrentScreen = startingConsole;
}
```

If you run the game, you'll see an `@` character in the middle of the screen.

![@ character in a roguelike map](images/part1-player.png)

## Closing thoughts

We've initialized SadConsole and displayed a simple console with a character. Pretty simple huh? A few lines of code and we're off and running.

In the [next step](part-2---character-and-map.md) of the tutorial we'll create our own custom console and display a player character on it.

[Download the source code for the entire series](sadconsole-rogueliketutorial.zip).