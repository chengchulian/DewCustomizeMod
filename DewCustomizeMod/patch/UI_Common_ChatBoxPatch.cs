using DewCustomizeMod.util;
using HarmonyLib;

namespace DewCustomizeMod.patch;

[HarmonyPatch(typeof(UI_Common_ChatBox))]
public class UI_Common_ChatBoxPatch
{
    [HarmonyPrefix]
    [HarmonyPatch("ClientEventOnMessageReceived")]
    public static bool ClientEventOnMessageReceivedPrefix(UI_Common_ChatBox __instance, ChatManager.Message obj)
    {
        GameManagerUtil.ClientSyncData(obj);
        return true;
    }
}