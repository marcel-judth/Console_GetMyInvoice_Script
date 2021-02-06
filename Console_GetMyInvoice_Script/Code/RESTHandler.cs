using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Console_GetMyInvoice_Script.Code
{
    class RESTHandler
    {
        private readonly string basURL = "https://api.getmyinvoices.com/accounts/v3";
        private readonly string api_key = "asi7-8uxr-jj8y-2snw-bpzv-35gv-rfa6";
        public async Task<string> GetDocumentList(string filter)
        {
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(basURL + "/documents"),
                Content = new StringContent(filter, Encoding.UTF8, "application/json"),
            };

            request.Headers.Add("X-API-KEY", api_key);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        }

        public async Task<string> GetDocument(int documentUid, string filter)
        {
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(basURL + "/documents/" + documentUid),
                Content = new StringContent(filter, Encoding.UTF8, "application/json"),
            };

            request.Headers.Add("X-API-KEY", api_key);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
