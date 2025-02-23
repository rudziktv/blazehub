using Adw;
using Gtk;

namespace FlowyApphub.Utils;

public static class ButtonUtils
{
    public static Button Create(string label = "", string iconName = "", params string[] cssClasses)
    {
        var button = Button.New();
        var content = ButtonContent.New();
        if (!string.IsNullOrEmpty(label))
            content.SetLabel(label);
        if (!string.IsNullOrEmpty(iconName))
            content.SetIconName(iconName);
        foreach (var cssClass in cssClasses)
        {
            button.AddCssClass(cssClass);
        }
        button.SetChild(content);
        return button;
    }
}