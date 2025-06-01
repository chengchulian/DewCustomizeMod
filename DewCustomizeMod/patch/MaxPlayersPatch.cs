using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using DewCustomizeMod.config;
using EpicTransport;
using Steamworks;
using TriangleNet;
using UnityEngine;

namespace DewCustomizeMod.patch
{
    [HarmonyPatch(typeof(LobbyServiceProvider))]
    public static class LobbyServiceProviderPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(LobbyServiceProvider.GetInitialAttr_maxPlayers))]
        public static bool GetInitialAttr_maxPlayers_Prefix(ref int __result)
        {
            

            __result = AttrCustomizeResources.Config.maxPlayer;
            return false;
        }
    }

    [HarmonyPatch(typeof(LobbyServiceSteam))]
    public static class ApplyLobbyDataPatch
    {
        // 显式指定私有方法 ApplyLobbyData，参数类型必须完全匹配
        static MethodBase TargetMethod()
        {
            // 传入参数类型：CSteamID 和 ref LobbyInstanceSteam
            return AccessTools.Method(typeof(LobbyServiceSteam), "ApplyLobbyData",
                new Type[] { typeof(CSteamID), typeof(LobbyInstanceSteam).MakeByRefType() });
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            

            var code = new List<CodeInstruction>(instructions);

            for (int i = 0; i < code.Count; i++)
            {
                if (code[i].opcode == OpCodes.Ldc_I4_4)
                {
                    code[i] = new CodeInstruction(OpCodes.Ldc_I4, 1024);
                    break;
                }
            }
            
            return code;
        }
    }
    [HarmonyPatch(typeof(UI_Lobby_HideIfSingleplayer))]
    public static class LobbyUIPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Start")]
        public static bool StartPrefix(MonoBehaviour __instance)
        {
            if (DewNetworkManager.networkMode == DewNetworkManager.Mode.Singleplayer)
            {
                __instance.gameObject.SetActive(false);
            }
            else
            {
                __instance.StartCoroutine(WaitLobbyLoadEndCoroutine(__instance));
            }

            // 跳过原始 Start 方法
            return false;
        }

        private static IEnumerator WaitLobbyLoadEndCoroutine(MonoBehaviour __instance)
        {
            int count = -1;
            while (count < 0)
            {
                yield return new WaitForSeconds(0.1f);
                try
                {
                    count = ManagerBase<LobbyManager>.instance.service.currentLobby.maxPlayers;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            Transform playList = __instance.transform.GetChild(7);
            GameObject playListGameObject = playList.gameObject;

            UI_Lobby_PlayerListItem[] uiLobbyPlayerListItems = playListGameObject.GetComponentsInChildren<UI_Lobby_PlayerListItem>();
            int addCount = count - uiLobbyPlayerListItems.Length;

            for (int i = 0; i < addCount; i++)
            {
                GameObject addGameObject = UnityEngine.Object.Instantiate(
                    uiLobbyPlayerListItems[0].gameObject,
                    playListGameObject.transform
                );

                addGameObject.name = $"UI_Lobby_PlayerListItem ({uiLobbyPlayerListItems.Length + i})";
                addGameObject.transform.SetParent(playListGameObject.transform, false);
                addGameObject.GetComponent<UI_Lobby_PlayerListItem>().index = playListGameObject.transform.childCount - 1;
            }

            yield return null;
        }
    }
}