using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Card
{
    public class CreateVocabularyDetailRequest
    {
        public string? Phonetic { get; set; }

        public string? AudioUrl { get; set; }

        public string? CefrLevel { get; set; }

        public List<CreateVocabularyMeaningRequest> Meanings { get; set; }
            = new();
    }

    public class CreateVocabularyMeaningRequest
    {
        public string PartOfSpeech { get; set; } = null!;

        public string Definition { get; set; } = null!;

        public string? Example { get; set; }

        public string? ExampleMeaning { get; set; }

        public List<string>? Synonyms { get; set; }

        public List<string>? Antonyms { get; set; }
    }
}
