title: Create a new SadConsole project
layout: docpage
---


This page describes how to create a new project in Visual Studio to use SadConsole. If you already have a **MonoGame template** project created, read about how to use **SadConsole** [here](monogame-template.md).

The best way to create a new SadConsole game is to start with the **Empty project** template provided by Visual Studio.

1. Click **File** > **New** > **Project...**

2. Select **Windows Desktop** in the left pane.

3. Select **Empty Project (.NET Framework)** as the project type.

4. Provide the **Name** and **Location** of where to save your project.

5. Set Framework to **.NET Framework 4.6.1** or greater.

6. Click **OK**. Your project will be created.

   ![new project dialog](images/new-project-dialog.png)

Now that you have the project created, you need to add reference to the SadConsole NuGet package.

1. Right-click on the project in the **Solution Explorer** pane and click **Manage NuGet Packages**.

2. Search for **SadConsole.Starter**.

3. Select the package and then click **Install**.

    ![nuget preview](images/part1-nuget.png)

4. In the same **Manage NuGet Packages** window, update SadConsole to the latest release.

   ![update sadconsole reference](images/project-update-sadconsole.png)

When **SadConsole.Starter** is added to your project, Visual Studio should open up the [NuGet Starter MonoGame](nuget-starter-monogame.md) page which tells you next steps you need to do.

## Next steps

Read the [NuGet Starter MonoGame](nuget-starter-monogame.md) article.