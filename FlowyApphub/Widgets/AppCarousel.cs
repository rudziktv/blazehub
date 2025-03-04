using Adw;
using BlazeHub.Models.Flathub;
using BlazeHub.Utils;
using Gtk;

namespace BlazeHub.Widgets;

public class AppCarousel : Box
{
    public delegate void SetImagePreview(AppScreenshot screenshot);
    
    public AppCarousel(AppScreenshot[] images, SetImagePreview setImagePreview)
    {
        SetOrientation(Orientation.Vertical);
        SetVexpand(false);
        
        var carousel = Carousel.New();
        carousel.SetAllowScrollWheel(false);
        var carouselIndicator = CarouselIndicatorDots.New();
        carouselIndicator.SetOrientation(Orientation.Horizontal);
        carouselIndicator.SetCarousel(carousel);

        var nextBtn = Button.New();
        var nCon = ButtonContent.New();
        nCon.SetIconName("go-next-symbolic");
        // nextBtn.Label = ">";
        nextBtn.SetChild(nCon);
        nextBtn.SetMargins(10);
        nextBtn.AddCssClass("circular");
        nextBtn.AddCssClass("osd");
        
        var nextReveal = Revealer.New();
        nextReveal.SetHalign(Align.End);
        nextReveal.SetValign(Align.Center);
        nextReveal.SetChild(nextBtn);
        nextReveal.TransitionType = RevealerTransitionType.SwingRight;
        nextReveal.SetRevealChild(images.Length > 1);
        
        
        var prevBtn = Button.New();
        var pCon = ButtonContent.New();
        pCon.SetIconName("go-previous-symbolic");
        prevBtn.SetChild(pCon);
        // prevBtn.SetHalign(Align.Start);
        // prevBtn.SetValign(Align.Center);
        prevBtn.SetMargins(10);
        prevBtn.AddCssClass("circular");
        prevBtn.AddCssClass("osd");

        var prevReveal = Revealer.New();
        prevReveal.SetHalign(Align.Start);
        prevReveal.SetValign(Align.Center);
        prevReveal.SetChild(prevBtn);
        prevReveal.TransitionType = RevealerTransitionType.SwingLeft;
        prevReveal.SetRevealChild(false);

        nextBtn.OnClicked += (sender, args) =>
        {
            var pos = (uint)Math.Abs(Math.Round(carousel.GetPosition()));
            
            if (pos + 1 >= images.Length) return;
            carousel.ScrollTo(carousel.GetNthPage(pos + 1), true);
        };

        prevBtn.OnClicked += (sender, args) =>
        {
            var pos = (uint)Math.Abs(Math.Round(carousel.GetPosition()));
            
            if ((int)pos - 1 < 0) return;
            carousel.ScrollTo(carousel.GetNthPage(pos - 1), true);
        };

        carousel.OnPageChanged += (sender, args) =>
        {
            prevReveal.SetRevealChild(args.Index != 0);
            nextReveal.SetRevealChild(args.Index < images.Length - 1);
        };
        
        // var imagePreviewReveal = Revealer.New();
        // imagePreviewReveal.SetCanTarget(false);
        // // imagePreviewOverlay.SetCanTarget(false);
        // // imagePreviewOverlay.AddOverlay(imagePreviewReveal);
        
        var overlay = Overlay.New();
        overlay.SetChild(carousel);
        overlay.AddOverlay(nextReveal);
        overlay.AddOverlay(prevReveal);
        
        foreach (var ss in images)
        {
            ss.ImageWidget.SetHexpand(true);
            ss.ImageWidget.SetVexpand(true);
            ss.ImageWidget.SetHalign(Align.Center);
            ss.ImageWidget.SetValign(Align.Center);
            ss.ImageWidget.SetImageHeight(400, ss.Width, ss.Height);

            var imgBox = Box.New(Orientation.Horizontal, 0);
            var clickCtr = GestureClick.New();
            clickCtr.OnReleased += (sender, args) =>
            {
                // OpenImagePreview(ss);
                setImagePreview.Invoke(ss);
                Console.WriteLine($"{ss.Caption} click controller");
            };
            
            ss.ImageWidget.AddController(clickCtr);
            imgBox.Append(ss.ImageWidget);
            carousel.Append(imgBox);
        }
        
        Append(overlay);
        Append(carouselIndicator);
    }

    private void OpenImagePreview(Revealer revealer, AppScreenshot screenshot)
    {
        var imgPreview = Image.NewFromPixbuf(screenshot.Pixbuf);
        revealer.SetChild(imgPreview);
        revealer.SetRevealChild(true);
    }
}