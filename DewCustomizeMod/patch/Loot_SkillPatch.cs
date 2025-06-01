using System.Collections.Generic;
using System.Linq;
using DewCustomizeMod.config;
using HarmonyLib;
using UnityEngine;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(Loot_Skill))]
public class LootSkillPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Loot_Skill.SelectSkillAndLevel))]
    public static bool SelectSkillAndLevelPrefix(Loot_Skill __instance, Rarity rarity, out SkillTrigger skill,
        out int level)
    {
        HashSet<string> pool = NetworkedManagerBase<LootManager>.instance.poolSkillsByRarity[rarity];

        if (AttrCustomizeResources.Config.enableHeroSkillAddShop)
        {
            switch (rarity)
            {
                case Rarity.Common:
                {
                    List<string> qSkill = new List<string>
                    {
                        "St_Q_GoldenBurst", "St_Q_HandCannon", "St_Q_IncendiaryRounds", "St_Q_Lunge", "St_Q_Fleche",
                        "St_Q_CruelSun", "St_Q_Discipline", "St_Q_Lunge", "St_Q_EtherealInfluence", "St_Q_SuperNova"
                    };
                    pool.UnionWith(qSkill);
                    break;
                }
                case Rarity.Rare:
                {
                    List<string> rSkill = new List<string>
                    {
                        "St_R_DangerousTheory", "St_R_QuickTrigger", "St_R_PrecisionShot",
                        "St_R_UnbreakableDetermination", "St_R_Parry", "St_R_SanctuaryOfEl", "St_R_Cataclysm",
                        "St_R_Tranquility"
                    };
                    pool.UnionWith(rSkill);
                    break;
                }
                case Rarity.Epic:
                {
                    List<string> identitySkill = new List<string>
                    {
                        "St_D_DisintegratingClaw", "St_D_SalamanderPowder", "St_D_DoubleTap", "St_D_AstridsMasterpiece",
                        "St_D_MercyOfEl", "St_D_Resolve", "St_D_ExoticMatter", "St_D_ConvergencePoint"
                    };
                    pool.UnionWith(identitySkill);
                    break;
                }
            }
        }

        string[] removeSkills = AttrCustomizeResources.Config.removeSkills;
        foreach (var removeSkill in removeSkills)
        {
            pool.Remove(removeSkill);
        }

        if (pool.Count == 0)
        {
            pool.Add("St_C_Sneeze");
        }

        skill = DewResources.GetByShortTypeName<SkillTrigger>(pool.ElementAt(Random.Range(0, pool.Count)));
        float a = __instance.skillLevelMinByZoneIndex.Get(rarity)
            .Evaluate(NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex);
        float max = __instance.skillLevelMaxByZoneIndex.Get(rarity)
            .Evaluate(NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex);
        float floatLevel = Mathf.Lerp(a, max, __instance.levelRandomCurve.Evaluate(Random.value));
        level = Mathf.Clamp(Mathf.RoundToInt(floatLevel), 1, 100);
        
        return false;
    }
}