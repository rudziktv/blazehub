using FlowyApphub.Utils;
using Gtk;
using HtmlAgilityPack;
using WrapMode = Pango.WrapMode;

namespace FlowyApphub.Services.DescriptionParser;

public static class DescriptionParser
{
    public static Widget ParseDescription(string description, string emptyReplace = "<p>Empty</p>")
    {
        if (string.IsNullOrEmpty(description))
            description = emptyReplace;
        
        var doc = new HtmlDocument();
        doc.LoadHtml(description);
        return ParseNodes(doc);
    }

    public static Widget ParseNodes(HtmlDocument doc)
    {
        var box = Box.New(Orientation.Vertical, 0);
        
        foreach (var node in doc.DocumentNode.ChildNodes)
        {
            if (node.Name == "p")
            {
                var paragraph = ParseParagraph(node);
                box.Append(paragraph);
            }
            else if (node.Name == "ul")
            {
                var list = ParseUnorderedList(node.ChildNodes);
                box.Append(list);
            }
        }

        return box;
    }

    // public static Widget ParseSingleNode()
    // {
    //     
    // }

    public static Widget ParseParagraph(HtmlNode node)
    {
        var label = Label.New(node.InnerText);

        label.SetMargins(0, 6);
        
        label.SetXalign(0);
        label.SetWrap(true);
        label.SetWrapMode(WrapMode.Word);
        
        return label;
    }

    public static Widget ParseUnorderedList(HtmlNodeCollection listItems)
    {
        var list = Box.New(Orientation.Vertical, 0);
        list.SetMargins(6, 4);
        list.SetMarginEnd(0);
        list.SetHexpand(true);

        foreach (var item in listItems)
        {
            if (item.Name == "li")
            {
                var label = Label.New("\u2022 " + item.InnerText);
                label.SetMargins(0, 2);

                label.SetXalign(0);
                label.SetWrap(true);
                label.SetWrapMode(WrapMode.Word);
                
                list.Append(label);
            }
        }
        
        return list;
    }
}