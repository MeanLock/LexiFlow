using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class IdiomCardDetail
{
    [Key]
    public Guid CardId { get; set; }

    public string? LiteralMeaning { get; set; }

    public string? FigurativeMeaning { get; set; }

    public string? UsageNote { get; set; }

    public string? Example { get; set; }

    public string? ExampleMeaning { get; set; }

    [ForeignKey("CardId")]
    [InverseProperty("IdiomCardDetail")]
    public virtual Card Card { get; set; } = null!;
}
