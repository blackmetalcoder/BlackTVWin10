using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using BlackTVWin10.ServiceBlack;

namespace BlackTVWin10
{
    public sealed class DataSource
    {
        public async static Task<dynamic> Loggain( string user, string pwd)
        {
           ServiceBlackSoapClient klient = new ServiceBlackSoapClient();
            var svar = await klient.HelloUserAsync(user, pwd);
            return svar;
        }
        public async static Task<string> GetUser(string user, string pwd)
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri("http://blacktv.se/ServiceBlack.asmx/Inloggning?foretag=" + user + "&losen=" + pwd );
                var Response = await client.GetAsync(uri);
                var statusCode = Response.StatusCode;
                Response.EnsureSuccessStatusCode();
                var ResponseText = await Response.Content.ReadAsStringAsync();
                return ResponseText;
            }

            catch (Exception ex)
            {
                return "fel";
            }
        }
        public async static Task<dynamic> Getgrupper(int ftgid)
        {
            ServiceBlackSoapClient klient = new ServiceBlackSoapClient();
            var svar = await klient.getGrupperAsync(ftgid.ToString());
            return svar;
        }
        public async static Task<string> GetTV()
        {
            try
            {
               
                var client = new HttpClient();
                var uri = new Uri("http://blackmetalcoder.se/blackservice.asmx/getTVjson?foretagsid=1");
                var Response = await client.GetAsync(uri);
                var statusCode = Response.StatusCode;
                Response.EnsureSuccessStatusCode();
                var ResponseText = await Response.Content.ReadAsStringAsync();
                return ResponseText;
            }

            catch (Exception ex)
            {
                return "fel";
            }
        }
    }
}
