using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace uDebug.API
{
    public class Search
    {
        private readonly Client client;

        public Search(Client client)
        {
            this.client = client;
        }


        public List<Problem> SearchProblems(string term, SearchCategory category)
        {
            var document = client.HttpClient.GetHtmlDocument($"https://udebug.com/?search_string={term}&search_category={(int)category}");

            return document.DocumentNode.Descendants("tbody").FirstOrDefault()?.Descendants("tr").Select(ExtractProblem).ToList();
        }

        public async Task<List<Problem>> SearchProblemsAsync(string term, SearchCategory category)
        {
            var document = await client.HttpClient.GetHtmlDocumentAsync($"https://udebug.com/?search_string={term}&search_category={(int)category}");

            return document.DocumentNode.Descendants("tbody").FirstOrDefault()?.Descendants("tr").Select(ExtractProblem).ToList();
        }
        
        private Problem ExtractProblem(HtmlNode tr)
        {
            var tdElements = tr.Descendants("td");
            var a = tdElements.ElementAt(1)?.Descendants("a").FirstOrDefault();

            return new Problem(client)
            {
                Number = int.Parse(tdElements.ElementAt(0)?.InnerText.Trim()),
                Category = tdElements.ElementAt(2)?.InnerText.Trim(),
                Title = a?.InnerText,
                Url = $"https://udebug.com{ a?.GetAttributeValue("href", "") }",
                Judge = Utilities.GetJudge(a?.GetAttributeValue("href", "").Split('/')[1])
            };
        }

    }
}
