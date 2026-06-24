using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LexiFlow.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class ReviewRecord
{
    [Key]
    public Guid Id { get; set; }

    public Guid ReviewSessionId { get; set; }

    public Guid UserId { get; set; }

    public Guid CardId { get; set; }

    public ReviewMode ReviewMode { get; set; }

    public int Rating { get; set; }

    public string? AnswerText { get; set; }

    public bool IsCorrect { get; set; }

    public int? ResponseTimeMs { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal PreviousEaseFactor { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal NewEaseFactor { get; set; }

    public int PreviousIntervalDays { get; set; }

    public int NewIntervalDays { get; set; }

    public DateTime ReviewedAt { get; set; }

    [ForeignKey("CardId")]
    [InverseProperty("ReviewRecords")]
    public virtual Card Card { get; set; } = null!;

    [ForeignKey("ReviewSessionId")]
    [InverseProperty("ReviewRecords")]
    public virtual ReviewSession ReviewSession { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ReviewRecords")]
    public virtual User User { get; set; } = null!;
}
