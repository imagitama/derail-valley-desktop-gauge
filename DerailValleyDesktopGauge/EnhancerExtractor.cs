using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using UnityModManagerNet;

namespace DerailValleyDesktopGauge;

public static class EnhancerExtractor
{
    public static UnityModManager.ModEntry.ModLogger Logger => Main.ModEntry.Logger;
    public static string DepsPath = Path.Combine(Main.ModEntry.Path, "Dependencies");
    public static string ZipExtractPath = Path.Combine(Main.ModEntry.Path, "OpenSimGauge");
    public static string GaugesDirName = "gauges";
    public static string EnhanceSourceDirPath = Path.Combine(DepsPath, "enhance");
    public static string GaugesSourceDirPath = Path.Combine(EnhanceSourceDirPath, GaugesDirName);
    public static string ClientConfigFileName = "client.json";

    public static void Extract()
    {
        Logger.Log("Extracting...");

        string zipPath = GetZipPath();

        if (string.IsNullOrEmpty(zipPath))
            throw new Exception($"No zip found in {DepsPath}");

        Logger.Log($"Extract {zipPath} => {ZipExtractPath}");

        ZipFile.ExtractToDirectory(zipPath, ZipExtractPath);

        Logger.Log("Extract done");
    }

    public static void CopyFile(string sourceFile, string destFile)
    {
        Logger.Log($"  Copy {sourceFile} => {destFile}");
        File.Copy(sourceFile, destFile);
    }

    public static void MoveFile(string sourceFile, string destFile)
    {
        Logger.Log($"  Move {sourceFile} => {destFile}");
        File.Move(sourceFile, destFile);
    }

    public static void CopyDirectory(string sourceDir, string destDir)
    {
        Logger.Log($"  Copy {sourceDir} => {destDir}");

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

    public static void ExtractAndEnhance()
    {
        Extract();

        Logger.Log("Enhancing...");

        CopyDirectory(EnhanceSourceDirPath, ZipExtractPath);

        Logger.Log("Enhance done");
    }

    /// <summary>
    /// If we update this mod and change a panel around, we want to introduce this change to the user *safely*.
    /// Safely = make a backup!
    /// </summary>
    public static bool NeedsMoreEnhancement()
    {
        var packagedClientConfigDate = File.GetLastWriteTime(Path.Combine(EnhanceSourceDirPath, ClientConfigFileName));
        var userClientConfigDate = File.GetLastWriteTime(Path.Combine(ZipExtractPath, ClientConfigFileName));
        var result = packagedClientConfigDate > userClientConfigDate;

        Logger.Log($"Does it need more enhancement? Packaged={packagedClientConfigDate} vs User={userClientConfigDate} = {result}");

        return result;
    }

    public static void EnhanceSafely()
    {
        Logger.Log("Enhancing safely...");

        var filePath = Path.Combine(ZipExtractPath, ClientConfigFileName);

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var newFileName = $"{Path.GetFileNameWithoutExtension(ClientConfigFileName)}_{timestamp}{Path.GetExtension(ClientConfigFileName)}";
        var newFilePath = Path.Combine(ZipExtractPath, newFileName);

        MoveFile(filePath, newFilePath);

        Logger.Log($"Kept backup: {newFilePath}");

        Logger.Log("Replacing config...");

        CopyFile(Path.Combine(EnhanceSourceDirPath, ClientConfigFileName), Path.Combine(ZipExtractPath, ClientConfigFileName));

        var gaugesDirPath = Path.Combine(ZipExtractPath, GaugesDirName);
        var newGaugesDirPath = gaugesDirPath + $"_{timestamp}";

        CopyDirectory(gaugesDirPath, newGaugesDirPath);

        Logger.Log($"Kept backup: {newGaugesDirPath}");

        CopyDirectory(GaugesSourceDirPath, Path.Combine(ZipExtractPath, GaugesDirName));

        Logger.Log("Safe enhancement done");
    }

    public static string GetZipPath()
    {
        Logger.Log($"Searching for ZIP in: {DepsPath}");

        string? firstZip = Directory
            .EnumerateFiles(DepsPath, "*.zip", SearchOption.TopDirectoryOnly)
            .FirstOrDefault();

        return firstZip;
    }

    public static bool IsExtracted()
    {
        return Directory.Exists(ZipExtractPath);
    }
}