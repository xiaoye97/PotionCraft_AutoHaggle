using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using PotionCraft.ObjectBased.Haggle;
using PotionCraft.ManagersSystem;

namespace xiaoye97
{
    [BepInPlugin("me.xiaoye97.plugin.PotionCraft.AutoHaggle", "AutoHaggle", "2.0.0")]
    public class AutoHaggle : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(AutoHaggle));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(HagglePointer), "Update")]
        public static void HagglePatch(HagglePointer __instance)
        {
            if (HaggleWindow.Instance.IsPaused)
                return;
            if (__instance.State != HagglePointerState.Moving)
                return;
            if (Managers.Trade.haggle.haggleCurrentBonuses == null || Managers.Trade.haggle.haggleCurrentBonuses.Count < 2)
                return;
            bool needHaggle = false;
            foreach (var b in Managers.Trade.haggle.haggleCurrentBonuses)
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