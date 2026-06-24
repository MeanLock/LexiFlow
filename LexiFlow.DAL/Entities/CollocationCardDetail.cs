using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class CollocationCardDetail
{
    [Key]
    public Guid CardId { get; set; }

    [StringLength(500)]
    public string Pattern { get; set; } = null!;

    public string? Explanation { get; set; }

    public string? Example { get; set; }

    public string? ExampleMeaning { get; set; }

    [ForeignKey("CardId")]
    [InverseProperty("CollocationCardDetail")]
    public virtual Card Card { get; set; } = null!;
}
