﻿using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;

namespace ModLib
{
    public static class SmeltingHelper
    {
        // Bannerlord Tweaks depend on it
        public static IEnumerable<CraftingPiece> GetNewPartsFromSmelting(ItemObject item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return item.WeaponDesign.UsedPieces
                .Select(x => x.CraftingPiece)
                .Where(x => x?.IsValid == true && !Campaign.Current.GetCampaignBehavior<CraftingCampaignBehavior>().IsOpened(x));
        }
    }
}