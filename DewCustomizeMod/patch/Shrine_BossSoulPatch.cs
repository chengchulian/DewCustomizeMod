using System.Linq;
using DewCustomizeMod.config;
using HarmonyLib;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(Shrine_BossSoul))]
public class ShrineBossSoulPatch
{
    private static AccessTools.FieldRef<Shrine_BossSoul, string> _droppedItemTypeNameRef =
        AccessTools.FieldRefAccess<Shrine_BossSoul, string>("_droppedItemTypeName");
    
    [HarmonyPrefix]
    [HarmonyPatch("Explode")]
    public static bool ExplodePrefix(Shrine_BossSoul __instance)
    {
        if (AttrCustomizeResources.Config.removeSkills.Contains(_droppedItemTypeNameRef(__instance))
            || AttrCustomizeResources.Config.removeGems.Contains(_droppedItemTypeNameRef(__instance))
            )
        {
            _droppedItemTypeNameRef(__instance) = null;
        }
         

        return true;
    }
}