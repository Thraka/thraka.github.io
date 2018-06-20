title: NuGet Starter for MonoGame
layout: docpage
---

>This page is for those who have just installed the [SadConsole NuGet](https://www.nuget.org/packages/SadConsole/). If you have not created a project yet, start with the [Create a new SadConsole project](create-a-new-sadconsole-project.md) article.

-----

Thank you for choosing **SadConsole**! 

If you added this NuGet package from:

1. An empty .NET project template.

    Setup is pretty easy at this point. Follow [the directions](#setup-sadconsole) below.

2. A new **MonoGame Game** project template.

    This way is only recommended when you're going to be making a MonoGame game that you would like to use parts of SadConsole with it. For more information, see the [MonoGame Template](monogame-template.md) article.

3. A **Console App** or **Windows Forms App** project template.

    This is pretty similar the directions below, except for a few items. If you can, delete this project and read the instructions at *step 1*. If you cannot switch project types, try the following step first, and then read [the directions](#setup-sadconsole) below.

    1. Delete the **program.cs** or **form1.cs** files from your project.

>### **IMPORTANT**
>If you're using **Linux**, NuGet may not setup your project correctly. Look at the folder containing your project. Add any **.font** and **.png** as project references and set them to **Copy to output folder**.

## Setup SadConsole

With your project created in Visual Studio and the [SadConsole NuGet](https://www.nuget.org/packages/SadConsole/) package added, do the following:

1. In the **Solution Explorer**, right-click on the project, and click **Manage NuGet Packages**.
2. Search for and install a _MonoGame.Framework.*_ package. You can use the desired platform.
    * If you're making a Windows app, choose either [MonoGame.Framework.WindowsDX](https://www.nuget.org/packages/MonoGame.Framework.WindowsDX/) or [MonoGame.Framework.DesktopGL](https://www.nuget.org/packages/MonoGame.Framework.DesktopGL/)
    * If you're making a Windows & Linux app, choose [MonoGame.Framework.DesktopGL](https://www.nuget.org/packages/MonoGame.Framework.DesktopGL/)
    * If you're making a UWP app, choose [MonoGame.Framework.WindowsUniversal](https://www.nuget.org/packages/MonoGame.Framework.WindowsUniversal/)
    * If you're making an Android app, choose [MonoGame.Framework.Android](https://www.nuget.org/packages/MonoGame.Framework.Android/)

    >### **IMPORTANT**
    >If you chose the **DesktopGL** library, MonoGame does not provide you a required file, a copy of **SDL2.dll**. You need to add this to your project. I've provided a copy until MonoGame fixes this problem. There is a copy [here](https://github.com/Thraka/SadConsole/raw/master/src/DemoProject/DesktopGL/SDL2.dll).
    >
    >1. Download this file and copy it to the root of your project.
    >2. Right-click the **Project** > **Add** > **Existing item...**.
    >3. Navigate to the dll file and select it.
    >4. Select the file in the **Solution explorer** and in the **Properties** pane, set **Copy to Output Directory** to **Copy if newer**.
    >
    >   
    > If you get a **System.BadImageFormatException** error, try downloading the 32-bit version of SDL2.dll from https://www.libsdl.org/download-2.0.php 

3. Rename `program-example.cs` to `program.cs`.

    The **SadConsole.Starter** NuGet package that was [installed previously](create-a-new-sadconsole-project.md) should have added a `program-example.cs` file to your project. 

    If you have an existing `program.cs` file, delete it.

5. Right-click on the **Project File** > **Properties** to open up the properties tab.

    1. There is a **Default namespace** text box in the **Application** tab. Copy that value.
    
    2. Back in the `program.cs` file, change `namespace MyProject` to `namespace YOUR-COPIED-VALUE` (the value from step 1)

    2. In the project properties, set the **Output type** to **Windows Application**.
    
    3. In the project properties, make sure the **Startup object** is set to your `Program` class.

6. Press F5 to run.

## Next steps

Now that you have the project created and working, check out the [existing tutorials](index.md). These will walk you through the basics.