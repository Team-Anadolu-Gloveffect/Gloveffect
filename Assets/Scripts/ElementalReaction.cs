using Enums;
using System.Collections.Generic;

public static class ElementalReaction
{
    public static Dictionary<SpellTypes, Dictionary<SpellTypes, int>> table = new Dictionary<SpellTypes, Dictionary<SpellTypes, int>>()
    {
        { SpellTypes.Flametron, new Dictionary<SpellTypes, int>() { { SpellTypes.Cryonite, 3 }, { SpellTypes.Lumisium, 1 }, { SpellTypes.Flametron, 2 }, { SpellTypes.Ignisis, 2 } } },
        { SpellTypes.Lumisium, new Dictionary<SpellTypes, int>() { { SpellTypes.Flametron, 3 }, { SpellTypes.Ignisis, 1 }, { SpellTypes.Lumisium, 2 }, { SpellTypes.Cryonite, 2 } } },
        { SpellTypes.Ignisis, new Dictionary<SpellTypes, int>() { { SpellTypes.Lumisium, 3 }, { SpellTypes.Cryonite, 1 }, { SpellTypes.Ignisis, 2 }, { SpellTypes.Flametron, 2 } } },
        { SpellTypes.Cryonite, new Dictionary<SpellTypes, int>() { { SpellTypes.Ignisis, 3 }, { SpellTypes.Flametron, 1 }, { SpellTypes.Cryonite, 2 }, { SpellTypes.Lumisium, 2 } } },
    };
}
