using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Dictionaries
{
    public class WordDefinitionResponseModel
    {
        public string Word { get; set; } = string.Empty;

        public List<PartOfSpeechModel> PartOfSpeeches { get; set; } = [];
    }
}
