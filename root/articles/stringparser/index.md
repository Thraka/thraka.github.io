---
description: String parser overview
ms.date: 09/13/2023
---

# String Parser overview

SadConsole includes a string processing system that's designed to allow you to easily apply styles and effects to strings without having to fallback on code. For example, you can use the string parser to change the foreground or background of text in the string. You can also parser [create your own commands](#create-a-command).

The string parser generates a <xref:SadConsole.ColoredString> object, which represents each character in the string with an associated foreground color, background color, mirrored setting, and effect.

To learn more about the parser syntax, see [String parser syntax](syntax.md).

## Namespaces and types

The parser instance and commands are available in the <xref:SadConsole.StringParser?displayProperty=fullName> namespace. The following table describes the objects in that namespace:

| Class                                                  | Description                                                                                    |
|--------------------------------------------------------|------------------------------------------------------------------------------------------------|
| <xref:SadConsole.StringParser.IParser>                 | An interface that contains the `Parse` method that transforms a string into a `ColoredString`. |
| <xref:SadConsole.StringParser.Default>                 | SadConsole's built-in parser that supports the parser commands.                                |
| <xref:SadConsole.StringParser.ParseCommandBase>        | The base class for a SadConsole parser command.                                                |
| <xref:SadConsole.StringParser.ParseCommandBlink>       | Applies an effect to a glyph.                                                                  |
| <xref:SadConsole.StringParser.ParseCommandClearEffect> | Removes an effect from a glyph.                                                                |
| <xref:SadConsole.StringParser.ParseCommandDecorator>   | Sets a decorator on a glyph.                                                                   |
| <xref:SadConsole.StringParser.ParseCommandGradient>    | Applies a gradient of colors across multiple glyphs.                                           |
| <xref:SadConsole.StringParser.ParseCommandMirror>      | Sets the mirror setting on a glyph.                                                            |
| <xref:SadConsole.StringParser.ParseCommandRecolor>     | Changes the foreground or background color of a glyph.                                         |
| <xref:SadConsole.StringParser.ParseCommandSetGlyph>    | Changes the character of a glyph.                                                              |
| <xref:SadConsole.StringParser.ParseCommandStacks>      | Holds the active commands that are applied across the glyphs in a string.                      |
| <xref:SadConsole.StringParser.ParseCommandUndo>        | Removes the previous command from the stack of commands.                                       |

For more information about how to use the commands, see [String parser syntax](syntax.md).

The <xref:SadConsole.StringParser.Default> parser is available through the <xref:SadConsole.ColoredString.Parser?displayProperty=nameWithType> property.

## How to parse a string

There are three parts of SadConsole that can parse a string to generate a <xref:SadConsole.ColoredString> object:

- <xref:SadConsole.ColoredString.Parser?displayProperty=nameWithType>

  This property is configured to use the built-in parser. It's used by the rest of SadConsole to parse strings. You can use it directly to parse a string.

  ```csharp
  ColoredString.Parser.Parse("Normal [c:r f:Blue][c:r b:Yellow]and colored");
  ```

  To learn more about the parser syntax, see [String parser syntax](syntax.md).

- The `Print` methods on surfaces.

  **If** the <xref:SadConsole.ICellSurface.UsePrintProcessor> property of the surface is set to **true**, the string printed is first passed through the string parser.

  The <xref:SadConsole.ICellSurface.UsePrintProcessor> property is set to **false** by default.

  Even if `UsePrintProcessor` is set to **false**, you can still print parsed strings by using the <xref:SadConsole.CellSurfaceEditor.Print(SadConsole.ISurface,System.Int32,System.Int32,SadConsole.ColoredString)> overload, which takes a `ColoredString` instance. First, use <xref:SadConsole.ColoredString.Parser?displayProperty=nameWithType> to parse the string and generate the `ColoredString` instance, then call the `Print` method.

- The `Print` methods on cursors.

  **If** the <xref:SadConsole.Components.Cursor.UseStringParser> property of a cursor is set to **true**, the string printed is first passed through the string parser before the cursor starts printing characters.

  The <xref:SadConsole.Components.Cursor.UseStringParser> property is set to **false** by default.

  Even if `UseStringParser` is set to **false**, you can still print parsed strings by using the <xref:SadConsole.Components.Cursor.Print(SadConsole.ColoredString)> overload, which takes a `ColoredString` instance. First, use <xref:SadConsole.ColoredString.Parser?displayProperty=nameWithType> to parse the string and generate the `ColoredString` instance, then call the `Print` method.

## Parser variables

SadConsole's default parser supports a find and replace system when parsing a string. "Variable" names are added to a dictionary in the parser, and associated with a delegate that returns a string. Instead of a simple find and replace system where a named string is replaced with a string value, the delegates are invoked to generate a string. Meaning, you get to run code to calculate values and generate the string that's replaced.

Variables are marked in the parsed string with `$$name` where **name** is the name of the variable-value you want to place into the string.

