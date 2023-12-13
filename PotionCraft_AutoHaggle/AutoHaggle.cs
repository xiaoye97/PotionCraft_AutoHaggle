using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using PotionCraft.ObjectBased.Haggle;

namespace xiaoye97
{
    [BepInPlugin("me.xiaoye97.plugin.PotionCraft.AutoHaggle", "AutoHaggle", "1.1.0")]
    public class AutoHaggle : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(AutoHaggle));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Pointer), "Update")]
        public static void HagglePatch(Pointer __instance)
        {
            if (HaggleWindow.Instance.isPaused)
                return;
            if (__instance.state != Pointer.State.Moving)
                return;
            if (HaggleWindow.Instance.currentBonuses == null || HaggleWindow.Instance.currentBonuses.Count < 2)
                return;
            bool needHaggle = false;
            foreach (var b in HaggleWindow.Instance.currentBonuses)
            {
                // 跳过左右两把的绿的
                if (b.haggleBonus.Position < 0.1f || b.haggleBonus.Position > 0.9f)
                {
                    continue;
                }
                if (Mathf.Abs(b.haggleBonus.Position - __instance.Position) <= b.size / 2f)
                {
                    needHaggle = true;
                    break;
                }
            }
            if (needHaggle)
            {
                HaggleWindow.Instance.bargainButton.OnButtonClicked(false);
            }
        }
    }
}