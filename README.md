# **This mod depends on https://github.com/imagitama/derail-valley-websocket**

# Derail Valley Desktop Gauge mod

A mod for the game Derail Valley that uses [OpenSimGauge](https://github.com/imagitama/open-sim-gauge) to render gauges on your desktop.

Template from https://github.com/derail-valley-modding/template-umm

## Customizing panels and gauges

Either edit `OpenSimGauge/client.json` manually or use the [OpenSimGauge](https://github.com/imagitama/open-sim-gauge) editor:

1. Download and extract OpenSimGauge
2. Copy your `OpenSimGauge/client.json` into the same folder as `editor.exe`
3. Launch `editor.exe`, make changes, save it back
4. Copy the JSON file back over

## Install

Download the zip and use Unity Mod Manager to install it.

## Development

Created in VSCode (with C# and C# Dev Kit extensions) and MSBuild.

1. Clone repo
2. Download the latest version (do NOT extract) of OpenSimGauge into `DerailValleyDesktopGauge/Dependencies` eg. https://github.com/imagitama/open-sim-gauge/releases/download/0.0.13/OpenSimGauge-0.0.13-win-x64.zip
3. Build the data source and copy:

   dotnet build ./data-source
   cp ./data-source/bin/Debug/net9.0/DerailValleyDataSource.dll ./DerailValleyDesktopGauge/Dependencies

## Publishing

1. Run `.\package.ps1`
