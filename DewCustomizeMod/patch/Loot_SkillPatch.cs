using System.Collections.Generic;
using System.Linq;
using DewCustomizeMod.config;
using HarmonyLib;
using UnityEngine;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(Loot_Skill))]
public class LootSkillPatch
{
    // 将技能字典提取为静态资源
    private static readonly Dictionary<Rarity, List<string>> SkillsByRarity = new()
    {
        {
            Rarity.Common, [
                "St_Q_GoldenBurst", "St_Q_HandCannon", "St_Q_IncendiaryRounds", "St_Q_Lunge", "St_Q_Fleche",
                "St_Q_CruelSun", "St_Q_Discipline", "St_Q_Lunge", "St_Q_EtherealInfluence", "St_Q_SuperNova"
            ]
        },
        {
            Rarity.Rare, [
                "St_R_DangerousTheory", "St_R_QuickTrigger", "St_R_PrecisionShot",
                "St_R_UnbreakableDetermination", "St_R_Parry", "St_R_SanctuaryOfEl", "St_R_Cataclysm",
                "St_R_Tranquility"
            ]
        },
        {
            Rarity.Epic, [
                "St_D_DisintegratingClaw", "St_D_SalamanderPowder", "St_D_DoubleTap", "St_D_AstridsMasterpiece",
                "St_D_MercyOfEl", "St_D_Resolve", "St_D_ExoticMatter", "St_D_ConvergencePoint"
            ]
        }
    };

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Loot_Skill.SelectSkillAndLevel))]
    public static bool SelectSkillAndLevelPrefix(Loot_Skill __instance, Rarity rarity, out SkillTrigger skill, out int level)
    {
        // 获取技能池并存储到局部变量中
        var pool = new HashSet<string>(NetworkedManagerBase<LootManager>.instance.poolSkillsByRarity[rarity]);

        // 使用静态字典来简化逻辑
        if (AttrCustomizeResources.Config.enableHeroSkillAddShop && SkillsByRarity.TryGetValue(rarity, out var additionalSkills))
        {
            pool.UnionWith(additionalSkills);
        }

        // 将需要移除的技能列表转换为 HashSet 以提高效率
        var removeSkillsSet = new HashSet<string>(AttrCustomizeResources.Config.removeSkills);
        pool.ExceptWith(removeSkillsSet);

        // 如果池为空，则添加默认技能
        if (pool.Count == 0)
        {
            pool.Add("St_C_Sneeze");
        }

        // 随机选择一个技能并获取其等级
        skill = DewResources.GetByShortTypeName<SkillTrigger>(pool.ElementAt(Random.Range(0, pool.Count)));

        // 计算技能等级
        var currentZoneIndex = NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;
        float minLevel = __instance.skillLevelMinByZoneIndex.Get(rarity).Evaluate(currentZoneIndex);
        float maxLevel = __instance.skillLevelMaxByZoneIndex.Get(rarity).Evaluate(currentZoneIndex);
        float floatLevel = Mathf.Lerp(minLevel, maxLevel, __instance.levelRandomCurve.Evaluate(Random.value));
        level = Mathf.Clamp(Mathf.RoundToInt(floatLevel), 1, 100);

        return false;
    }
}