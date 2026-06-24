using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Dictionaries
{
    public class WordsApiResultModel
    {
        [JsonPropertyName("definition")]
        public string Definition { get; set; } = string.Empty;

        [JsonPropertyName("partOfSpeech")]
        public string PartOfSpeech { get; set; } = string.Empty;

        [JsonPropertyName("synonyms")]
        public List<string>? Synonyms { get; set; }

        [JsonPropertyName("antonyms")]
        public List<string>? Antonyms { get; set; }

        [JsonPropertyName("examples")]
        public List<string>? Examples { get; set; }
    }
}
