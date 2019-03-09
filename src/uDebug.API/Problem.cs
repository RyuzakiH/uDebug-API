using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using uDebug.API.Models;

namespace uDebug.API
{
    public class Problem
    {
        private readonly Client client;

        private string form_build_id;
        private string form_id;

        public bool IsLoaded { get; private set; }

        public int Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public Judge Judge { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string ProblemLink { get; set; }
        public List<Input> Inputs { get; set; }
        public int InputsCount { get; set; }
        public User SolutionBy { get; set; }
        public User MostPopularInputUser { get; set; }


        public Problem(Client client)
        {
            this.client = client;
            IsLoaded = false;
        }


        public void Load()
        {
            if (Url == null)
                throw new MissingFieldException("Url of the problem is needed");

            Load(Url);
        }

        public void Load(Judge judge, int problemNumber)
        {
            Number = problemNumber;
            Judge = judge;
            Url = $"{Constants.BASE_URL}/{Constants.Judges[judge]}/{problemNumber}";

            var document = client.HttpClient.GetHtmlDocument(Url);

            ExtractProblem(document);
        }

        public async Task LoadAsync(Judge judge, int problemNumber)
        {
            Number = problemNumber;
            Judge = judge;
            Url = $"{Constants.BASE_URL}/{Constants.Judges[judge]}/{problemNumber}";

            var document = await client.HttpClient.GetHtmlDocumentAsync(Url);

            ExtractProblem(document);
        }

        public void Load(string url)
        {
            var urlParts = new Stack<string>(url.Split('/'));
            Number = int.Parse(urlParts.Pop());
            Judge = Utilities.GetJudge(urlParts.Pop());
            Url = url;

            var document = client.HttpClient.GetHtmlDocument(Url);

            ExtractProblem(document);
        }

        public async Task LoadAsync(string url)
        {
            var urlParts = new Stack<string>(url.Split('/'));
            Number = int.Parse(urlParts.Pop());
            Judge = Utilities.GetJudge(urlParts.Pop());
            Url = url;

            var document = await client.HttpClient.GetHtmlDocumentAsync(Url);

            ExtractProblem(document);
        }

        private void ExtractProblem(HtmlDocument document)
        {
            Id = int.Parse(document.GetElementsByName("input", "problem_nid").FirstOrDefault()?.GetAttributeValue("value", "-1").Trim());

            Title = document.GetElementsByClassName("h1", "problem-title").FirstOrDefault()?.InnerText.Trim();

            Type = document.GetElementsByClassName("span", "type-of-problem").FirstOrDefault()?.InnerText.Trim();

            Category = document.GetElementsByClassName("span", "category").FirstOrDefault()?.InnerText.Trim();

            ProblemLink = document.GetElementsByClassName("a", "problem-statement").FirstOrDefault()?.GetAttributeValue("href", null);

            SolutionBy = ExtractUser(document.GetElementsByClassName("div", "problem-user-solution").FirstOrDefault());

            MostPopularInputUser = ExtractUser(document.GetElementsByClassName("div", "problem-user-solution-input").FirstOrDefault());

            InputsCount = int.Parse(document.GetElementsByClassName("span", "total-input-count").FirstOrDefault()?
                .InnerText.Replace("(", "").Replace(")", ""));

            Inputs = document.DocumentNode.Descendants("tbody").FirstOrDefault()?.Descendants("tr").Select(ExtractInput).ToList();

            ExtractHiddenData(document);

            IsLoaded = true;
        }

        private Input ExtractInput(HtmlNode tr)
        {
            var tdElements = tr.Descendants("td");

            var a2 = tdElements.ElementAt(2).Descendants("a").FirstOrDefault();

            return new Input
            {
                Id = int.Parse(a2?.GetAttributeValue("data-id", null)),
                User = tdElements.ElementAt(1).Descendants("a").FirstOrDefault()?.InnerText.Trim(),
                Date = DateTime.Parse($"{a2?.FirstChild?.InnerText.Trim()} {a2?.FirstChild.NextSibling.InnerText.Trim()}"),
                Votes = int.Parse(tdElements.ElementAt(3).Descendants("span").FirstOrDefault()?.InnerText.Trim()),
                Problem = this
            };
        }

        private User ExtractUser(HtmlNode node)
        {
            var a = node.Descendants("a").FirstOrDefault();
            return new User(client)
            {
                Username = a?.GetAttributeValue("href", null).Split('/').Last(),
                Name = a?.InnerText.Trim(),
                Url = $"{Constants.BASE_URL}{a?.GetAttributeValue("href", null)}",
                PictureUrl = node?.Descendants("img").FirstOrDefault()?.GetAttributeValue("src", null)
            };
        }

        private void ExtractHiddenData(HtmlDocument document)
        {
            form_build_id = document.GetElementsByName("form_build_id").LastOrDefault()?.GetAttributeValue("value", null);
            form_id = document.GetElementsByName("form_id").LastOrDefault()?.GetAttributeValue("value", null);
        }



        public string GetInput(Input input)
        {
            return input.Data = GetInput(input.Id);
        }

        public async Task<string> GetInputAsync(Input input)
        {
            return input.Data = await GetInputAsync(input.Id);
        }

        public string GetInput(int inputId)
        {
            var problemUrl = IsLoaded && Inputs.Exists(input => input.Id.Equals(inputId)) ? Url : null;

            return GetInput(problemUrl, inputId);
        }

        public async Task<string> GetInputAsync(int inputId)
        {
            var problemUrl = IsLoaded && Inputs.Exists(input => input.Id.Equals(inputId)) ? Url : null;

            return await GetInputAsync(problemUrl, inputId);
        }

        private string GetInput(string problemUrl, int inputId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{Constants.BASE_URL}/udebug-custom-get-selected-input-ajax"))
            {
                requestMessage.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                requestMessage.Headers.Referrer = problemUrl != null ? new Uri(problemUrl) : null;
                requestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");

                requestMessage.Content = new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "input_nid", $"{inputId}" }
                    }
                );

