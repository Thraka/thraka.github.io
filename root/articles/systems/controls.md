---
title: Overview of Controls
description: Learn about what UI controls are available  with SadConsole.
ms.date: 11/07/2023
---

# Controls overview (SadConsole Systems)

SadConsole provides a controls and forms-like framework for you to display GUI controls in your game. This includes controls like buttons, checkboxes, and lists. You can use the controls provided by SadConsole, extend those controls, or build your own controls.

UI related types are in these two namespaces:

- `SadConsole.UI` - Basic supporting objects, such as the controls host, colors, and window types.
- `SadConsole.UI.Controls` - All of the controls provided by SadConsole.

## Create and show controls

Controls are hosted and managed by the <xref:SadConsole.UI.ControlHost?displayProperty=fullName> component. When this component is added to a surface, that surface has can draw controls, which the user can interact with. Controls are added to the component and not the surface directly.

The following code demonstrates a `ScreenSurface` with a `ControlHost` component. A control is added to the component:

```csharp
ScreenSurface surfaceObject = new(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY);
ControlHost controls = new();
        
surfaceObject.SadComponents.Add(controls);

ListBox list = new(18, 7);
list.DrawBorder = true;
list.Items.Add("John");
list.Items.Add("Steve");
list.Items.Add("Nancy");
list.Items.Add("Lewis");
list.Items.Add("Karl");
list.Items.Add("Carl with a C");
list.Position = new Point(1, 1);

controls.Add(list);
GameHost.Instance.Screen = surfaceObject;
```

### ControlsConsole

The <xref:SadConsole.UI.ControlsConsole?displayProperty=fullName> type combines a <xref:SadConsole.Console> (which inherits from `ScreenSurface`) with the <xref:SadConsole.UI.ControlHost> component. The host is exposed through the <xref:SadConsole.UI.ControlsConsole.Controls> property. To demonstrate, the example from the previous section has been rewritten to use `ControlsConsole` instead of `ScreenSurface`:

```csharp
ControlsConsole console = new(GameHost.Instance.ScreenCellsX, GameHost.Instance.ScreenCellsY);

ListBox list = new(18, 7);
list.DrawBorder = true;
list.Items.Add("John");
list.Items.Add("Steve");
list.Items.Add("Nancy");
list.Items.Add("Lewis");
list.Items.Add("Karl");
list.Items.Add("Carl with a C");
list.Position = new Point(1, 1);

console.Controls.Add(list);
GameHost.Instance.Screen = console;
```

## Color system  

All of the controls share a color scheme that each control uses to draw itself. The controls draw themselves with the color scheme based on the state the control is in, such as when the mouse is over the control.

When a control draws itself, it calls the <xref:SadConsole.UI.Controls.ControlBase.FindThemeColors?displayProperty=nameWithType> method. As indicated by the method name, this method finds the color scheme used in drawing the control. Colors are set globally by default by the <xref:SadConsole.UI.Colors.Default?displayProperty=fullName> property. You can edit these colors to customize the colors for all controls. But, you can set the colors separately from the default set of colors by setting them on the control's host or the control itself. When set on the host, all controls added to that host use those colors instead of the default. Further, if set directly on the control, those colors are used. To summerize, when a control draws itself, it uses the first set of colors it finds based on the following order:

- The control itself. (Colors are set with the <xref:SadConsole.UI.Controls.ControlBase.SetThemeColors(SadConsole.UI.Colors)> method)
- The control's host's <xref:SadConsole.UI.ControlHost.ThemeColors> property.
- <xref:SadConsole.UI.Colors.Default?displayProperty=fullName> property.

SadConsole provides two color schemes you can use as the default. Of course, you can always create your own color scheme, but a classic black Ansi-inspired theme and a blue-ish theme are provided. The black theme is the default, and is created by calling the <xref:SadConsole.UI.Colors.CreateAnsi?displayProperty=fullName> method. The blue theme is created by calling the <xref:SadConsole.UI.Colors.CreateSadConsoleBlue?displayProperty=fullName> method.

The following sections describe how the color system works with controls, if you're interested in how to simply change the colors, see the following articles:

