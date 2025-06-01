using BepInEx;
using DewCustomizeMod.controller;
using DewCustomizeMod.i18n;
using DewCustomizeMod.ui;
using HarmonyLib;
using UnityEngine;

namespace DewCustomizeMod
{
    [BepInPlugin("com.chengchulian.DewCustomizeMod.customizable", "Dew Customizable", "0.2.5")]
    public class AttrCustomizeConfigPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("自定制Mod插件已加载");

            var harmony = new Harmony("com.chengchulian.DewCustomizeMod.customizable");
            harmony.PatchAll(); 
            
            gameObject.AddComponent<UIStateController>();
        }
    }
}