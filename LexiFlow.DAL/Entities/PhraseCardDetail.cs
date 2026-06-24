using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class PhraseCardDetail
{
    [Key]
    public Guid CardId { get; set; }

    [StringLength(100)]
    public string? PhraseType { get; set; }

    public string? UsageNote { get; set; }

    public string? Example { get; set; }

    public string? ExampleMeaning { get; set; }

    [ForeignKey("CardId")]
    [InverseProperty("PhraseCardDetail")]
    public virtual Card Card { get; set; } = null!;
}
