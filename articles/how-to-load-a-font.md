---
title: Load a font in SadConsole
---

# Load and use a font with SadConsole

Fonts provide a way to supply different graphics for the standard 256 glyphs used by SadConsole. This article teaches you how to load a font into SadConsole and then how to use that font in a console.

For more information about how fonts are built, see [Basic font information](basic-font-information.md).

## Load a font

SadConsole stores fonts in the <xref:SadConsole.Global.Fonts?displayProperty=fullName> dictionary. SadConsole has a helper that will automatically read a font config file and load it into SadConsole for you to use. Call the <xref:SadConsole.Global.LoadFont(System.String)?displayProperty=fullName> method and pass it the location (relative to your app executable) of the **.font** file to use.

```csharp
var fontMaster = SadConsole.Global.LoadFont("fonts/IBM.font");
```

The code above loads the font and returns the <xref:SadConsole.Global.FontMaster> object. By calling <xref:SadConsole.Global.LoadFont(System.String)> the font was also added to the <xref:SadConsole.Global.Fonts?displayProperty=fullName> dictionary.

The <xref:SadConsole.Global.FontMaster> instance is used to generate a font that can be used by a console.

```csharp
var normalSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.One);
```

## Set the font of a console

Fonts can be set on a console during or after creation of the console. The current font is stored on the <xref:SadConsole.Console.Font?displayProperty=fullName> property.

Unless specified, all consoles created automatically use the <xref:SadConsole.Global.FontDefault?displayProperty=fullName> which is a font automatically loaded when SadConsole starts. And usually that is a font sized at x1.

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

## Next steps

* [Learn about how fonts are put together](basic-font-information.md).