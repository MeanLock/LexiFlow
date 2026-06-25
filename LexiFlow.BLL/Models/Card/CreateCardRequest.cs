using LexiFlow.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Models.Card
{
    public class CreateCardRequest
    {
        public Guid DeckId { get; set; }

        public CardType CardType { get; set; }

        public string Term { get; set; } = null!;

        public string? PrimaryMeaning { get; set; }

        public bool IsPublic { get; set; }

        public CardSourceType SourceType { get; set; }

        public CreateVocabularyDetailRequest? Vocabulary { get; set; }

        public CreateCollocationDetailRequest? Collocation { get; set; }

        public CreateIdiomDetailRequest? Idiom { get; set; }

        public CreatePhraseDetailRequest? Phrase { get; set; }
    }
}
