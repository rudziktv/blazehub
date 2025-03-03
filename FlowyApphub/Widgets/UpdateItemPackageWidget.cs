using Gtk;
using FlowyApphub.Utils;
using FlowyApphub.Services.Flatpak;

namespace FlowyApphub.Widgets;

public class UpdateItemPackageWidget : Box
{
    private readonly FlatpakPackageProgress _package;
    private readonly ProgressBar _progressBar;
    private readonly Label _downloadedSize;
    private readonly Label _percentage;
    
    public UpdateItemPackageWidget(FlatpakPackageProgress package)
    {
        var id = Label.New(package.PackageID);
        id.SetHalign(Align.Start);
        _progressBar = ProgressBar.New();
        
        _downloadedSize = Label.New("0 bytes");
        _downloadedSize.SetHalign(Align.End);
        
        var size = Label.New($" / {NetworkUnitUtils.BytesToString(package.SizeBytes)}");
        size.SetHalign(Align.End);
        
        _percentage = Label.New("0 %");
        _percentage.SetHalign(Align.End);

        var topBox = Box.New(Orientation.Horizontal, 16);
        topBox.Append(id);
        topBox.Append(BoxUtils.Spacer());
        topBox.Append(_percentage);
        topBox.SetHexpand(true);

        var bottomBox = Box.New(Orientation.Horizontal, 0);
        bottomBox.Append(_downloadedSize);
        bottomBox.Append(size);
        bottomBox.SetHalign(Align.End);
        
        this.SetMargins(12);
        SetOrientation(Orientation.Vertical);
        SetSpacing(4);
        Append(topBox);
        Append(_progressBar);
        Append(bottomBox);
        
        _package = package;
    }

    public void PackageUpdateListener()
    {
        GLib.Functions.IdleAdd(0, () =>
        {
            _percentage.SetText($"{_package.Progress * 100:F0} %");
            _downloadedSize.SetLabel(NetworkUnitUtils.BytesToString(_package.EstimatedDownloadedSize));
            _progressBar.SetFraction(Math.Clamp(_package.Progress, 0, 1));
            return false;
        });
    }
}