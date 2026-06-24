using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class ReviewSession
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid DeckId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int TotalCards { get; set; }

    public int CorrectCount { get; set; }

    [ForeignKey("DeckId")]
    [InverseProperty("ReviewSessions")]
    public virtual Deck Deck { get; set; } = null!;

    [InverseProperty("ReviewSession")]
    public virtual ICollection<ReviewRecord> ReviewRecords { get; set; } = new List<ReviewRecord>();

    [ForeignKey("UserId")]
    [InverseProperty("ReviewSessions")]
    public virtual User User { get; set; } = null!;
}
