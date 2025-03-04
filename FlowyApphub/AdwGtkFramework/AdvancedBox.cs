using Gtk;

namespace BlazeHub.AdwGtkFramework;

public class AdvancedBox : Box
{
    public Box BoundingBox { get; } = Box.New(Orientation.Vertical, 0);

    public AdvancedBox(Orientation orientation, int spacing = 0)
    {
        BoundingBox.Append(this);
        SetOrientation(orientation);
        SetSpacing(spacing);
    }

    public Box ToBox() => BoundingBox;
}