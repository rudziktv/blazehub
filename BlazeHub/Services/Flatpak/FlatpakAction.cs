namespace BlazeHub.Services.Flatpak;

public class FlatpakAction(FlatpakActionType actionType, string appTarget, bool sysAction = true)
{
    public FlatpakActionType ActionType => actionType;
    public string AppTarget => appTarget;
    public bool SysAction => sysAction;
}