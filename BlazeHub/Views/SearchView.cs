using Adw;
using BlazeHub.Services.Flathub;
using BlazeHub.Utils;
using BlazeHub.Windows;
using Gtk;

namespace BlazeHub.Views;

public class SearchView : Box
{
    private readonly SearchEntry _searchEntry;
    
    public SearchView()
    {
        _searchEntry = SearchEntry.New();
        _searchEntry.SetHexpand(true);
        _searchEntry.SetPlaceholderText("AppID ex. re.sonny.Eloquent");
        var btn = Button.NewFromIconName("folder-saved-search-symbolic");
        btn.OnClicked += OnIDSearchBtn;
        
        
        var box = Box.New(Orientation.Horizontal, 8);
        box.SetHexpand(true);
        box.Append(_searchEntry);
        box.Append(btn);
        var topClamp = Clamp.New();
        topClamp.SetChild(box);
        topClamp.SetOrientation(Orientation.Horizontal);
        topClamp.SetMaximumSize(512);

        this.SetMargins(12);
        SetOrientation(Orientation.Vertical);
        SetVexpand(true);
        Append(Label.New("SearchView - DevTesting"));
        Append(topClamp);
    }

    private async void OnIDSearchBtn(Button sender, EventArgs args)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_searchEntry.GetText())) return;
        
            var app = await FlathubAPI.GetAppDetails(_searchEntry.GetText());
            if (app == null) return;
            MainWindow.Navigation.Push(ViewUtils.WrapViewIntoPage(new AppSiteView(app.Id), app.Name));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}