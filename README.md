# Hair Modding Plus
by Butterfish

*For those of us that play RimWorld like The Sims, but with ~~more~~ equal amounts of murder.*
<br><br>

## Description
TLDR: This mod gives modders the ability to have a hair texture render behind a pawn, as well as use alpha masks on hair, without additional fiddling with XML.

If you've made/tried to make a hair mod, you'll likely realize that RimWorld just slaps the texture on top of the pawn, colors the entire thing, and calls it a day.<br>
This mod gives modders an additional hair layer to work with that renders behind the pawn. So you'll be able to make, for example, long hair that flows down the back, without having to tailor-make it for one/each body type or make design compromises.<br>
In addition, this mod also enables the use of alpha masks on hair, so you can make hair with decorations/accessories that won't be recolored along with the rest of the hair.<br>
What's more, you don't even have to add more information to your HairDef XML, just provide the textures and you're done.

![Hair That Actually Fits All](https://raw.githubusercontent.com/TheButterfish/HairModdingPlus/master/ReadmeImages/sample.jpg)

*Note: This mod will not magically convert existing hair to work as above. You'll have to get hair from other mods made with this mod in mind. I myself am in the process of making one, releasing Soon<sup>TM<sup>.*
<br><br>

## Dependencies
You'll need Harmony to use this mod. Get it at [GitHub](https://github.com/pardeike/HarmonyRimWorld/releases) or [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077).
<br><br>

## Compatibility
Should be compatible with most mods, unless they skip RenderPawnInternal or ResolveAllGraphics.<br>

Patched to be compatible with [Facial Stuff](https://steamcommunity.com/workshop/filedetails/?id=818322128).<br>

Safe to add or remove from existing saves.
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
![](https://raw.githubusercontent.com/TheButterfish/HairModdingPlus/master/ReadmeImages/addmask.jpg)

"demo_southm.png" is the mask that will be applied to "demo_south.png", and "demo_south_backm.png" will be applied to "demo_south_back.png". Masks are optional, textures without a corresponding "m.png" will display as they usually would.

**West or east masks will NOT be automatically flipped if only one is provided.** This is intentional, to allow modders to make asymmetrical hairs if they wish. To have a mask be used for both east and west sides, just make a copy of it and rename the copy accordingly i.e "name_eastm.png" and "name_westm.png".
<br><br>

## Credits
Andreas Pardeike, for creating [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)<br>
Killface, for a code snippet I borrowed from his [Facial Stuff](https://steamcommunity.com/workshop/filedetails/?id=818322128) mod<br>
The various contributors to the [RimWorld modding wiki](https://rimworldwiki.com/wiki/Modding_Tutorials)<br>
Tynan Sylvester, for ~~stealing my time and money~~ creating RimWorld