> [!IMPORTANT]
> The <xref:SadConsole.StringParser.IParser> interface only provides a `Parse` method, so you must cast parser instance provided by the `ColoredString` object to the `Default` parser provided by SadConsole.

The following code adds two variables to the parser, `first_name` and `last_name`. The delegates assigned to the variables return a name when the variable is found in the string.

```csharp
var castedParser = (SadConsole.StringParser.Default)SadConsole.ColoredString.Parser;

castedParser.Variables["first_name"] = () => "John";
castedParser.Variables["last_name"] = () => "Smith";

castedParser.Parse("The player's name is $$first_name $$last_name.");

// Creates the following colored string text: 
//   The player's name is John Smith.
```

## Create a command

The string parser looks for command triggers in the string it's parsing, but it only knows to look for the built-in commands. You can extend the parsing logic to look for triggers of your own commands. The extended parser logic is processed first, and if no command is returned to the parser, the built-in system processes the command trigger. With this logic, you can provide your own commands with new triggers, or override the existing triggers with new commands.

> [!IMPORTANT]
> The <xref:SadConsole.StringParser.IParser> interface only provides a `Parse` method, so you must cast parser instance provided by the `ColoredString` object to the `Default` parser provided by SadConsole.

The following code attaches a delegate named `CustomParseCommand` to the parser's `CustomProcessor` property:

```csharp
((Default)ColoredString.Parser).CustomProcessor = CustomParseCommand;
```

The magic keyword trigger is `[c:name` where `name` is the command name. The command name is sent to the command parser. The parameters, which is the rest of the command string until `]` is encountered, is passed as the `parameters` of the command.

The following example is the parser extension registered in the preceding code. The logic checks for the command name `t` or `retext`, and if found, returns that command. If the name doesn't match, `null` is returned which lets the parser check the built-in commands.

```csharp
private ParseCommandBase? CustomParseCommand(string command, string parameters, ColoredGlyphBase[] glyphString,
                                                    ICellSurface? surface, ParseCommandStacks? commandStacks)
{
    return command switch
    {
        "retext" => new ParseCommandRetext(parameters),
        "t" => new ParseCommandRetext(parameters),
        _ => null,
    };
}
```

> [!IMPORTANT]
> The surface that's passed to the method may be `null`. When the parser is working on a surface, the instance of the surface is provided. When `null`, this means that the string is being built outside of a surface or cursor's `Print` statement, like with the `ColoredString.Parser.Parse` method.

With the parser method declared, you can create a new command type based on the <xref:SadConsole.StringParser.ParseCommandBase> type. This has one important method, `Build`. The `Build` method takes in a colored glyph instance and transforms it according to the command. The command must set the <xref:SadConsole.StringParser.ParseCommandBase.CommandType> property to the aspect of the glyph the command manipulates, otherwise, it won't be processed when parsing string. There are 6 different types of valid commands.

| Type         | Description |
| ------------ | ---------- |
| Foreground   | Command is added to the foreground stack of commands. |
| Background   | Command is added to the background stack of commands. |
| Glyph        | Command is added to the glyph stack of commands. |
| SpriteEffect | Command is added to the SpriteEffect (mirror) stack of commands. |
| Effect       | Command is added to the cell effect stack of commands. |
| PureCommand  | Command isn't added to the list of active commands, and it's `build` is never called. It is only created once and during creation, and can manipulate things. The **Undo** command behaves like this. |

The following code example demonstrates the `t` command declared in the preceding code example. The command is supposed to change the glyphs in the string to a specific glyph. The command's parameters expect a character and an optional count of how many glyphs to change: `[c:t glyph_index[:count]]`

```csharp
private class ParseCommandRetext : ParseCommandBase
{
    public int Counter;
    public char Glyph;

    public ParseCommandRetext(string parameters)
    {
        string[] parts = parameters.Split(new char[] { ':' }, 2);

        // Count and glyph type provided
        if (parts.Length == 2)
            Counter = int.Parse(parts[1]);
        else
            Counter = -1;

        // Get character
        Glyph = parts[0][0];

        // No exceptions, set the type
        CommandType = CommandTypes.Glyph;
    }

    public override void Build(ref ColoredGlyphAndEffect glyphState, ColoredGlyphAndEffect[] glyphString, int surfaceIndex, ICellSurface surface, ref int stringIndex, System.ReadOnlySpan<char> processedString, ParseCommandStacks commandStack)
    {
        glyphState.Glyph = Glyph;

        if (Counter != -1)
        {
            Counter--;

            if (Counter == 0)
                commandStack.RemoveSafe(this);
        }
    }
}
```

The command can be triggered by adding the syntax to a string that's parsed:

```text
The text after the command [c:t Z]is changed to 'Z'.
```

Which outputs the following text:

```text
The text after the command ZZZZZZZZZZZZZZZZZZ
```
