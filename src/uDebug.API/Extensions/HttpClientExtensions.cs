using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static HttpResponseMessage Get(this HttpClient client, string requestUri)
        {
            return Task.Run(() => client.GetAsync(requestUri)).Result;
        }

        public static HtmlDocument GetHtmlDocument(this HttpClient client, string url)
        {
            var response = client.Get(url);

            var document = new HtmlDocument();
            document.LoadHtml(response.Content.ReadAsString());

            return document;
        }

        public static async Task<HtmlDocument> GetHtmlDocumentAsync(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);

            var document = new HtmlDocument();
            await Task.Run(() => document.LoadHtml(response.Content.ReadAsString()));

            return document;
        }

        public static HttpResponseMessage Send(this HttpClient client, HttpRequestMessage request)
        {
            return Task.Run(() => client.SendAsync(request)).Result;
        }

        public static string ReadAsString(this HttpContent content)
        {
            return Task.Run(() => content.ReadAsStringAsync()).Result;
        }

        public static T ReadAsJsonObject<T>(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<T>(content.ReadAsString());
        }

        public static async Task<T> ReadAsJsonObjectAsync<T>(this HttpContent content)
        {
            return await Task.Run(async () => JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync()));
        }

        public static HtmlDocument ReadAsHtmlDocument(this HttpContent content)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content.ReadAsString());
            return document;
        }

        public static async Task<HtmlDocument> ReadAsHtmlDocumentAsync(this HttpContent content)
        {
            var document = new HtmlDocument();
            document.LoadHtml(await content.ReadAsStringAsync());
            return document;
        }

    }
}
