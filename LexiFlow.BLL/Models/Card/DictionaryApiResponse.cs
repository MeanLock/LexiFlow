using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Card
{
    public class DictionaryApiResponse
    {
        public string? Word { get; set; }

        public string? Phonetic { get; set; }

        public List<PhoneticDto>? Phonetics { get; set; }

        public List<MeaningDto>? Meanings { get; set; }
    }

    public class PhoneticDto
    {
        public string? Text { get; set; }

        public string? Audio { get; set; }
    }

    public class MeaningDto
    {
        public string? PartOfSpeech { get; set; }

        public List<DefinitionDto>? Definitions { get; set; }
    }

    public class DefinitionDto
    {
        public string? Definition { get; set; }

        public string? Example { get; set; }

        public List<string>? Synonyms { get; set; }

        public List<string>? Antonyms { get; set; }
    }
}
