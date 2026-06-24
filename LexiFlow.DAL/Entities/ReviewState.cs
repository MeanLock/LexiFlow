using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class ReviewState
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid CardId { get; set; }

    public DateOnly DueDate { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal EaseFactor { get; set; }

    public int IntervalDays { get; set; }

    public int Repetition { get; set; }

    public int Lapses { get; set; }

    public DateTime? LastReviewedAt { get; set; }

    public DateTime? SuspendedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CardId")]
    [InverseProperty("ReviewStates")]
    public virtual Card Card { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ReviewStates")]
    public virtual User User { get; set; } = null!;
}
