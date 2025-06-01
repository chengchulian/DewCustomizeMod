using DewCustomizeMod.config;
using HarmonyLib;

namespace DewCustomizeMod.patch;
[HarmonyPatch(typeof( Star_Global_ShopMoreItems))]
public static class StarGlobalShopMoreItemsPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("OnStartInGame")]
    public static void OnStartInGamePostfix(Star_Global_ShopMoreItems __instance)
    {
        // 确保 player 存在
        if (__instance.player != null)
        {
            __instance.player.shopAddedItems = AttrCustomizeResources.Config.shopAddedItems;
        }
    }
}