using BepInEx;
using DewCustomizeMod.controller;
using DewCustomizeMod.util;
using HarmonyLib;

namespace DewCustomizeMod
{
    [BepInPlugin(AttrCustomizeConstant.PluginGuid, AttrCustomizeConstant.PluginName,AttrCustomizeConstant.PluginVersion)]
    public class AttrCustomizeConfigPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo($"{AttrCustomizeConstant.PluginName} 插件已加载");

            var harmony = new Harmony(AttrCustomizeConstant.PluginGuid);
            harmony.PatchAll(); 
            
            gameObject.AddComponent<UIStateController>();
        }
    }
}