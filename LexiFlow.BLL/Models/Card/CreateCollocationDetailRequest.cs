using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Card
{
    public class CreateCollocationDetailRequest
    {
        public string Pattern { get; set; } = null!;

        public string? Explanation { get; set; }

        public string? Example { get; set; }

        public string? ExampleMeaning { get; set; }
    }
}
