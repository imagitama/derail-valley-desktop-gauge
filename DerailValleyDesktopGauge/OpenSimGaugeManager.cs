using System;
using System.IO;
using System.Diagnostics;
using UnityModManagerNet;
using System.Linq;

namespace DerailValleyDesktopGauge;

public class OpenSimGaugeManager
{
    private static UnityModManager.ModEntry.ModLogger Logger => Main.ModEntry.Logger;
    private static Process? _externalProc;
    public static string ExeName = "OpenSimGauge";

    public bool GetIsRunning()
    {
        if (_externalProc != null && !_externalProc.HasExited)
            return true;

        var detectedProcess = Process.GetProcessesByName(ExeName).FirstOrDefault();

        return detectedProcess != null;
    }

    public void Start()
    {
        Logger.Log("Starting OpenSimGauge...");

        var isRunning = GetIsRunning();

        Logger.Log($"Is already running: {isRunning}");

        if (isRunning)
        {
            Logger.Log("Cannot start: already running");
            return;
        }

        if (!EnhancerExtractor.IsExtracted())
            EnhancerExtractor.ExtractAndEnhance();

        if (!EnhancerExtractor.IsExtracted())
            throw new Exception("Failed to extract");

        if (EnhancerExtractor.NeedsMoreEnhancement())
            EnhancerExtractor.EnhanceSafely();

        var psi = new ProcessStartInfo
        {
            FileName = Path.Combine(Main.ModEntry.Path, $"OpenSimGauge/{ExeName}.exe"),
            UseShellExecute = false,
            CreateNoWindow = false,
        };

        _externalProc = Process.Start(psi);
    }

    public void Stop()
    {
        Logger.Log("Stopping OpenSimGauge...");

        try
        {
            if (_externalProc != null && !_externalProc.HasExited)
            {
                Logger.Log("Closing main window...");

                _externalProc.CloseMainWindow();

                if (!_externalProc.WaitForExit(3000))
                {
                    Logger.Log("Failed to close main window - killing process...");
                    _externalProc.Kill();
                    Logger.Log("Done");
                }

                Logger.Log("Closed");
            }
            else
            {
                Logger.Log("Already closed");
            }
        }
        catch (Exception ex)
        {
            Logger.LogException("Failed to close external process:", ex);
        }
    }

    public void Restart()
    {
        Logger.Log("Restarting OpenSimGauge...");

        Stop();
        Start();
    }
}