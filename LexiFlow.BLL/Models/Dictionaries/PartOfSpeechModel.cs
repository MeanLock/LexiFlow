using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Dictionaries
{
    public class PartOfSpeechModel
    {
        public string? Phonetic { get; set; }

        public string AudioUrl { get; set; } = string.Empty;

        public string PartOfSpeech { get; set; } = string.Empty;

        public List<MeaningModel> Meanings { get; set; } = [];
    }
}
