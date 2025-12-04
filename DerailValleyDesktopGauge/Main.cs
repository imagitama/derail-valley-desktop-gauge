using System;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace DerailValleyDesktopGauge;

#if DEBUG
[EnableReloading]
#endif
public static class Main
{
    public static UnityModManager.ModEntry ModEntry;
    public static Settings settings;
    public static OpenSimGaugeManager openSimGaugeManager;

    private static bool Load(UnityModManager.ModEntry modEntry)
    {
        ModEntry = modEntry;

        Harmony? harmony = null;
        try
        {
            settings = Settings.Load<Settings>(modEntry);
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            openSimGaugeManager = new OpenSimGaugeManager();
            openSimGaugeManager.Start();

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

    private static void OnGUI(UnityModManager.ModEntry modEntry)
    {
        settings.Draw(modEntry);
    }

    private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
        settings.Save(modEntry);
    }

    private static bool Unload(UnityModManager.ModEntry entry)
    {
        openSimGaugeManager?.Stop();
        ModEntry.Logger.Log("DerailValleyDesktopGauge stopped");
        return true;
    }
}
