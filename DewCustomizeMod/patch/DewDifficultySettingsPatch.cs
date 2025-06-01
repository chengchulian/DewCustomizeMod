using System;
using System.Reflection;
using DewCustomizeMod.util;
using HarmonyLib;
using UnityEngine;
using DewCustomizeMod.config;
using DewCustomizeMod.util;

namespace DewCustomizeMod.patch
{
    [HarmonyPatch]
    public static class DewDifficultySettingsPatch
    {
        // 使用 TargetMethod 指定 internal 方法
        static MethodBase TargetMethod()
        {
            var type = typeof(DewDifficultySettings);
            return type.GetMethod("ApplyDifficultyModifiers", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(DewDifficultySettings __instance, Entity entity)
        {
            if (entity is Monster)
            {
                entity.Status.AddStatBonus(new StatBonus
                {
                    maxHealthPercentage = __instance.enemyHealthPercentage,
                    attackDamagePercentage = __instance.enemyPowerPercentage,
                    abilityPowerPercentage = __instance.enemyPowerPercentage,
                    movementSpeedPercentage = __instance.enemyMovementSpeedPercentage *
                                              AttrCustomizeResources.Config.enemyMovementSpeedPercentage,
                    attackSpeedPercentage =
                        __instance.enemyAttackSpeedPercentage *
                        AttrCustomizeResources.Config.enemyAttackSpeedPercentage,
                    abilityHasteFlat = __instance.enemyAbilityHasteFlat *
                                       AttrCustomizeResources.Config.enemyAbilityHasteFlat
                });
            }

            if (entity is Monster m)
            {
                float healthMultiplier;
                float damageMultiplier;

                if (m is BossMonster)
                {
                    healthMultiplier = NetworkedManagerBase<GameManager>.instance
                        .GetBossMonsterHealthMultiplierByScaling(__instance.scalingFactor);
                    damageMultiplier = NetworkedManagerBase<GameManager>.instance
                        .GetBossMonsterDamageMultiplierByScaling(__instance.scalingFactor);
                    healthMultiplier *= AttrCustomizeResources.Config.bossHealthMultiplier;
                    damageMultiplier *= AttrCustomizeResources.Config.bossDamageMultiplier;
                }
                else if (m.type == Monster.MonsterType.MiniBoss)
                {
                    healthMultiplier = NetworkedManagerBase<GameManager>.instance
                        .GetMiniBossMonsterHealthMultiplierByScaling(__instance.scalingFactor);
                    damageMultiplier = NetworkedManagerBase<GameManager>.instance
                        .GetMiniBossMonsterDamageMultiplierByScaling(__instance.scalingFactor);
                    healthMultiplier *= AttrCustomizeResources.Config.miniBossHealthMultiplier;
                    damageMultiplier *= AttrCustomizeResources.Config.miniBossDamageMultiplier;
                }
                else
                {
                    healthMultiplier = NetworkedManagerBase<GameManager>.instance
                        .GetRegularMonsterHealthMultiplierByScaling(__instance.scalingFactor);
                    damageMultiplier = NetworkedManagerBase<GameManager>.instance
                        .GetRegularMonsterDamageMultiplierByScaling(__instance.scalingFactor);
                    healthMultiplier *= AttrCustomizeResources.Config.littleMonsterHealthMultiplier;
                    damageMultiplier *= AttrCustomizeResources.Config.littleMonsterDamageMultiplier;
                }

                int instanceCurrentZoneIndex = NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;

                healthMultiplier = AttrCustomizeUtil.ExponentialGrowth(instanceCurrentZoneIndex,
                    healthMultiplier, AttrCustomizeResources.Config.extraHealthGrowthMultiplier);
                damageMultiplier = AttrCustomizeUtil.ExponentialGrowth(instanceCurrentZoneIndex,
                    damageMultiplier, AttrCustomizeResources.Config.extraDamageGrowthMultiplier);

                entity.Status.AddStatBonus(new StatBonus
                {
                    maxHealthPercentage = (healthMultiplier - 1f) * 100f,
                    attackDamagePercentage = (damageMultiplier - 1f) * 100f,
                    abilityPowerPercentage = (damageMultiplier - 1f) * 100f
                });
            }
            else if (entity is Hero)
            {
                entity.takenHealProcessor.Add(
                    delegate(ref HealData data, Actor actor, Entity target)
                    {
                        data.ApplyRawMultiplier(
                            NetworkedManagerBase<GameManager>.instance.difficulty.healRawMultiplier *
                            AttrCustomizeResources.Config.healRawMultiplier);
                    }, 100);
            }

            // 跳过原始方法
            return false;
        }
    }
}