## Build process
### Preparing
First, build your nuget-sources, with [flatpak-builder-tools/dotnet](https://github.com/flatpak/flatpak-builder-tools/tree/master/dotnet])
```bash
python3 /path/to/tool/flatpak-dotnet-generator.py nuget-sources.json ./BlazeHub/BlazeHub.csproj --runtime linux-x64 --dotnet-args --no-cache --verbosity detailed
```
Next create your manifest file
```json
{
    "app-id": "io.github.flamedev.blazehub",
    "runtime": "org.gnome.Platform",
    "runtime-version": "47",
    "sdk": "org.gnome.Sdk",
    "sdk-extensions": [
        "org.freedesktop.Sdk.Extension.dotnet8"
    ],
    "command": "BlazeHub",
    "finish-args": [
        "--socket=fallback-x11",
        "--socket=wayland",
        "--device=dri",
        "...permissions"
    ],
    "build-options": {
        "append-path": "/usr/lib/sdk/dotnet8/bin",
        "append-ld-library-path": "/usr/lib/sdk/dotnet8/lib",
        "append-pkg-config-path": "/usr/lib/sdk/dotnet8/lib/pkgconfig",
        "env": {
            "DOTNET_CLI_TELEMETRY_OPTOUT": "1",
            "DOTNET_NOLOGO": "1",
            "DOTNET_SKIP_FIRST_TIME_EXPERIENCE": "1"
        }
    },
    "modules": [
        {
            "name": "BlazeHub",
            "buildsystem": "simple",
            "build-commands": [
              ". /usr/lib/sdk/dotnet8/enable.sh",
              "dotnet publish -c Release --self-contained --source nuget-sources BlazeHub/BlazeHub.csproj -o ${FLATPAK_DEST}/bin",
              "mkdir ${FLATPAK_DEST}/share",
              "cp -r ${FLATPAK_DEST}/bin/share/* ${FLATPAK_DEST}/share"
            ],
            "sources": [
                {
                    "type": "dir",
                    "path": ".."
                },
              "../BlazeHub/nuget-sources.json"
            ]
        }
    ]
}
```


### Building
At the root of the project, run command:
```bash
flatpak-builder build-dir --user --force-clean --install --repo=repo flatpak/io.github.flamedev.blazehub.json
```

If you want to bundle it into `.flatpak`
```bash
flatpak build-bundle repo blazehub.flatpak io.github.flamedev.blazehub --runtime-repo=https://flathub.org/repo/flathub.flatpakrepo
```