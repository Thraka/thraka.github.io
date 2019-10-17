# How to use Graphical Tiles

![screenshot of sadconsole with graphical tiles](~/images/graphical-tiles.png)

SadConsole supports graphical tiles! It's recommended that you have at least something basic drawn on-screen (eg. character, floor, walls) before adding graphics, but you can do this at any stage of development. As you can see from the screenshot, you can mix graphical tiles with regular fonts.

## How Graphical Tiles Work

If you read the [basic font information](~/articles/basic-font-information.md) tutorial, you'll see that SadConsole uses simple PNG images for fonts. We can leverage this by creating a font image that uses regular graphical tiles as a spritesheet, instead of ASCII characters.

When you substitue graphics, pay attention to the position in the font file. With an ASCII font, writing the `@` glyph renders the `@` image; in the spriesheet, it'll render whatever image occupies that same cell in the spritesheet.

## Creating the Font Spritesheet

To start out, procure or create the images you need. You can also use the excellent [1-bit Pack](https://kenney.nl/assets/bit-pack) by Kenney, which includes numerous high-quality sprites.

Next, create an empty spritesheet in your graphical editing software of choice. Ideally, include a grid, so you can easily distinguish tiles. Draw or copy/paste a few characters into the appropriate position. If you're not sure which cell corresponds to which character, you can refer to the existing font files, such as the `IBM_8x16.png` file.

Hide the grid and export the file into your project, under the `Fonts` directory. Next, create the `.font` metadata file. Here's a sample:

```
{
    "FilePath": "MyCustomFont_16x16.png",
    "GlyphHeight": 16,
    "GlyphPadding": 0,
    "GlyphWidth": 16,
    "Name": "MyCustomFont",
    "SolidGlyphIndex": 219
}
```

Replace the font sizes and the filename with appropriate values. Note the `SolidGlyphIndex` value: it's used whenever you print an empty tile, or call a method to fill a console.

## Using the Spritesheet Font In-Game

Somewhere in your game console constructor, set the font:

```
var fontMaster = SadConsole.Global.LoadFont("Fonts/MyCustomFont.font");
var normalSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.One);
this.Font = normalSizedFont;
```

You're done! Any calls to `SetGlyph` will use the image from your custom font spritesheet. You can also change the enumeration value of `SadConsole.Font.FontSizes` to a different value (eg. `Two` or `Quarter`) to zoom your game in/out or to render at a different size than the sprite image size.

Since SadConsole limits you to one font per console, you need to create sub-consoles for anything you want to render normally, such as a status bar, sub-menu, etc.

## Coloring Sprites

To colour your sprites, simply specify a `Color` in your `SetGlyph` calls. By default, your font image uses white, so your images will render white. If you specify a color to `SetGlyph`, SadConsole will multiply that colour by the spritesheet colour when it draws.

You can use this for effects; just be aware that drawing pixels in your spritesheet in a color other than white, will restrict what colors they can appear with in-game.

If you prefer full-color sprites instead, simply colour them as desired within the spritesheet, and always render them as white when you call `SetGlyph`. They will appear exactly as they do in the spritesheet, colors and all.
