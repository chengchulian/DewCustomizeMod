using DewCustomizeMod.config;
using HarmonyLib;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(DewQuest))]
public class DewQuestPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(DewQuest.SetNextGoal_ReachNode))]
    public static bool SetNextGoal_ReachNode_Prefix(DewQuest __instance, NextGoalSettings settings)
    {
        if (__instance is not Quest_LostSoul questLostSoul ||
            !AttrCustomizeResources.Config.enableCurrentNodeGenerateLostSoul)
        {
            return true;
        }


        SingletonDewNetworkBehaviour<Room>.instance.props.TryGetGoodNodePosition(out var shrinePos);
        __instance.CreateActor(shrinePos, null, (Shrine_HeroSoul r) =>
        {
            var prop = typeof(Shrine_HeroSoul).GetProperty("targetHero",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public);
            if (prop != null)
            {
                prop.SetValue(r, questLostSoul.targetHero);
            }
        });


        return false;
    }
}