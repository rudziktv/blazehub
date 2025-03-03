## Build process
### Preparing
First, build your nuget-sources, with [this](https://github.com/flatpak/flatpak-builder-tools/tree/master/dotnet])
```bash
python3 /path/to/tool/flatpak-dotnet-generator.py nuget-sources.json ./FlowyApphub/FlowyApphub.csproj --runtime linux-x64 --dotnet-args --no-cache --verbosity detailed
```
Next create your manifest file
```json
{
    "app-id": "io.github.flamedev.blazeapphub",
    "runtime": "org.gnome.Platform",
    "runtime-version": "47",
    "sdk": "org.gnome.Sdk",
    "sdk-extensions": [
        "org.freedesktop.Sdk.Extension.dotnet8"
    ],
    "command": "/app/blazeapphub",
    "finish-args": [
        "--socket=fallback-x11",
        "--socket=wayland",
        "--device=dri",
        ...permissions
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
            "name": "blazeapphub",
            "buildsystem": "simple",
            "build-commands": [
                ". /usr/lib/sdk/dotnet8/enable.sh",
                "dotnet publish -c Release --self-contained --source nuget-sources FlowyApphub/FlowyApphub.csproj -o /app"
            ],
            "sources": [
                {
                    "type": "dir",
                    "path": ".."
                },
                "../FlowyApphub/nuget-sources.json"
            ]
        }
    ]
}
```


### Building
At the root of the project, run command:
```bash
flatpak-builder --force-clean --user --repo=repo --install builddir flatpak/io.github.flamedev.blazeapphub.json
```
