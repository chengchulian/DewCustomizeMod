using DewCustomizeMod.config;
using HarmonyLib;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(Star_Mist_ParryAllDirections))]
public class StarMistParryAllDirectionsPatch
{
    [HarmonyPrefix]
    [HarmonyPatch("ActorEventOnAbilityInstanceBeforePrepare")]
    public static bool ActorEventOnAbilityInstanceBeforePrepare(Star_Mist_ParryAllDirections __instance,
        EventInfoAbilityInstance obj)
    {
        if (obj.instance is Se_R_Parry_Start se_R_Parry_Start)
        {
            se_R_Parry_Start.allowAnyDirection = AttrCustomizeResources.Config.enableMistAllowAnyDirection;
        }
        return false;
    }

    
}