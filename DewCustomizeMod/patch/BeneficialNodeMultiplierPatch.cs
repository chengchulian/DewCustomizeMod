using DewCustomizeMod.config;
using HarmonyLib;
using UnityEngine;

namespace DewCustomizeMod.patch
{
    [HarmonyPatch(typeof(RoomModifierBase), nameof(RoomModifierBase.GetScaledChance))]
    public static class RoomModifierBasePatch
    {
        [HarmonyPostfix]
        public static void GetScaledChance_Postfix(RoomModifierBase __instance, ref float __result)
        {
            if (__instance.difficultyScaling == ScaleWithDifficultyMode.Beneficial)
            {
                float multiplier = AttrCustomizeResources.Config.beneficialNodeMultiplier;
                __result *= multiplier;
            }
        }
    }
}