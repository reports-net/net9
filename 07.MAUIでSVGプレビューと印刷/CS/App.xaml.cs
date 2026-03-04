namespace SvgPreviewMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new MainPage());
            window.Title = "Reports.net - SVG プレビュー (MAUI)";
            window.Width = 1100;
            window.Height = 800;
            return window;
        }
    }
}
