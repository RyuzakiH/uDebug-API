using Newtonsoft.Json;
using System;

namespace uDebug.API.Models
{
    public class Input
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("Date")]
        public DateTime Date { get; set; }

        [JsonProperty("Votes")]
        public int Votes { get; set; }

        public string Data { get; set; }

        public Problem Problem { get; set; }

    }
}
