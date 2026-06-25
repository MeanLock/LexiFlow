using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Card
{
    public class DictionaryResponse
    {
        public string? Phonetic { get; set; }

        public string? AudioUrl { get; set; }

        public List<DictionaryMeaningResponse> Meanings { get; set; } = [];
    }

    public class DictionaryMeaningResponse
    {
        public string PartOfSpeech { get; set; } = string.Empty;

        public string? Definition { get; set; }

        public string? Example { get; set; }

        public List<string> Synonyms { get; set; } = [];

        public List<string> Antonyms { get; set; } = [];
    }
}
