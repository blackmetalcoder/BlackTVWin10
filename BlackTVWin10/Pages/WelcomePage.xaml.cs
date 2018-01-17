using System;
using Windows.UI.Xaml.Controls;

namespace BlackTVWin10.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.UI.Xaml.Controls.WebView.ClearTemporaryWebDataAsync();
            Uri u = new Uri("http://blacktv.se/appstartNy.html");
            WebStart.Navigate(u);
        }
    }
}
