using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class Card
{
    [Key]
    public Guid Id { get; set; }

    public Guid DeckId { get; set; }

    public int CardType { get; set; }

    [StringLength(300)]
    public string Term { get; set; } = null!;

    [StringLength(300)]
    public string NormalizedTerm { get; set; } = null!;

    [StringLength(1000)]
    public string? PrimaryMeaning { get; set; }

    public bool IsPublic { get; set; }

    public int SourceType { get; set; }

    public Guid? SourceCardId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Card")]
    public virtual CollocationCardDetail? CollocationCardDetail { get; set; }

    [ForeignKey("DeckId")]
    [InverseProperty("Cards")]
    public virtual Deck Deck { get; set; } = null!;

    [InverseProperty("Card")]
    public virtual IdiomCardDetail? IdiomCardDetail { get; set; }

    [InverseProperty("SourceCard")]
    public virtual ICollection<Card> InverseSourceCard { get; set; } = new List<Card>();

    [InverseProperty("Card")]
    public virtual PhraseCardDetail? PhraseCardDetail { get; set; }

    [InverseProperty("Card")]
    public virtual ICollection<ReviewRecord> ReviewRecords { get; set; } = new List<ReviewRecord>();

    [InverseProperty("Card")]
    public virtual ICollection<ReviewState> ReviewStates { get; set; } = new List<ReviewState>();

    [ForeignKey("SourceCardId")]
    [InverseProperty("InverseSourceCard")]
    public virtual Card? SourceCard { get; set; }

    [InverseProperty("Card")]
    public virtual VocabularyCardDetail? VocabularyCardDetail { get; set; }

    [InverseProperty("Card")]
    public virtual ICollection<VocabularyMeaning> VocabularyMeanings { get; set; } = new List<VocabularyMeaning>();
}
