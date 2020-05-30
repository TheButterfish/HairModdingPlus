# Hair Modding Plus
by Butterfish

*For those of us that play RimWorld like The Sims, but with ~~more~~ equal amounts of murder.*

## Description
TLDR: This mod gives modders the ability to have a hair texture render behind a pawn, as well as use alpha masks on hair.

If you've made/tried to make a hair mod, you'll likely realize that RimWorld just slaps the texture on top of the pawn, colors the entire thing, and calls it a day.<br>
This mod gives modders an additional hair layer to work with that renders behind the pawn. So you'll be able to make, for example, long hair that flows down the back, without having to tailor-make it for one/each body type or make design compromises.<br>
In addition, this mod also enables the use of alpha masks on hair, so you can make hair with decorations/accessories that won't be recolored along with the rest of the hair.

![Hair That Actually Fits All](https://raw.githubusercontent.com/TheButterfish/HairModdingPlus/master/ReadmeImages/sample.jpg)

*Note: This mod will not magically convert existing hair to work as above. You'll have to get hair from other mods made with this mod in mind. I myself am in the process of making one, releasing Soon<sup>TM<sup>.*

## Dependencies
You'll need Harmony to use this mod. Get it at [GitHub](https://github.com/pardeike/HarmonyRimWorld/releases) or [Steam](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077).

## Compatibility
Should be compatible with most mods, unless they skip RenderPawnInternal or ResolveAllGraphics.<br>

Contains compatibility patches for [Facial Stuff](https://steamcommunity.com/workshop/filedetails/?id=818322128).<br>

Safe to add or remove from existing saves.

## How to Use
*Note: The instructions below assume you already know how to make a regular hair mod.<br>
If you're interested in making a hair mod and don't know where to start, check [this guide](https://steamcommunity.com/sharedfiles/filedetails/?id=1899180537) out for more information.*

### Back Hair Layer
To have a texture render behind the pawn, simply add "\_back" to the end of your file name.<br>

For example, the hair shown in the sample image above consists of the following two textures:<br>
![](https://raw.githubusercontent.com/TheButterfish/HairModdingPlus/master/ReadmeImages/addback.jpg)

"demo_south.png" is your regular texture displayed when the pawn faces south, and "demo_south_back.png" is the texture that will be behind the pawn when the pawn faces south. This works the same for the other orientations, e.g. "demo_east.png" and "demo_east_back.png". Textures without a corresponding "\_back.png" will display as they usually would.

**West or east back textures will NOT be automatically flipped if only one is provided.** This is intentional, to allow modders to make asymmetrical hairs if they wish. To have a back texture be used for both east and west sides, just make a copy of it and rename the copy accordingly for the other side.

### Alpha Masks
To apply an alpha mask to a hair texture...

TODO
Link to alpha masking guide
Show picture
Complete desc

## Credits
TODO
