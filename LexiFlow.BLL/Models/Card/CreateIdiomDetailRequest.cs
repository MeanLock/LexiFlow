using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Card
{
    public class CreateIdiomDetailRequest
    {
        public string? LiteralMeaning { get; set; }

        public string? FigurativeMeaning { get; set; }

        public string? UsageNote { get; set; }

        public string? Example { get; set; }

        public string? ExampleMeaning { get; set; }
    }
}
