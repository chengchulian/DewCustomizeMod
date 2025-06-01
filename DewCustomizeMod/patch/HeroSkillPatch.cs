using System;
using System.Collections.Generic;
using DewCustomizeMod.config;
using DewCustomizeMod.util;
using HarmonyLib;

namespace DewCustomizeMod.patch
{
    [HarmonyPatch(typeof(HeroSkill), nameof(HeroSkill.GetMaxGemCount))]
    public static class HeroSkillGetMaxGemCountPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(HeroSkill __instance, HeroSkillLocation type, ref int __result)
        {
            switch (type)
            {
                case HeroSkillLocation.Q:
                    __result = AttrCustomizeResources.Config.skillQGemCount;
                    break;
                case HeroSkillLocation.W:
                    __result = AttrCustomizeResources.Config.skillWGemCount;
                    break;
                case HeroSkillLocation.E:
                    __result = AttrCustomizeResources.Config.skillEGemCount;
                    break;
                case HeroSkillLocation.R:
                    __result = AttrCustomizeResources.Config.skillRGemCount;
                    break;
                case HeroSkillLocation.Identity:
                    __result = AttrCustomizeResources.Config.skillIdentityGemCount;
                    break;
                case HeroSkillLocation.Movement:
                    __result = AttrCustomizeResources.Config.skillMovementGemCount;
                    break;
                default:
                    __result = 0;
                    break;
            }

            __result = Math.Clamp(__result, AttrCustomizeConstant.MinGemCount, AttrCustomizeConstant.MaxGemCount);

            return false; // 跳过原方法
        }
    }
    
     [HarmonyPatch(typeof(HeroSkill), nameof(HeroSkill.TryGetEquippedGemOfSameType))]
      public static class HeroSkillTryGetEquippedGemOfSameTypePatch
     { 
          [HarmonyPrefix]
         public static bool Prefix(HeroSkill __instance, ref bool __result,Type type, out GemLocation loc, out Gem gem)
         {
             foreach (KeyValuePair<GemLocation, Gem> p in __instance.gems)
             {
                 if (p.Value.GetType() == type)
                 {
                     loc = p.Key;
                     gem = p.Value;
                     __result = AttrCustomizeResources.Config.enableGemMerge;
                     return false;
                 }
             }
             loc = default;
             gem = null;
             __result = false;
             
             return false; // 跳过原方法
         }
     }
    
}