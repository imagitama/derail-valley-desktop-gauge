using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityModManagerNet;

namespace DerailValleyDesktopGauge;

public class Settings : UnityModManager.ModSettings, IDrawable
{
    // public int Port = 9450;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
        Save(this, modEntry);
    }

    public void Draw(UnityModManager.ModEntry modEntry)
    {
        GUILayout.Label("Want to resize or reposition your panel or gauges? Edit the config file and re-position as needed.");
        GUILayout.Label("Want your gauges on a different screen? Edit the config file and set   \"screen\": 1   (or whatever screen number you want)");
        GUILayout.Label("Is the websocket running on a different port? Edit server.json in the OpenSimGauge folder");

        GUILayout.Label("");
        GUILayout.Label("OpenSimGauge:");

        if (GUILayout.Button("Open Folder"))
            OpenOpenSimGaugeFolder();
        if (GUILayout.Button("Edit Config"))
            OpenConfigForEditing();
        if (GUILayout.Button("Edit Default Config"))
            OpenDefaultConfigForEditing();

        GUILayout.Label($"Is running: {Main.openSimGaugeManager?.GetIsRunning()}");

        if (GUILayout.Button("Start"))
            Main.openSimGaugeManager?.Start();
        if (GUILayout.Button("Stop"))
            Main.openSimGaugeManager?.Stop();
        if (GUILayout.Button("Restart"))
            Main.openSimGaugeManager?.Restart();
    }

    void OpenOpenSimGaugeFolder()
    {
        var path = EnhancerExtractor.ZipExtractPath;
        if (Directory.Exists(path))
            Process.Start("explorer.exe", path.Replace("/", "\\"));
    }

    void OpenConfigForEditing()
    {
        var path = Path.Combine(EnhancerExtractor.ZipExtractPath, EnhancerExtractor.ClientConfigFileName);
        Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
    }

    void OpenDefaultConfigForEditing()
    {
        var path = Path.Combine(EnhancerExtractor.EnhanceSourceDirPath, EnhancerExtractor.ClientConfigFileName);
        Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
    }

    public void OnChange()
    {
    }
}
