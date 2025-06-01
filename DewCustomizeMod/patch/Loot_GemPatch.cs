using System.Collections.Generic;
using System.Linq;
using DewCustomizeMod.config;
using HarmonyLib;
using UnityEngine;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(Loot_Gem))]
public class LootGemPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Loot_Gem.SelectGemAndQuality))]
    public static bool SelectGemAndQualityPrefix(Loot_Gem __instance, Rarity rarity, out Gem gem, out int quality)
    {
        var pool = NetworkedManagerBase<LootManager>.instance.poolGemsByRarity[rarity];

        string[] removeGems = AttrCustomizeResources.Config.removeGems;
        foreach (var remove in removeGems)
        {
            pool.Remove(remove);
        }

        if (pool.Count == 0)
        {
            pool.Add("Gem_C_Charcoal");
        }

        gem = DewResources.GetByShortTypeName<Gem>(pool.ElementAt(Random.Range(0, pool.Count)));
        quality = __instance.SelectQuality(rarity);


        return false;
    }
}