using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace DerailValleyDesktopGauge;

public class OpenSimGaugeManager {
    
    private static Process? _externalProc;
    private static string DepsPath = Path.Combine(Main.ModEntry.Path, "Dependencies");
    private static string ZipExtractPath = Path.Combine(Main.ModEntry.Path, "OpenSimGauge");

    private void Extract()
    {
        Main.ModEntry.Logger.Log("Extracting...");

        string zipPath = GetZipPath();

        if (string.IsNullOrEmpty(zipPath))
            throw new Exception($"No zip found in {DepsPath}");

        Main.ModEntry.Logger.Log($"Extract {zipPath} => {ZipExtractPath}");

        ZipFile.ExtractToDirectory(zipPath, ZipExtractPath);
        
        Main.ModEntry.Logger.Log("Extract done");
    }


    public static void CopyDirectory(string sourceDir, string destDir)
    {
        Main.ModEntry.Logger.Log($"Copy {sourceDir} => {destDir}");

        Directory.CreateDirectory(destDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var fileName = Path.GetFileName(file);
            var destPath = Path.Combine(destDir, fileName);

            File.Copy(file, destPath, overwrite: true);
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            var dirName = Path.GetFileName(dir);
            var destPath = Path.Combine(destDir, dirName);

            CopyDirectory(dir, destPath);
        }
    }

    private void ExtractAndEnhance()
    {
        Extract();

        Main.ModEntry.Logger.Log("Enhancing...");

        var enhanceDirPath = Path.Combine(DepsPath, "enhance");

        CopyDirectory(enhanceDirPath, ZipExtractPath);
        
        Main.ModEntry.Logger.Log("Enhance done");
    }

    private string GetZipPath()
    {
        Main.ModEntry.Logger.Log($"Searching for ZIP in: {DepsPath}");

        string? firstZip = Directory
            .EnumerateFiles(DepsPath, "*.zip", SearchOption.TopDirectoryOnly)
            .FirstOrDefault();
        return firstZip;
    }

    private bool IsExtracted()
    {
        return Directory.Exists(ZipExtractPath);
    }

    public void Start()
    {
        Main.ModEntry.Logger.Log("Starting OpenSimGauge...");

        if (!IsExtracted())
        {
            ExtractAndEnhance();
        }

        if (!IsExtracted())
            throw new Exception("Failed to extract");

        var psi = new ProcessStartInfo
        {
            FileName = Path.Combine(Main.ModEntry.Path, "OpenSimGauge/OpenSimGauge.exe"),
            UseShellExecute = false,
            CreateNoWindow = false,
        };

        _externalProc = Process.Start(psi);
    }

    public void Stop()
    {
        Main.ModEntry.Logger.Log("Stopping OpenSimGauge...");

        try
        {
            if (_externalProc != null && !_externalProc.HasExited)
            {        
                Main.ModEntry.Logger.Log("Closing main window...");

                _externalProc.CloseMainWindow();

                if (!_externalProc.WaitForExit(3000))
                {
                    Main.ModEntry.Logger.Log("Failed to close main window - killing process...");
                    _externalProc.Kill();
                    Main.ModEntry.Logger.Log("Done");
                }
                
                Main.ModEntry.Logger.Log("Closed");
            }
            else
            {
                Main.ModEntry.Logger.Log("Already closed");
            }
        }
        catch (Exception ex)
        {
            Main.ModEntry.Logger.LogException("Failed to close external process:", ex);
        }
    }
}