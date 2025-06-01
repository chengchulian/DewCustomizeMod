using DewCustomizeMod.config;
using HarmonyLib;
using UnityEngine;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(Se_HeroKnockedOut))]
public class Se_HeroKnockedOutPatch
{
    
    
    // 创建访问 _didAddQuest 的字段引用
    private static readonly AccessTools.FieldRef<Se_HeroKnockedOut, bool> _didAddQuestRef =
        AccessTools.FieldRefAccess<Se_HeroKnockedOut, bool>("_didAddQuest");
    
    [HarmonyPostfix]
    [HarmonyPatch("ActiveLogicUpdate")]
    public static void ActiveLogicUpdate_Postfix(Se_HeroKnockedOut __instance, float dt)
    {
        if (!__instance.isServer)
        {
            return;
        }
        if (!_didAddQuestRef(__instance)
            && Time.time - __instance.creationTime > 1f
            && Dew.SelectRandomAliveHero(fallbackToDead: false) != null
            && (AttrCustomizeResources.Config.enableBossRoomGenerateLostSoul 
                || NetworkedManagerBase<ZoneManager>.instance.currentNode.type != WorldNodeType.ExitBoss))
        {
            _didAddQuestRef(__instance) = true;
    
            NetworkedManagerBase<QuestManager>.instance.StartQuest<Quest_LostSoul>(s => 
            {
                s.NetworktargetHero = (Hero)__instance.victim;
            });
        }
        __instance.victim.Status.SetHealth(0.01f);
        
    }
}