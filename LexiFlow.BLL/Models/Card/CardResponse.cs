using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Card
{
    public class CardResponse
    {
        public Guid Id { get; set; }
        public string Term { get; set; } = null!;
        public string PrimaryMeaning { get; set; }
    }
}
