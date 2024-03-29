# Hair Modding Plus
by Butterfish

*For those of us that play RimWorld like The Sims, but with ~~more~~ equal amounts of murder.*
<br><br>

## Download
GitHub: https://github.com/TheButterfish/HairModdingPlus/releases/latest<br>
Steam: https://steamcommunity.com/sharedfiles/filedetails/?id=2114832515
<br><br>

## Description
_**TLDR: This mod gives modders the ability to have a hair texture render behind a pawn, as well as use alpha masks on hair, without additional fiddling with XML.**_

If you've made/tried to make a hair mod, you'll likely realize that RimWorld just slaps the texture on top of the pawn, colors the entire thing, and calls it a day.<br>
This mod gives modders an additional hair layer to work with that renders behind the pawn. So you'll be able to make, for example, long hair that flows down the back, without having to tailor-make it for one/each body type or make design compromises.<br>
In addition, this mod also enables the use of alpha masks on hair, so you can make hair with decorations/accessories that can be colored separately in-game.<br>
What's more, you don't even have to add more information to your HairDef XML, just provide the textures and you're done.

![Hair That Actually Fits All](https://raw.githubusercontent.com/TheButterfish/HairModdingPlus/master/ReadmeImages/sample.jpg)

*Note: This mod will not magically convert existing hair to work as above. You'll have to get hair from other mods that utilize this mod. Below are a few examples of such hair mods:*
- [Roo's Accessory Hairstyles](https://steamcommunity.com/sharedfiles/filedetails/?id=1991452568)
- [Roo's Royalty Hairstyles](https://steamcommunity.com/sharedfiles/filedetails/?id=2120391876)
- [RimNGE](https://steamcommunity.com/sharedfiles/filedetails/?id=1687909697)
- [Hamefura](https://steamcommunity.com/sharedfiles/filedetails/?id=2317290564)
- [Absolute Legends Hair](https://steamcommunity.com/sharedfiles/filedetails/?id=2195744587)<br>

*Note 2: Gradient hair shown above is from [Gradient Hair](https://steamcommunity.com/sharedfiles/filedetails/?id=1687053679).*
<br><br>

## Dependencies
You'll need Harmony to use this mod. Get it at [GitHub](https://github.com/pardeike/HarmonyRimWorld/releases/latest) or [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077).
<br><br>

## Compatibility
Should be compatible with most mods, unless they skip RenderPawnInternal or ResolveAllGraphics.<br>

Patched to be compatible with:
* [Prepare Carefully](https://steamcommunity.com/sharedfiles/filedetails/?id=735106432)
* [Gradient Hair](https://steamcommunity.com/sharedfiles/filedetails/?id=1687053679)
* [Babies And Children 1.2](https://steamcommunity.com/sharedfiles/filedetails/?id=2373187996) by CentAtMoney
* [Babies And Children 1.3](https://steamcommunity.com/sharedfiles/filedetails/?id=2559574784) by cometopapa
* [[KV] Show Hair With Hats or Hide All Hats](https://steamcommunity.com/sharedfiles/filedetails/?id=1180826364)
* [Hats Display Selection](https://steamcommunity.com/sharedfiles/filedetails/?id=1542291825)
* [Humanoid Alien Races 2.0](https://steamcommunity.com/sharedfiles/filedetails/?id=839005762), but needs more testing.
* [Children and Pregnancy 1.2](https://ludeon.com/forums/index.php?topic=51258.0) (not yet updated for 1.3 at time of writing)
* [Facial Stuff](https://steamcommunity.com/workshop/filedetails/?id=818322128) (not updated since 1.1, your mileage may vary)

Secondary hair color is selectable with Prepare Carefully (select "Hair Color 2" from the dropdown).

Safe to add or remove from existing saves.
<br><br>

## Known Issues
Semi-transparent portions of hair might appear significantly more transparent or significantly less transparent due to shader issues. This may cause certain hairstyles that rely on it (e.g. shaved hair) to appear wrong. No fix is possible at the moment.
<br><br>

## How to Use
*Note: The instructions below assume you already know how to make a regular hair mod.<br>
If you're interested in making a hair mod and don't know where to start, check [this guide](https://steamcommunity.com/sharedfiles/filedetails/?id=1899180537) out for more information.*
<br><br>

### Back Hair Layer
To have a texture render behind the pawn, simply add "\_back" to the end of your texture's file name.<br>
![](https://raw.githubusercontent.com/TheButterfish/HairModdingPlus/master/ReadmeImages/addback.jpg)

"demo_south.png" is your regular texture displayed when the pawn faces south, and "demo_south_back.png" is the texture that will be behind the pawn when the pawn faces south. This works the same for the other orientations, e.g. "demo_east.png" and "demo_east_back.png". Back textures are optional, textures without a corresponding "\_back.png" will display as they usually would.

**West or east back textures will NOT be automatically flipped if only one is provided.** This is intentional, to allow modders to make asymmetrical hairs if they wish. To have a back texture be used for both east and west sides, just make a copy of it and rename the copy accordingly i.e "name_east_back.png" and "name_west_back.png".
<br><br>

### Alpha Masks
*For more information on alpha masks, check [this guide](https://github.com/seraphile/rimshare/wiki/Colouring-in-Images) out.*

To apply an alpha mask to a hair texture, simply add "m" to the end of your mask's file name. This works for both front textures and back textures.<br>
Areas marked red (#FF0000) will be painted with the primary color, areas marked green (#00FF00) will be painted with the secondary color, and areas marked black (#000000) will not be painted.<br>
![](https://raw.githubusercontent.com/TheButterfish/HairModdingPlus/master/ReadmeImages/addmask.jpg)

"demo_southm.png" is the mask that will be applied to "demo_south.png", and "demo_south_backm.png" will be applied to "demo_south_back.png". Masks are optional, textures without a corresponding "m.png" will display as they usually would.

**West or east masks will NOT be automatically flipped if only one is provided.** This is intentional, to allow modders to make asymmetrical hairs if they wish. To have a mask be used for both east and west sides, just make a copy of it and rename the copy accordingly i.e "name_eastm.png" and "name_westm.png".
<br><br>

### Examples
Download the demo hair mod from a source below to see working examples (includes the hair shown above):<br>
Github: https://github.com/TheButterfish/HairModdingPlus/blob/master/ButterfishHairModdingPlusHairSamples.zip<br>
Steam: https://steamcommunity.com/sharedfiles/filedetails/?id=2324262910<br>
or try out one of the mods mentioned under Description above.
<br><br>

## Licensing
If you wish to improve, fork, add patches, borrow code snippets, include in a modpack, or take over if I go on hiatus, go ahead. Just mention me in the credits and you're good to go.
<br><br>

## Credits
Andreas Pardeike, for creating [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)<br>
Killface, for a code snippet I borrowed from his [Facial Stuff](https://steamcommunity.com/workshop/filedetails/?id=818322128) mod<br>
sumghai, for contributing some hair samples for the demo mod<br>
The various contributors to the [RimWorld modding wiki](https://rimworldwiki.com/wiki/Modding_Tutorials)<br>
Tynan Sylvester, for ~~stealing my time and money~~ creating [RimWorld](https://en.wikipedia.org/wiki/Cocaine)
