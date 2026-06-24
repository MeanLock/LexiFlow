using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Deck
{
    public class CreateDeckRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
