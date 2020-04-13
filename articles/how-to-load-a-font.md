---
description: Learn how to use fonts in SadConsole.
ms.date: 04/12/2020
---

# Load and use a font with SadConsole

Fonts provide a way to supply different graphics for the standard 256 (or more) glyphs used by SadConsole. This article teaches you how to load a font into SadConsole and then how to use that font in a console.

For more information about how fonts are built, see [Basic font information](basic-font-information.md).

## Load a font

SadConsole stores fonts in the <xref:SadConsole.Global.Fonts?displayProperty=fullName> dictionary. SadConsole has a helper that will automatically read a font config file and load it into SadConsole for you to use. Call the <xref:SadConsole.Global.LoadFont(System.String)?displayProperty=fullName> method and pass it the location (relative to your app executable) of the **.font** file to use.

```csharp
var fontMaster = SadConsole.Global.LoadFont("fonts/IBM.font");
```

The code above loads the font and returns the <xref:SadConsole.FontMaster?displayProperty=fullName> object. By calling <xref:SadConsole.Global.LoadFont(System.String)> the font was also added to the <xref:SadConsole.Global.Fonts?displayProperty=fullName> dictionary.

The <xref:SadConsole.FontMaster> instance is used to generate a font that can be used by a console.

```csharp
var normalSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.One);
```

## Set the font of a console

Fonts can be set on a console during or after creation of the console. The current font is stored on the <xref:SadConsole.Console.Font?displayProperty=fullName> property.

Unless specified, all consoles created automatically use the <xref:SadConsole.Global.FontDefault?displayProperty=fullName> property which is a font automatically loaded when SadConsole starts. And usually that is a font sized at x1.

```csharp
var myConsole = new SadConsole.Console(10, 10);
```

You can use the default font to generate a different sized font. The code below would use a doubled font, x2 in size.

```csharp
var myConsole = new SadConsole.Console(10, 10, SadConsole.Global.FontDefault.Master.GetFont(Font.FontSizes.Two));
```

You can change the font of a console after it was already created.

```csharp
// Use the font created earlier
myConsole.Font = normalSizedFont;

// Use the font master created earlier to get a different sized font
myConsole.Font = fontMaster.GetFont(SadConsole.Font.FontSizes.Four);
```

You can use a double sized font (x2 on both axis) by changing the `GetFont` call.

```csharp
myConsole.Font = fontMaster.GetFont(SadConsole.Font.FontSizes.Two);
```

## Change the default font

All consoles automatically use the default font defined by the <xref:SadConsole.Global.FontDefault?displayProperty=fullName> property. Unless changed, SadConsole loads this font from a font embedded in the library. The font defaults to **1x** in size.

You can use the existing default font and generate a different size from it. Each font contains reference to the <xref:SadConsole.FontMaster> through the <xref:SadConsole.Console.Font.Master> property. Simply generate a new font from the master and assign it back to the `FontDefault` property.

```csharp
SadConsole.Global.FontDefault = SadConsole.Global.FontDefault.Master.GetFont(Font.FontSizes.Two);
```

In general, this is done at the start of the game. This introduces a problem though. The initial window was created at a specific size, based on the size of the font. As soon as you introduce a new default font of a different size, the window may no longer be aligned to the cells the font uses. Simply resize the window based on that font and the number of cells you want along the *x* and *y* axis.

```csharp
int countOfCellsX = 80;
int countOfCellsY = 25;

SadConsole.Global.FontDefault.ResizeGraphicsDeviceManager(SadConsole.Global.GraphicsDeviceManager,
                                                          countOfCellsX, countOfCellsY, 0, 0);
```

## Next steps

* [Learn about how fonts are put together](basic-font-information.md).