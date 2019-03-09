using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace uDebug.API
{
    public class User
    {
        private readonly Client client;

        public string Username { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public int Solutions { get; set; }
        public int Inputs { get; set; }
        public int Hints { get; set; }
        public int Votes { get; set; }
        public int Helpfulness { get; set; }
        public int Karma { get; set; }
        public string Country { get; set; }
        public string CountryImageUrl { get; set; }
        public string PictureUrl { get; set; }
        public bool IsLoaded { get; private set; }


        public User(Client client)
        {
            this.client = client;
        }

        public void Load()
        {
            if (Url == null && Username == null)
                throw new MissingFieldException("Url or Username of the user is needed");

            LoadByUrl(Url);
        }

        public async Task LoadAsync()
        {
            if (Url == null && Username == null)
                throw new MissingFieldException("Url or Username of the user is needed");

            await LoadByUrlAsync(Url);
        }

        public void Load(string username)
        {
            Url = $"{Constants.BASE_URL}/{username}";
            Username = username;

            var document = client.HttpClient.GetHtmlDocument(Url);

            ExtractProfile(document);
        }

        public async Task LoadAsync(string username)
        {
            Url = $"{Constants.BASE_URL}/{username}";
            Username = username;

            var document = await client.HttpClient.GetHtmlDocumentAsync(Url);

            ExtractProfile(document);
        }

        public void LoadByUrl(string url)
        {
            Url = url;
            Username = Url.Split('/').LastOrDefault();

            var document = client.HttpClient.GetHtmlDocument(Url);

            ExtractProfile(document);
        }

        public async Task LoadByUrlAsync(string url)
        {
            Url = url;
            Username = Url.Split('/').LastOrDefault();

            var document = await client.HttpClient.GetHtmlDocumentAsync(Url);

            ExtractProfile(document);
        }


        private void ExtractProfile(HtmlDocument document)
        {
            Name = document.GetElementsByClassName("div", "user-name").FirstOrDefault()?.InnerText.Trim();

            PictureUrl = document.GetElementsByClassName("div", "user-picture").FirstOrDefault()?.Descendants("img").FirstOrDefault()?.GetAttributeValue("src", null);

            Website = document.GetElementsByClassName("div", "field-name-field-user-website").FirstOrDefault()?.Descendants("a").FirstOrDefault()?.GetAttributeValue("href", null);

            Country = document.GetElementsByClassName("img", "countryicon").FirstOrDefault()?.NextSibling?.InnerText.Trim();

            CountryImageUrl = document.GetElementsByClassName("img", "countryicon").FirstOrDefault()?.GetAttributeValue("src", null);

            Solutions = int.Parse(document.GetElementsByClassName("span", "user-solutions").FirstOrDefault()?.InnerText.Trim());

            Inputs = int.Parse(document.GetElementsByClassName("span", "user-inputs").FirstOrDefault()?.InnerText.Trim());

            Hints = int.Parse(document.GetElementsByClassName("span", "user-hints").FirstOrDefault()?.InnerText.Trim());

            Votes = int.Parse(document.GetElementsByClassName("span", "user-votes").FirstOrDefault()?.InnerText.Trim());

            Helpfulness = int.Parse(document.GetElementsByClassName("span", "user-helpfulness").FirstOrDefault()?.InnerText.Trim());

            Karma = int.Parse(document.GetElementsByClassName("span", "user-good-karma").FirstOrDefault()?.InnerText.Trim());

            IsLoaded = true;
        }

    }
}
