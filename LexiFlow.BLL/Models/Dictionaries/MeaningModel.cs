using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Dictionaries
{
    public class MeaningModel
    {
        public string Definition { get; set; } = string.Empty;

        public string? Example { get; set; }

        public List<string> Synonyms { get; set; } = [];

        public List<string> Antonyms { get; set; } = [];
    }
}
