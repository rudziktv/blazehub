using System.Globalization;
using Adw;
using FlowyApphub.Models.Flathub;
using FlowyApphub.Services.DescriptionParser;
using FlowyApphub.Utils;
using Gtk;
using Dialog = Adw.Dialog;
using HeaderBar = Adw.HeaderBar;

namespace FlowyApphub.Widgets;

public class AppChangelog : Box
{
    public AppChangelog(List<FlathubAppReleaseModel> changelog)
    {
        SetOrientation(Orientation.Vertical);
        
        if (changelog.Count == 0) return;

        var cssProvider = CssProvider.New();
        cssProvider.LoadFromString(@"
            .expander {
                border-radius: 5px;
            }
        ");
        GetStyleContext().AddProvider(cssProvider, 99999);
        
        var lastRelease = changelog[0];
        var dateTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToUInt32(lastRelease.Timestamp)).UtcDateTime;
        var onlyDate = DateOnly.FromDateTime(dateTime);

        var ex = Expander.New("WTF");

        var boxedList = ListBox.New();
        boxedList.SetSelectionMode(SelectionMode.None);
        boxedList.AddCssClass("boxed-list");
        boxedList.SetMargins(3, 0);
        
        var changesExpander = ExpanderRow.New();
        changesExpander.SetOverflow(Overflow.Hidden);
        changesExpander.SetTitle($"Changes in version {lastRelease.Version}");
        changesExpander.SetSubtitle($"{onlyDate.ToString(CultureInfo.CurrentCulture)}, {DateUtils.TimeAgo(dateTime)}");
        changesExpander.Selectable = false;
        
        boxedList.Append(changesExpander);
        
        // show only last release
        var rowBox = Box.New(Orientation.Vertical, 8);
        rowBox.AddCssClass("body");
        rowBox.SetMargins(8);
        var rowTitle = Label.New(lastRelease.Version);
        rowTitle.SetHalign(Align.Start);
        rowTitle.AddCssClass("title-3");
        rowBox.Append(rowTitle);
        rowBox.Append(DescriptionParser.ParseDescription(lastRelease.Description, "<ul><li>No changelog provided</li></ul>"));
        changesExpander.AddRow(rowBox);

        var seeChangelogBox = Box.New(Orientation.Vertical, 0);
        var seeChangelogLabel = Label.New("See changelog");
        seeChangelogLabel.AddCssClass("heading");
        seeChangelogLabel.SetMargins(8);
        seeChangelogLabel.Align(Align.Center);
        seeChangelogBox.Append(seeChangelogLabel);
        changesExpander.AddRow(seeChangelogBox);

        var seeChangelogCtr = GestureClick.New();
        seeChangelogCtr.OnReleased += (sender, args) => OpenFullAppChangelog(changelog);
        seeChangelogBox.AddController(seeChangelogCtr);
        
        Append(boxedList);
    }

    private void OpenFullAppChangelog(List<FlathubAppReleaseModel> changelog)
    {
        var dialog = Dialog.New();
        dialog.SetPresentationMode(DialogPresentationMode.Floating);
        dialog.SetContentWidth(600);
        dialog.SetContentHeight(600);
        dialog.SetTitle("Full App Changelog");
        // dialog.SetFollowsContentSize(true);
        // dialog.SetCanClose(true);
        
        var masterBox = Box.New(Orientation.Vertical, 0);
        
        var header = HeaderBar.New();
        
        var toolbar = ToolbarView.New();
        toolbar.SetRevealTopBars(true);
        toolbar.AddTopBar(header);
        toolbar.SetTopBarStyle(ToolbarStyle.Flat);
        // header.AddCssClass("background");
        
        
        // dialog.Title = "Full App Changelog";
        
        var dialogScroll = ScrolledWindow.New();
        dialogScroll.SetVexpand(true);
        dialogScroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        var dialogList = ListBox.New();
        dialogList.SetSelectionMode(SelectionMode.None);
        dialogList.AddCssClass("boxed-list");
        dialogList.SetMargins(32);
        dialogList.SetVexpand(false);

        foreach (var release in changelog)
        {
            var releaseBox = Box.New(Orientation.Vertical, 8);
            releaseBox.SetMargins(8);

            var versionBox = Box.New(Orientation.Horizontal, 8);
            versionBox.SetHexpand(true);
            var title = Label.New(release.Version);
            title.SetHalign(Align.Start);
            title.AddCssClass("heading");
            var date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToUInt32(release.Timestamp)).UtcDateTime;
            var ago = Label.New(DateUtils.TimeAgo(date));
            ago.AddCssClass("dim-label");
            var spacer = Box.New(Orientation.Horizontal, 0);
            spacer.SetHexpand(true);
            ago.SetHalign(Align.End);
            versionBox.Append(title);
            versionBox.Append(spacer);
            versionBox.Append(ago);
            
            var description = DescriptionParser.ParseDescription(release.Description, "<ul><li>No changelog provided</li></ul>");
            
            releaseBox.Append(versionBox);
            releaseBox.Append(description);
            dialogList.Append(releaseBox);
        }
        
        // masterBox.Append(toolbar);
        // masterBox.Append(dialogScroll);
        // toolbar.SetContent(dialogList);
        
        masterBox.Append(dialogList);
        masterBox.SetVexpand(false);
        dialogScroll.SetChild(masterBox);
        // masterBox.SetVexpand(true);
        
        toolbar.SetContent(dialogScroll);
        // toolbar.SetVexpand(true);
        dialog.SetChild(toolbar);
        dialog.Present(this);
    }
}