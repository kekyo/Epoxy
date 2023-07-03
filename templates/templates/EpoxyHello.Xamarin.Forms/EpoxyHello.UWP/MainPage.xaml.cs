namespace EpoxyHello.UWP;

public partial class MainPage
{
    public MainPage()
    {
        this.InitializeComponent();

        LoadApplication(new EpoxyHello.App());
    }
}