                return client.HttpClient.Send(requestMessage).Content.ReadAsJsonObject<dynamic>()["input_value"];
            }
        }

        private async Task<string> GetInputAsync(string problemUrl, int inputId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{Constants.BASE_URL}/udebug-custom-get-selected-input-ajax"))
            {
                requestMessage.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                requestMessage.Headers.Referrer = problemUrl != null ? new Uri(problemUrl) : null;
                requestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");

                requestMessage.Content = new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "input_nid", $"{inputId}" }
                    }
                );

                return (await (await client.HttpClient.SendAsync(requestMessage)).Content.ReadAsJsonObjectAsync<dynamic>())["input_value"];
            }
        }



        public string GetOutput(Input input)
        {
            if (!IsLoaded || !Inputs.Exists(inp => inp.Id.Equals(input.Id)) || (!input.Problem.IsLoaded && input.Problem.Url == null))
                throw new MissingFieldException("The problem of the input needed");

            if (input.Data == null)
                GetInput(input);

            return GetOutput(input.Problem.Number, input.Problem.Url, input.Data);
        }

        public async Task<string> GetOutputAsync(Input input)
        {
            if (!IsLoaded || !Inputs.Exists(inp => inp.Id.Equals(input.Id)) || (!input.Problem.IsLoaded && input.Problem.Url == null))
                throw new MissingFieldException("The problem of the input needed");

            if (input.Data == null)
                await GetInputAsync(input);

            return await GetOutputAsync(input.Problem.Number, input.Problem.Url, input.Data);
        }

        public string GetOutput(string input)
        {
            if (!IsLoaded && Url == null)
                throw new MissingFieldException("The problem of the input needed");

            return GetOutput(Number, Url, input);
        }

        public async Task<string> GetOutputAsync(string input)
        {
            if (!IsLoaded && Url == null)
                throw new MissingFieldException("The problem of the input needed");

            return await GetOutputAsync(Number, Url, input);
        }

        private string GetOutput(int problemNumber, string problemUrl, string inputData)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, problemUrl))
            {
                requestMessage.Headers.Referrer = new Uri(problemUrl);
                requestMessage.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");

                requestMessage.Content = new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "problem_nid", $"{problemNumber}" },
                        { "input_data", inputData },
                        { "node_nid", "" },
                        { "op", "Get Accepted Output" },
                        { "output_data", "" },
                        { "user_output", "" },
                        { "form_build_id", form_build_id },
                        { "form_id", form_id },
                    }
                );

                var document = client.HttpClient.Send(requestMessage).Content.ReadAsHtmlDocument();

                return WebUtility.HtmlDecode(document.GetElementbyId("edit-output-data").InnerText.Trim());
            }
        }

        private async Task<string> GetOutputAsync(int problemNumber, string problemUrl, string inputData)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, problemUrl))
            {
                requestMessage.Headers.Referrer = new Uri(problemUrl);
                requestMessage.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");

                requestMessage.Content = new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "problem_nid", $"{problemNumber}" },
                        { "input_data", inputData },
                        { "node_nid", "" },
                        { "op", "Get Accepted Output" },
                        { "output_data", "" },
                        { "user_output", "" },
                        { "form_build_id", form_build_id },
                        { "form_id", form_id },
                    }
                );

                var document = await (await client.HttpClient.SendAsync(requestMessage)).Content.ReadAsHtmlDocumentAsync();

                return WebUtility.HtmlDecode(document.GetElementbyId("edit-output-data").InnerText.Trim());
            }
        }

    }
}
