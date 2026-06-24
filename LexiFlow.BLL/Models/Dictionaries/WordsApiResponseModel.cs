using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Dictionaries
{
    public class WordsApiResponseModel
    {
        [JsonPropertyName("word")]
        public string Word { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public List<WordsApiResultModel> Results { get; set; } = [];

        [JsonPropertyName("pronunciation")]
        public Dictionary<string, string>? Pronunciation { get; set; }
    }
}
