using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class VocabularyCardDetail
{
    [Key]
    public Guid CardId { get; set; }

    [StringLength(200)]
    public string? Phonetic { get; set; }

    [StringLength(1000)]
    public string? AudioUrl { get; set; }

    [StringLength(20)]
    public string? CefrLevel { get; set; }

    [ForeignKey("CardId")]
    [InverseProperty("VocabularyCardDetail")]
    public virtual Card Card { get; set; } = null!;
}
