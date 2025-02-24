using Adw;
using FlowyApphub.Models.Flathub;
using FlowyApphub.Models.FlathubApp;
using FlowyApphub.Services.Data;
using FlowyApphub.Services.DescriptionParser;
using FlowyApphub.Services.Flathub;
using FlowyApphub.Services.Images;
using FlowyApphub.Services.Requests;
using FlowyApphub.Utils;
using FlowyApphub.Widgets;
using Gdk;
using Gtk;
using WebKit;

namespace FlowyApphub.Views;

public class AppSiteView : Box
{
    private FlathubAppModel? _appModel;
    
    public AppSiteView()
    {
        // AddCssClass("view");
        SetOrientation(Orientation.Vertical);
        GetAppModel("org.gnome.World.Iotas");
        // GetAppModel("net.eudic.dict");
    }

    private async void GetAppModel(string appId)
    {
        try
        {
            SetSpinner();
            var appModel = await FlathubAPI.GetAppDetails(appId);

            var appSummary = await FlathubAPI.GetAppSummary(appId);
            if (appModel == null || appSummary == null) return;
            var icon = await ImageRequests.GetImageFromUrl(appModel.Icon);
            var images = await ScreenshotsRequests.GetScreenshots(appModel.Screenshots);
            SetAppSiteView(appModel, appSummary, images, icon);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void SetSpinner()
    {
        this.Clear();
        var spinner = Adw.Spinner.New();
        spinner.SetSizeRequest(48, 48);
        spinner.SetMargins(10);
        Append(spinner);
    }

    private void SetAppSiteView(FlathubAppModel appModel, FlathubAppSummary summary, AppScreenshot[] images, Image? icon = null)
    {
        this.Clear();
        _appModel = appModel;

        var scrollView = ScrolledWindow.New();
        scrollView.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        
        var topClamp = Clamp.New();
        topClamp.SetHexpand(true);
        topClamp.SetOrientation(Orientation.Horizontal);
        topClamp.SetMaximumSize(650);
        topClamp.SetHalign(Align.Fill);

        var topBox = Box.New(Orientation.Horizontal, 0);
        topBox.SetHexpand(true);

        var infoBox = Box.New(Orientation.Vertical, 4);
        infoBox.SetHexpand(false);
        infoBox.Align(Align.Start, Align.Center);
        if (icon != null)
        {
            icon.SetSizeRequest(128, 128);
            topBox.Append(icon);
        }
        var title = Label.New(appModel.Name);
        title.AddCssClass("title-1");
        title.SetHalign(Align.Start);
        var author = Label.New(appModel.DeveloperName);
        author.SetHalign(Align.Start);
        infoBox.Append(title);
        infoBox.Append(author);
        
        var spacerTopBox = Box.New(Orientation.Horizontal, 0);
        spacerTopBox.SetHexpand(true);

        var installButton = ButtonUtils.Create(
            "Get", "folder-download-symbolic", "suggested-action");
        installButton.SetValign(Align.Center);
        
        topBox.Append(infoBox);
        topBox.Append(spacerTopBox);
        topBox.Append(installButton);
        
        
        topClamp.SetMargins(10);
        topClamp.SetChild(topBox);

        
        var imagePreviewReveal = Revealer.New();
        imagePreviewReveal.SetCanTarget(false);

        var imagePreviewOverlay = Overlay.New();
        imagePreviewOverlay.SetChild(scrollView);
        imagePreviewOverlay.AddOverlay(imagePreviewReveal);
        imagePreviewReveal.SetTransitionType(RevealerTransitionType.Crossfade);
        
        
        var bottomBox = Box.New(Orientation.Vertical, 8);
        bottomBox.SetHexpand(true);
        bottomBox.SetMarginBottom(10);
        
        var descWidget = DescriptionParser.ParseDescription(appModel.Description);
        descWidget.SetHexpand(true);
        
        var summaryText = Label.New(appModel.Summary);
        summaryText.SetXalign(0);
        summaryText.SetWrap(true);
        summaryText.AddCssClass("title-2");
        summaryText.SetMargins(0, 12);

        var expander = Expander.New("");
        expander.SetLabelWidget(summaryText);
        expander.SetChild(descWidget);

        var paramsBox = FlowBox.New();
        paramsBox.SetColumnSpacing(8);
        paramsBox.SetHexpand(true);
        paramsBox.SetOrientation(Orientation.Horizontal);
        paramsBox.SetHomogeneous(true);
        paramsBox.SetMinChildrenPerLine(2);
        paramsBox.SetSelectionMode(SelectionMode.None);
        
        var safetyBox = Box.New(Orientation.Vertical, 0);
        safetyBox.AddCssClass("card");
        safetyBox.SetHexpand(true);
        safetyBox.SetMargins(0);
        safetyBox.Append(Label.New(summary.Metadata.Permissions.Shared[0]));

        var licenseBox = Box.New(Orientation.Vertical, 0);
        licenseBox.AddCssClass("card");
        // licenseBox.SetHexpand(true);
        licenseBox.Append(Label.New(appModel.IsFreeLicense ? "Open-Source" : "Proprietary"));

        
        
        
        paramsBox.Append(StoreAppCards.GetSafetyCard(summary.Metadata.Permissions));
        paramsBox.Append(StoreAppCards.GetLicenseCard(appModel));
        
        
        // summary
        // var boxList = ListBox.New();
        // boxList.AddCssClass("boxed-list");
        // boxList.SelectionMode = SelectionMode.None;
        // boxList.Append(Label.New("Some info"));
        // boxList.Append(Label.New("Some info"));
        // boxList.Append(Label.New("Some info"));
        
        bottomBox.Append(expander);
        bottomBox.Append(new AppChangelog(appModel.Releases));
        bottomBox.Append(paramsBox);
        
        var bottomClamp = Clamp.New();
        bottomClamp.SetChild(bottomBox);
        bottomClamp.SetHexpand(true);
        bottomClamp.SetMaximumSize(650);


        
        
        var viewBox = Box.New(Orientation.Vertical, 0);
        viewBox.SetHexpand(true);
        viewBox.SetVexpand(true);
        viewBox.Append(topClamp);
        viewBox.Append(Separator.New(Orientation.Horizontal));
        viewBox.Append(new AppCarousel(images, screenshot =>
        {
            var previewBoxCtr = GestureClick.New();
            previewBoxCtr.OnReleased += (sender, args) => CloseImagePreview();
            
            var previewBox = Box.New(Orientation.Vertical, 0);
            previewBox.SetHexpand(true);
            previewBox.SetVexpand(true);
            previewBox.AddController(previewBoxCtr);

            var cssProvider = CssProvider.New();
            cssProvider.LoadFromString(@"
                .preview-background {
                    background-color: rgba(0, 0, 0, 0.5);
                }
            ");
            // GetStyleContext().AddProvider(cssProvider, 999);
            StyleContext.AddProviderForDisplay(GetDisplay(), cssProvider, 800);
            previewBox.AddCssClass("preview-background");
            
            var closeBtn = ButtonUtils.Create(iconName: "window-close-symbolic", cssClasses: ["circular"]);
            closeBtn.Align(Align.End, Align.Start);
            closeBtn.SetMargins(12);
            closeBtn.OnClicked += (sender, args) => CloseImagePreview();
            closeBtn.AddCssClass("background");

            var openImageViewerBtn = ButtonUtils.Create(iconName: "image-x-generic-symbolic", cssClasses: "circular");
            openImageViewerBtn.Align(Align.Start, Align.Start);
            openImageViewerBtn.SetMargins(12);
            openImageViewerBtn.OnClicked += (sender, args) => OpenImageViewer();
            openImageViewerBtn.AddCssClass("background");

            
            var imgPreview = Image.NewFromPixbuf(screenshot.Pixbuf);
            imgPreview.SetHexpand(true);
            imgPreview.SetVexpand(true);
            imgPreview.SetPixelSize(4);
            
            previewBox.Append(imgPreview);
            
            var previewOverlay = Overlay.New();
            previewOverlay.SetChild(previewBox);
            previewOverlay.AddOverlay(closeBtn);
            previewOverlay.AddOverlay(openImageViewerBtn);

            imagePreviewReveal.SetChild(previewOverlay);
            imagePreviewReveal.SetRevealChild(true);
            imagePreviewReveal.SetCanTarget(true);

            void CloseImagePreview()
            {
                imagePreviewReveal.SetRevealChild(false);
                imagePreviewReveal.SetCanTarget(false);
            }

            void OpenImageViewer()
            {
                // try
                // {
                //     throw new Exception("Testing error dialog.");
                // }
                // catch (Exception e)
                // {
                //     ErrorDialogService.ShowErrorDialog(e);
                //     Console.WriteLine(e);
                // }
                // return;

                
                var done = ImagesService.SaveImageFromUrl(screenshot.Pixbuf, screenshot.Url, AppData.TempDataFolder, out var path);
                Console.WriteLine($"Saving to {path}, {(done ? "success" : "error")}");

                if (done)
                    ImagesService.OpenImageViewer(path);
            }
        }));
        viewBox.Append(Separator.New(Orientation.Horizontal));
        viewBox.Append(bottomClamp);
        

        
        scrollView.SetChild(viewBox);
        scrollView.SetVexpand(true);
        
        Append(imagePreviewOverlay);
    }
}