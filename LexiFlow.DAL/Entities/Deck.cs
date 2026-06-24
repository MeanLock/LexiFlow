using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.DAL.Entities;

public partial class Deck
{
    [Key]
    public Guid Id { get; set; }

    public Guid OwnerId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Deck")]
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    [ForeignKey("OwnerId")]
    [InverseProperty("Decks")]
    public virtual User Owner { get; set; } = null!;

    [InverseProperty("Deck")]
    public virtual ICollection<ReviewSession> ReviewSessions { get; set; } = new List<ReviewSession>();
}
