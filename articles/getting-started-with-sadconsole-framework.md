# Create a new .NET Framework project

This article describes how to setup SadConsole with a new project in Visual Studio.

It is recommended that you use .NET Core to create your SadConsole game. To do so, follow the [.NET Core](getting-started-sadconsole-core-cli-template.md) tutorial.

First, create a new project in Visual Studio. Depending on your version, some dialogs will look different. If you are not familiar with Visual Studio, follow the Microsoft documentation on [how to create a Visual Studio project](https://docs.microsoft.com/visualstudio/ide/creating-solutions-and-projects?view=vs-2017#to-create-a-project-from-a-project-template).

## Create a new .NET project

Using Visual Studio, create a new project. It is recommended that you create a new **Empty Project (.NET Framework)** and target the latest version of .NET, or at a minimum, **4.6.1**.

>[!NOTE]
>If you are going to use the **MonoGame Game** project template, read the [MonoGame Template](getting-started-with-monogame-template.md) tutorial.

## Setup SadConsole

With your project created in Visual Studio, do the following:

01. In the **Solution Explorer**, right-click on the project you created and click **Manage NuGet Packages**.

    ![manage nuget package for sadconsole](~/images/project-manage-nuget-packages.png)

01. Search for and install a _MonoGame.Framework_ package. You can use the desired platform.

    * If you're making a Windows & Linux game, choose [MonoGame.Framework.DesktopGL](https://www.nuget.org/packages/MonoGame.Framework.DesktopGL/)
    * If you're making a Windows game, choose either [MonoGame.Framework.WindowsDX](https://www.nuget.org/packages/MonoGame.Framework.WindowsDX/) or [MonoGame.Framework.DesktopGL](https://www.nuget.org/packages/MonoGame.Framework.DesktopGL/)
    * If you're making a UWP game, choose [MonoGame.Framework.WindowsUniversal](https://www.nuget.org/packages/MonoGame.Framework.WindowsUniversal/)
    * If you're making an Android game, choose [MonoGame.Framework.Android](https://www.nuget.org/packages/MonoGame.Framework.Android/)

01. Search for and install the **SadConsole** package.

01. In the **Solution Explorer**, right-click on the project and click **Properties**. 

    01. Set the **Output type** to **Windows Application**.

    02. Set the **Startup Object** to your the class defined by your program.

        ![set the output type and startup object for sadconsole](~/images/project-set-output.png)

01. Open up your **program.cs** file.

01. Change the `using` statements at the top of your code file to the following.

    ```csharp
    using System;
    using SadConsole;
    using Microsoft.Xna.Framework;
    using Console = SadConsole.Console;
    ```

01. Replace the `Main` method with the following code.

    ```csharp

    static void Main()
    {
        // Setup the engine and create the main window.
        SadConsole.Game.Create(80, 25);

        // Hook the start event so we can add consoles to the system.
        SadConsole.Game.OnInitialize = Init;

        // Start the game.
        SadConsole.Game.Instance.Run();
        SadConsole.Game.Instance.Dispose();
    }
    ```

01. Next, add the `Init` method referenced by the preceding code. This method is used to setup your starting console. This code should be added after `static void Main()` block.

    ```csharp
    static void Init()
    {
        var console = new Console(80, 25);
        console.FillWithRandomGarbage();
        console.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0);
        console.Print(4, 4, "Hello from SadConsole");

        SadConsole.Global.CurrentScreen = console;
    }
    ```

01. Press **F5** to run your game. You should see the following.

    ![a new console in sadconsole with hello text](~/images/new-core-hello-window.png)

Success! You now have a basic SadConsole game running. The code in your **program.cs** file should look like the following:

```csharp
using System;
using SadConsole;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;

namespace MySadConsoleProject
{
    class Program
    {
        static void Main()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(80, 25);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        static void Init()
        {
            var console = new Console(80, 25);
            console.FillWithRandomGarbage();
            console.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0);
            console.Print(4, 4, "Hello from SadConsole");

            SadConsole.Global.CurrentScreen = console;
        }
    }
}
```

If you're using Visual Basic, your code would look like the following:

```vb
Imports Console = SadConsole.Console
Imports Microsoft.Xna.Framework

Module Module1

    Sub Main()

        ' Setup the engine And create the main window.
        SadConsole.Game.Create(80, 25)

        ' Hook the start event so we can add consoles to the system.
        SadConsole.Game.OnInitialize = AddressOf Init

        ' Start the game.
        SadConsole.Game.Instance.Run()
        SadConsole.Game.Instance.Dispose()

    End Sub

    Sub Init()

        Dim Console = New Console(80, 25)

        Console.FillWithRandomGarbage()
        Console.Fill(New Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0)
        Console.Print(4, 4, "Hello from SadConsole")

        SadConsole.Global.CurrentScreen = Console

    End Sub

End Module
```


## Next steps

Now that you have the project created and working, check out the [existing tutorials](intro.md). These will walk you through the basics.