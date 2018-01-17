using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using BlackTVWin10.ServiceBlack;
using Newtonsoft.Json;

namespace BlackTVWin10.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page1 : Page
    {
        public Page1()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GetData();
        }

        private void Page_Loading(Windows.UI.Xaml.FrameworkElement sender, object args)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //Konto
            Object valueKonto = localSettings.Values["Konto"];
            if (valueKonto == null)
            {
                // No data
            }
            else
            {
                txtUser.Text = valueKonto.ToString();
            }
            //Password
            Object valuePwd = localSettings.Values["Password"];
            if (valuePwd == null)
            {
                // No data
            }
            else
            {
                txtPassword.Password = valuePwd.ToString();
            }
            
            //Sekunder för sid visning
            Object tid = localSettings.Values["tid"];
            if (tid == null)
            {
                // No data
                sldTime.Value = 20;
            }
            else
            {
                int i = int.Parse(tid.ToString());
                sldTime.Value = i;
            }
            //Minuter mellan uppdatering från databas
            Object uppD = localSettings.Values["uppdatering"];
            if (tid == null)
            {
                // No data
                sldUppdatering.Value = 60;
            }
            else
            {
                int i = int.Parse(tid.ToString());
                sldUppdatering.Value = i;
            }
            if (valueKonto != null && valuePwd != null)
            {
                GetData();
            }
        }

        private async void GetData()
        {
            ring.IsActive = true;
            List<grupp> G = new List<grupp>();
            ServiceBlackSoapClient service = new ServiceBlackSoapClient();
            var s = await service.InloggningAsync(txtUser.Text, txtPassword.Password);
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Konto"] = txtUser.Text;
            localSettings.Values["Password"] = txtPassword.Password;
            localSettings.Values["tid"] = sldTime.Value;
            localSettings.Values["uppdatering"] = sldUppdatering.Value;
            string ss = s.Body.InloggningResult;
            var j = JsonConvert.DeserializeObject<List<user>>(ss);
            var test = j;
            foreach (var item in j)
            {
                int id = item.Id;
                localSettings.Values["id"] = id;
                var g = await service.getGrupper10Async(id);
                string gg = g.Body.getGrupper10Result;
                var ListaGrupp = JsonConvert.DeserializeObject<List<grupp>>(gg);

                foreach (var SgRUPP in ListaGrupp)
                {
                    grupp ggg = new grupp();
                    ggg.Grupp = SgRUPP.Grupp;
                    ggg.Info = SgRUPP.Info;
                    ggg.id = SgRUPP.id;
                    G.Add(ggg);
                }
            }
            lstGrupper.ItemsSource = G;
            ring.IsActive = false;
        }
        private void lstGrupper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var myClickedItem = e.AddedItems[0] as grupp;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["grupp"] = myClickedItem.id;
            

        }

        private void sldTime_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            int val = Convert.ToInt32(e.NewValue);
            string msg = String.Format("Antal sekunder för sidvisning: {0}", val);
            this.sliderValue.Text = msg;
        }

        private void sldUppdatering_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            int val = Convert.ToInt32(e.NewValue);
            string msg = String.Format("Antal minuter mellan uppdatering från databas: {0}", val);
            this.txtMinuter.Text = msg;
        }
    }
}
