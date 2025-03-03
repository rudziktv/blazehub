namespace FlowyApphub.Services.Flatpak;

public readonly record struct FlatpakAction(FlatpakActionType actionType, string appTarget)
{
    public FlatpakActionType ActionType => actionType;
    public string AppTarget => appTarget;
}