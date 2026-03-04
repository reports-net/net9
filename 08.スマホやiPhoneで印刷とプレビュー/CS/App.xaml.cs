namespace SvgPreviewMobile
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
            window.Title = "Reports.net - SVG 印刷プレビュー (Mobile)";
            window.Width = 420;
            window.Height = 800;
            return window;
        }
    }
}
