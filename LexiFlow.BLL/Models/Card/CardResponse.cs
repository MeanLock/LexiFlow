using LexiFlow.DAL.Enums;
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

        public string Term { get; set; }

        public string? PrimaryMeaning { get; set; }

        public CardType CardType { get; set; }

        public CardSourceType SourceType { get; set; }

        public bool IsPublic { get; set; }

        public object? Detail { get; set; }
    }
}
