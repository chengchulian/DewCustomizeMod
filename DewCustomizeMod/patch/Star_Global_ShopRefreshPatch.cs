using DewCustomizeMod.config;
using HarmonyLib;

namespace DewCustomizeMod.patch;
[HarmonyPatch(typeof(Star_Global_ShopRefresh))]
public static class StarGlobalShopRefreshPatch
{
    [HarmonyPostfix]
    [HarmonyPatch( "OnStartInGame")]
    public static void OnStartInGamePostfix(Star_Global_ShopRefresh __instance)
    {
        // 确保 player 存在
        if (__instance.player != null)
        {
            __instance.player.allowedShopRefreshes = AttrCustomizeResources.Config.shopRefreshes;
        }
    }
}