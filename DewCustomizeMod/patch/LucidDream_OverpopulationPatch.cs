using DewCustomizeMod.config;
using HarmonyLib;

namespace DewCustomizeMod.patch;


[HarmonyPatch(typeof( LucidDream_Overpopulation))]
public class LucidDreamOverpopulationPatch
{
    
    [HarmonyPostfix]
    [HarmonyPatch( "OnCreate")]
    public static void OnCreatePostfix(LucidDream_Overpopulation __instance)
    {
        if (__instance.isServer)
        {
            NetworkedManagerBase<GameManager>.instance.maxAndSpawnedPopulationMultiplier =
                AttrCustomizeResources.Config.maxAndSpawnedPopulationMultiplier;
        }
    }
}