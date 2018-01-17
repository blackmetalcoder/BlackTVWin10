using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BlackTVWin10.ServiceBlack;
using Newtonsoft.Json;

namespace BlackTVWin10.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page2 : Page
    {
        List<BlackInfo> B = new List<BlackInfo>();
        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer timerUppdatering = new DispatcherTimer();
        private int iAntal;
        private int iNu;
        public Page2()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Hamta();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object tid = localSettings.Values["tid"];
            int i = 20;
            if (tid == null)
            {
                // No data
            }
            else
            {
                tid = int.Parse(tid.ToString());
                i = int.Parse(tid.ToString());
            }
            timer.Interval = new TimeSpan(00, 0, i);
            
            bool enabled = timer.IsEnabled;
            bool video = false;
            //Hämtar minuter för uppdatering
            Object tidUppd = localSettings.Values["uppdatering"];
            int iU = 60;
            if (tidUppd == null)
            {
                // No data
            }
            else
            {
                tidUppd = int.Parse(tidUppd.ToString());
                iU = int.Parse(tidUppd.ToString());
            }
            timerUppdatering.Interval = new TimeSpan(00, iU, 00);
            timerUppdatering.Start();
            timerUppdatering.Tick += timerUppdatering_Tick;
        }

        private async void timerUppdatering_Tick(object sender, object e)
        {
            Hamta();
        }
        private async void timer_Tick(object sender, object e)
        {
            iNu++;
            if (iNu > iAntal - 1)
            {
                iNu = 0;
                visare.NavigateToString(B[iNu].info);
            }
            else
            {
                visare.NavigateToString(B[iNu].info);
            }


        }
        private async void Hamta()
        {
            try
            {
                ProgressRing1.Visibility = Visibility.Visible;
                int GruppID = 0;
                int FtgId = 0;
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                Object valueGrupp = localSettings.Values["grupp"];
                DateTime today = DateTime.Today;
                if (valueGrupp == null)
                {
                    // No data
                }
                else
                {
                    GruppID = int.Parse(valueGrupp.ToString());
                }
                Object valueFtgId = localSettings.Values["id"];
                if (valueFtgId == null)
                {
                    // No data
                }
                else
                {
                    FtgId = int.Parse(valueFtgId.ToString());
                }
                ServiceBlackSoapClient klient = new ServiceBlackSoapClient();
                var s = await klient.getTVjsonAsync(FtgId, GruppID, today);
                string ss = s.Body.getTVjsonResult;
                var j = JsonConvert.DeserializeObject<List<BlackInfo>>(ss);
                foreach (var item in j)
                {
                    BlackInfo b = new BlackInfo();
                    b.ForetagsID = item.ForetagsID;
                    string HTML = "<!DOCTYPE html>" +
                                  "<html>" +
                                  "<head>" +
                                  "<title> BLACKTV </title>" +
                                  "<meta charset = utf - 8 />" +
                                  "<meta name = viewport  content = width = device - width, initial - scale = 1.0, maximum - scale = 1.0, user - scalable = no/>" +
                                  "<!--Latest compiled and minified CSS -->" +
                                  "<link rel = stylesheet  href='ms-appx-web:///Resources/bootstrap.min.css'>" +
                                  "<!--Optional theme-- >" +
                                  "<link rel = stylesheet  href='ms-appx-web:///Resources/bootstrap-theme.min.css'>" +
                                  "<script src='ms-appx-web:///Resources/jquery-3.1.0.min.js'></script>" +
                                  "<!--Latest compiled and minified JavaScript -->" +
                                  "<script src ='ms-appx-web:///Resources/bootstrap.min.js'></script>" +
                                  "</head >" +
                                  "<body >" +
                                  item.info +
                                  "</body></html > ";
                    b.info = HTML;
                    b.Video = item.Video;
                    B.Add(b);
                }
                iAntal = B.Count;
                iNu = 0;
                visare.NavigateToString(B[iNu].info);
                timer.Start();
            }
            catch (Exception x)
            {
                string s = x.Message.ToString();
            }
        }

        private void visare_LoadCompleted(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            ProgressRing1.Visibility = Visibility.Collapsed;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double remainingWidth = Windows.UI.Xaml.Window.Current.Bounds.Width - 62;
            visare.Width = remainingWidth;
            visare.Height = e.NewSize.Height-100;
        }
    }
}
