using DesktopTracker.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json.Linq;

namespace DesktopTracker.Utils
{
    internal class Sender
    {
        public static async Task<HttpResponseMessage> SendToServer(string json, Settings? settings, bool auth)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            string url = settings?.SendTo ?? throw new ArgumentNullException("Url value is null");

            if (auth)
            {
                string authKey = settings.AuthKey ?? string.Empty;

                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", String.Format("{0}", authKey));

                response = await client.PostAsync(url, content);

                return response;
            }
            
            response = await client.PostAsync(url, content);

            return response;
        }
    }
}
