using FlowyApphub.Utils;
using Gtk;

namespace FlowyApphub.Widgets;

public class CardWithBadge : Box
{
    private readonly Box _badgesBox;
    private readonly Box _labelBox;

    public CardWithBadge()
    {
        var contentBox = Box.New(Orientation.Vertical, 8);
        contentBox.SetVexpand(true);
        contentBox.SetValign(Align.Center);
        contentBox.SetMargins(12);
        
        _badgesBox = New(Orientation.Horizontal, 8);
        _badgesBox.SetHalign(Align.Center);
        _labelBox = New(Orientation.Vertical, 8);
        
        AddCssClass("card");
        SetOrientation(Orientation.Vertical);
        contentBox.Append(_badgesBox);
        contentBox.Append(_labelBox);
        Append(contentBox);
    }
    
    public void AppendBadges(Widget widget)
        => _badgesBox.Append(widget);
    public void AppendLabel(Widget widget)
        => _labelBox.Append(widget);
}