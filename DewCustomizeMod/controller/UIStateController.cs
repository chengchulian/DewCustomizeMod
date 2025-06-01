﻿using DewCustomizeMod.i18n;
using DewCustomizeMod.ui;
using UnityEngine;

namespace DewCustomizeMod.controller
{
    public class UIStateController : MonoBehaviour
    {
        public static bool ShowWindow { get; private set; } = false;
        private Rect _windowRect = new Rect(20, 20, 550, 600);

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F8))
            {
                ShowWindow = !ShowWindow;
            }
        }

        private void OnGUI()
        {
            if (!ShowWindow) return;

            _windowRect = GUI.Window(337845818, _windowRect, AttrCustomizeConfigWindow.DrawWindowContents,
                LocalizationConfig.Get("config_editor_title"));
        }
    }
}