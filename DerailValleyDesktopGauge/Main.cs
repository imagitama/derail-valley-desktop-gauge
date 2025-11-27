using System;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;

namespace DerailValleyDesktopGauge;

public static class Main
{
    public static UnityModManager.ModEntry ModEntry;
    public static Settings Settings;
    private static OpenSimGaugeManager _openSimGaugeManager;

    private static bool Load(UnityModManager.ModEntry modEntry)
    {
        ModEntry = modEntry;

        Harmony? harmony = null;
        try
        {
            Settings = Settings.Load<Settings>(modEntry);

            // modEntry.OnGUI = OnGUI;
            // modEntry.OnSaveGUI = OnSaveGUI;

            harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _openSimGaugeManager = new OpenSimGaugeManager();
            _openSimGaugeManager.Start();

            ModEntry.Logger.Log("DerailValleyDesktopGauge started");
        }
        catch (Exception ex)
        {
            ModEntry.Logger.LogException($"Failed to load {modEntry.Info.DisplayName}:", ex);
            harmony?.UnpatchAll(modEntry.Info.Id);
            return false;
        }

        modEntry.OnUnload = Unload;
        return true;
    }

    // private static void OnGUI(UnityModManager.ModEntry modEntry)
    // {
    //     GUILayout.Label("Mod Settings", UnityEngine.GUI.skin.label);

    //     Settings.Port = int.Parse(
    //         GUILayout.TextField(Settings.Port.ToString())
    //     );
    // }

    // private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    // {
    //     Settings.Save(modEntry);
    // }

    private static bool Unload(UnityModManager.ModEntry entry)
    {
        _openSimGaugeManager?.Stop();
        ModEntry.Logger.Log("DerailValleyDesktopGauge stopped");
        return true;
    }
}
