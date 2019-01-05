title: Version 7.3 released
date: 2019-01-05 13:00:17
tags:
- v7
- SadConsole
category:
- SadConsole
---

Version 7.3 was released and included a bunch of minor fixes. While this version was being produced, work started on V8.0 which includes radical changes to the key types. I will follow up with a post about that in the future. Both the 8.0 prerelease and 7.3 final versions are on NuGet.

##### Changes

Here are the changes since 7.0:

**12/29/2018 V7.3.0**

- Windows now default MoveToFrontOnMouseClick = true.
- Consoles are brought forward and focused via LeftMouseDown instead of LeftMouseClick. (Thanks VGA256) #188
- You can set ListBox.SelectedItem = null now (Thanks darrellp) #183
- TextBox supports moving the cursor and inserting characters now (Thanks darrellp) #145
- Window could steal the mouse focus while dragging some other control across the title bar (Thanks VGA256) #165
- ColoredString.Parse now uses CultureInvariant (Thanks GPugnet) #173
- SadConsole.Standard was missing the non extended font embedded resource.
- Various helper methods in ColoredGlyph, ColoredString, and SurfaceBase added (Thanks INeedAUniqueUsername) #187

**11/19/2018 V7.2.0**

- ControlsConsole.Controls collection now uses a foreach loop to make sure the collection is not modified when processing the mouse.
- Button theme would crash if ShowEnds was on and the width of the button was < 3. (reported by Hoonius)
- Renamed and promoted the method that forwarded entity animation states to the entity: OnAnimationStateChanged. Override this on custom entities to detect the state changes.
- Blink event did not respect the BlinkCounter property. (reported by Hoonius)
- Default font is now a non-extended IBM 8x16 font.
- Added SadConsole.Settings.UseDefaultExtendedFont setting and when set to true, loads the IBM 8x16 extended font instead of the normal font. Must be set before creating the game.

**10/18/2018 V7.1.0**

- Moved SurfaceBase.GetIndexFromPoint to Helpers class.
- Fixed bug in EntityManager that did not remove entity/hotspot/zone parents when the EntityManager's parent was cleared.
- EntityManager does not support .Clear calls on collections. Instead, use the RemoveAll extension method.
- An extension method was added: SurfaceBase.CenterViewPortOnPoint
- An extension method was added: Rectangle.CenterOnPoint

**09/08/2018 V7.0.3 / 7.0.4**

- Fixed bug with textbox displaying two carets.
- If TextBox was first control in console, rendering was wrong.
- Added int overload for Helpers.*Flag related methods.

**08/30/2018 V7.0.2**

- Fixed render bug with Entity/Animation if no parent was attached.
- Fixed ColoredString + operator.
- SadConsole IBM Extended font embedded in library now.