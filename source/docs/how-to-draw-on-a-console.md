title: How to draw on a Console
layout: docpage
---

Really, you don't draw to a console. The console object only combines a text surface with a renderer and lets you use the keyboard/mouse. The console provides all the methods you need to work with a text surface and edit it.

The `SadConsole.Surfaces.BasicSurface` is broken up into multiple cells, all arranged in a grid. Each cell can change its foreground and background color, and the character it is currently showing. 


### Color
The foreground or the background of a cell can be colored. When the background is colored, it fills the entire cell with that color. When the foreground is colored, it changes the color of the character being displayed in that cell.

#### Foreground
You can set or get the foreground color of a cell by using its x, y coordinates and the `SetForeground` or `GetForeground` method.

```csharp
console.SetForeground(10, 10, Microsoft.Xna.Framework.Color.Yellow);
// - or -
var color1 = console.GetForeground(10, 10);
```

#### Background
You can set or get the background color of a cell by using its x, y coordinates and the `SetBackground` or `GetBackground` method.
```csharp
console.SetBackground(10, 10, Microsoft.Xna.Framework.Color.White);
// - or -
var color1 = console.GetBackground(10, 10);
```

#### Character
The character displayed on a cell is changed by calling the `SetGlyph` method. It works much the same as working with the color does. You can get or set the character in a cell. The overloads for the `SetGlyph` method also allow you to provide foreground and background colors if you want.

##### Set/Get a character using int

```csharp
// Set the character at 41,22 to index 1 on the font sheet, a smiley face.
console.SetGlyph(41, 22, 1);
// - or -
int character = console.SetGlyph(41, 22);
```

##### Set a character using System.Char
```csharp
char character = 'B';
console.SetGlyph(41, 22, character);
```

##### Set a character with a foreground color
```csharp
console.SetGlyph(41, 22, 13, Color.Purple);
```

##### Set a character with a foreground and background color
```csharp
console.SetGlyph(41, 22, 13, Color.Purple, Color.Green);
```

#### Working with strings
Setting the character cell-by-cell is not very efficient when you have a string you want to print. The cell surface provides a way to print an entire string.

```csharp
console.Print(2, 0, "This will be printed at 2,0");
```

Additionally, you can stuff the cell data characters into a string, by using an x,y location, and a length of string you want. For example, the previous code example printed a string at 2,0 that was 27 characters long, to pull that string back out into a variable, you would use:

```csharp
string text = console.GetString(2, 0, 27);
```