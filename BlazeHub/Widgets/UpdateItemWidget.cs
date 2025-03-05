using BlazeHub.Services.Flatpak;
using BlazeHub.Utils;
using Gtk;

namespace BlazeHub.Widgets;

public class UpdateItemWidget : Box
{
    private readonly Label _speed;
    private readonly ProgressBar _progressBar;
    private readonly Label _size;
    private readonly Label _downloadedSize;
    public FlatpakAction Action { get; }
    private readonly Button _cancelButton;
    
    private readonly Label _percentage;
    
    private readonly ListBox _packages;
    private readonly List<UpdateItemPackageWidget> _packageWidgets = [];
    
    public UpdateItemWidget(FlatpakAction action)
    {
        Action = action;
        _progressBar = ProgressBar.New();
        _size = Label.New(" / --.- MB");
        _downloadedSize = Label.New("0 bytes");
        _downloadedSize.SetHalign(Align.End);

        
        _speed = Label.New("0 bytes/s");
        _speed.SetHalign(Align.Start);
        _percentage = Label.New("0 %");
        _percentage.SetHalign(Align.End);
        
        var progressBoxTop = Box.New(Orientation.Horizontal, 16);
        progressBoxTop.SetHexpand(true);
        progressBoxTop.Append(_speed);
        progressBoxTop.Append(BoxUtils.Spacer());
        progressBoxTop.Append(_percentage);
        
        var progressBoxBottom = Box.New(Orientation.Horizontal, 0);
        progressBoxBottom.Append(_downloadedSize);
        progressBoxBottom.Append(_size);
        progressBoxBottom.SetHalign(Align.End);
        
        var progressBox = Box.New(Orientation.Vertical, 4);
        progressBox.Append(progressBoxTop);
        progressBox.Append(_progressBar);
        progressBox.Append(progressBoxBottom);

        var name = Label.New(action.AppTarget);
        name.AddCssClass("heading");
        _cancelButton = Button.NewFromIconName("window-close-symbolic");
        _cancelButton.AddCssClass("circular");
        _cancelButton.AddCssClass("osd");
        
        var header = Box.New(Orientation.Horizontal, 16);
        header.Append(name);
        header.Append(BoxUtils.Spacer());
        header.Append(_cancelButton);
        
        var packagesLabel = Label.New("Packages");
        packagesLabel.SetHalign(Align.Start);
        _packages = ListBox.New();
        _packages.AddCssClass("boxed-list");
        _packages.SetSelectionMode(SelectionMode.None);
        // _packages.SetMargins(12);
        
        SetOrientation(Orientation.Vertical);
        SetSpacing(4);
        this.SetMargins(12);
        
        Append(header);
        Append(progressBox);
        Append(packagesLabel);
        Append(_packages);

        // TODO - REPLACE FlatpakAction -> FlatpakProgress (maybe rename it a little)
        // UpdateItemWidget should use FlatpakProgress by default
        // instead of FlatpakAction, which doesn't provide things like packages, progress etc.
        // all required props should have proper events for updates during retrieving from CLI
        if (FlatpakQueue.Queue.Count > 0 && FlatpakQueue.Queue[0] == action)
        {
            OnFlatpakQueueNextTask(action);
            Console.WriteLine($"INITIALIZING UPDATEWIDGET {FlatpakQueue.CurrentProgress == null} {FlatpakQueue.CurrentProgress?.PackagesProgress.Count}");
            if (FlatpakQueue.CurrentProgress != null)
                foreach (var package in FlatpakQueue.CurrentProgress.PackagesProgress)
                {
                    OnFlatpakPackageAttached(package);
                }
        }
        else
        {
            FlatpakQueue.OnTaskStarted += OnFlatpakQueueNextTask;
        }
        FlatpakQueue.OnFlatpakPackageAttached += OnFlatpakPackageAttached;
    }

    private void OnFlatpakPackageAttached(FlatpakPackageProgress package)
    {
        var widget = new UpdateItemPackageWidget(package);
        _packageWidgets.Add(widget);
        _packages.Append(widget);
        
        if (FlatpakQueue.CurrentProgress != null)
            _size.SetText($" / {NetworkUnitUtils.
                BytesToString(FlatpakQueue.CurrentProgress.OverallSizeBytes)}");
    }

    private void OnFlatpakQueueNextTask(FlatpakAction obj)
    {
        if (obj.AppTarget == Action.AppTarget)
        {
            FlatpakQueue.OnTaskStarted -= OnFlatpakQueueNextTask;
            FlatpakQueue.OnTaskProgress += OnFlatpakQueueProgress;
            FlatpakQueue.OnTaskFinished += OnTaskFinished;
        }
        else
        {
            FlatpakQueue.OnTaskProgress -= OnFlatpakQueueProgress;
        }
    }

    private void OnTaskFinished(FlatpakAction obj)
    {
        FlatpakQueue.OnFlatpakPackageAttached -= OnFlatpakPackageAttached;
        FlatpakQueue.OnTaskProgress -= OnFlatpakQueueProgress;
        FlatpakQueue.OnTaskFinished -= OnTaskFinished;
    }

    private void OnFlatpakQueueProgress()
    {
        if (FlatpakQueue.CurrentProgress == null) return;
        var stage = FlatpakQueue.CurrentProgress.CurrentStage;
        
        GLib.Functions.IdleAdd(0, () =>
        {
            _downloadedSize.SetText($"{NetworkUnitUtils.
                BytesToString(FlatpakQueue.CurrentProgress.EstimatedDownloadedSize)}");
            _speed.SetText(FlatpakQueue.CurrentProgress.CurrentNetworkSpeed);
            _percentage.SetText($"{(Math.Clamp(FlatpakQueue.CurrentProgress.Progress, 0, 1) * 100):F0} %");
            _progressBar.SetFraction(Math.Clamp(FlatpakQueue.CurrentProgress.Progress, 0, 1));
            return false;
        });
        
        _packageWidgets[stage].PackageUpdateListener();
    }
}