- [How to edit the default colors.](../how-to-change-control-colors.md#how-to-edit-the-default-colors)
- [How to set the colors for a host.](../how-to-change-control-colors.md#how-to-set-the-colors-for-a-host)
- [How to set the colors for a control.](../how-to-change-control-colors.md#how-to-set-the-colors-for-a-control)

## Control color theme parts

The <xref:SadConsole.UI.Colors> object has a few properties representing a standard set of named colors, such as **Red** or **Gold**. It also has a set of properties that represent the foreground and background colors used to draw the control when it's in a specific state. These color combinates are combined into a single `ColoredGlyph` object that's used in drawing.

- **Normal**

  This is what the control looks like when you create it.
  
  The `Colors.ControlBackgroundNormal` and `Colors.ControlForegroundNormal` colors are combined into the `Colors.Appearance_ControlNormal` property.

- **Disabled**

  This is what the control looks like when it's disabled and all interaction with it is prevented.
  
  The `Colors.ControlBackgroundDisabled` and `Colors.ControlForegroundDisabled` colors are combined into the `Colors.Appearance_ControlDisabled` property.

- **Focused**

  This is what the control looks like when it's the focused object. The control host can focus a single object. If there's text on the control, it's generally recolored to indicate it's the focused object.
  
  The `Colors.ControlBackgroundFocused` and `Colors.ControlForegroundFocused` colors are combined into the `Colors.Appearance_ControlFocused` property.

- **Mouse Over**

  When the mouse is over the control, but no buttons are pressed, these are the colors used to draw it.
  
  The `Colors.ControlBackgroundMouseOver` and `Colors.ControlForegroundMouseOver` colors are combined into the `Colors.Appearance_ControlOver` property.

- **Mouse Down**

  When the mouse is over the control and a mouse button is pressed, these are the colors ued to draw it.
  
  The `Colors.ControlBackgroundMouseDown` and `Colors.ControlForegroundMouseDown` colors are combined into the `Colors.Appearance_ControlMouseDown` property.

- **Selected**

  This is what the control looks like when it's in the **selected** state. This state is used by the <xref:SadConsole.UI.Controls.ListBox> control to color the selected item.
  
  The `Colors.ControlBackgroundSelected` and `Colors.ControlForegroundSelected` colors are combined into the `Colors.Appearance_ControlSelected` property.

When you adjust the individual colors, such as `Colors.ControlForegroundNormal`, use the <xref:SadConsole.UI.Colors.RebuildAppearances> method to combine the two colors into the final `Colors.Appearance_*` property. You can also change the `Colors.Appearance_*` properties directly, but don't forget to also change the associated foreground and background colors too.

### Control host

The <xref:SadConsole.UI.ControlHost> component recolors the hosting surface it's attached to the color's designated by the `Colors.ControlHostBackground` and `Colors.ControlHostForeground` properties. This happens one time, when the component is added to the surface object. You can prevent this by setting the <xref:SadConsole.UI.ControlHost.ClearOnAdded?displayProperty=nameWithType> property to `false` before the component is added.

To learn how to change the colors of a host, see [How to set the colors for a host](../how-to-change-control-colors.md#how-to-set-the-colors-for-a-host).

## Reference: Built-in controls

The following is a list of common controls provided by SadConsole:

- <xref:SadConsole.UI.Controls.Button>
- <xref:SadConsole.UI.Controls.Button3d>
- <xref:SadConsole.UI.Controls.ButtonBox>
- <xref:SadConsole.UI.Controls.CheckBox>
- <xref:SadConsole.UI.Controls.ComboBox>
- <xref:SadConsole.UI.Controls.DrawingArea>
- <xref:SadConsole.UI.Controls.Label>
- <xref:SadConsole.UI.Controls.ListBox>
- <xref:SadConsole.UI.Controls.NumberBox>
- <xref:SadConsole.UI.Controls.ProgressBar>
- <xref:SadConsole.UI.Controls.RadioButton>
- <xref:SadConsole.UI.Controls.SelectionButton>
- <xref:SadConsole.UI.Controls.TextBox>
- <xref:SadConsole.UI.Controls.ToggleSwitch>

The following is a list of specialized controls. These are more advanced than the simple common control previously listed; these support are either container controls or support other controls.

- <xref:SadConsole.UI.Controls.Panel>
- <xref:SadConsole.UI.Controls.ScrollBar>
- <xref:SadConsole.UI.Controls.SurfaceViewer>
- <xref:SadConsole.UI.Controls.TabControl>

The following is a list of base classes used for other controls:

- <xref:SadConsole.UI.Controls.ControlBase>

  This is the base class for all controls. When you want to create a completely new control, you inherit from this class.

- <xref:SadConsole.UI.Controls.CompositeControl>

  This is a base class that contains multiple other controls. For example, the `ListBox` is a `CompositeControl` that hosts a `ScrollBar`.

- <xref:SadConsole.UI.Controls.ButtonBase>

  This is the base class for all button types, such as `Button` and `ButtonBox`.

- <xref:SadConsole.UI.Controls.ToggleButtonBase>

  Inherits from `ButtonBase` and is the base class for all button types that have an **on** and **off** state, such as a `CheckBox` or `ToggleSwitch`.
