using DewCustomizeMod.config;
using HarmonyLib;
using UnityEngine;

namespace DewCustomizeMod.patch;

[ HarmonyPatch(typeof(Star_Global_StartSkill))]
public class StarGlobalStartSkillPatch
{
    [HarmonyPostfix]
    [HarmonyPatch( "OnStartInGame")]
    public static void OnStartInGamePostfix(Star_Global_ShopRefresh __instance)
    {
        Vector3 pivot = __instance.hero.agentPosition + (Rift.instance.transform.position - __instance.hero.agentPosition).normalized * 2.5f;
        pivot = Dew.GetGoodRewardPosition(pivot, 1f);
        
        string[] startSkills = AttrCustomizeResources.Config.startSkills;
        int[] startSkillsLevel = AttrCustomizeResources.Config.startSkillsLevel;
			
        for (var i = 0; i < startSkills.Length; i++)
        {
            SkillTrigger skillTrigger = DewResources.FindOneByTypeSubstring<SkillTrigger>(startSkills[i]);
            Dew.CreateSkillTrigger(skillTrigger, pivot, startSkillsLevel[i], __instance.player);
        }

        string[] startGems = AttrCustomizeResources.Config.startGems;
        int[] startGemsQuality = AttrCustomizeResources.Config.startGemsQuality;
			
        for (var i = 0; i < startGems.Length; i++)
        {
            Gem gem = DewResources.FindOneByTypeSubstring<Gem>(startGems[i]);
            Dew.CreateGem(gem, pivot, startGemsQuality[i], __instance.player);
        }
    }
}