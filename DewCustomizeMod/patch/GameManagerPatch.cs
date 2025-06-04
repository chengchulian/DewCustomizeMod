using DewCustomizeMod.util;
using HarmonyLib;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(GameManager))]
public class GameManagerPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(GameManager.OnStartServer))]
    public static bool OnStartServerPrefix(GameManager __instance)
    {
        if (__instance.isServer)
        {
            GameManagerUtil.ExecGameStartLoadBefore(__instance);
        }

        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameManager.OnStartServer))]
    public static void OnStartServerPostfix(GameManager __instance)
    {
        if (__instance.isServer)
        {
            GameManagerUtil.ExecGameStartLoadAfter(__instance);
        }
    }
}