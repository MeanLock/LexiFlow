using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class VocabularyMeaning
{
    [Key]
    public Guid Id { get; set; }

    public Guid CardId { get; set; }

    [StringLength(50)]
    public string PartOfSpeech { get; set; } = null!;

    public string Definition { get; set; } = null!;

    public string? Example { get; set; }

    public string? ExampleMeaning { get; set; }

    public string? Synonyms { get; set; }

    public string? Antonyms { get; set; }

    [ForeignKey("CardId")]
    [InverseProperty("VocabularyMeanings")]
    public virtual Card Card { get; set; } = null!;
}
