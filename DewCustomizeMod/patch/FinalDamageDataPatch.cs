using System;
using DewCustomizeMod.config;
using HarmonyLib;
using UnityEngine;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(FinalDamageData))]
public class FinalDamageDataPatch
{
    
    
    [HarmonyPatch(MethodType.Constructor)]
    [HarmonyPatch(new[] { typeof(DamageData), typeof(float), typeof(Entity) })]
    [HarmonyPostfix]
    public static void ConstructorPostfix(ref FinalDamageData __instance, DamageData data, float armorAmount, Entity victim)
    {
        if (victim is BossMonster)
        {
            float multiplier = AttrCustomizeResources.Config.bossSingleInjuryHealthMultiplier;
            if (multiplier < 0.999f)
            {
                __instance.amount = Mathf.Min(__instance.amount, victim.Status.maxHealth * multiplier);
            }
        }
    }
    
}