using DesktopTracker.Models;
using System.Net.Http.Headers;

namespace DesktopTracker.Utils
{
    internal class Sender
    {
        public static async Task<HttpResponseMessage> SendToServer(string json, Settings? settings, bool auth)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            string url = settings?.SendTo ?? throw new ArgumentNullException("Url value is null");

            if (auth)
            {
                string authKey = settings.AuthKey ?? string.Empty;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + authKey);

                response = await client.PostAsync(url, new StringContent(json));

                return response;
            }

            response = await client.PostAsync(url, new StringContent(json));

            return response;
        }
    }
}
