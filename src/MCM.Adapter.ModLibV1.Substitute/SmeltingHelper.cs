using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.Core;

namespace ModLib
{
    public static class SmeltingHelper
    {
        [Obsolete("Do not use!", true)]
        public static IEnumerable<CraftingPiece> GetNewPartsFromSmelting(ItemObject item) => Enumerable.Empty<CraftingPiece>();
    }
}