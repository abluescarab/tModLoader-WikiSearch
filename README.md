# WikiSearch
Based on DrEinsteinium's tWiki mod, WikiSearch is a tModLoader mod that allows you to search the [Terraria Wiki](http://terraria.gamepedia.com/) for whatever is under the mouse cursor.

Press Q to search the wiki while hovering over any item, NPC, or tile.

## Registering Your Mod
To register your mod, you must use the `Call()` method. Place the following code into your mod's main file (e.g. if your mod is MyMod, place this code into `MyMod.cs`; in other words, into the file that inherits from `Terraria.ModLoader.Mod`).

```csharp
public override void PostSetupContent() {
    Mod wikiSearch = ModLoader.GetMod("WikiSearch");

    if(wikiSearch != null) {
        wikiSearch.Call("RegisterMod", this, "http://mymod.gamepedia.com/index.php?search=%s");
    }
}
```

Note that the Gamepedia link can be any wiki link with the search term replaced. Just go to your wiki, search for something that isn't a real page (like "testtest"), and replace the search term with "%s".

## Known Issues
* Some naturally-placed tiles cannot be searched
* Chests open the chest page instead of the individual page

## Credits
* [DrEinsteinium](https://forums.terraria.org/index.php?members/dreinsteinium.48502/) for the original tWiki mod
* [DarkLight](https://forums.terraria.org/index.php?members/%C3%90ark%C5%81ight.75173/) for suggesting mod support and linking some mod wikis