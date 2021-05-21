# Vgui.Net
Library for parsing and working with [Valve's Vgui](https://developer.valvesoftware.com/wiki/Category:VGUI).

Provided in .NET Standard 2.0 for support in the latest versions of Unity.

## Get Started

Its important to have a solid understanding of Vgui in general, in order to get the most out of this library. Please check out the [wiki, here](https://developer.valvesoftware.com/wiki/Category:VGUI).

There is also [this guide for making HUDs in TF2](http://doodlesstuff.com/?p=tf2hud), which are parsed and rendered using Vgui.

### Basic Usage

Given a working directory, you can parse any Vgui resource file in that directory or any subdirectory like so:

```c#
using Vgui;

// the object returned by FromFile will always be a "Root" object, which is not present
// in the source resource file.
var obj = VguiSerializer.FromFile("path/to/resource/dir", "resource/ClientScheme.res");

// Get the "Scheme" object and its "Colors" sub object:
var colors = obj.Get("Scheme")?.Get("Colors");

// Get a property "Brown", which should be a value and have no child properties:
var brown = colors?.Get("Brown");
Debug.Assert(brown.IsValue);

var value = brown.Value;
```